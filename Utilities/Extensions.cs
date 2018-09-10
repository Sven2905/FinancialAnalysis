using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class Extensions
    {
        public static ObservableCollection<T> ToOberservableCollection<T>(this IEnumerable<T> items)
        {
            ObservableCollection<T> newCollection = new ObservableCollection<T>();
            foreach (T item in items)
            {
                newCollection.Add(item);
            }

            return newCollection;
        }

        public static void AddRange<T>(this ObservableCollection<T> destination,
                                   IEnumerable<T> source)
        {
            foreach (T item in source)
            {
                destination.Add(item);
            }
        }
    }
}
