using System;
using System.Text;
using System.Globalization;

namespace Assi.SlugifyExtension.Core
{
    public static class StringExtension
    {
        /// <summary>
        /// An extension method for generating slugs from any language
        /// </summary>
        /// <param name="textToSlug">Text to convert to slug</param>
        /// <param name="maxLength">Maximum number of characters of the generated slug</param>
        /// <returns>human-readable and SEO friendly string</returns>
        public static string Slugify(this string textToSlug, int maxLength = 1000)
        {
            string stFormKD = textToSlug.ToLower().Normalize(NormalizationForm.FormKD);
            var sb = new StringBuilder();

            foreach (char t in stFormKD)
            {
                // Allowed symbols
                if (t == '-' || t == '_' || t == '~')
                {
                    sb.Append(t);
                    continue;
                }

                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(t);
                switch (uc)
                {
                    case UnicodeCategory.LowercaseLetter:
                    case UnicodeCategory.OtherLetter:
                    case UnicodeCategory.DecimalDigitNumber:
                        // Keep letters and digits
                        sb.Append(t);
                        break;
                    case UnicodeCategory.NonSpacingMark:
                        // Remove diacritics
                        break;
                    default:
                        // Replace all other chars with dash
                        sb.Append('-');
                        break;
                }
            }

            var slug = sb.ToString().Normalize(NormalizationForm.FormC);

            // Simplifies dash groups 
            for (int i = 0; i < slug.Length - 1; i++)
            {
                if (slug[i] == '-')
                {
                    int j = 0;
                    while (i + j + 1 < slug.Length && slug[i + j + 1] == '-')
                    {
                        j++;
                    }
                    if (j > 0)
                    {
                        slug = slug.Remove(i + 1, j);
                    }
                }
            }

            // Limit resultant string length to maxLength
            if (slug.Length > maxLength)
            {
                slug = slug.Substring(0, maxLength);
            }

            // Remove any leading and trailing hyphen, underscore or dot
            slug = slug.Trim('-', '_', '.');


            return slug;
        }
    }
}
