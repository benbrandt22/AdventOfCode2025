using System.Diagnostics;
using Core.Shared;
using Core.Shared.Extensions;
using Core.Shared.Modules;

namespace Core.Day04;

public class PrintingDepartment : BaseDayModule
{
    public PrintingDepartment(ITestOutputHelper outputHelper) : base(outputHelper) { }
    
    public override int Day => 4;

    public override string Title => "Printing Department";

    [Fact] public void Part1_Sample() => ExecutePart1(GetData(InputType.Sample)).ShouldBe(13);
    [Fact] public void Part1() => ExecutePart1(GetData(InputType.Input));

    [Fact] public void Part2_Sample() => ExecutePart2(GetData(InputType.Sample)).ShouldBe(43);
    [Fact] public void Part2() => ExecutePart2(GetData(InputType.Input));

    public long ExecutePart1(string data)
    {
        var floorMap = LoadGrid(data);

        var solution = GetAccessiblePaperLocations(floorMap).Count();
        WriteLine($"Solution: {solution}");
        return solution;
    }
    
    public long ExecutePart2(string data)
    {
        var floorMap = LoadGrid(data);

        var removablePaperLocationCount = 0;
        while (true)
        {
            var accessiblePaperLocations = GetAccessiblePaperLocations(floorMap).ToList();
            if(accessiblePaperLocations.Count == 0)
            {
                break;
            }

            removablePaperLocationCount += accessiblePaperLocations.Count;

            RemovePaper(floorMap, accessiblePaperLocations);
        }

        var solution = removablePaperLocationCount;
        WriteLine($"Solution: {solution}");
        return solution;
    }

    private Grid<char> LoadGrid(string inputText)
    {
        var grid = new Grid<char>(inputText.ToGrid(removeEmptyLines: true));
        WriteLine($"Loaded grid of {grid.RowCount} rows and {grid.ColumnCount} columns");
        return grid;
    }

    private IEnumerable<GridCoordinate> GetAccessiblePaperLocations(Grid<char> floorMap)
    {
        bool hasPaper(GridCoordinate x) => x.IsInBounds(floorMap) && floorMap[x] == '@';

        foreach (var coord in floorMap.AllCoordinates())
        {
            if (hasPaper(coord) && GetAllNeighborCoordinates(coord).Count(hasPaper) < 4)
            {
                yield return coord;
            }
        }
    }

    private IEnumerable<GridCoordinate> GetAllNeighborCoordinates(GridCoordinate coordinate)
    {
        return GridDirection.AllDirections.Select(d => coordinate.Move(d));
    }

    private void RemovePaper(Grid<char> floorMap, IEnumerable<GridCoordinate> locationsToClear)
    {
        foreach (var coord in locationsToClear)
        {
            floorMap[coord] = '.';
        }
    }

}

