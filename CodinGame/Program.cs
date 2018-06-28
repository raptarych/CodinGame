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
        Storage = new Dictionary<char, int>
        {
            ['A'] = int.Parse(data[3]),
            ['B'] = int.Parse(data[4]),
            ['C'] = int.Parse(data[5]),
            ['D'] = int.Parse(data[6]),
            ['E'] = int.Parse(data[7])
        };
    }
    public string Target { get; set; }
    public int Eta { get; set; }
    public int Score { get; set; }
    public Dictionary<char, int> Storage { get; set; }
    public int StorageA { get; set; }
    public int StorageB { get; set; }
    public int StorageC { get; set; }
    public int StorageD { get; set; }
    public int StorageE { get; set; }
    public int CurrentLoad => Storage.Values.Sum();
}

class Sample
{
    public Sample(string[] inputs)
    {
        SampleId = int.Parse(inputs[0]);
        CarriedBy = int.Parse(inputs[1]);
        Rank = int.Parse(inputs[2]);
        Health = int.Parse(inputs[4]);
        Costs = new Dictionary<char, int>
        {
            ['A'] = int.Parse(inputs[5]),
            ['B'] = int.Parse(inputs[6]),
            ['C'] = int.Parse(inputs[7]),
            ['D'] = int.Parse(inputs[8]),
            ['E'] = int.Parse(inputs[9])
        };
    }
    public int SampleId { get; set; }
    public int CarriedBy { get; set; }
    public int Health { get; set; }
    public int Rank { get; set; }
    public Dictionary<char, int> Costs { get; set; }
    public int TotalCost => Costs.Values.Sum();
    public bool OwnByPlayer => CarriedBy == 0;
    public bool Diagnozed => Costs.Values.All(i => i > -1);
    public bool OwnByOpponent => CarriedBy == 1;
    public bool InCloud => CarriedBy == -1;

    public bool CanBeResearched
    {
        get
        {
            return Costs.All(cost => CurrentGameState.Player.Storage[cost.Key] >= cost.Value);
        }
    }

    public bool ShouldRequestMolecules()
    {
        return Costs.All(cost => cost.Value - CurrentGameState.Player.Storage[cost.Key] > CurrentGameState.AvailableMolecules[cost.Key]);
    }
}

static class CurrentGameState
{
    public static Dictionary<char, int> AvailableMolecules { get; set; }
    public static List<Sample> Samples { get; set; }
    public static List<Sample> PlayersSamples => Samples.Where(i => i.OwnByPlayer).ToList();
    public static List<Sample> OpponentSamples => Samples.Where(i => i.OwnByOpponent).ToList();
    public static Player Player { get; set; }
    public static Player Opponent { get; set; }
}

