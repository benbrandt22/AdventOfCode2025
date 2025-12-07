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

    [Fact] public void Part2_Sample() => ExecutePart2(GetData(InputType.Sample)).ShouldBe(3263827);
    [Fact] public void Part2() => ExecutePart2(GetData(InputType.Input));

    public long ExecutePart1(string data)
    {
        var rawProblems = LoadWorksheetProblems(data);

        var solution = rawProblems.Sum(p => p.SolveAsHuman());
        WriteLine($"Solution: {solution}");
        return solution;
    }
    
    public long ExecutePart2(string data)
    {
        var rawProblems = LoadWorksheetProblems(data);

        var solution = rawProblems.Sum(p => p.SolveAsCephalopod());
        WriteLine($"Solution: {solution}");
        return solution;
    }

    private IEnumerable<RawMathProblem> LoadWorksheetProblems(string data)
    {
        var lines = data.ToLines(removeEmptyLines: true);

        var operatorLine = lines.Last();

        // position of each operator lines up with the start of each problem column
        var problemStartIndexes = operatorLine.AllIndexesOf("+", StringComparison.OrdinalIgnoreCase)
            .Concat(lines.Last().AllIndexesOf("*", StringComparison.OrdinalIgnoreCase))
            .Order().ToList();

        for (int problemId = 0; problemId < problemStartIndexes.Count; problemId++)
        {
            var startIndex = problemStartIndexes[problemId];
            var endIndex = (problemId + 1 < problemStartIndexes.Count)
                ? (problemStartIndexes[problemId + 1] - 2)
                : (operatorLine.Length - 1);

            var numberBlock = lines
                .Take(lines.Count - 1)
                .Select(line => line[startIndex..(endIndex + 1)])
                .ToList();

            var operatorChar = operatorLine[startIndex];

            yield return new RawMathProblem(numberBlock, operatorChar);
        }
    }

    public record RawMathProblem(List<string> NumberTextLines, char Operator)
    {
        /// <summary>
        /// Reads horizontal numbers top to bottom
        /// </summary>
        public long SolveAsHuman() {
            var humanNumbers = NumberTextLines.Select(x => long.Parse(x));
            return Solve(humanNumbers);
        }

        /// <summary>
        /// Reads vertical numbers right to left
        /// </summary>
        public long SolveAsCephalopod() {
            var columnCount = NumberTextLines.First().Length;
            var cephalopodNumbers = new List<long>();
            for (int columnIndex = (columnCount-1); columnIndex >= 0; columnIndex--)
            {
                var numberString = new string(NumberTextLines.Select(l => l[columnIndex]).ToArray());
                cephalopodNumbers.Add(long.Parse(numberString));
            }
            return Solve(cephalopodNumbers);
        }

        private long Solve(IEnumerable<long> Numbers) => Operator switch
        {
            '+' => Numbers.Sum(),
            '*' => Numbers.Aggregate(1L, (acc, val) => acc * val),
            _ => throw new InvalidOperationException($"Unknown operator {Operator}"),
        };
    };

}

