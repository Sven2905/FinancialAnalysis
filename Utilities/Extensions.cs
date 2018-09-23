using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Extensions
    {
        public static ObservableCollection<T> ToOberservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }

        public static SvenTechCollection<T> ToSvenTechCollection<T>(this IEnumerable<T> collection)
        {
            return new SvenTechCollection<T>(collection);
        }

        public static void AddRange<T>(this ObservableCollection<T> destination, IEnumerable<T> source)
        {
            foreach (T item in source)
                destination.Add(item);
        }

        public static bool In<T>(this T source, params T[] list)
        {
            if (null == source) throw new ArgumentNullException("source");
            return list.Contains(source);
        }

        public static bool IsNull(this object source)
        {
            return source == null;
        }

        public static void Raise(this EventHandler eventHandler, object sender, EventArgs e)
        {
            eventHandler?.Invoke(sender, e);
        }

        public static void Raise<T>(this EventHandler<T> eventHandler, object sender, T e) where T : EventArgs
        {
            eventHandler?.Invoke(sender, e);
        }

        public static int ToInt(this string input)
        {
            int result;
            if (!int.TryParse(input, out result))
                result = 0;
            return result;
        }

        public static decimal ToDecimal(this string input)
        {
            decimal result;
            if (!decimal.TryParse(input, out result))
                result = 0;
            return result;
        }

        public static double ToDouble(this string input)
        {
            double result;
            if (!double.TryParse(input, out result))
                result = 0;
            return result;
        }

        public static float ToFloat(this string input)
        {
            float result;
            if (!float.TryParse(input, out result))
                result = 0;
            return result;
        }

        public static int Occurrence(this String instr, string search)
        {
            return Regex.Matches(instr, search).Count;
        }

        public static T To<T>(this IConvertible value)
        {
            try
            {
                Type t = typeof(T);
                Type u = Nullable.GetUnderlyingType(t);

                if (u != null)
                {
                    if (value == null || value.Equals(""))
                        return default(T);

                    return (T)Convert.ChangeType(value, u);
                }
                else
                {
                    if (value == null || value.Equals(""))
                        return default(T);

                    return (T)Convert.ChangeType(value, t);
                }
            }

            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Calculates the sum of the given timeSpans.
        /// </summary>
        public static TimeSpan Sum(this IEnumerable<TimeSpan> timeSpans)
        {
            TimeSpan sumTillNowTimeSpan = TimeSpan.Zero;

            foreach (TimeSpan timeSpan in timeSpans)
            {
                sumTillNowTimeSpan += timeSpan;
            }

            return sumTillNowTimeSpan;
        }

        /// <summary>
        /// Checks if IEnumerable is not null and not empty.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>Returns true when IEnumerable is not null and not empty, otherwise false.</returns>
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }

        /// <summary>
        /// Clones an object with all his values without references.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns>New object with same values.</returns>
        public static T Clone<T>(this T obj)
        {
            var inst = obj.GetType().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            return (T)inst?.Invoke(obj, null);
        }
    }
}
