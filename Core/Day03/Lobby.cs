using Core.Shared;
using Core.Shared.Extensions;
using Core.Shared.Modules;

namespace Core.Day03;

public class Lobby : BaseDayModule
{
    public Lobby(ITestOutputHelper outputHelper) : base(outputHelper) { }
    
    public override int Day => 3;

    public override string Title => "Lobby";

    [Fact] public void Part1_Sample() => ExecutePart1(GetData(InputType.Sample)).ShouldBe(357);
    [Fact] public void Part1() => ExecutePart1(GetData(InputType.Input));

    [Fact] public void Part2_Sample() => ExecutePart2(GetData(InputType.Sample)).ShouldBe(3121910778619);
    [Fact] public void Part2() => ExecutePart2(GetData(InputType.Input));

    public long ExecutePart1(string data)
    {
        var banks = LoadBatteryBanks(data);

        var solution = banks.Sum(b => b.MaximumJoltage(2));
        WriteLine($"Solution: {solution}");
        return solution;
    }
    
    public long ExecutePart2(string data)
    {
        var banks = LoadBatteryBanks(data);

        var solution = banks.Sum(b => b.MaximumJoltage(12));
        WriteLine($"Solution: {solution}");
        return solution;
    }

    public List<BatteryBank> LoadBatteryBanks(string data)
    {
        var banks = new List<BatteryBank>();
        var lines = data.ToLines(removeEmptyLines: true);
        foreach (var line in lines)
        {
            var batteries = line.Select(c => int.Parse(c.ToString())).ToList();
            banks.Add(new BatteryBank(batteries));
        }
        return banks;
    }

    public record Indexed<T>(int Index, T Value);

    public class BatteryBank
    {
        public BatteryBank(List<int> batteries)
        {
            Batteries = batteries;
            IndexedBatteries = batteries.Select((value, index) => new Indexed<int>(index, value)).ToArray();
        }

        public List<int> Batteries { get; }

        private Indexed<int>[] IndexedBatteries;

        public long MaximumJoltage(int batteriesToTurnOn)
        {
            return GetSubJoltage(batteriesToTurnOn);
        }

        private long GetSubJoltage(int batteriesToTurnOn, int previousIndex = -1)
        {
            if(batteriesToTurnOn == 0) {  return 0; }

            // starting after the last index, find the range of possible values that it could be
            var possibleValues = IndexedBatteries[(previousIndex + 1)..(IndexedBatteries.Length - (batteriesToTurnOn - 1))];
            // find the first occurrence of the maximum value in that range
            var winner = possibleValues
                .OrderByDescending(x => x.Value)
                .ThenBy(x => x.Index)
                .First();

            var thisDigitValue = winner.Value * (long)Math.Pow(10, batteriesToTurnOn - 1);
            var nextDigitValue = GetSubJoltage(batteriesToTurnOn - 1, winner.Index);

            return thisDigitValue + nextDigitValue;
        }
    }

}

