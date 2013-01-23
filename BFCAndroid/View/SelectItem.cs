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
using BFCCore.BusinessLayer;
using BFCCore.DataLayer;

namespace BFCAndroid.View
{
    [Activity]
    public class SelectItem : ListActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.SelectItem);

            var adap = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, _items);
            ListAdapter = adap;
            ListView.TextFilterEnabled = true;
            ListView.ChoiceMode = ChoiceMode.Multiple;

            _listWhat = Intent.GetStringExtra("pick");

            switch (_listWhat)
            {
                case "manufacturer":
                    {
                        Title = "Pick Manufacturer";
                        var manu = BFCDatabase.GetTable<Manufacturer>();
                        DisplayListOnUI(manu);
                    }
                    break;
                case "nozzle":
                    {
                        Title = "Pick nozzle";
                        var n = BFCDatabase.GetNozzleFor(BFCAndroidGlobal.SelectedManufacturer);
                        DisplayListOnUI(n);
                    }
                    break;
                case "pressure":
                    {
                        Title = "Pick pressure";
                        var p = BFCDatabase.GetPressureFor(BFCAndroidGlobal.SelectedNozzle);
                        DisplayListOnUI(p);
                    }
                    break;
                case "waterflow":
                    {
                        Title = "Pick water flow";
                        var wf = BFCDatabase.GetWaterFlowFor(BFCAndroidGlobal.SelectedNozzle);
                        DisplayListOnUI(wf);
                    }
                    break;
                default:
                    throw new InvalidOperationException("Invalid list type");
            }
            ((ArrayAdapter)ListAdapter).NotifyDataSetChanged();

            var filterEditText = FindViewById<EditText>(Resource.Id.filterEditText);
            filterEditText.TextChanged += (sender, arg) =>
            {
                var et = (EditText)sender;
                ((ArrayAdapter<string>)ListAdapter).Filter.InvokeFilter(et.Text);
            };
        }

        JavaList<string> _items = new JavaList<string>();
        string _listWhat;
        const int Pick_Nozzle = 0;
        const int Pick_Pressure = 1;
        const int Pick_WaterFlow = 2;
        List<object> _selectionList = new List<object>();

        private void DisplayListOnUI<T>(IEnumerable<T> list)
        {
            RunOnUiThread(() =>
            {
                _items.Clear();
                _selectionList.Clear();
                foreach (var c in list)
                {
                    _selectionList.Add(c);
                    _items.Add(c.ToString());
                }
                ((ArrayAdapter)ListAdapter).NotifyDataSetChanged();
            });
        }

        protected override void OnListItemClick(ListView l, Android.Views.View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);
            switch (_listWhat)
            {
                case "manufacturer":
                    {
                        BFCAndroidGlobal.SelectedManufacturer = (Manufacturer)_selectionList[position];

                        var intent = new Intent(this, typeof(View.SelectItem));
                        intent.PutExtra("pick", "nozzle");
                        StartActivityForResult(intent, Pick_Nozzle);
                    }
                    break;
                case "nozzle":
                    {
                        BFCAndroidGlobal.SelectedNozzle = (Nozzle)_selectionList[position];

                        var intent = new Intent(this, typeof(View.SelectItem));
                        intent.PutExtra("pick", "pressure");
                        StartActivityForResult(intent, Pick_Pressure);
                    }
                    break;
                case "pressure":
                    {
                        BFCAndroidGlobal.SelectedPressure = (Pressure)_selectionList[position];

                        var intent = new Intent(this, typeof(View.SelectItem));
                        intent.PutExtra("pick", "waterflow");
                        StartActivityForResult(intent, Pick_Pressure);
                    }
                    break;
                case "waterflow":
                    {
                        BFCAndroidGlobal.SelectedWaterFlow = (WaterFlow)_selectionList[position];
                        var sq = BFCDatabase.GetSprayQualityFor(BFCAndroidGlobal.SelectedPressure, BFCAndroidGlobal.SelectedWaterFlow);
                        BFCAndroidGlobal.SelectedSprayQuality = sq;

                        System.Diagnostics.Debug.WriteLine("Selected sq: {0} {1}", sq.Id, sq.Name);

                        SetResult(Result.Ok);
                        Finish();
                    }
                    break;
                default:
                    throw new InvalidOperationException("Invalid list type");
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                SetResult(Result.Ok);
                Finish();
            }
        }
    }
}