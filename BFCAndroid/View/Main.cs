using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Com.Slidingmenu.Lib.App;
using ActionbarSherlock.App;
using System.Threading.Tasks;
using BFCCore.DataLayer;
using BFCCore.ServiceAccessLayer;
using BFCCore.BusinessLayer;

namespace BFCAndroid.View
{
    [Activity(Label = "Buffer zone calculator", MainLauncher = true)]
    public class Main : SlidingFragmentActivity, MenuFragment.ISlidingMenuAct
    {
        public override void OnCreate(Bundle p0)
        {
            base.OnCreate(p0);
            // Create your application here
            SetContentView(Resource.Layout.Main);
            SetBehindContentView(Resource.Layout.Menu);

            SupportFragmentManager.BeginTransaction()
                .Add(Resource.Id.fragment_container, new AboutFragment())
                .Commit();

            SupportFragmentManager.BeginTransaction()
                .Add(Resource.Id.menu_fragment_container, new MenuFragment())
                .Commit();

            // Setup action bar
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            // Setup the sliding menu
            SlidingMenu.SetShadowWidthRes(Resource.Dimension.SlidingMenuShadowWidth);
            SlidingMenu.SetShadowDrawable(Resource.Drawable.SlidingMenuShadow);
            SlidingMenu.SetBehindOffsetRes(Resource.Dimension.SlidingmenuOffset);
            SlidingMenu.SetFadeDegree(0.35f);
            SlidingMenu.TouchModeAbove = Com.Slidingmenu.Lib.SlidingMenu.TouchmodeFullscreen;
        }

        const string CalcFragTag = "calfragtag";

        [Export]
        public void LabelSprayClicked(Android.Views.View v)
        {
            var f = SupportFragmentManager.FindFragmentByTag(CalcFragTag);
            if (f != null)
            {
                ((CalcFragment)f).LabelSprayClicked(v);
            }
        }

        [Export]
        public void BoomHeightClicked(Android.Views.View v)
        {
            var f = SupportFragmentManager.FindFragmentByTag(CalcFragTag);
            if (f != null)
            {
                ((CalcFragment)f).BoomHeightClicked(v);
            }
        }

        [Export]
        public void WindSpeedClicked(Android.Views.View v)
        {
            var f = SupportFragmentManager.FindFragmentByTag(CalcFragTag);
            if (f != null)
            {
                ((CalcFragment)f).WindSpeedClicked(v);
            }
        }

        [Export]
        public void SprayQualityCaclClicked(Android.Views.View v)
        {
            var f = SupportFragmentManager.FindFragmentByTag(CalcFragTag);
            if (f != null)
            {
                ((CalcFragment)f).SprayQualityCaclClicked(v);
            }
        }

        [Export]
        public void SprayQualityClicked(Android.Views.View v)
        {
            var f = SupportFragmentManager.FindFragmentByTag(CalcFragTag);
            if (f != null)
            {
                ((CalcFragment)f).SprayQualityClicked(v);
            }
        }

        public void SelectedItemChanged(int position, string label)
        {
            switch (label)
            {
                case "Home":
                    {
                        var calcFrag = new CalcFragment();
                        var transaction = SupportFragmentManager.BeginTransaction();
                        transaction.Replace(Resource.Id.fragment_container, calcFrag, CalcFragTag);
                        transaction.Commit();
                        SlidingMenu.ShowContent();
                    }
                    break;
                case "Help":
                    {
                        Toast.MakeText(this.SupportActionBar.ThemedContext, "help clicked", ToastLength.Short).Show();
                    }
                    break;
                case "About":
                    {
                        var aFrag = new AboutFragment();
                        var transaction = SupportFragmentManager.BeginTransaction();
                        transaction.Replace(Resource.Id.fragment_container, aFrag, CalcFragTag);
                        transaction.Commit();
                        SlidingMenu.ShowContent();
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}