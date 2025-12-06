using Core.Shared;

namespace Tests;

public class DiscreteRangeTests(ITestOutputHelper outputHelper)
{
    [Fact]
    public void DiscreteRange_throwsException_whenMinIsBeforeMax()
    {
        Action action = () => new DiscreteRange(2, 1);
        action.ShouldThrow<ArgumentException>();
    }
    
    [Fact]
    public void DiscreteRange_doesNotThrowException_whenMinIsEqualToMax()
    {
        Action action = () => new DiscreteRange(2, 2);
        action.ShouldNotThrow();
    }

    [Theory]
    [InlineData(10, 15, 20, 25, false)]
    [InlineData(15, 20, 20, 25, true)]
    [InlineData(15, 22, 20, 25, true)]
    [InlineData(15, 25, 20, 25, true)]
    [InlineData(20, 25, 20, 25, true)]
    [InlineData(22, 25, 20, 25, true)]
    [InlineData(22, 30, 20, 25, true)]
    [InlineData(25, 30, 20, 25, true)]
    [InlineData(26, 30, 20, 25, false)]
    [InlineData(30, 35, 20, 25, false)]
    public void DiscreteRange_intersectsWith_worksAsExpected(int minA, int maxA, int minB, int maxB, bool expected)
    {
        outputHelper.WriteLine($"[{minA}-{maxA}] [{minB}-{maxB}] IntersectsWith expected: {expected}");
        var rangeA = new DiscreteRange(minA, maxA);
        var rangeB = new DiscreteRange(minB, maxB);
        rangeA.IntersectsWith(rangeB).ShouldBe(expected);
        rangeB.IntersectsWith(rangeA).ShouldBe(expected);
    }
    
    [Theory]
    [InlineData(15, 20, 20, 25, 20,20)]
    [InlineData(15, 22, 20, 25, 20,22)]
    [InlineData(15, 25, 20, 25, 20,25)]
    [InlineData(20, 25, 20, 25, 20,25)]
    [InlineData(22, 25, 20, 25, 22,25)]
    [InlineData(22, 30, 20, 25, 22,25)]
    [InlineData(25, 30, 20, 25, 25,25)]
    public void DiscreteRange_Intersection_returnsExpectedRange(int minA, int maxA, int minB, int maxB, int expectedMin, int expectedMax)
    {
        outputHelper.WriteLine($"[{minA}-{maxA}] [{minB}-{maxB}] Intersect expected: [{expectedMin}-{expectedMax}]");
        var rangeA = new DiscreteRange(minA, maxA);
        var rangeB = new DiscreteRange(minB, maxB);
        var expectedRange = new DiscreteRange(expectedMin, expectedMax);
        rangeA.Intersection(rangeB).ShouldBeEquivalentTo(expectedRange);
        rangeB.Intersection(rangeA).ShouldBeEquivalentTo(expectedRange);
    }
    
    [Theory]
    [InlineData(10, 15, 20, 25)]
    [InlineData(30, 35, 20, 25)]
    public void DiscreteRange_Intersection_returnsNull_whenRangesDoNotIntersect(int minA, int maxA, int minB, int maxB)
    {
        outputHelper.WriteLine($"Non-intersecting ranges: [{minA}-{maxA}] [{minB}-{maxB}]");
        var rangeA = new DiscreteRange(minA, maxA);
        var rangeB = new DiscreteRange(minB, maxB);
        rangeA.Intersection(rangeB).ShouldBeNull();
        rangeB.Intersection(rangeA).ShouldBeNull();
    }
    
    [Fact]
    public void DiscreteRange_Subtract_returnsOriginalRange_whenRangesDoNotIntersect()
    {
        var rangeA = new DiscreteRange(10, 15);
        var rangeB = new DiscreteRange(20, 25);
        rangeA.Subtract(rangeB).ShouldBeEquivalentTo(new List<DiscreteRange> {rangeA});
        rangeB.Subtract(rangeA).ShouldBeEquivalentTo(new List<DiscreteRange> {rangeB});
    }

    [Fact]
    public void DiscreteRange_Subtract_returnsEmpty_whenRangesAreEqual()
    {
        var rangeA = new DiscreteRange(10, 15);
        var rangeB = new DiscreteRange(10, 15);
        rangeA.Subtract(rangeB).ShouldBeEmpty();
        rangeB.Subtract(rangeA).ShouldBeEmpty();
    }
    
    [Theory]
    [ClassData(typeof(DiscreteRangeSubtractTestData))]
    public void DiscreteRange_Subtract_returnsExpectedRanges_whenRangesIntersect(DiscreteRange rangeA, DiscreteRange rangeB, List<DiscreteRange> expected)
    {
        outputHelper.WriteLine($"[{rangeA.Min} to {rangeA.Max}] Subtract [{rangeB.Min} to {rangeB.Max}]");
        expected.ForEach(x => outputHelper.WriteLine($"Expected: [{x.Min}-{x.Max}]"));
        rangeA.Subtract(rangeB).ShouldBeEquivalentTo(expected);
    }

    public class DiscreteRangeSubtractTestData : TheoryData<DiscreteRange, DiscreteRange, List<DiscreteRange>>
    {
        public DiscreteRangeSubtractTestData()
        {
            Add(new(0, 10), new(10, 20), new List<DiscreteRange> { new(0, 9) });
            Add(new(0, 10), new(-5, 5), new List<DiscreteRange> { new(6, 10) });
            Add(new(0, 10), new(0, 5), new List<DiscreteRange> { new(6, 10) });
            Add(new(0, 10), new(7, 10), new List<DiscreteRange> { new(0, 6) });
            Add(new(0, 10), new(7, 15), new List<DiscreteRange> { new(0, 6) });
            Add(new(0, 10), new(2, 7), new List<DiscreteRange> { new(0, 1), new(8, 10) });
        }
    }

    [Theory]
    [InlineData(0, 10, 0, true)]
    [InlineData(0, 10, 5, true)]
    [InlineData(0, 10, 10, true)]
    [InlineData(0, 10, -1, false)]
    [InlineData(0, 10, 11, false)]
    public void DiscreteRange_Contains_worksAsExpected(int min, int max, int value, bool expected)
    {
        outputHelper.WriteLine($"[{min}-{max}].Contains({value}) expected: {expected}");
        var range = new DiscreteRange(min, max);
        range.Contains(value).ShouldBe(expected);
    }

}
