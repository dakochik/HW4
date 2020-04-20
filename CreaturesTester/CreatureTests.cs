using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CreaturesLibrary;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace CreaturesTester
{
    [TestClass]
    public class CreatureTests
    {
        [TestMethod]
        public void ToStringTester()
        {
            Creature creature = new Creature("KennyMcC", MovementType.Walking, 1337.5);
            Assert.AreEqual(creature.ToString(), $"{creature.MovementType} creature {creature.Name}: Health = {creature.Health:F3}");
        }

        [TestMethod]
        public void MultiplicationTester1()
        {
            Creature firstCreature = new Creature("Rickkkk", MovementType.Walking, 17.8);
            Creature secondCreature = new Creature("Mortyy", MovementType.Walking, 14.3);
            Creature sthStrange = firstCreature * secondCreature;
            Assert.AreEqual(sthStrange, new Creature("Rictyy", MovementType.Walking, 0.0));
        }

        [TestMethod]
        public void MultiplicationTester2()
        {
            Creature firstCreature = new Creature("Rickkkk", MovementType.Walking, 7.8);
            Creature secondCreature = new Creature("Mortyy", MovementType.Walking, 4.3);
            Creature sthStrange = firstCreature * secondCreature;
            Assert.AreEqual(sthStrange, new Creature("Rictyy", MovementType.Walking, 6.05));
        }

        [TestMethod]
        public void MultiplicationTester3()
        {
            Creature firstCreature = new Creature("Rickkkk", MovementType.Walking, 7.8);
            Creature secondCreature = new Creature("Mortyy", MovementType.Swimming, 4.3);
            Assert.ThrowsException<ArgumentException>( delegate() { return firstCreature * secondCreature; } );
        }

        [TestMethod]
        public void SerializerTester()
        {
            Creature firstCreature = new Creature("IJustWD", MovementType.Walking, 7.8);
            DataContractSerializer serializer = new DataContractSerializer(typeof(Creature));
            try
            {
                using (FileStream fs = new FileStream("testerFile.xml", FileMode.Create)) // Сериализуем список существ.
                {
                    serializer.WriteObject(fs, firstCreature);
                }
                Creature secondCreature;
                using (FileStream fs = new FileStream("testerFile.xml", FileMode.Open)) // Сериализуем список существ.
                {
                    secondCreature = (Creature)serializer.ReadObject(fs);
                }
                File.Delete("testerFile.xml");
                Assert.AreEqual(firstCreature, secondCreature);
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
