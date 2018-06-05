using System;
using System.Linq;
using System.Collections.Generic;

/**
 * Bring data on patient samples from the diagnosis machine to the laboratory with enough molecules to produce medicine!
 **/

class Player
{
    public Player(string[] data)
    {
        Target = data[0];
        Eta = int.Parse(data[1]);
        Score = int.Parse(data[2]);
        StorageA = int.Parse(data[3]);
        StorageB = int.Parse(data[4]);
        StorageC = int.Parse(data[5]);
        StorageD = int.Parse(data[6]);
        StorageE = int.Parse(data[7]);
    }
    public string Target { get; set; }
    public int Eta { get; set; }
    public int Score { get; set; }
    public int StorageA { get; set; }
    public int StorageB { get; set; }
    public int StorageC { get; set; }
    public int StorageD { get; set; }
    public int StorageE { get; set; }
}

class Sample
{
    public Sample(string[] inputs)
    {
        SampleId = int.Parse(inputs[0]);
        CarriedBy = int.Parse(inputs[1]);
        Rank = int.Parse(inputs[2]);
        Health = int.Parse(inputs[4]);
        CostA = int.Parse(inputs[5]);
        CostB = int.Parse(inputs[6]);
        CostC = int.Parse(inputs[7]);
        CostD = int.Parse(inputs[8]);
        CostE = int.Parse(inputs[9]);
    }
    public int SampleId { get; set; }
    public int CarriedBy { get; set; }
    public int Health { get; set; }
    public int Rank { get; set; }
    public int CostA { get; set; }
    public int CostB { get; set; }
    public int CostC { get; set; }
    public int CostD { get; set; }
    public int CostE { get; set; }
    public int TotalCost => CostA + CostB + CostC + CostD + CostE;
    public bool OwnByPlayer => CarriedBy == 0;
    public bool OwnByOpponent => CarriedBy == 1;
    public bool InCloud => CarriedBy == -1;
}

class Game
{
    static void Main(string[] args)
    {
        string[] inputs;
        Player ourPlayer;
        Player opponentPlayer;
        int projectCount = int.Parse(Console.ReadLine());
        for (int i = 0; i < projectCount; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int a = int.Parse(inputs[0]);
            int b = int.Parse(inputs[1]);
            int c = int.Parse(inputs[2]);
            int d = int.Parse(inputs[3]);
            int e = int.Parse(inputs[4]);
        }


        // game loop
        while (true)
        {
            ourPlayer = new Player(Console.ReadLine()?.Split(' '));
            opponentPlayer = new Player(Console.ReadLine()?.Split(' '));
            inputs = Console.ReadLine().Split(' ');
            /*int availableA = int.Parse(inputs[0]);
            int availableB = int.Parse(inputs[1]);
            int availableC = int.Parse(inputs[2]);
            int availableD = int.Parse(inputs[3]);
            int availableE = int.Parse(inputs[4]);*/
            int sampleCount = int.Parse(Console.ReadLine());
            var samples = new List<Sample>();
            for (int i = 0; i < sampleCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                samples.Add(new Sample(inputs));
            }

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            var currentTarget = ourPlayer.Target;
            switch (currentTarget)
            {
                case "START_POS":
                    Console.WriteLine("GOTO DIAGNOSIS");
                    break;
                case "DIAGNOSIS":
                    var currentLoad = samples.Where(i => i.OwnByPlayer).Select(i => i.TotalCost).Sum();
                    if (currentLoad < 10)
                    {
                        var maxSample = samples.Where(i => i.InCloud).Where(i => i.TotalCost < 10 - currentLoad).OrderByDescending(i => i.TotalCost).FirstOrDefault();
                        if (maxSample != null)
                        {
                            Console.Error.WriteLine($"Got sample: {maxSample.CostA} {maxSample.CostB} {maxSample.CostC} {maxSample.CostD} {maxSample.CostE} ");
                            Console.WriteLine($"CONNECT {maxSample.SampleId}");
                            break;
                        }
                    }
                    Console.WriteLine("GOTO MOLECULES");
                    break;
                case "MOLECULES":
                    var ourPlayerSamples = samples.Where(i => i.OwnByPlayer);
                    if (ourPlayerSamples.Select(i => i.CostA).Sum() - ourPlayer.StorageA > 0)
                    {
                        Console.WriteLine("CONNECT A");
                        break;
                    }
                    if (ourPlayerSamples.Select(i => i.CostB).Sum() - ourPlayer.StorageB > 0)
                    {
                        Console.WriteLine("CONNECT B");
                        break;
                    }
                    if (ourPlayerSamples.Select(i => i.CostC).Sum() - ourPlayer.StorageC > 0)
                    {
                        Console.WriteLine("CONNECT C");
                        break;
                    }
                    if (ourPlayerSamples.Select(i => i.CostD).Sum() - ourPlayer.StorageD > 0)
                    {
                        Console.WriteLine("CONNECT D");
                        break;
                    }
                    if (ourPlayerSamples.Select(i => i.CostE).Sum() - ourPlayer.StorageE > 0)
                    {
                        Console.WriteLine("CONNECT E");
                        break;
                    }

                    Console.WriteLine("GOTO LABORATORY");
                    break;
                case "LABORATORY":
                    if (samples.Any(i => i.OwnByPlayer))
                    {
                        Console.Error.WriteLine($"Player got: {ourPlayer.StorageA} {ourPlayer.StorageB} {ourPlayer.StorageC} {ourPlayer.StorageD} {ourPlayer.StorageE}");
                        Console.WriteLine($"CONNECT {samples.First(i => i.OwnByPlayer).SampleId}");
                        break;
                    }
                    Console.WriteLine("GOTO DIAGNOSIS");
                    break;
            }
            
            Console.Error.WriteLine($"Current pos {currentTarget}");
        }
    }
}
