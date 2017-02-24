using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Model.Entity;

namespace BlackJack.ViewModel
{
    public static class Extension
    {
        public static void Shuffle(this ObservableCollection<CardEntity> list)
        {
            var newList = new ObservableCollection<CardEntity>();
            for (int i = 0; i < list.Count; i++)
            {
                //newList.Add()
            }
        }
    }
}
