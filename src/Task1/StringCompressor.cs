using System;
using System.Text;
using Task1.Resources;

namespace Task1
{
    public static class StringCompressor
    {
        /// <summary>
        /// Compresses a string of lowercase Latin letters by replacing consecutive repeating characters with the character and count.
        /// Single characters are written as is without a count.
        /// </summary>
        /// <param name="input">The input string containing only lowercase Latin characters.</param>
        /// <returns>The compressed string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if input is null.</exception>
        /// <exception cref="ArgumentException">Thrown if input contains non-lowercase Latin characters.</exception>
        public static string Compress(string input)
        {
            ArgumentNullException.ThrowIfNull(input);

            if(input.Length == 0)
            {
                return string.Empty;
            }

            var result = new StringBuilder();
            var length = input.Length;

            for(int i = 0; i < length; i++)
            {
                char current = input[i];

                if(current < 'a' || current > 'z')
                {
                    throw new ArgumentException(Messages.Error_NonLatinLower, nameof(input));
                }

                int count = 1;
                while(i + 1 < length && input[i + 1] == current)
                {
                    count++;
                    i++;
                }

                result.Append(current);
                if(count > 1)
                {
                    result.Append(count);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Decompresses a string compressed with the Compress method back to the original lowercase Latin string.
        /// </summary>
        /// <param name="input">The compressed string.</param>
        /// <returns>The decompressed string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if input is null.</exception>
        /// <exception cref="ArgumentException">Thrown if input is not in a valid compressed format.</exception>
        public static string Decompress(string input)
        {
            ArgumentNullException.ThrowIfNull(input);

            if(input.Length == 0)
            {
                return string.Empty;
            }

            var result = new StringBuilder();
            var length = input.Length;
            int i = 0;

            while(i < length)
            {
                char current = input[i++];

                if(current < 'a' || current > 'z')
                {
                    throw new ArgumentException(Messages.Error_InvalidDecompressChar, nameof(input));
                }

                if(i < length && char.IsAsciiDigit(input[i]))
                {
                    if(input[i] == '0')
                    {
                        throw new ArgumentException(Messages.Error_LeadingZero, nameof(input));
                    }

                    int digitStart = i;
                    while(i < length && char.IsAsciiDigit(input[i]))
                    {
                        i++;
                    }

                    int count = int.Parse(input.AsSpan(digitStart, i - digitStart));

                    if(count < 2)
                    {
                        throw new ArgumentException(Messages.Error_CountTooSmall, nameof(input));
                    }

                    result.Append(current, count);
                }
                else
                {
                    result.Append(current);
                }
            }

            return result.ToString();
        }
    }
}
