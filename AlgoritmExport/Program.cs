using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.Threading;

namespace AlgoritmExport
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)     // -h -s tst -i
        {
            try
            {
                bool Ishelp = false;
                string AutoStart = null;
                bool IsInterfase = true;
                List<string> my_params = new List<string>();
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == @"-h" || args[i] == @"-H" || args[i] == @"-?" || args[i] == @"\h" || args[i] == @"\H" || args[i] == @"\?" || args[i] == @"/h" || args[i] == @"/H" || args[i] == @"/?") Ishelp = true;
                    if (args[i] == @"-s" || args[i] == @"\s" || args[i] == @"/s" || args[i] == @"-S" || args[i] == @"\S" || args[i] == @"/S") { i++; AutoStart = args[i]; }
                    if (args[i] == @"-i" || args[i] == @"\i" || args[i] == @"/i" || args[i] == @"-I" || args[i] == @"\I" || args[i] == @"/I") IsInterfase = false;
                    if (args[i] == @"-p" || args[i] == @"\p" || args[i] == @"/p" || args[i] == @"-P" || args[i] == @"\P" || args[i] == @"/P")
                    {
                        i++;
                        foreach (var item in args[i].Split(','))
                        {
                            string[] t = item.Split('=');
                            if (t.Count() == 2 && t[0].IndexOf('@')>-1)
                            {
                                my_params.Add(item);
                            }
                        }
                    }
                }

                // Проверка, если пользователь вызвал справку, то запускать прогу не надо
                if (Ishelp)
                {
                    Console.WriteLine(@"-s ShortName (Name Task Auto Run)");
                    Console.WriteLine(@"-i (not run interface)");
                    Console.WriteLine(@"-p ""@Product=1,@Product=-1""");
                }
                else
                {
                    // Проверка по процессам, чтобы приложение было в единственном экземпляре.
                    bool oneOnlyProg;
                    Mutex m = new Mutex(true, Application.ProductName, out oneOnlyProg);
                    if (oneOnlyProg == true || AutoStart != null)       // Если это автоматический запуск то можно запускать несколько экземпляров нашего приложения
                    {

                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);

                        try
                        {
                            // Создаём наш собственнй объект
                            MyApp MyApp = new MyApp(my_params);
                            DialogResult rez = DialogResult.Yes;

                            // Проверка валидности лицензии
                            while (!MyApp.MyCom.Lic.isValid && rez == DialogResult.Yes)
                            {
                                if (!IsInterfase)
                                {
                                    rez=DialogResult.Yes;
                                    Console.WriteLine("У вас есть проблемы с лицензией. Обратитесь к вендору.");
                                }
                                else
                                {
                                    Application.Run(new Form_Lic(MyApp.MyCom));
                                    //
                                    if (!MyApp.MyCom.Lic.isValid) rez = MessageBox.Show("Для запуска программы необходимо ввести лицензию, вы хотите это сделать?", "Лицензирование", MessageBoxButtons.YesNo);
                                }
                            }
                            // Проверка валидности лиценции
                            if (MyApp.MyCom.Lic.isValid)
                            {
                                if (IsInterfase)
                                {
                                    //Application.Run();
                                    Application.Run(new Form_Start(MyApp.MyCom, AutoStart));
                                }
                                else
                                {
                                    // Eсли есть подключение и есть задача но без запуска интерфейса, то запускаем процесс в фоне.
                                    if (AutoStart != null && MyApp.MyCom.Provider != null && MyApp.MyCom.Provider.ConnectString() != null && MyApp.MyCom.Provider.ConnectString().Trim() != string.Empty)
                                    {
                                        MyApp.MyCom.Threader.StartWorkflow(AutoStart, my_params);

                                        while (MyApp.MyCom.Threader.Status != "Completed" && MyApp.MyCom.Threader.Status != "ERROR")
                                        {
                                            Thread.Sleep(MyApp.MyCom.IOWhileInt);
                                        }
                                    }
                                }
                            }
                            MyApp.MyCom.Dispose();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        //MessageBox.Show("Это приложение уже запущено.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR");
                Console.WriteLine(ex.Message);
            }
        }
    }

    /// <summary>
    /// Класс контроля всего приложения
    /// </summary>
    public class MyApp
    {
        public Common.Com MyCom = new Common.Com(Environment.CurrentDirectory);
        public List<string> my_params;
        public MyApp(List<string> my_params)
        {
            //MyCom.onClickMenuItem += new EventHandler<Com.onMenuItem>(MyCom_onClickMenuItem);
            //MyCom.onDClicTreyIcon += new EventHandler<Com.onTreyIconDClick>(MyCom_onDClicTreyIcon);
        }


        // Обработка события выхода их системы
        /*void MyCom_onClickMenuItem(object sender, Com.onMenuItem e)
        {
            if (e.IsSystem && e.Tag.ToString() == "Exit")
            {
                this.MyCom.Dispose();
                this.MyCom.EventSave("Завершение работы", Com.MyEvent.Stopping);
                Application.Exit();
            }
        }*/

        // Срабтало событие разворота окна
        /*
        private bool IsShowDialogFStart = false;
        void MyCom_onDClicTreyIcon(object sender, Com.onTreyIconDClick e)
        {
            
            try
            {
                if (!IsShowDialogFStart)
                {
                    IsShowDialogFStart = true;

                    this.MyCom.EventSave("Загружаем форму основное окно", Com.MyEvent.UserEvent);
                    FStart Fform = new FStart((Com.MyCom)sender);
                    Fform.ShowDialog();

                    IsShowDialogFStart = false;

                    //Application.Run(new FStart((Com.MyCom)sender));
                }
                else { e.Action = false; }
            }
            catch (Exception ex) { IsShowDialogFStart = false; MessageBox.Show(ex.Message); }
             
        }*/
    }
}
