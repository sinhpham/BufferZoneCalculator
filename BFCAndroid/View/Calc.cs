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

namespace BFCAndroid.View
{
    [Activity(Label = "Buffer zone calculator", MainLauncher=true)]
    public class Calc : SlidingActivity
    {
        public override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            // Create your application here
            SupportActionBar.SetDisplayShowTitleEnabled(false);

            var choices = new string[] { "Ground", "Aerial", "blah", "blah" };
            var adap = new ArrayAdapter<string>(SupportActionBar.ThemedContext, Resource.Layout.sherlock_spinner_dropdown_item, choices);

            SupportActionBar.NavigationMode = ActionBar.NavigationModeList;
            var abddl = new ActionBarDropDownListener();
            abddl.currAct = this;
            SupportActionBar.SetListNavigationCallbacks(adap, abddl);

            SetContentView(Resource.Layout.Calc);
            SetBehindContentView(Resource.Layout.Main);

            SlidingMenu.SetShadowWidthRes(Resource.Dimension.SlidingMenuShadowWidth);
            SlidingMenu.SetShadowDrawable(Resource.Drawable.SlidingMenuShadow);
            SlidingMenu.SetBehindOffsetRes(Resource.Dimension.SlidingmenuOffset);
            SlidingMenu.SetFadeDegree(0.35f);
            SlidingMenu.TouchModeAbove = Com.Slidingmenu.Lib.SlidingMenu.TouchmodeFullscreen;
        }

        class ActionBarDropDownListener : Java.Lang.Object, ActionBar.IOnNavigationListener
        {
            public SherlockActivity currAct { get; set; }

            public bool OnNavigationItemSelected(int p0, long p1)
            {
                // TODO: change view according to selected item.
                return true;
            }
        }

        const int Pick_Manufacturer = 0;

        public override bool OnCreateOptionsMenu(ActionbarSherlock.View.IMenu p0)
        {
            p0.Add(new Java.Lang.String("update"));
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
                case "update":
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

        private void RefreshResult()
        {
            var bfAquatic = -1;
            var okBfAquatic = int.TryParse(FindViewById<EditText>(Resource.Id.editBfAquatic).Text, out bfAquatic);
            var bfTerrestrial = -1;
            var okBfTerrestrial = int.TryParse(FindViewById<EditText>(Resource.Id.editBfTerrestrial).Text, out bfTerrestrial);

            var resAquatic = FindViewById<TextView>(Resource.Id.resBfAquaticTextView);
            var resTerrestrial = FindViewById<TextView>(Resource.Id.resBfTerrestrialTextView);


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

        private void CreateSelectDialog<T>(IList<T> list, Func<T, string> display, string title, Action<T> selectedAction)
        {
            // Return selected index
            var displayList = list.Select(display).ToArray();
            var adap = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, displayList);

            new AlertDialog.Builder(this)
            .SetTitle(title)
            .SetSingleChoiceItems(adap, -1, (s, arg) =>
            {
                var ad = (AlertDialog)s;
                var pos = ad.ListView.CheckedItemPosition;
                ad.Dismiss();
                selectedAction(list[pos]);
            }).Create().Show();
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
            var intent = new Intent(this, typeof(View.SelectItem));
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

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok && requestCode == Pick_Manufacturer)
            {
                var b = FindViewById<Button>(Resource.Id.sprayQualityButton);
                b.Text = BFCAndroidGlobal.SelectedSprayQuality.Name;
                RefreshResult();
            }
        }
    }
}