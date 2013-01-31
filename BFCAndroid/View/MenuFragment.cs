using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ActionbarSherlock.App;

namespace BFCAndroid.View
{
    public class MenuFragment : SherlockListFragment
    {
        public override Android.Views.View OnCreateView(LayoutInflater p0, ViewGroup p1, Bundle p2)
        {
            _items = new List<string> { "Home", "Help", "About" };
            var adap = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleListItem1, _items);
            ListAdapter = adap;
            return base.OnCreateView(p0, p1, p2);
        }

        List<string> _items;

        public override void OnListItemClick(ListView p0, Android.Views.View p1, int p2, long p3)
        {
            base.OnListItemClick(p0, p1, p2, p3);
            var position = p2;
            var label = _items[position];

            var act = (ISlidingMenuAct)Activity;
            act.SelectedItemChanged(position, label);
        }

        public interface ISlidingMenuAct
        {
            void SelectedItemChanged(int position, string label);
        }
    }
}