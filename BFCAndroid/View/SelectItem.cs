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

namespace BFCAndroid.View
{
    [Activity]
    public class SelectItem : ListActivity
    {
        JavaList<string> _items = new JavaList<string>();
        string _listWhat;
        const int Pick_Crop_Stage = 0;
        const int Pick_Weed = 1;
        const int Pick_Weed_Stage = 2;
        List<object> _selectionList = new List<object>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.SelectItem);

            var adap = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, _items);
            ListAdapter = adap;
            ListView.TextFilterEnabled = true;
            ListView.ItemClick += ListView_ItemClick;
            ListView.ChoiceMode = ChoiceMode.Multiple;

            _listWhat = Intent.GetStringExtra("pick");

            switch (_listWhat)
            {
                
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

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            switch (_listWhat)
            {
                
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