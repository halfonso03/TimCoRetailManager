using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.Library.Helpers
{
    public static class Extensions
    {
        public static void Replace2<T>(this ObservableCollection<T> collection, T OldItem, T NewItem)
        {
            var index = collection.IndexOf(OldItem);

            collection.RemoveAt(index);

            collection.Insert(index, NewItem);
        }
    }
}
