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

            // Create your application here
            SupportActionBar.SetDisplayShowTitleEnabled(false);

            var choices = new string[] { "Ground", "Aerial", "blah", "blah" };
            var adap = new ArrayAdapter<string>(SupportActionBar.ThemedContext, Resource.Layout.sherlock_spinner_dropdown_item, choices);

            SupportActionBar.NavigationMode = ActionBar.NavigationModeList;
            var abddl = new ActionBarDropDownListener();
            abddl.currAct = this;
            SupportActionBar.SetListNavigationCallbacks(adap, abddl);

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

                //if (SavedInstanceState != null)
                //{
                //    return;
                //}

                // Create an instance of ExampleFragment
                var aboutFragment = new AboutFragment();

                // In case this activity was started with special instructions from an Intent,
                // pass the Intent's extras to the fragment as arguments
                aboutFragment.Arguments = Intent.Extras;

                // Add the fragment to the 'fragment_container' FrameLayout
                SupportFragmentManager.BeginTransaction()
                        .Add(Resource.Id.fragment_container, aboutFragment).Commit();
            }
        }

        class ActionBarDropDownListener : Java.Lang.Object, ActionBar.IOnNavigationListener
        {
            public SherlockFragmentActivity currAct { get; set; }

            public bool OnNavigationItemSelected(int p0, long p1)
            {
                // TODO: change view according to selected item.
                return true;
            }
        }

        public override bool OnCreateOptionsMenu(ActionbarSherlock.View.IMenu p0)
        {
            p0.Add(new Java.Lang.String("Update"));
            return true;
        }

        public override bool OnOptionsItemSelected(ActionbarSherlock.View.IMenuItem p0)
        {
            var text = p0.TitleFormatted.ToString();
            if (text == Title)
            {
                Finish();
                return true;
            }
            switch (text)
            {
                case "Update":
                    UpdateDatabase();
                    break;
                default:
                    {
                        var toast = Toast.MakeText(this, p0.TitleFormatted, ToastLength.Long);
                        toast.Show();
                    }
                    break;
            }
            return true;
        }

        private void UpdateDatabase()
        {
            Task.Factory.StartNew(() =>
            {
                BFCDatabase.DropTables();
                BFCDatabase.CreateTables();

                var n = new BFCNetwork();
                n.GetSprayQuality(data =>
                {
                    BFCDatabase.AddToDb(data);
                    RunOnUiThread(() =>
                    {
                        Toast.MakeText(this, string.Format("Done updating {0}", data.First().GetType().Name), ToastLength.Short).Show();
                    });
                });

                n.GetLabelSprayQuality(data =>
                {
                    BFCDatabase.AddToDb(data);
                    RunOnUiThread(() =>
                    {
                        Toast.MakeText(this, string.Format("Done updating {0}", data.First().GetType().Name), ToastLength.Short).Show();
                    });
                });

                n.GetBoomHeight(data =>
                {
                    BFCDatabase.AddToDb(data);
                    RunOnUiThread(() =>
                    {
                        Toast.MakeText(this, string.Format("Done updating {0}", data.First().GetType().Name), ToastLength.Short).Show();
                    });
                });

                n.GetWindSpeed(data =>
                {
                    BFCDatabase.AddToDb(data);
                    RunOnUiThread(() =>
                    {
                        Toast.MakeText(this, string.Format("Done updating {0}", data.First().GetType().Name), ToastLength.Short).Show();
                    });
                });

                n.GetMultiplier(data =>
                {
                    BFCDatabase.AddToDb(data);
                    RunOnUiThread(() =>
                    {
                        Toast.MakeText(this, string.Format("Done updating {0}", data.First().GetType().Name), ToastLength.Short).Show();
                    });
                });

                //var sprayQualitys = new List<SprayQuality> {
                //    new SprayQuality{Id = 1, Name = "Coarse"},
                //    new SprayQuality{Id = 2, Name = "Medium"},
                //    new SprayQuality{Id = 3, Name = "Fine"}
                //};
                //BFCDatabase.AddToDb(sprayQualitys);

                //var windSpeeds = new List<WindSpeed> {
                //    new WindSpeed{Id = 1, Max = 8, Min = 1},
                //    new WindSpeed{Id = 2, Max = 16, Min = 9},
                //    new WindSpeed{Id = 3, Max = 25, Min = 17}
                //};
                //BFCDatabase.AddToDb(windSpeeds);

                //var boomHeights = new List<BoomHeight>{
                //    new BoomHeight{Id = 1, Name = "Low"},
                //    new BoomHeight{Id = 2, Name = "Medium"},
                //    new BoomHeight{Id = 3, Name = "High"}
                //};
                //BFCDatabase.AddToDb(boomHeights);

                //var labelSprayQualitys = new List<LabelSprayQuality> {
                //    new LabelSprayQuality{Id = 1, Name = "Coarse"},
                //    new LabelSprayQuality{Id = 2, Name = "Medium"},
                //    new LabelSprayQuality{Id = 3, Name = "Fine"}
                //};
                //BFCDatabase.AddToDb(labelSprayQualitys);

                var manufacturers = new List<Manufacturer> {
                    new Manufacturer{Id = 1, Name = "Tee jet"}
                };
                BFCDatabase.AddToDb(manufacturers);

                var nozzle = new List<Nozzle> {
                    new Nozzle{Id = 1, ManufacturerId = 1, Name = "abc"},
                    new Nozzle{Id = 2, ManufacturerId = 1, Name = "bcd"},
                    new Nozzle{Id = 3, ManufacturerId = 1, Name = "cde"},
                };
                BFCDatabase.AddToDb(nozzle);

                var pressure = new List<Pressure> {
                    new Pressure{Id = 1, NozzleId = 1, Value = 15},
                    new Pressure{Id = 2, NozzleId = 1, Value = 25},
                    new Pressure{Id = 3, NozzleId = 1, Value = 35},
                };
                BFCDatabase.AddToDb(pressure);

                var wf = new List<WaterFlow> {
                    new WaterFlow{Id = 1, NozzleId = 1, Value = "wf1"},
                    new WaterFlow{Id = 2, NozzleId = 1, Value = "wf2"},
                    new WaterFlow{Id = 3, NozzleId = 1, Value = "wf3"},
                };
                BFCDatabase.AddToDb(wf);

                var csq = new List<CalcSprayQuality> {
                    new CalcSprayQuality{WaterFlowId = 1, PressureId = 1, SprayQualityId = 1},
                    new CalcSprayQuality{WaterFlowId = 2, PressureId = 2, SprayQualityId = 2},
                    new CalcSprayQuality{WaterFlowId = 3, PressureId = 3, SprayQualityId = 3},
                };
                BFCDatabase.AddToDb(csq);

                //var multi = new List<Multiplier> {
                //    new Multiplier{SprayQualityId = 1, LabelSprayQualityId = 1, BoomHeightId = 1, WindSpeedId = 1, Value = 2},
                //    new Multiplier{SprayQualityId = 2, LabelSprayQualityId = 2, BoomHeightId = 2, WindSpeedId = 2, Value = 4},
                //    new Multiplier{SprayQualityId = 3, LabelSprayQualityId = 3, BoomHeightId = 3, WindSpeedId = 3, Value = 8},
                //};
                //BFCDatabase.AddToDb(multi);

                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, "Done updating database", ToastLength.Long).Show();
                });
            });
        }

        [Export]
        public void StartClicked(Android.Views.View v)
        {
            var calcFrag = new CalcFragment();
            var transaction = SupportFragmentManager.BeginTransaction();

            // Replace whatever is in the fragment_container view with this fragment,
            // and add the transaction to the back stack so the user can navigate back
            transaction.Replace(Resource.Id.fragment_container, calcFrag);
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