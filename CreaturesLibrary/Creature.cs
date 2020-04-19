using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace CreaturesLibrary
{
    [DataContract]
    public class Creature
    {
        /// <summary>
        /// Целочисленная константа, хранящая информацию о минимальной длинне имени существа.
        /// </summary>
        const int minNameLength = 6;

        /// <summary>
        /// Целочисленная константа, хранящая информацию о максимальной длинне имени существа.
        /// </summary>
        const int maxNameLength = 10;

        /// <summary>
        /// Название существа.
        /// </summary>
        string name;

        /// <summary>
        /// Типе передвижения существа.
        /// </summary>
        MovementType movementType;

        /// <summary>
        /// Здоровье существа.
        /// </summary>
        double health;

        /// <summary>
        /// Свойство - имя существа, проверяющее, принадлежит ли длинна имени отрезку [0, 10], и составлено ли только из подходящих букв.
        /// </summary>
        [DataMember]
        public string Name { get => name; private set { if (value.Length > maxNameLength || value.Length < minNameLength) { throw new ArgumentException("Недопустимая длинна имени"); } else if (!CheckName(value)) { throw new ArgumentException("Недопустимые символы в имени"); } else { name = value; } } }

        /// <summary>
        /// Свойство - способ передвижения существа, никакую проверку не производит.
        /// </summary>
        [DataMember]
        public MovementType MovementType { get => movementType; private set { movementType = value; } }

        /// <summary>
        /// Свойство - здоровье существа, проверяющее, принадлежит ли его значение промежутку [0;10).
        /// </summary>
        [DataMember]
        public double Health { get => health; private set { if (value < 10 && value >= 0) health = value; } }

        /// <summary>
        /// Конструктор без параметров, необходимый для (де)сериализации.
        /// </summary>
        public Creature() { }

        /// <summary>
        /// Конструктор с параметрами, создающий новый объект типа Существо.
        /// </summary>
        /// <param name="name">Имя существа</param>
        /// <param name="movementType">Способо передвижения существа</param>
        /// <param name="health">Здоровье существа</param>
        public Creature(string name, MovementType movementType, double health)
        {
            if (name == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                Name = name;
            }

            this.movementType = movementType;

            Health = health;
        }

        /// <summary>
        /// Метод, проверяющий, состоит ли имя из строчных или заглавных латинских букв.
        /// </summary>
        /// <param name="name">Имя для проверки</param>
        /// <returns>Вердик по проверке</returns>
        static bool CheckName(string name)
        {
            foreach (char letter in name)
            {
                if ((letter < 'a' || letter > 'z') && (letter < 'A' || letter > 'Z'))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Переопределение оператора умножения для двух существ. Результат перемножения новое существо, имя которого состоит из частей имен родителей, способ передвижения - спобос передвижения родителей, здоровье - среднее арифметическое здоровий родителей.
        /// </summary>
        /// <param name="first">Первый множитель - существо</param>
        /// <param name="second">Втоорой множитель - существо</param>
        /// <returns>Произведение существ -существо</returns>
        public static Creature operator *(Creature first, Creature second)
        {
            if (first.MovementType != second.MovementType)
                throw new ArgumentException("Значения MovementType множителей не равны.");

            string name = "";
            if (first.Name.Length > second.Name.Length)
            {
                name += first.Name.Substring(0, first.Name.Length / 2) + second.Name.Substring(second.Name.Length / 2, second.Name.Length - second.Name.Length / 2);
            }
            else
            {
                name += second.Name.Substring(0, second.Name.Length / 2) + first.Name.Substring(first.Name.Length / 2, first.Name.Length - first.Name.Length / 2);
            }

            return new Creature(name, first.MovementType, (first.Health + second.Health) / 2);
        }

        /// <summary>
        /// Метод проверяющий эквивалентность существа и объекта через эквивалентность их реализаций методов ToString()
        /// </summary>
        /// <param name="obj">Объект, который подозревается на эквивалентность</param>
        /// <returns>Результат сравнения</returns>
        public override bool Equals(object obj)
        {
            return ToString().Equals(obj.ToString());
        }

        /// <summary>
        /// Метод, выводящий основную информацию о существе в формате: "{Способ передвижения существа} creature {имя существа}: Health = {здоровье существа в формате три знака после запятой}"
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{MovementType} creature {Name}: Health = {Health:F3}";
        }
    }
}
