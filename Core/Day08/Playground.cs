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

    [Fact] public void Part1_Sample() => ExecutePart1(GetData(InputType.Sample), 10).ShouldBe(40);
    [Fact] public void Part1() => ExecutePart1(GetData(InputType.Input), 1000);

    [Fact] public void Part2_Sample() => ExecutePart2(GetData(InputType.Sample)).ShouldBe(25272);
    [Fact] public void Part2() => ExecutePart2(GetData(InputType.Input));

    public long ExecutePart1(string data, int numConnections)
    {
        var points = LoadJunctionPoints(data);
        var shortestPairs = FindAllDistanceOrderedPairings(points).Take(numConnections).ToList();

        var circuits = GenerateCircuits(shortestPairs);

        var solution = circuits
            .OrderByDescending(c => c.Count)
            .Take(3)
            .Select(c => c.Count)
            .Aggregate(1L, (acc, val) => acc * val);

        WriteLine($"Solution: {solution}");
        return solution;
    }

    public long ExecutePart2(string data)
    {
        var points = LoadJunctionPoints(data);
        var shortestPairs = FindAllDistanceOrderedPairings(points).ToList();

        var lastSegmentOfSingleCicuit = GenerateSingleCircuit_ReturnLastSegment(shortestPairs);

        var solution = lastSegmentOfSingleCicuit.P1.X * lastSegmentOfSingleCicuit.P2.X;
        WriteLine($"Solution: {solution}");
        return solution;
    }

    private List<Point3d> LoadJunctionPoints(string data)
    {
        return data
            .ToLines(removeEmptyLines: true)
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
    public class Segment
    {
        public Segment(Point3d p1, Point3d p2)
        {
            P1 = p1;
            P2 = p2;
            Distance = p1.DistanceTo(p2);
        }

        public Point3d P1 { get; }
        public Point3d P2 { get; }
        public float Distance { get; }
    }

    public List<Segment> FindAllDistanceOrderedPairings(List<Point3d> points)
    {
        var distanceOrderedPairings = points
            .GetAllPairs()
            .Select(pair => new Segment(pair.Item1, pair.Item2))
            .OrderBy(pair => pair.Distance)
            .ToList();
        return distanceOrderedPairings;
    }

    private void AddSegmentToCircuits(List<HashSet<Point3d>> circuits, Segment seg)
    {
        var circuitsWithThesePoints = circuits.Where(c => c.Contains(seg.P1) || c.Contains(seg.P2)).ToList();
        if (circuitsWithThesePoints.Count == 0)
        {
            var newCircuit = new HashSet<Point3d> { seg.P1, seg.P2 };
            circuits.Add(newCircuit);
        }
        else if (circuitsWithThesePoints.Count == 1)
        {
            var circuit = circuitsWithThesePoints[0];
            circuit.Add(seg.P1);
            circuit.Add(seg.P2);
        }
        else if (circuitsWithThesePoints.Count == 2)
        {
            var circuit1 = circuitsWithThesePoints[0];
            var circuit2 = circuitsWithThesePoints[1];
            circuit1.UnionWith(circuit2);
            circuits.Remove(circuit2);
        }
    }

    private List<HashSet<Point3d>> GenerateCircuits(List<Segment> shortestSegments)
    {
        var circuits = new List<HashSet<Point3d>>();

        foreach (var segment in shortestSegments)
        {
            AddSegmentToCircuits(circuits, segment);
        }

        return circuits.OrderByDescending(c => c.Count).ToList();
    }

    private Segment GenerateSingleCircuit_ReturnLastSegment(List<Segment> orderedSegmentsToConnect)
    {
        var totalPointCount = orderedSegmentsToConnect
            .SelectMany(s => new[] { s.P1, s.P2 })
            .Distinct()
            .Count();

        var circuits = new List<HashSet<Point3d>>();

        foreach (var segment in orderedSegmentsToConnect)
        {
            AddSegmentToCircuits(circuits, segment);
            if (circuits.Count == 1 && circuits[0].Count == totalPointCount)
            {
                return segment;
            }
        }

        throw new InvalidOperationException("Could not generate single circuit from provided segments.");
    }

}

