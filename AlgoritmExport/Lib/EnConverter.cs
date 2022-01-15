using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoritmExport.Lib
{
    /// <summary>
    /// Конвертация нумераторов из строки в кастомизированный объект
    /// </summary>
    public static class EnConverter
    {
        /// <summary>
        /// Конвертация в Provider_En
        /// </summary>
        /// <param name="str">Текстовый вариант Provider_En</param>
        /// <param name="Default">Занчение по умолчанию если неудалось конвертировать</param>
        /// <returns></returns>
        public static Provider_En Convert(string str, Provider_En Default)
        {
            try
            {
                Provider_En rez = Default;
                Enum.TryParse(str, true, out rez);
                return rez;
            }
            catch (Exception) 
            { 
                return Default; 
            }
        }
    }
}
