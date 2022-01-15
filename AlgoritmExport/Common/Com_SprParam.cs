using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;

namespace AlgoritmExport.Common
{
    public partial class Com
    {
        public class Com_SprParam : IEnumerable
        {
            #region Properties
                private Com _MyCom;
                private List<MyParam> _Param;

                /// <remarks>Индексатор</remarks>
                public MyParam this[int i]
                {
                    get { return this._Param[i]; }
                    set { this._Param[i] = value; }
                }
                public MyParam this[string s]
                {
                    get
                    {
                        foreach (MyParam item in this._Param)
                            if (item.Name == s) return item;

                        ApplicationException ex = new ApplicationException("Указано несуществующие имя параметра Name:" + s);
                        ex.Source = Com.MyEvent.Param.ToString();
                        this._MyCom.SystemEvent(ex.Source + @": " + ex.Message, Com.MyEvent.ERROR);
                        throw ex;
                    }
                    private set { }
                }
            #endregion

            #region Public metod

                /// <summary>
                /// Конструктор
                /// </summary>
                /// <param name="MyCom">Основной объект</param>
                public Com_SprParam(Com MyCom)
                {
                    this._MyCom = MyCom;
                    this._Param = new List<MyParam>();
                }

                /// <remarks>Передаём перечислитель чтобы можно было использовать в коде forech </remarks>
                public IEnumerator GetEnumerator()
                {
                    return this._Param.GetEnumerator();
                }

                // Пользователь добавляет задание
                public bool Add(MyParam Param)
                {
                    bool rez = false;
                    lock (this._Param)
                    {
                        try
                        {
                                Param.InicializationForSprWorkflow(this, this._MyCom);
                                this._Param.Add(Param);
                                rez = true;

                        }
                        catch (Exception ex)
                        {
                            ex.Source += Com.MyEvent.Param.ToString();

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
