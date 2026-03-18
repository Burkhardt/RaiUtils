namespace RaiUtils.Tests;

public class RandomExtensionsTests
{
	[Fact]
	public void Random_ReturnsDefault_ForEmptySequence()
	{
		var items = Array.Empty<string>();

		var result = items.Random(new Random(123));

		Assert.Null(result);
	}

	[Fact]
	public void Random_ReturnsOnlyElement_ForSingleItemSequence()
	{
		var items = new[] { 42 };

		var result = items.Random(new Random(123));

		Assert.Equal(42, result);
	}

	[Fact]
	public void Shuffle_WithSeededRandom_ProducesDeterministicPermutation()
	{
		var original = Enumerable.Range(1, 8).ToList();
		var first = original.ToList();
		var second = original.ToList();

		first.Shuffle(new Random(77));
		second.Shuffle(new Random(77));

		Assert.Equal(first, second);
		Assert.Equal(original.OrderBy(x => x), first.OrderBy(x => x));
	}

	[Fact]
	public void ShuffleEnumerable_ReturnsShuffledCopy_WithoutChangingSource()
	{
		var source = new[] { 1, 2, 3, 4, 5 };

		var shuffled = ((IEnumerable<int>)source).Shuffle(new Random(7)).ToList();

		Assert.Equal(new[] { 1, 2, 3, 4, 5 }, source);
		Assert.Equal(source.OrderBy(x => x), shuffled.OrderBy(x => x));
		Assert.Equal(source.Length, shuffled.Count);
	}

	[Fact]
	public void TakeAny_ReturnsRequestedCount_AndValuesFromSource()
	{
		var source = new[] { "a", "b", "c" };

		var result = source.TakeAny(12).ToList();

		Assert.Equal(12, result.Count);
		Assert.All(result, item => Assert.Contains(item, source));
	}
}