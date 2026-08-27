using System;
using Task1;
using Xunit;

namespace Task1Tests
{
    public class StringCompressorTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("a", "a")]
        [InlineData("ab", "ab")]
        [InlineData("aaabbcccdde", "a3b2c3d2e")]
        [InlineData("aabcc", "a2bc2")]
        public void Compress_ValidInput_ReturnsCompressedString(string input, string expected)
        {
            string result = StringCompressor.Compress(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Compress_NullInput_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => StringCompressor.Compress(null!));
            Assert.Contains("Value cannot be null", exception.Message);
        }

        [Theory]
        [InlineData("aB", "Input string must contain only lowercase Latin letters.")]
        [InlineData("abcD", "Input string must contain only lowercase Latin letters.")]
        [InlineData("abc1", "Input string must contain only lowercase Latin letters.")]
        [InlineData("abc!", "Input string must contain only lowercase Latin letters.")]
        [InlineData(" ", "Input string must contain only lowercase Latin letters.")]
        public void Compress_InvalidCharacters_ThrowsArgumentException(string input, string expectedMessage)
        {
            var exception = Assert.Throws<ArgumentException>(() => StringCompressor.Compress(input));
            Assert.Contains(expectedMessage, exception.Message);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("a", "a")]
        [InlineData("ab", "ab")]
        [InlineData("a3b2c3d2e", "aaabbcccdde")]
        [InlineData("a2bc2", "aabcc")]
        [InlineData("a12", "aaaaaaaaaaaa")]
        public void Decompress_ValidInput_ReturnsDecompressedString(string input, string expected)
        {
            string result = StringCompressor.Decompress(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Decompress_NullInput_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => StringCompressor.Decompress(null!));
            Assert.Contains("Value cannot be null", exception.Message);
        }

        [Theory]
        [InlineData("aB", "Character must be a lowercase Latin letter.")]
        [InlineData("a3!", "Character must be a lowercase Latin letter.")]
        [InlineData("a1", "Count must be greater than or equal to 2.")]
        [InlineData("a0", "Count cannot start with leading zero.")]
        [InlineData("a05", "Count cannot start with leading zero.")]
        [InlineData("1a", "Character must be a lowercase Latin letter.")]
        public void Decompress_InvalidCompressedFormat_ThrowsArgumentException(string input, string expectedMessage)
        {
            var exception = Assert.Throws<ArgumentException>(() => StringCompressor.Decompress(input));
            Assert.Contains(expectedMessage, exception.Message);
        }
    }
}
