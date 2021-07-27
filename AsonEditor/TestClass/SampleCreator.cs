using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aritiafel.Artifacts;
using Aritiafel.Organizations;
using Aritiafel.Locations;


namespace AsonEditor.TestClass
{
    public static class SampleCreator
    {

        public static List<Creature> GetSample()
        {
            List<Person> personList = new List<Person>();
            List<Creature> petList = new List<Creature>();
            List<Creature> result = new List<Creature>();

            ChaosBox cb = new ChaosBox();            
            int rnd = cb.DrawOutByte(100);            
            while (personList.Count < 6 || petList.Count < 3 || rnd >= 10)
            {
                Creature c;
                if (rnd >= 85) //Dog
                {
                    c = new Dog();
                    c.Name = "A Dog";
                    c.Gender = cb.DrawOutByte(1) == 0 ? Gender.Male : Gender.Female;
                    ((Dog)c).Breed = cb.DrawOutByte(1) == 0 ? "American Bulldog" : "Others";
                    ((Dog)c).IsOmnivores = cb.DrawOutByte(1) == 0;
                    ((Dog)c).Sleepy = cb.DrawOutByte(1) == 0;
                    petList.Add((Pet)c);
                }
                else if (rnd >= 70) // Cat
                {
                    c = new Cat();
                    c.Name = "A Cat";
                    c.Gender = cb.DrawOutByte(1) == 0 ? Gender.Male : Gender.Female;
                    ((Cat)c).Breed = cb.DrawOutByte(1) == 0 ? "American Bulldog" : "Others";
                    ((Cat)c).HateDogs = cb.DrawOutByte(1) == 0;
                    petList.Add((Pet)c);
                }
                else //Person
                {
                    c = new Person();
                    c.Gender = cb.DrawOutByte(1) == 0 ? Gender.Male : Gender.Female;
                    if (c.Gender == Gender.Female)
                        c.Name = WizardGuild.RandomChineseFemaleName();
                    else
                        c.Name = WizardGuild.RandomChineseMaleName();

                    ((Person)c).Address = "(An Address)";                    
                    rnd = cb.DrawOutByte(1, 6);
                    if (rnd > personList.Count)
                        rnd = personList.Count;
                    ((Person)c).Friends = cb.DrawOutItemsFromList(personList, rnd);
                    rnd = cb.DrawOutByte(3);
                    if (rnd > petList.Count)
                        rnd = petList.Count;
                    ((Person)c).Pets = cb.DrawOutItemsFromList(petList, rnd);
                    personList.Add((Person)c);

                }
                c.Birthday = cb.DrawOutDate(new DateTime(1990, 1, 1), DateTime.Now);

                result.Add(c);
                rnd = cb.DrawOutByte(100);
            }
            return result;
        }
    }
}
