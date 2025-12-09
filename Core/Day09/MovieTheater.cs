using System.Diagnostics;
using Core.Shared;
using Core.Shared.Extensions;
using Core.Shared.Modules;

namespace Core.Day09;

public class MovieTheater : BaseDayModule
{
    public MovieTheater(ITestOutputHelper outputHelper) : base(outputHelper) { }
    
    public override int Day => 9;

    public override string Title => "Movie Theater";

    [Fact] public void Part1_Sample() => ExecutePart1(GetData(InputType.Sample)).ShouldBe(50);
    [Fact] public void Part1() => ExecutePart1(GetData(InputType.Input));

    [Fact(Skip = "Not yet implemented")] public void Part2_Sample() => ExecutePart2(GetData(InputType.Sample)).ShouldBe(-1);
    [Fact(Skip = "Not yet implemented")] public void Part2() => ExecutePart2(GetData(InputType.Input));

    public long ExecutePart1(string data)
    {
        var tileCoordinates = LoadTiles(data);

        var maxArea = tileCoordinates
            .GetAllPairs()
            .Select(pair => RectangleArea(pair.Item1, pair.Item2))
            .Max();

        var solution = maxArea;
        WriteLine($"Solution: {solution}");
        return solution;
    }
    
    public long ExecutePart2(string data)
    {
        var solution = 0;
        WriteLine($"Solution: {solution}");
        return solution;
    }

    public record TileCoordinate(long X, long Y);

    private List<TileCoordinate> LoadTiles(string data)
    {
        var coordinates = data
            .ToLines(removeEmptyLines: true)
            .Select(line => {
                var parts = line.Split(',', StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();
                return new TileCoordinate(parts[0], parts[1]);
            })
            .ToList();

        return coordinates;
    }

    private long RectangleArea(TileCoordinate a, TileCoordinate b)
    {
        var width = Math.Abs(b.X - a.X) + 1;
        var height = Math.Abs(b.Y - a.Y) + 1;
        return width * height;
    }

}

