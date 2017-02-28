using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BlackJack.ViewModel
{
    public static class Extension
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            var result = new ObservableCollection<T>();
            foreach (var item in source)
                result.Add(item);
            return result;
        }
    }
}
