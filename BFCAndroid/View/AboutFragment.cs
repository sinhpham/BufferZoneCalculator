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
using Android.Support.V4.App;
using ActionbarSherlock.App;
using Com.Slidingmenu.Lib.App;

namespace BFCAndroid.View
{
    public class AboutFragment : SherlockFragment
    {
        public override void OnCreate(Bundle p0)
        {
            SetHasOptionsMenu(true);
            base.OnCreate(p0);
        }

        public override Android.Views.View OnCreateView(LayoutInflater p0, ViewGroup p1, Bundle p2)
        {
            var ab = SherlockActivity.SupportActionBar;
            ab.NavigationMode = ActionBar.NavigationModeStandard;
            ab.SetDisplayShowTitleEnabled(true);
            Activity.Title = "About";
            return p0.Inflate(Resource.Layout.AboutFragment, p1, false);
        }

        public override void OnCreateOptionsMenu(ActionbarSherlock.View.IMenu p0, ActionbarSherlock.View.MenuInflater p1)
        {
            p0.Clear();
            base.OnCreateOptionsMenu(p0, p1);
        }

        public override bool OnOptionsItemSelected(ActionbarSherlock.View.IMenuItem p0)
        {
            var text = p0.TitleFormatted.ToString();
            if (text == Activity.Title)
            {
                ((SlidingFragmentActivity)Activity).SlidingMenu.ShowMenu();
                return true;
            }
            return base.OnOptionsItemSelected(p0);
        }
    }
}