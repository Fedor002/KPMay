using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Общие методы работы, для которых целый класс будет излишен
namespace KPMay
{
    public class AD_General
    {
        /// <summary>
        /// Метод для преобразования пути с переменными окружения в полный путь. Пример переменной окружения: %APPDATA%
        /// </summary>
        public static string ConvertEnviromentPatToPath(string path)
        {
            try
            {
                return Environment.ExpandEnvironmentVariables(path);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка при чтении пути: {path}", ex); 
            }
        }
    }
}
