using System.Diagnostics;
using System.Security.Cryptography;
using Core.Shared;
using Core.Shared.Extensions;
using Core.Shared.Modules;

namespace Core.Day08;

public class Playground : BaseDayModule
{
    public Playground(ITestOutputHelper outputHelper) : base(outputHelper) { }
    
    public override int Day => 8;

    public override string Title => "Playground";

    [Fact] public void Part1_Sample() => ExecutePart1(GetData(InputType.Sample)).ShouldBe(40);
    [Fact] public void Part1() => ExecutePart1(GetData(InputType.Input));

    [Fact(Skip = "Not yet implemented")] public void Part2_Sample() => ExecutePart2(GetData(InputType.Sample)).ShouldBe(-1);
    [Fact(Skip = "Not yet implemented")] public void Part2() => ExecutePart2(GetData(InputType.Input));

    public long ExecutePart1(string data)
    {
        var points = LoadJunctionPoints(data);
        var pairs = PairWithClosestPoints(points);

        var solution = 0;
        WriteLine($"Solution: {solution}");
        return solution;
    }

    public long ExecutePart2(string data)
    {
        var solution = 0;
        WriteLine($"Solution: {solution}");
        return solution;
    }

    private List<Point3d> LoadJunctionPoints(string data)
    {
        return data
            .ToLines()
            .Select(line =>
            {
                var nums = line.Split(',').Select(long.Parse).ToArray();
                return new Point3d(nums[0], nums[1], nums[2]);
            })
            .ToList();
    }

    [DebuggerDisplay("({X}, {Y}, {Z})")]
    public record Point3d(long X, long Y, long Z)
    {
        public float DistanceTo(Point3d other)
        {
            var dx = X - other.X;
            var dy = Y - other.Y;
            var dz = Z - other.Z;
            return MathF.Sqrt((dx * dx) + (dy * dy) + (dz * dz));
        }
    }

    [DebuggerDisplay("{P1} | {P2} | {Distance}")]
    public class PairedPoints
    {
        public PairedPoints(Point3d p1, Point3d p2)
        {
            P1 = p1;
            P2 = p2;
            Distance = p1.DistanceTo(p2);
        }

        public Point3d P1 { get; }
        public Point3d P2 { get; }
        public float Distance { get; }
    }

    public List<PairedPoints> PairWithClosestPoints(List<Point3d> points)
    {
        var output = new List<PairedPoints>();
        while (true)
        {
            var p1 = points[0];

            // sort by distance from first point. First should be 0, second should be its closest neighbor
            points = points.OrderBy(p => p.DistanceTo(p1)).ToList();

            output.Add(new PairedPoints(points[0], points[1]));

            points.RemoveAt(1);
            points.RemoveAt(0);

            if(points.Count == 0) { break; }
        }
        return output.OrderBy(x => x.Distance).ToList();
    }
}

