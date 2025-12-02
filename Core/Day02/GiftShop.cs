using System.Diagnostics;
using System.Text.RegularExpressions;
using Core.Shared;
using Core.Shared.Extensions;
using Core.Shared.Modules;

namespace Core.Day02;

public class GiftShop : BaseDayModule
{
    public GiftShop(ITestOutputHelper outputHelper) : base(outputHelper) { }
    
    public override int Day => 2;

    public override string Title => "Gift Shop";

    [Fact] public void Part1_Sample() => ExecutePart1(GetData(InputType.Sample)).ShouldBe(1227775554);
    [Fact] public void Part1() => ExecutePart1(GetData(InputType.Input));

    [Fact] public void Part2_Sample() => ExecutePart2(GetData(InputType.Sample)).ShouldBe(4174379265);
    [Fact] public void Part2() => ExecutePart2(GetData(InputType.Input));

    public long ExecutePart1(string data)
    {
        var ranges = LoadRanges(data);
        var invalidIds = ranges.SelectMany(range => GetInvalidIdsInRange(range, IsInvalidIdMatchingHalves));

        var solution = invalidIds.Sum();
        WriteLine($"Solution: {solution}");
        return solution;
    }

    public long ExecutePart2(string data)
    {
        var ranges = LoadRanges(data);
        var invalidIds = ranges.SelectMany(range => GetInvalidIdsInRange(range, IsInvalidIdAnyRepeatingPattern));

        var solution = invalidIds.Sum();
        WriteLine($"Solution: {solution}");
        return solution;
    }

    public record IdRange(long Min, long Max);

    public List<IdRange> LoadRanges(string data)
    {
        var pattern = new Regex(@"(?<Min>\d+)-(?<Max>\d+)");
        return pattern.Matches(data).MapEachTo<IdRange>().ToList();
    }

    private IEnumerable<long> GetInvalidIdsInRange(IdRange range, Func<long, bool> isInvalidFunction)
    {
        for (long id = range.Min; id <= range.Max; id++)
        {
            if (isInvalidFunction(id))
            {
                yield return id;
            }
        }
    }

    /// <summary>
    /// Returns false for any ID which is made only of some sequence of digits repeated twice
    /// </summary>
    private bool IsInvalidIdMatchingHalves(long id)
    {
        var idString = id.ToString();
        if(idString.Length % 2 != 0)
        {
            return false; // odd length IDs are always valid
        }
        // even number of digits

        // return true (invalid) if first half matches second half
        if(idString.Substring(0, idString.Length / 2) == idString.Substring(idString.Length / 2))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns false for any ID which is made only of some sequence of digits repeated at least twice
    /// </summary>
    private bool IsInvalidIdAnyRepeatingPattern(long id)
    {
        var idString = id.ToString();
        
        for (int patternLength = 1; patternLength <= idString.Length / 2; patternLength++)
        {
            if (idString.Length % patternLength != 0)
            {
                continue; // pattern length must divide evenly into the ID length
            }
            var pattern = idString.Substring(0, patternLength);
            var repeatedPattern = string.Concat(Enumerable.Repeat(pattern, idString.Length / patternLength));
            if (repeatedPattern == idString)
            {
                return true; // invalid ID
            }
        }

        return false;
    }
}

