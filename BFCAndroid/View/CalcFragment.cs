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
    public class CalcFragment : Fragment
    {
        public override Android.Views.View OnCreateView(LayoutInflater p0, ViewGroup p1, Bundle p2)
        {
            return p0.Inflate(Resource.Layout.CalcFragment, p1, false);
        }

        public const int Pick_Manufacturer = 0;

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