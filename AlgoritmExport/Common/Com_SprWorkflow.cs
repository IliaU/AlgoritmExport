using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;

namespace AlgoritmExport.Common
{
    public partial class Com
    {
        public class Com_SprWorkflow : IEnumerable
        {
            #region Properties
                private Com _MyCom;
                private List<MyWorkflow> _Wokflow;
                public int Count { get { return _Wokflow != null ? _Wokflow.Count : 0; } private set { } }

                /// <remarks>Индексатор</remarks>
                public MyWorkflow this[int i]
                {
                    get { return this._Wokflow[i]; }
                    set { this._Wokflow[i] = value; }
                }
                public MyWorkflow this[string s]
                {
                    get
                    {
                        foreach (MyWorkflow item in this._Wokflow)
                            if (item.ShortName == s) return item;

                        ApplicationException ex = new ApplicationException("Указано несуществующие имя задачи ShortName:" + s);
                        ex.Source = Com.MyEvent.Wokflow.ToString();
                        this._MyCom.SystemEvent(ex.Source + @": " + ex.Message, Com.MyEvent.ERROR);
                        throw ex;
                    }
                    private set { }
                }
                

                /// <summary>
                /// Событие добавление задания в список задачи
                /// </summary>
                public event EventHandler<Com.onWokflowIteratorArg> onAddWorkflow;      // Сохранение строки подключения
            #endregion

            #region Public metod

                /// <summary>
                /// Конструктор
                /// </summary>
                /// <param name="MyCom">Основной объект</param>
                public Com_SprWorkflow(Com MyCom)
                {
                    this._MyCom = MyCom;
                    this._Wokflow = new List<MyWorkflow>();
                }

                /// <remarks>Передаём перечислитель чтобы можно было использовать в коде forech </remarks>
                public IEnumerator GetEnumerator()
                {
                    return this._Wokflow.GetEnumerator();
                }

                // Пользователь добавляет задание
                public bool Add(MyWorkflow Wokflow)
                {
                    bool rez = false;
                    lock (this._Wokflow)
                    {
                        try
                        {
                            if (this._MyCom.SystemEvent(@"Добавление задачи " + Wokflow.ShortName + @" в список заданий " , Com.MyEvent.SprWorkflow))
                            {
                                Wokflow.InicializationForSprWorkflow(this, this._MyCom);

                                // Собственно обработка событие добовление задачи в задание
                                Com.onWokflowIteratorArg myArg = new Com.onWokflowIteratorArg(Wokflow);
                                this._Wokflow.Add(Wokflow);
                                if (onAddWorkflow != null)
                                {
                                    onAddWorkflow.Invoke(this, myArg);
                                }

                                rez = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.Source += Com.MyEvent.Wokflow.ToString();

                            this._MyCom.SystemEvent(ex.Source + @": " + ex.Message, Com.MyEvent.ERROR);
                            throw ex;
                        }
                    }
                    return rez;
                }

            #endregion

            #region Private metod

            #endregion
        }
    }
}
