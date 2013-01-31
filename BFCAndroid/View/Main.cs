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
    public class Main : SlidingFragmentActivity
    {
        public override void OnCreate(Bundle p0)
        {
            base.OnCreate(p0);
            // Create your application here
            SetContentView(Resource.Layout.Main);
            SetBehindContentView(Resource.Layout.Menu);

            // Setup action bar
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            // Setup the sliding menu
            SlidingMenu.SetShadowWidthRes(Resource.Dimension.SlidingMenuShadowWidth);
            SlidingMenu.SetShadowDrawable(Resource.Drawable.SlidingMenuShadow);
            SlidingMenu.SetBehindOffsetRes(Resource.Dimension.SlidingmenuOffset);
            SlidingMenu.SetFadeDegree(0.35f);
            SlidingMenu.TouchModeAbove = Com.Slidingmenu.Lib.SlidingMenu.TouchmodeFullscreen;

            // Check that the activity is using the layout version with
            // the fragment_container FrameLayout
            if (FindViewById(Resource.Id.fragment_container) != null)
            {

                // However, if we're being restored from a previous state,
                // then we don't need to do anything and should return or else
                // we could end up with overlapping fragments.

                if (p0 != null)
                {
                    return;
                }
                
                // Create an instance of ExampleFragment
                var aboutFragment = new AboutFragment();

                // In case this activity was started with special instructions from an Intent,
                // pass the Intent's extras to the fragment as arguments
                aboutFragment.Arguments = Intent.Extras;

                // Add the fragment to the 'fragment_container' FrameLayout
                SupportFragmentManager.BeginTransaction()
                        .Add(Resource.Id.fragment_container, aboutFragment)
                        .Commit();
            }
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

        [Export]
        public void StartClicked(Android.Views.View v)
        {
            var calcFrag = new CalcFragment();
            var transaction = SupportFragmentManager.BeginTransaction();

            // Replace whatever is in the fragment_container view with this fragment,
            // and add the transaction to the back stack so the user can navigate back
            transaction.Replace(Resource.Id.fragment_container, calcFrag, CalcFragTag);
            //transaction.addToBackStack(null);

            // Commit the transaction
            transaction.Commit();
            SlidingMenu.ShowContent();
        }

        [Export]
        public void HelpClicked(Android.Views.View v)
        {
            Toast.MakeText(this.SupportActionBar.ThemedContext, "help clicked", ToastLength.Short).Show();
        }

        [Export]
        public void AboutClicked(Android.Views.View v)
        {
            var aFrag = new AboutFragment();
            var transaction = SupportFragmentManager.BeginTransaction();

            // Replace whatever is in the fragment_container view with this fragment,
            // and add the transaction to the back stack so the user can navigate back
            transaction.Replace(Resource.Id.fragment_container, aFrag);
            //transaction.addToBackStack(null);

            // Commit the transaction
            transaction.Commit();

            SlidingMenu.ShowContent();
        }
    }
}