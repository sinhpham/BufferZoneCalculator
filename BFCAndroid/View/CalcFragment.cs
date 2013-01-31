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
using BFCCore.DataLayer;
using BFCCore.BusinessLayer;
using System.Threading.Tasks;
using BFCCore.ServiceAccessLayer;
using ActionbarSherlock.App;
using Com.Slidingmenu.Lib.App;
using Android.Support.V4.App;

namespace BFCAndroid.View
{
    [Activity(Label = "Buffer zone calculator")]
    public class CalcFragment : SherlockFragment
    {
        public override void OnCreate(Bundle p0)
        {
            SetHasOptionsMenu(true);
            base.OnCreate(p0);
        }

        public const int Pick_Manufacturer = 0;

        public override Android.Views.View OnCreateView(LayoutInflater p0, ViewGroup p1, Bundle p2)
        {
            var ab = SherlockActivity.SupportActionBar;
            ab.NavigationMode = ActionBar.NavigationModeStandard;
            ab.SetDisplayShowTitleEnabled(false);
            Activity.Title = "Calc";

            var choices = new string[] { "Ground", "Aerial", "blah", "blah" };
            var adap = new ArrayAdapter<string>(ab.ThemedContext, Resource.Layout.sherlock_spinner_dropdown_item, choices);

            ab.NavigationMode = ActionBar.NavigationModeList;
            var abddl = new ActionBarDropDownListener();
            abddl.currAct = SherlockActivity;
            ab.SetListNavigationCallbacks(adap, abddl);

            return p0.Inflate(Resource.Layout.CalcFragment, p1, false);
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

        private void CreateSelectDialog<T>(IList<T> list, Func<T, string> display, string title, Action<T> selectedAction)
        {
            // Return selected index
            var displayList = list.Select(display).ToArray();
            var adap = new ArrayAdapter(Activity, Android.Resource.Layout.SimpleSpinnerDropDownItem, displayList);

            new AlertDialog.Builder(Activity)
            .SetTitle(title)
            .SetSingleChoiceItems(adap, -1, (s, arg) =>
            {
                var ad = (AlertDialog)s;
                var pos = ad.ListView.CheckedItemPosition;
                ad.Dismiss();
                selectedAction(list[pos]);
            }).Create().Show();
        }

        public override void OnCreateOptionsMenu(ActionbarSherlock.View.IMenu p0, ActionbarSherlock.View.MenuInflater p1)
        {
            p0.Clear();
            p0.Add(new Java.Lang.String("Update"));
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
            switch (text)
            {
                case "Update":
                    UpdateDatabase();
                    break;
                default:
                    {
                        var toast = Toast.MakeText(Activity, p0.TitleFormatted, ToastLength.Long);
                        toast.Show();
                    }
                    break;
            }
            return base.OnOptionsItemSelected(p0);
        }

        private void UpdateDatabase()
        {
            Task.Factory.StartNew(() =>
            {
                //BFCDatabase.DropTables();
                //BFCDatabase.CreateTables();

                //var n = new BFCNetwork();
                //n.GetSprayQuality(data =>
                //{
                //    BFCDatabase.AddToDb(data);
                //    Activity.RunOnUiThread(() =>
                //    {
                //        Toast.MakeText(Activity, string.Format("Done updating {0}", data.First().GetType().Name), ToastLength.Short).Show();
                //    });
                //});

                //n.GetLabelSprayQuality(data =>
                //{
                //    BFCDatabase.AddToDb(data);
                //    Activity.RunOnUiThread(() =>
                //    {
                //        Toast.MakeText(Activity, string.Format("Done updating {0}", data.First().GetType().Name), ToastLength.Short).Show();
                //    });
                //});

                //n.GetBoomHeight(data =>
                //{
                //    BFCDatabase.AddToDb(data);
                //    Activity.RunOnUiThread(() =>
                //    {
                //        Toast.MakeText(Activity, string.Format("Done updating {0}", data.First().GetType().Name), ToastLength.Short).Show();
                //    });
                //});

                //n.GetWindSpeed(data =>
                //{
                //    BFCDatabase.AddToDb(data);
                //    Activity.RunOnUiThread(() =>
                //    {
                //        Toast.MakeText(Activity, string.Format("Done updating {0}", data.First().GetType().Name), ToastLength.Short).Show();
                //    });
                //});

                //n.GetMultiplier(data =>
                //{
                //    BFCDatabase.AddToDb(data);
                //    Activity.RunOnUiThread(() =>
                //    {
                //        Toast.MakeText(Activity, string.Format("Done updating {0}", data.First().GetType().Name), ToastLength.Short).Show();
                //    });
                //});

                //var manufacturers = new List<Manufacturer> {
                //    new Manufacturer{Id = 1, Name = "Tee jet"}
                //};
                //BFCDatabase.AddToDb(manufacturers);

                //var nozzle = new List<Nozzle> {
                //    new Nozzle{Id = 1, ManufacturerId = 1, Name = "abc"},
                //    new Nozzle{Id = 2, ManufacturerId = 1, Name = "bcd"},
                //    new Nozzle{Id = 3, ManufacturerId = 1, Name = "cde"},
                //};
                //BFCDatabase.AddToDb(nozzle);

                //var pressure = new List<Pressure> {
                //    new Pressure{Id = 1, NozzleId = 1, Value = 15},
                //    new Pressure{Id = 2, NozzleId = 1, Value = 25},
                //    new Pressure{Id = 3, NozzleId = 1, Value = 35},
                //};
                //BFCDatabase.AddToDb(pressure);

                //var wf = new List<WaterFlow> {
                //    new WaterFlow{Id = 1, NozzleId = 1, Value = "wf1"},
                //    new WaterFlow{Id = 2, NozzleId = 1, Value = "wf2"},
                //    new WaterFlow{Id = 3, NozzleId = 1, Value = "wf3"},
                //};
                //BFCDatabase.AddToDb(wf);

                //var csq = new List<CalcSprayQuality> {
                //    new CalcSprayQuality{WaterFlowId = 1, PressureId = 1, SprayQualityId = 1},
                //    new CalcSprayQuality{WaterFlowId = 2, PressureId = 2, SprayQualityId = 2},
                //    new CalcSprayQuality{WaterFlowId = 3, PressureId = 3, SprayQualityId = 3},
                //};
                //BFCDatabase.AddToDb(csq);

                BFCDatabase.CreateDummyData();

                Activity.RunOnUiThread(() =>
                {
                    Toast.MakeText(Activity, "Done updating database", ToastLength.Long).Show();
                });
            });
        }

        private void RefreshResult()
        {
            var bfAquatic = -1;
            var okBfAquatic = int.TryParse(View.FindViewById<EditText>(Resource.Id.editBfAquatic).Text, out bfAquatic);
            var bfTerrestrial = -1;
            var okBfTerrestrial = int.TryParse(View.FindViewById<EditText>(Resource.Id.editBfTerrestrial).Text, out bfTerrestrial);

            var resAquatic = View.FindViewById<TextView>(Resource.Id.resBfAquaticTextView);
            var resTerrestrial = View.FindViewById<TextView>(Resource.Id.resBfTerrestrialTextView);


            if (BFCAndroidGlobal.SelectedSprayQuality == null ||
                BFCAndroidGlobal.SelectedLabelSprayQuality == null ||
                BFCAndroidGlobal.SelectedBoomHeight == null ||
                BFCAndroidGlobal.SelectedWindSpeed == null ||
                !okBfAquatic || !okBfTerrestrial)
            {
                resAquatic.Text = "Buffer zone aquatic:";
                resTerrestrial.Text = "Buffer zone terrestrial:";
                return;
            }

            var m = BFCDatabase.GetMultiplierFor(BFCAndroidGlobal.SelectedSprayQuality, BFCAndroidGlobal.SelectedLabelSprayQuality,
                BFCAndroidGlobal.SelectedBoomHeight, BFCAndroidGlobal.SelectedWindSpeed);
            if (!m.HasValue)
            {
                resAquatic.Text = string.Format("Buffer zone aquatic: {0}", "not found");
                resTerrestrial.Text = string.Format("Buffer zone terrestrial: {0}", "not found");
            }
            else
            {
                resAquatic.Text = string.Format("Buffer zone aquatic: {0}", m.Value * bfAquatic);
                resTerrestrial.Text = string.Format("Buffer zone terrestrial: {0}", m.Value * bfTerrestrial);
            }
        }

        public override void OnActivityResult(int p0, int p1, Intent p2)
        {
            var requestCode = p0;
            var resultCode = (Android.App.Result)p1;
            if (resultCode == Result.Ok && requestCode == CalcFragment.Pick_Manufacturer)
            {
                var b = View.FindViewById<Button>(Resource.Id.sprayQualityButton);
                b.Text = BFCAndroidGlobal.SelectedSprayQuality.Name;
                RefreshResult();
            }
            base.OnActivityResult(p0, p1, p2);
        }

        [Export]
        public void LabelSprayClicked(Android.Views.View v)
        {
            var lsq = BFCDatabase.GetTable<LabelSprayQuality>();
            CreateSelectDialog<LabelSprayQuality>(lsq, x => x.Name, "Choose label spray quality", selected =>
            {
                var b = (Button)v;
                b.Text = selected.Name;
                BFCAndroidGlobal.SelectedLabelSprayQuality = selected;
                RefreshResult();
            });
        }

        [Export]
        public void BoomHeightClicked(Android.Views.View v)
        {
            var bh = BFCDatabase.GetTable<BoomHeight>();
            CreateSelectDialog<BoomHeight>(bh, x => x.Name, "Choose boom height", selected =>
            {
                var b = (Button)v;
                b.Text = selected.Name;
                BFCAndroidGlobal.SelectedBoomHeight = selected;
                RefreshResult();
            });
        }

        [Export]
        public void WindSpeedClicked(Android.Views.View v)
        {
            var ws = BFCDatabase.GetTable<WindSpeed>();
            CreateSelectDialog<WindSpeed>(ws, x => string.Format("From {0} to {1}", x.Min, x.Max), "Choose wind speed", selected =>
            {
                var b = (Button)v;
                b.Text = string.Format("From {0} to {1}", selected.Min, selected.Max);
                BFCAndroidGlobal.SelectedWindSpeed = selected;
                RefreshResult();
            });
        }

        [Export]
        public void SprayQualityCaclClicked(Android.Views.View v)
        {
            var intent = new Intent(Activity, typeof(View.SelectItem));
            intent.PutExtra("pick", "manufacturer");

            StartActivityForResult(intent, Pick_Manufacturer);
        }

        [Export]
        public void SprayQualityClicked(Android.Views.View v)
        {
            var sq = BFCDatabase.GetTable<SprayQuality>();
            CreateSelectDialog<SprayQuality>(sq, x => x.Name, "Choose spray quality", selected =>
            {
                var b = (Button)v;
                b.Text = selected.Name;
                BFCAndroidGlobal.SelectedSprayQuality = selected;
                RefreshResult();
            });
        }
    }
}