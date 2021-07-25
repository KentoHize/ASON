﻿using System;
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
            List<Person> peronList = new List<Person>();
            List<Creature> petList = new List<Creature>();

            List<Creature> result = new List<Creature>();
            ChaosBox cb = new ChaosBox();
            int rnd = cb.DrawOutByte(100);            
            while (rnd >= 10)
            {
                Creature c;
                if (rnd >= 85) //Dog
                {
                    c = new Dog();
                    c.Name = "A Dog";
                    c.Gender = cb.DrawOutByte(2) == 0 ? Gender.Male : Gender.Female;
                    ((Dog)c).Breed = cb.DrawOutByte(2) == 0 ? "American Bulldog" : "Others";
                    ((Dog)c).IsOmnivores = cb.DrawOutByte(2) == 0;
                    ((Dog)c).Sleepy = cb.DrawOutByte(2) == 0;
                }
                else if (rnd >= 70) // Cat
                {
                    c = new Cat();
                    c.Name = "A Cat";
                    c.Gender = cb.DrawOutByte(2) == 0 ? Gender.Male : Gender.Female;
                    ((Cat)c).Breed = cb.DrawOutByte(2) == 0 ? "American Bulldog" : "Others";
                    ((Cat)c).HateDogs = cb.DrawOutByte(2) == 0;
                }
                else //Person
                {
                    c = new Person();
                    c.Gender = cb.DrawOutByte(2) == 0 ? Gender.Male : Gender.Female;
                    if (c.Gender == Gender.Female)
                        c.Name = WizardGuild.RandomChineseFemaleName();
                    else
                        c.Name = WizardGuild.RandomChineseMaleName();


                    ((Person)c).Address = "(An Address)";
                    rnd = cb.DrawOutByte(6);

                    for(int i = 0; i < rnd; i++)
                    {
                        for(int j = 0; j < result.Count; j++)
                        {
                            if()
                        }
                        ((Person)c).Friends
                    }
                        

                    rnd = cb.DrawOutByte(3);
                    ((Person)c).Pets = "(An Address)";

                }
                c.Birthday = new DateTime(cb.DrawOutInteger(1900, 2020), cb.DrawOutInteger(1, 12), cb.DrawOutInteger(1, 29));

                result.Add(c);
                rnd = cb.DrawOutByte(100);
            }
        }

    }
}