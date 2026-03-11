using System.Text.RegularExpressions;
using System.Globalization;
using System.Linq;

namespace WMS.Application.Utilities
{
    /// <summary>
    /// Provides global utility methods for data validation, sanitization, and string formatting.
    /// </summary>
    public static class DataValidatorHelper
    {
        // --- Section 1: Cleaning & Formatting ---

        /// <summary>
        /// Trims leading/trailing whitespaces, collapses internal multiple spaces into a single space, 
        /// and converts the string to Pascal Case (Title Case).
        /// <example>Example: "  suAdi  ArABIA  " -> "Saudi Arabia"</example>
        /// </summary>
        /// <param name="input">The string to clean and format.</param>
        /// <returns>A formatted Pascal Case string.</returns>
        public static string ToCleanPascalCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input?.Trim();

            // Replace multiple spaces with a single space
            var cleaned = Regex.Replace(input.Trim(), @"\s+", " ");

            // Convert to Title Case
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cleaned.ToLower());
        }

        /// <summary>
        /// Removes all special characters and symbols from the string, keeping only letters, numbers, and spaces.
        /// </summary>
        /// <param name="input">The raw input string.</param>
        /// <returns>A sanitized string containing only alphanumeric characters and spaces.</returns>
        public static string Sanitize(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            // \p{L} matches any letter, \p{N} matches any number
            return Regex.Replace(input, @"[^\p{L}\p{N} ]", "");
        }


        // --- Section 2: Validation Methods ---

        private static readonly Regex EngAlphaNumRegex = new Regex(@"^[a-zA-Z0-9]+$", RegexOptions.Compiled);
        private static readonly Regex EngOnlyRegex = new Regex(@"^[a-zA-Z]+$", RegexOptions.Compiled);
        private static readonly Regex ArabOnlyRegex = new Regex(@"^[\u0600-\u06FF\s]+$", RegexOptions.Compiled);

        /// <summary>
        /// Checks if the string contains only English letters and numbers (No spaces or special characters).
        /// </summary>
        public static bool IsEnglishAlphanumeric(this string input) =>
            !string.IsNullOrEmpty(input) && EngAlphaNumRegex.IsMatch(input);

        /// <summary>
        /// Checks if the string contains only English letters (No numbers, spaces, or special characters).
        /// </summary>
        public static bool IsEnglishOnly(this string input) =>
            !string.IsNullOrEmpty(input) && EngOnlyRegex.IsMatch(input);

        /// <summary>
        /// Checks if the string contains only Arabic characters and spaces.
        /// </summary>
        public static bool IsArabicOnly(this string input) =>
            !string.IsNullOrEmpty(input) && ArabOnlyRegex.IsMatch(input);

        /// <summary>
        /// Checks if the string consists entirely of numeric digits.
        /// </summary>
        public static bool IsNumericOnly(this string input) =>
            !string.IsNullOrEmpty(input) && input.All(char.IsDigit);

        /// <summary>
        /// Checks if all characters in the string belong to the basic ASCII character set (0-127).
        /// </summary>
        public static bool IsAsciiOnly(this string input) =>
            !string.IsNullOrEmpty(input) && input.All(c => c <= 127);
    }
}