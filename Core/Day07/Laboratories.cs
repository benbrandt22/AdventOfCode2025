using System.Diagnostics;
using Core.Shared;
using Core.Shared.Extensions;
using Core.Shared.Modules;

namespace Core.Day07;

public class Laboratories : BaseDayModule
{
    public Laboratories(ITestOutputHelper outputHelper) : base(outputHelper) { }

    public override int Day => 7;

    public override string Title => "Laboratories";

    [Fact] public void Part1_Sample() => ExecutePart1(GetData(InputType.Sample)).ShouldBe(21);
    [Fact] public void Part1() => ExecutePart1(GetData(InputType.Input));

    [Fact] public void Part2_Sample() => ExecutePart2(GetData(InputType.Sample)).ShouldBe(40);
    [Fact] public void Part2() => ExecutePart2(GetData(InputType.Input));

    public long ExecutePart1(string data)
    {
        var (grid, start) = LoadGridAndStart(data);

        var result = PropagateBeams(grid, start);

        var solution = result.Splits;
        WriteLine($"Solution: {solution}");
        return solution;
    }

    public long ExecutePart2(string data)
    {
        var (grid, start) = LoadGridAndStart(data);

        var result = PropagateBeams(grid, start);

        var solution = result.Timelines;
        WriteLine($"Solution: {solution}");
        return solution;
    }

    private (Grid<char>, GridCoordinate) LoadGridAndStart(string input)
    {
        var grid = new Grid<char>(input.ToGrid(removeEmptyLines: true));
        var start = grid.AllCoordinates().First(c => grid[c] == 'S');
        return (grid, start);
    }

    private (long Splits, long Timelines) PropagateBeams(Grid<char> grid, GridCoordinate start)
    {
        var beamEndpoints = new List<TimelineCoordinate>() { new TimelineCoordinate(start, 1) };
        var splitCount = 0;

        while (true)
        {
            beamEndpoints = beamEndpoints
                .SelectMany(coord => PropagateDown(grid, coord, () => splitCount++))
                .ToList();

            beamEndpoints = beamEndpoints.GroupBy(tc => tc.Coordinate)
                .Select(grp =>
                {
                    return new TimelineCoordinate(grp.Key, grp.Sum(tc => tc.Timelines));
                })
                .ToList();

            // at the bottom?
            if (beamEndpoints.Any(x => x.Coordinate.Row == grid.RowCount - 1))
            {
                break;
            }
        }

        var timelines = beamEndpoints.Sum(tc => tc.Timelines);

        return (splitCount, timelines);
    }

    private List<TimelineCoordinate> PropagateDown(Grid<char> grid, TimelineCoordinate timeCoord, Action splitAction)
    {
        var nextValueDown = grid[timeCoord.Coordinate.Move(GridDirection.Down)];
        if (nextValueDown == '^')
        {
            splitAction.Invoke();
            return [
                new TimelineCoordinate(timeCoord.Coordinate.Move(GridDirection.Down).Move(GridDirection.Left), timeCoord.Timelines),
                new TimelineCoordinate(timeCoord.Coordinate.Move(GridDirection.Down).Move(GridDirection.Right), timeCoord.Timelines)
            ];
        }
        else
        {
            return [ new TimelineCoordinate(timeCoord.Coordinate.Move(GridDirection.Down), timeCoord.Timelines) ];
        }
    }

    private record TimelineCoordinate(GridCoordinate Coordinate, long Timelines);

}