class Game
{
    static void Main(string[] args)
    {
        string[] inputs;
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
            ReadGameState();

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");
            
            var currentLoad = CurrentGameState.Player.CurrentLoad;

            DebugInfo(currentLoad);
            //Console.Error.Write($"Game config:\n{string.Join("\n", samplesInfo)}\n");

            switch (CurrentGameState.Player.Target)
            {
                case "START_POS":
                    Console.WriteLine("GOTO SAMPLES");
                    break;
                case "SAMPLES":
                    if (CurrentGameState.PlayersSamples.Count() < 2)
                    {
                        Console.WriteLine("CONNECT 2");
                        break;
                    } else if (CurrentGameState.PlayersSamples.Count() < 3)
                    {
                        Console.WriteLine("CONNECT 1");
                        break;
                    }
                    Console.WriteLine("GOTO DIAGNOSIS");
                    break;
                case "DIAGNOSIS":
                    var unDiagnozedSample = CurrentGameState.PlayersSamples.FirstOrDefault(i => !i.Diagnozed);

                    if (unDiagnozedSample != null)
                    {
                        Console.WriteLine($"CONNECT {unDiagnozedSample.SampleId}");
                        break;
                    }

                    var tooLarge = CurrentGameState.PlayersSamples.FirstOrDefault(i => i.TotalCost > 10);

                    if (tooLarge != null)
                    {
                        Console.WriteLine($"CONNECT {tooLarge.SampleId}");
                        break;
                    }

                    var notEnoughMolecules = CurrentGameState.PlayersSamples
                        .FirstOrDefault(i => i.Costs.All(cost => cost.Value > CurrentGameState.AvailableMolecules[cost.Key]));
                    if (notEnoughMolecules != null)
                    {
                        Console.WriteLine($"CONNECT {notEnoughMolecules.SampleId}");
                        break;
                    }

                    /*if (ownSamples.Count > 1)
                    {
                        var minHealth = ownSamples.OrderBy(i => i.Health).First();
                        Console.WriteLine($"CONNECT {minHealth.SampleId}");
                        break;
                    }*/
                    
                    Console.WriteLine("GOTO MOLECULES");
                    break;
                case "MOLECULES":
                    var ourPlayerSamples = CurrentGameState.PlayersSamples.Where(sample => sample.ShouldRequestMolecules() && !sample.CanBeResearched);

                    var samplesCost = 0;
                    //TODO
                    /*if (ourPlayerSamples.Select(i => i.CostA).Sum() - CurrentGameState.Player.StorageA > 0)
                    {
                        Console.WriteLine("CONNECT A");
                        break;
                    }
                    if (ourPlayerSamples.Select(i => i.CostB).Sum() - CurrentGameState.Player.StorageB > 0)
                    {
                        Console.WriteLine("CONNECT B");
                        break;
                    }
                    if (ourPlayerSamples.Select(i => i.CostC).Sum() - CurrentGameState.Player.StorageC > 0)
                    {
                        Console.WriteLine("CONNECT C");
                        break;
                    }
                    if (ourPlayerSamples.Select(i => i.CostD).Sum() - CurrentGameState.Player.StorageD > 0)
                    {
                        Console.WriteLine("CONNECT D");
                        break;
                    }
                    if (ourPlayerSamples.Select(i => i.CostE).Sum() - CurrentGameState.Player.StorageE > 0)
                    {
                        Console.WriteLine("CONNECT E");
                        break;
                    }*/

                    Console.WriteLine("GOTO LABORATORY");
                    break;

                case "LABORATORY":
                    if (CurrentGameState.Samples.Any(i => i.OwnByPlayer))
                    {
                        Console.WriteLine($"CONNECT {CurrentGameState.Samples.First(i => i.OwnByPlayer).SampleId}");
                        break;
                    }
                    Console.WriteLine("GOTO SAMPLES");
                    break;

            }
            
            Console.Error.WriteLine($"Current pos {CurrentGameState.Player.Target}");
        }
    }

    private static void DebugInfo(int currentLoad)
    {
        var opponentSamplesInfo = CurrentGameState.OpponentSamples.Where(i => i.Diagnozed).Select(sample =>
            $"{sample.SampleId}: rank {sample.Rank}, {string.Join(" ", sample.Costs.Values)}");
        var playerSamplesInfo = CurrentGameState.PlayersSamples.Where(i => i.Diagnozed).Select(sample =>
            $"{sample.SampleId}: rank {sample.Rank}, {string.Join(" ", sample.Costs.Values)}");
        Console.Error.WriteLine(
            $"Available molecules:\n{string.Join("\n", CurrentGameState.AvailableMolecules.Select(i => $"{i.Key}: {i.Value}"))}");
        Console.Error.WriteLine($"Opponent got:\n{string.Join("\n", opponentSamplesInfo)}");
        Console.Error.WriteLine($"Player got:\n{string.Join("\n", playerSamplesInfo)}");
        Console.Error.WriteLine(
            $"Got samples: {CurrentGameState.PlayersSamples.Count(i => i.Diagnozed)} (+{CurrentGameState.PlayersSamples.Count(i => !i.Diagnozed)} undiaznozed)");
        Console.Error.WriteLine($"Current molecular load: {currentLoad}");
    }

    private static void ReadGameState()
    {
        string[] inputs;
        CurrentGameState.Player = new Player(Console.ReadLine()?.Split(' '));
        CurrentGameState.Opponent = new Player(Console.ReadLine()?.Split(' '));
        inputs = Console.ReadLine().Split(' ');
        CurrentGameState.AvailableMolecules = new Dictionary<char, int>
        {
            ['A'] = int.Parse(inputs[0]),
            ['B'] = int.Parse(inputs[1]),
            ['C'] = int.Parse(inputs[2]),
            ['D'] = int.Parse(inputs[3]),
            ['E'] = int.Parse(inputs[4])
        };
        int sampleCount = int.Parse(Console.ReadLine());
        CurrentGameState.Samples = new List<Sample>();
        for (int i = 0; i < sampleCount; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            CurrentGameState.Samples.Add(new Sample(inputs));
        }
    }
}
