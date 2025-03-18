using System;
using System.Collections.Generic;
using System.Linq;

class Creature
{
    public string Type { get; set; } // "Hawk" or "Dove"
    public double Food { get; set; } = 0;
    public bool Survives { get; set; } = true;

    public Creature(string type)
    {
        Type = type;
    }
}

class EvolutionSimulation
{
    static Random rand = new Random();
    const int FoodPieces = 50;
    const int InitialHawks = 50;
    const int InitialDoves = 50;

    static void Main()
    {
        List<Creature> population = new List<Creature>();
        for (int i = 0; i < InitialHawks; i++) population.Add(new Creature("Hawk"));
        for (int i = 0; i < InitialDoves; i++) population.Add(new Creature("Dove"));

        int totalHawks = 0;
        int totalDoves = 0;

        for (int generation = 1; generation <= 100; generation++)
        {
            SimulateDay(population);
            population = EvolvePopulation(population);
            int hawksCount = population.Count(c => c.Type == "Hawk");
            int dovesCount = population.Count(c => c.Type == "Dove");
            totalHawks += hawksCount;
            totalDoves += dovesCount;

            Console.WriteLine($"Generation {generation}: Hawks = {hawksCount}, Doves = {dovesCount}");
        }

        // Calculate and display the average number of Hawks and Doves
        double avgHawks = (double)totalHawks / 100;
        double avgDoves = (double)totalDoves / 100;

        Console.WriteLine($"\nAverage Hawks over 100 generations: {avgHawks:F2}");
        Console.WriteLine($"Average Doves over 100 generations: {avgDoves:F2}");
    }

    static void SimulateDay(List<Creature> population)
    {
        foreach (var creature in population) creature.Food = 0;

        List<int> foodLocations = Enumerable.Range(0, FoodPieces).ToList();
        Dictionary<int, List<Creature>> foodAssignments = new Dictionary<int, List<Creature>>();

        foreach (var creature in population)
        {
            int foodIndex = foodLocations[rand.Next(foodLocations.Count)];

            // Ensure only up to two creatures can be assigned to a food location
            if (!foodAssignments.ContainsKey(foodIndex))
            {
                foodAssignments[foodIndex] = new List<Creature>();
            }

            // Only assign food to the food location if there are fewer than two creatures already
            if (foodAssignments[foodIndex].Count < 2)
            {
                foodAssignments[foodIndex].Add(creature);
            }
        }

        // Handle food distribution
        foreach (var kvp in foodAssignments)
        {
            var creatures = kvp.Value;
            if (creatures.Count == 1)
            {
                // One creature gets the full food piece
                creatures[0].Food += 1;
            }
            else if (creatures.Count == 2)
            {
                // Two creatures share the food based on their types
                if (creatures[0].Type == "Dove" && creatures[1].Type == "Dove")
                {
                    creatures[0].Food += 0.5;
                    creatures[1].Food += 0.5;
                }
                else if (creatures[0].Type == "Hawk" && creatures[1].Type == "Dove")
                {
                    creatures[0].Food += 0.75;
                    creatures[1].Food += 0.25;
                }
                else if (creatures[0].Type == "Dove" && creatures[1].Type == "Hawk")
                {
                    creatures[1].Food += 0.75;
                    creatures[0].Food += 0.25;
                }
                else
                {
                    creatures[0].Food += 0;
                    creatures[1].Food += 0;
                }
            }
        }
    }


    static List<Creature> EvolvePopulation(List<Creature> population)
    {
        List<Creature> newGeneration = new List<Creature>();
        foreach (var creature in population)
        {
            if (creature.Food >= 0.5)
            {
                newGeneration.Add(new Creature(creature.Type));
                if (creature.Food >= 0.75 && rand.NextDouble() < 0.5)
                {
                    newGeneration.Add(new Creature(creature.Type));
                }
            }
            else if (creature.Food == 0.25 && rand.NextDouble() < 0.5)
            {
                newGeneration.Add(new Creature(creature.Type));
            }
        }
        return newGeneration;
    }
}
