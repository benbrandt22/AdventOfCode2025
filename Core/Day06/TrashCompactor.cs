using System.Diagnostics;
using Core.Shared;
using Core.Shared.Extensions;
using Core.Shared.Modules;

namespace Core.Day06;

public class TrashCompactor : BaseDayModule
{
    public TrashCompactor(ITestOutputHelper outputHelper) : base(outputHelper) { }
    
    public override int Day => 6;

    public override string Title => "Trash Compactor";

    [Fact] public void Part1_Sample() => ExecutePart1(GetData(InputType.Sample)).ShouldBe(4277556);
    [Fact] public void Part1() => ExecutePart1(GetData(InputType.Input));

    [Fact(Skip = "Not yet implemented")] public void Part2_Sample() => ExecutePart2(GetData(InputType.Sample)).ShouldBe(-1);
    [Fact(Skip = "Not yet implemented")] public void Part2() => ExecutePart2(GetData(InputType.Input));

    public long ExecutePart1(string data)
    {
        var (rows, operators) = LoadWorksheet(data);


        var problemSolutions = new List<long>();

        for (int i = 0; i < operators.Count; i++)
        {
            var problemNumbers = rows.Select(r => r[i]).ToList();
            
            var problemSolution = operators[i] switch
            {
                "+" => problemNumbers.Sum(),
                "*" => problemNumbers.Aggregate(1L, (acc, val) => acc * val),
                _ => throw new InvalidOperationException($"Unknown operator {operators[i]}"),
            };

            problemSolutions.Add(problemSolution);
        }


        var solution = problemSolutions.Sum();
        WriteLine($"Solution: {solution}");
        return solution;
    }
    
    public long ExecutePart2(string data)
    {
        var solution = 0;
        WriteLine($"Solution: {solution}");
        return solution;
    }

    private (List<List<long>> rows, List<string> operators) LoadWorksheet(string data)
    {
        var lines = data.ToLines(removeEmptyLines: true);
        var rows = new List<List<long>>();
        var operators = new List<string>();

        for(int i = 0; i < (lines.Count-1); i++)
        {
            var row = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToList();
            rows.Add(row);
        }

        operators = lines.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

        return (rows, operators);
    }

}

