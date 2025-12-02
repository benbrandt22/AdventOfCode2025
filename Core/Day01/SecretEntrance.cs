using Core.Shared;
using Core.Shared.Extensions;
using Core.Shared.Modules;
using System.Text.RegularExpressions;

namespace Core.Day01;

public class SecretEntrance : BaseDayModule
{
    public SecretEntrance(ITestOutputHelper outputHelper) : base(outputHelper) { }
    
    public override int Day => 1;

    public override string Title => "Secret Entrance";

    [Fact] public void Part1_Sample() => ExecutePart1(GetData(InputType.Sample)).ShouldBe(3);
    [Fact] public void Part1() => ExecutePart1(GetData(InputType.Input));

    [Fact] public void Part2_Sample() => ExecutePart2(GetData(InputType.Sample)).ShouldBe(6);
    [Fact] public void Part2() => ExecutePart2(GetData(InputType.Input));

    public long ExecutePart1(string data)
    {
        var movements = LoadMovements(data);

        var dial = new SafeDial(100, 50);
        var zeroCount = 0;
        foreach (var movement in movements)
        {
            dial.Turn(movement);
            if(dial.Position == 0)
            {
                zeroCount++;
            }
        }

        var solution = zeroCount;
        WriteLine($"Solution: {solution}");
        return solution;
    }
    
    public long ExecutePart2(string data)
    {
        // admittedly this is a bit lazy to brute force it like this, but we'll give it a try and optimize if necessary
        var expandedMovements = LoadMovements(data)
            .SelectMany(m => Enumerable.Range(0, m.Distance).Select(_ => new DialMovement(m.Direction, 1)));

        var dial = new SafeDial(100, 50);

        var zeroCount = 0;
        foreach (var movement in expandedMovements)
        {
            dial.Turn(movement);
            if (dial.Position == 0)
            {
                zeroCount++;
            }
        }

        var solution = zeroCount;
        WriteLine($"Solution: {solution}");
        return solution;
    }

    public List<DialMovement> LoadMovements(string data)
    {
        var pattern = new Regex(@"^(?<Direction>[LR])(?<Distance>\d+)", RegexOptions.Multiline);
        var movements = pattern.Matches(data).MapEachTo<DialMovement>();
        return movements.ToList();
    }

    public record DialMovement(DialDirection Direction, int Distance);

    public enum DialDirection { L, R }
}

public class SafeDial
{
    /// <summary>
    /// SafeDial constructor
    /// </summary>
    /// <param name="dialSize">Total number of positions. Use 100 for a dial of 0 through 99</param>
    /// <param name="startPosition">Value of starting position</param>
    public SafeDial(int dialSize, int startPosition)
    {
        _dialSize = dialSize;
        Position = startPosition;
    }

    private int _dialSize;
    public int Position { get; private set; }

    public void Turn(SecretEntrance.DialMovement movement)
    {
        int movementDelta = movement.Direction == SecretEntrance.DialDirection.L ? -movement.Distance : movement.Distance;
        // Normalize using modulo; the double-mod ensures no negatives
        int newValue = ((Position + movementDelta) % _dialSize + _dialSize) % _dialSize;

        Position = newValue;
    }
}