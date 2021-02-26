﻿using System;
using System.Data.Entity;
using System.Linq;
using FrameworkLib_Common;

namespace EF_BeaversLife_Framework.Queries
{
    public class Mix
    {
        public void UseCustomIdName()
        {
            using var context = new AnimalContext();
            var       animals = context.Animals.Where(animal => animal.CustomIdName == 1);

            Console.ForegroundColor = ConsoleColor.Magenta;

            foreach (var animal in animals)
            {
                Console.WriteLine(animal);
                if (animal.Clubs != null)
                {
                    foreach (var club in animal.Clubs)
                    {
                        Console.Write("\t");
                        Console.WriteLine(club);
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        ///     Include is needed.
        /// </summary>
        public void UseInverseProperties()
        {
            using var context = new AnimalContext();
            var       persons = context.Persons.Include(p => p.AnimalsLoved).Include(p => p.AnimalsHated);

            Console.ForegroundColor = ConsoleColor.Magenta;

            foreach (var person in persons)
            {
                Console.WriteLine(person);
                foreach (var animalLoved in person.AnimalsLoved)
                {
                    Console.Write("\t");
                    Console.WriteLine(animalLoved);
                }

                foreach (var animalHated in person.AnimalsHated)
                {
                    Console.Write("\t");
                    Console.Write("\t");
                    Console.WriteLine(animalHated);
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}