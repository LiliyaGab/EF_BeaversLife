﻿namespace EF_BeaversLife.Queries;

public class UseAwaitForeach
{
    /// <summary>
    /// Include required
    /// </summary>
    public async void UseAwaitForeach1()
    {
        await using var context = new AnimalContext();

        await foreach (var animal in context.Animals)
        {
#if NET5_0
            if (animal.Clubs != null)
                foreach (var course in animal.Clubs)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine();
                }
#endif
#if !NET5_0
            if (animal.Food != null)
            {
            }
#endif
        }
    }

    public async void UseAwaitForeach2(AnimalContext context)
    {
        await foreach (var department in context.Animals.AsAsyncEnumerable())
        {
            if (department.Clubs != null)
                foreach (var course in department.Clubs)
                {
                }
        }
    }
}
