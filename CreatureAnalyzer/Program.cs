using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreaturesLibrary;
using System.IO;
using System.Runtime.Serialization;

namespace CreatureAnalyzer
{
    class Program
    {
        /// <summary>
        /// Константа типа строка, хранящая информацию о пути к файлу, в котороый идет запись списка существ.
        /// </summary>
        const string pathForCreatures = "../../../creatures.xml";

        /// <summary>
        /// Константа целочисленного типа, обозначающая сколько первых эл-в нужно вывести после сортировки.
        /// </summary>
        const int numberOfFirstCreatures = 10;

        static void Main(string[] args)
        {
            List<Creature> creatures; // Создаем список существ.
            DataContractSerializer serializer = new DataContractSerializer(typeof(List<Creature>)); // Десериализуем существ из файла.
            using (FileStream fs = new FileStream(pathForCreatures, FileMode.Open))
            {
                creatures= (List<Creature>)serializer.ReadObject(fs);
            }

            var swimmingCreatures = from Creature creature in creatures where creature.MovementType == MovementType.Swimming select creature; // С помощью LINQ вытаскиваем из писка только тех существ, которые в жизни умеют только плавать.

            Console.WriteLine("Количество плавающих существ: "+swimmingCreatures.Count()); // Количество существ из списка, которые в жизни умеют только плавать.

            var doFirstSorting = creatures.Where(creatureA => creatureA != null).OrderBy(creature => -creature.Health); // С помощью LINQ сортируем всех существ по убыванию здоровья.
            Console.WriteLine(Environment.NewLine + "Существа с наибольшим показателем здоровья:");
            for (int i=0; i<Math.Min(doFirstSorting.ToArray().Length, numberOfFirstCreatures); ++i) // Выводим первых 10 существ с наибольшим показателем здоровья, если в списке изначально было меньше 10 существ, выводим их всех.
            {
                Console.WriteLine(doFirstSorting.ToArray()[i]);
            }

            Console.WriteLine(Environment.NewLine+"Группируем существ...");
            var CreaturesGroups = from Creature creature in creatures group creature by creature.MovementType; // Группируем существ по способам передвижения.

            List<Creature> lastThreeCreatures = new List<Creature>(); // Список существ, которые будут являться произведениями всех существ в каждой из групп.

            foreach(var group in CreaturesGroups) // Выводим информацию о произведения всех существ в одной группе.
            {
                Console.WriteLine("Произведение существ, умеющих в "+group.Key+":");


                Creature firstCreature = group.ToArray()[0]; // С каждым произведением мы склеиваем два существа в одно.
                for (int i = 1; i < group.ToArray().Length; ++i) // Поэтому нам достаточно перемножить всех существ группы на первого существа.
                {
                    firstCreature = group.ToArray()[i] * firstCreature;
                }
                Console.WriteLine(firstCreature.ToString() + Environment.NewLine); // Выводим информацию о результате перемножения.

                lastThreeCreatures.Add(firstCreature); //Доавляем результат перемножения.

                //Альтернативное решение для подсчета общего произведения:
                //Creature otherfirstCreature = group.Aggregate((a, b) => a * b);
                //Console.WriteLine(otherfirstCreature.ToString() + Environment.NewLine);
                //lastThreeCreatures.Add(otherfirstCreature);
            }

            var doSecondSorting = lastThreeCreatures.Where(creatureA => creatureA != null).OrderBy(creature => -creature.Health); // Повторяем сортировку с помощью LINQ среди всех рещультатов произведений.
            Console.WriteLine(Environment.NewLine + "Существа с наибольшим показателем здоровья:");
            for (int i = 0; i < Math.Min(doSecondSorting.ToArray().Length, numberOfFirstCreatures); ++i) // Выводим информацию об отсортированном списке.
            {
                Console.WriteLine(doSecondSorting.ToArray()[i]);
            }
        }
    }
}
