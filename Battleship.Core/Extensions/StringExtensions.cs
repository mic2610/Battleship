using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Battleship.Core.Extensions
{
    /// <summary>
    /// General purpose extension methods for strings.
    /// </summary>
    public static class StringExtensions
    {
        private static readonly Regex kebabSanitizeRegex = new Regex("[^a-z0-9-]", RegexOptions.Compiled);

        /// <summary>
        /// Converts the first character of the given string to lower case.
        /// </summary>
        public static string ToCamelCase(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                return char.ToLower(value[0]) + value.Substring(1);

            return value;
        }

        /// <summary>
        /// Replaces spaces in the given string with hyphen characters '-', and optionally removes non-alphanumeric characters.
        /// </summary>
        public static string ToKebabCase(this string value, bool alphaNumericOnly = false)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            value = value.ToLowerInvariant().Replace(" ", "-");
            return alphaNumericOnly ? kebabSanitizeRegex.Replace(value, string.Empty) : value;
        }

        /// <summary>
        /// Simple a short-hand way to call string.format with parameters. Replaces the format item in a specified string with
        /// the string representation of a corresponding object in a specified array.
        /// </summary>
        public static string With(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// If this string is not null or empty, return it, otherwise return the alternative.
        /// </summary>
        public static string Otherwise(this string value, string alternative)
        {
            return string.IsNullOrEmpty(value)
                ? alternative
                : value;
        }

        /// <summary>
        /// Truncate the end of the string if length is greater than the given max length.
        /// </summary>
        public static string Truncate(this string value, int maxLength)
        {
            return string.IsNullOrEmpty(value) || value.Length <= maxLength
                ? value
                : value.Substring(0, maxLength);
        }

        /// <summary>
        /// Tries to retrieve a substring of the given string, but returns null if the startIndex or count. 
        /// falls outside the bounds of the string.
        /// </summary>
        public static string SafeSubstring(this string value, int startIndex, int count)
        {
            if (string.IsNullOrWhiteSpace(value) || startIndex < 0 || startIndex >= value.Length)
                return value;

            var remainingStringLength = value.Length - startIndex;
            return value.Substring(startIndex, Math.Min(remainingStringLength, count));
        }

        /// <summary>
        /// Tries to split the given string using the specified separator, but returns null if the string is null or empty.
        /// </summary>
        public static string[] SafeSplit(this string value, string[] separator, StringSplitOptions stringSplitOptions = StringSplitOptions.RemoveEmptyEntries)
        {
            if (string.IsNullOrWhiteSpace(value) || separator.Length < 0 || separator.Length >= value.Length)
                return null;

            var values = value.Split(separator, value.Length, stringSplitOptions);
            return !values.IsNullOrEmpty() ? values : null;
        }

        /// <summary>
        /// Remove any numbers from the end of the given string.
        /// </summary>
        public static string TrimEndNumbers(this string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                for (var i = value.Length - 1; i >= 0; i--)
                {
                    if (!char.IsDigit(value[i]))
                        return i == value.Length - 1 ? value : value.Substring(0, i);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets a string with all words starting with a capital letter.
        /// </summary>
        public static string ToTitleCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var array = value.ToCharArray();

            // Scan through the letters, convert characters to uppercase when following spaces or hyphens.
            for (var i = 0; i < array.Length; i++)
            {
                if ((i == 0 || array[i - 1] == ' ' || array[i - 1] == '-') && char.IsLower(array[i]))
                    array[i] = char.ToUpper(array[i]);
            }

            return new string(array);
        }

        /// <summary>
        /// Insert spaces before capital characters in the givben string.
        /// </summary>
        public static string ToSpaced(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            var newText = new StringBuilder(name.Length * 2);
            newText.Append(name[0]);

            for (int i = 1; i < name.Length; i++)
            {
                if (char.IsUpper(name[i]) && i < name.Length - 1 && !char.IsUpper(name[i + 1]))
                    newText.Append(' ');

                newText.Append(name[i]);
            }

            return newText.ToString();
        }

        /// <summary>
        /// Gets the first characters of each word in a string.
        /// </summary>
        public static string ToAcronym(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return string.Join(
                string.Empty,
                value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s[0]));
        }

        /// <summary>
        /// Gets the index of the first numerical character.
        /// </summary>
        public static int IndexOfNumber(this string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (char.IsNumber(value[i]))
                        return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Tries to remove the specified part of the given string, but returns the original string if unable to due
        /// to startIndex or count being oustide the bounds of the string value.
        /// </summary>
        public static string SafeRemove(this string value, int startIndex, int count)
        {
            return string.IsNullOrWhiteSpace(value) || startIndex < 0 || startIndex + count > value.Length - 1
                ? value
                : value.Remove(startIndex, count);
        }

        /// <summary>
        /// Remove the specified string from the start of the value.
        /// </summary>
        public static string RemoveStart(this string value, string startString)
        {
            return value.StartsWith(startString) ? value.Remove(0, startString.Length) : value;
        }

        /// <summary>
        /// Removes words in given order from the start of the string and also trimming any whitespaces in between.
        /// </summary>
        public static string RemoveStart(this string value, params string[] starts)
        {
            if (string.IsNullOrEmpty(value) || starts.IsNullOrEmpty())
                return value?.TrimStart();

            int i = SkipWhiteSpace(value);
            foreach (var start in starts)
            {
                if (i + start.Length <= value.Length)
                    i += value.IndexOf(start, i, start.Length, StringComparison.OrdinalIgnoreCase) >= 0 ? start.Length : 0;
                i = SkipWhiteSpace(value, i);
            }

            return value.Substring(i);
        }

        /// <summary>
        /// Determines whether the beginning of the string value matches any of the specified strings.
        /// </summary>
        public static bool StartsWith(this string value, string[] starts, StringComparison comparison = StringComparison.CurrentCulture)
        {
            return starts.Any(start => value.StartsWith(start, comparison));
        }

        /// <summary>
        /// Determines whether any of the words in the string sentence match the specified start.
        /// </summary>
        public static bool WordsStartWith(this string value, string start, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            if (value == null)
                return false;

            if (start == null || value.StartsWith(start, comparisonType))
                return true;

            var words = value.Split();
            return words.Any(t => t.StartsWith(start, comparisonType));
        }

        /// <summary>
        /// Parse a string to a nullable datetime object
        /// </summary>
        public static DateTime? ToDateTime(this string dateTime, DateTimeFormatInfo dateTimeFormatInfo = null, DateTimeStyles dateTimeStyles = DateTimeStyles.None)
        {
            return DateTime.TryParse(dateTime, dateTimeFormatInfo ?? DateTimeFormatInfo.CurrentInfo, dateTimeStyles, out DateTime result)
                ? result
                : default(DateTime?);
        }

        /// <summary>
        /// Removes the special characters from the given string
        /// </summary>
        public static string RemoveSpecialCharacters(this string input, params char[] allowedCharacters)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            char[] buffer = new char[input.Length];
            int index = 0;

            foreach (char c in input.Where(c => char.IsLetterOrDigit(c) || allowedCharacters.Any(x => x == c)))
            {
                buffer[index++] = c;
            }

            return new string(buffer, 0, index);
        }

        /// <summary>
        /// Returns the zero-based index of the first non-whitespace character occuring within the given string, starting from startIndex.
        /// </summary>
        private static int SkipWhiteSpace(string source, int startIndex = 0)
        {
            if (startIndex < 0 || startIndex >= source.Length)
                return 0;

            while (char.IsWhiteSpace(source[startIndex]))
                startIndex++;

            return startIndex;
        }
    }
}
