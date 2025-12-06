using Core.Shared;
using Core.Shared.Extensions;
using Core.Shared.Modules;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Core.Day05;

public class Cafeteria : BaseDayModule
{
    public Cafeteria(ITestOutputHelper outputHelper) : base(outputHelper) { }
    
    public override int Day => 5;

    public override string Title => "Cafeteria";

    [Fact] public void Part1_Sample() => ExecutePart1(GetData(InputType.Sample)).ShouldBe(3);
    [Fact] public void Part1() => ExecutePart1(GetData(InputType.Input));

    [Fact] public void Part2_Sample() => ExecutePart2(GetData(InputType.Sample)).ShouldBe(14);
    [Fact] public void Part2() => ExecutePart2(GetData(InputType.Input));

    public long ExecutePart1(string data)
    {
        var (ranges, ids) = LoadData(data);

        var idsInAnyRange = ids.Where(id => ranges.Any(r => r.Contains(id))).ToList();

        var solution = idsInAnyRange.Count;
        WriteLine($"Solution: {solution}");
        return solution;
    }
    
    public long ExecutePart2(string data)
    {
        var (ranges, ids) = LoadData(data);

        WriteLine("RANGES");
        ranges.ForEach(r => WriteLine(r.Description));

        var unionedRanges = Union(ranges).ToList();

        WriteHorizontalRule();
        WriteLine("UNIONED RANGES");
        unionedRanges.ForEach(r => WriteLine(r.Description));

        var solution = unionedRanges.Sum(r => r.ValueCount);
        WriteLine($"Solution: {solution}");
        return solution;
    }

    public (List<DiscreteRange> Ranges, List<long> Ids) LoadData(string data)
    {
        var sections = data.ToParagraphs();
        var rangeRegEx = new Regex(@"^(?<Min>\d+)-(?<Max>\d+)", RegexOptions.Multiline);
        var ranges = rangeRegEx.Matches(sections[0]).MapEachTo<DiscreteRange>().ToList();
        var ids = sections[1].ToLines(removeEmptyLines: true).Select(long.Parse).ToList();
        return (ranges, ids);
    }

    public DiscreteRange Union(DiscreteRange a, DiscreteRange b)
    {
        if (!a.IntersectsWith(b))
        {
            throw new Exception("Ranges do not intersect");
        }
        var min = Math.Min(a.Min, b.Min);
        var max = Math.Max(a.Max, b.Max);
        return new DiscreteRange(min, max);
    }

    public List<DiscreteRange> Union(List<DiscreteRange> ranges)
    {

        bool didMerge = false;
        var unionedRanges = new List<DiscreteRange>();

        while (true)
        {
            if(ranges.Count == 0) { break; }

            var currentRange = ranges[0];
            ranges.RemoveAt(0);

            // step backward through the other ranges to find intersections
            for (int i = (ranges.Count-1); i >= 0; i--)
            {
                if (ranges[i].IntersectsWith(currentRange))
                {
                    currentRange = Union(currentRange, ranges[i]);
                    didMerge = true;
                    ranges.RemoveAt(i);
                }
            }

            unionedRanges.Add(currentRange);
        }

        if (didMerge)
        {
            // recursively union again until no more merges occur
            return Union(unionedRanges);
        }
        else
        {
            return unionedRanges;
        }

    }

}

