using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DiaporamaPlayer
{
    internal class LayoutRegistrer<T> where T: FrameworkElement
    {
        private readonly Dictionary<string, List<T>> registeredElements;
        private readonly int maxElementPerLayout;

        internal LayoutRegistrer(int maxElementPerLayout)
        {
            this.maxElementPerLayout = maxElementPerLayout;
            this.registeredElements = new Dictionary<string, List<T>>();
        }

        internal T? RegisterNewAndGetOldestElement(T newElement, FinalLayout layout)
        {
            string key = GetDicKey(newElement, layout);

            InsertInDic(newElement, key);

            return registeredElements[key].Count > maxElementPerLayout ? 
                GetOldestDicEntry(key) : 
                null;
        }

        private T GetOldestDicEntry(string key)
        {
            var polaroidToRemove = registeredElements[key].ElementAt(0);
            registeredElements[key].Remove(polaroidToRemove);

            return polaroidToRemove;
        }

        private void InsertInDic(T newElement, string key)
        {
            if (!registeredElements.ContainsKey(key))
            {
                registeredElements.Add(key, new List<T>());
            }

            registeredElements[key].Add(newElement);
        }

        private static string GetDicKey(FrameworkElement entry, FinalLayout layout)
        {
            var orientation = entry.ActualHeight > entry.ActualWidth ? Orientation.Portrait : Orientation.Landscape;
            return $"{orientation}-{layout}";
        }
    }
}
