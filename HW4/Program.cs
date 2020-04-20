using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreaturesLibrary;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;

namespace HW4
{
    /// <summary>
    /// Кочик Дмитрий Алексеевич
    /// БПИ 199
    /// Вариант 1
    /// </summary>


    class Program
    {
        /// <summary>
        /// Статистичекая переменна для генерации случайных чисел.
        /// </summary>
        static Random rnd = new Random();

        /// <summary>
        /// Константа целочисленного типа, обозначающая минимальную длинну генерируемых имен существ.
        /// </summary>
        const int minNameLength = 6;

        /// <summary>
        /// Константа целочисленного типа, обозначающая максимальную длинну генерируемых имен существ.
        /// </summary>
        const int maxNameLength = 10;

        /// <summary>
        /// Константа целочисленного типа, обозначающая размер списка генерируемых существ.
        /// </summary>
        const int numberOfAnimals = 30;

        /// <summary>
        /// Константа типа строка, хранящая информацию о пути к файлу, в котороый идет запись списка существ.
        /// </summary>
        const string pathForCreatures = "../../../creatures.xml";

        /// <summary>
        /// Метод, создающий имя для существа, согласно спецификации имен существ из класса CreaturesLibrary.
        /// </summary>
        /// <returns>Созданное имя</returns>
        static string CreateName()
        {
            StringBuilder name = new StringBuilder(); //StringBuilder - это альтернативное решение, а могли использовать просто строки и делать +=.
            for (int i = 0; i < rnd.Next(minNameLength, maxNameLength + 1); ++i)
            {
                if (rnd.Next(0, 2) == 1)
                {
                    name.Append((char)rnd.Next('a', 'z' + 1));
                }
                else
                {
                    name.Append((char)rnd.Next('A', 'Z' + 1));
                }
            }
            return name.ToString();
        }

        static void Main(string[] args)
        {
            List<Creature> creatures = new List<Creature>(); // Создаем список существ.
            for (int i = 0; i < numberOfAnimals; ++i) // Заполняем его случайносозданными существами.
            {
                creatures.Add(new Creature(CreateName(), (MovementType)rnd.Next(0, 3), rnd.Next(0, 10) + rnd.NextDouble()));
                if (i != numberOfAnimals - 1)
                {
                    Console.WriteLine(creatures[i].ToString() + ", ");
                }
                else
                {
                    Console.WriteLine(creatures[i].ToString());
                }
            }

            try
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(List<Creature>));
                using (FileStream fs = new FileStream(pathForCreatures, FileMode.Create)) // Сериализуем список существ.
                {
                    serializer.WriteObject(fs, creatures);
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Файл не найден");
            }
            catch (IOException)
            {
                Console.WriteLine("Ошибка ввода-вывода.");
            }
            catch (System.Security.SecurityException)
            {
                Console.WriteLine("Ошибка безопасности.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Запрет на доступ к файлу.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
