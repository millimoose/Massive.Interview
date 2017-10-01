using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Massive.Interview.Entities
{
    /// <summary>
    /// Utility code for converting entities to string for debugging.
    /// </summary>
    static class EntityFormattingExtensions
    {
        /// <summary>
        /// Convert a collection of items to a string, using short representations of the items.
        /// </summary>
        public static string ToShortString<TItem>(this IEnumerable<TItem> items) where TItem : IFormattable {
            return items.ToString()
                + "{"
                + string.Join(", ", from it in items select it.ToString("g", CultureInfo.CurrentCulture))
                + "}";
        }
    }
}
