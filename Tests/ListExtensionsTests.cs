using Core.Shared.Extensions;

namespace Tests
{
    public class ListExtensionsTests
    {

        [Fact]
        public void ExtractItems_RemovesMatchingItemsAndReturnsThem()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3, 4, 5, 6 };
            // Act
            list.ExtractItems(x => x % 2 == 0, out var evenNumbers);
            // Assert
            evenNumbers.ShouldBe(new List<int> { 2, 4, 6 });
            list.ShouldBe(new List<int> { 1, 3, 5 });
        }

        [Fact]
        public void ExtractItems_WithNoMatchingItems_ReturnsEmptyList()
        {
            // Arrange
            var list = new List<int> { 1, 2, 3 };
            // Act
            list.ExtractItems(x => x < 0, out var negativeNumbers);
            // Assert
            negativeNumbers.ShouldBeEmpty();
            list.ShouldBe(new List<int> { 1, 2, 3 });
        }

    }
}
