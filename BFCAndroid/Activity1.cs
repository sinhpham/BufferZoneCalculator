using System;
using System.Linq;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using BFCCore.DataLayer;
using System.Collections.Generic;
using BFCCore.BusinessLayer;
using Java.Interop;

namespace BFCAndroid
{
    [Activity(Label = "Buffer Zone Calculator", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var appMethods = new string[] {
                "Ground",
                "Aerial",
                "blah",
                "blah"
            };
            SetSpinnerData(Resource.Id.appMethodsSpinner, appMethods);

            RefreshOptions();
        }

        const int Pick_Manufacturer = 0;

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add("update");
            menu.Add("refresh");
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var text = item.TitleFormatted.ToString();
            switch (text)
            {
                case "update":
                    UpdateDatabase();
                    break;
                case "refresh":
                    RefreshOptions();
                    break;
                default:
                    {
                        var toast = Toast.MakeText(this, item.TitleFormatted, ToastLength.Long);
                        toast.Show();
                    }
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void RefreshOptions()
        {
            Task.Factory.StartNew(() =>
            {
                var sq = BFCDatabase.GetTable<SprayQuality>().Select(a => a.Name).ToArray();
                var ws = BFCDatabase.GetTable<WindSpeed>().Select(a => string.Format("From {0} to {1}", a.Min, a.Max)).ToArray();
                var bh = BFCDatabase.GetTable<BoomHeight>().Select(a => a.Name).ToArray();
                var lsq = BFCDatabase.GetTable<LabelSprayQuality>().Select(a => a.Name).ToArray();

                RunOnUiThread(() =>
                {
                    SetSpinnerData(Resource.Id.sprayQualitySpinner, sq);
                    SetSpinnerData(Resource.Id.windSpeedSpinner, ws);
                    SetSpinnerData(Resource.Id.boomHeightSpinner, bh);
                    SetSpinnerData(Resource.Id.labelSpraySpinner, lsq);
                });
            });
        }

        private void SetSpinnerData(int spinId, string[] data)
        {
            var spin = FindViewById<Spinner>(spinId);
            var adap = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, data);
            adap.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spin.Adapter = adap;
        }

        private void UpdateDatabase()
        {
            Task.Factory.StartNew(() =>
            {
                BFCDatabase.DropTables();
                BFCDatabase.CreateTables();

                var sprayQualitys = new List<SprayQuality> {
                    new SprayQuality{Id = 1, Name = "Coarse"},
                    new SprayQuality{Id = 2, Name = "Fine"},
                    new SprayQuality{Id = 3, Name = "Medium"}
                };
                BFCDatabase.AddToDb(sprayQualitys);

                var windSpeeds = new List<WindSpeed> {
                    new WindSpeed{Id = 1, Max = 8, Min = 1},
                    new WindSpeed{Id = 2, Max = 16, Min = 9}
                };
                BFCDatabase.AddToDb(windSpeeds);

                var boomHeights = new List<BoomHeight>{
                    new BoomHeight{Id = 1, Name = "Low"},
                    new BoomHeight{Id = 2, Name = "Medium"},
                    new BoomHeight{Id = 3, Name = "High"}
                };
                BFCDatabase.AddToDb(boomHeights);

                var labelSprayQualitys = new List<LabelSprayQuality> {
                    new LabelSprayQuality{Id = 1, Name = "Coarse"},
                    new LabelSprayQuality{Id = 2, Name = "Fine"},
                    new LabelSprayQuality{Id = 3, Name = "Medium"}
                };
                BFCDatabase.AddToDb(labelSprayQualitys);

                var manufacturers = new List<Manufacturer> {
                    new Manufacturer{Id = 1, Name = "Tee jet"}
                };
                BFCDatabase.AddToDb(manufacturers);

                var n = new List<Nozzle> {
                    new Nozzle{Id = 1, ManufacturerId = 1, Name = "abc"},
                    new Nozzle{Id = 2, ManufacturerId = 1, Name = "bcd"},
                    new Nozzle{Id = 3, ManufacturerId = 1, Name = "cde"},
                };
                BFCDatabase.AddToDb(n);

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

                RunOnUiThread(() =>
                {
                    Toast.MakeText(this, "Done updating database", ToastLength.Long).Show();
                });
            });
        }

        [Export]
        public void SprayQualityCaclClicked(Android.Views.View v)
        {
            var intent = new Intent(this, typeof(View.SelectItem));
            intent.PutExtra("pick", "manufacturer");

            StartActivityForResult(intent, Pick_Manufacturer);
        }
    }
}

