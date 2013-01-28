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

namespace BFCAndroid.View
{
    [Activity(Label = "Buffer zone calculator", MainLauncher=true)]
    public class Main : ActionbarSherlock.App.SherlockActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.Main);
        }

        [Export]
        public void StartClicked(Android.Views.View v)
        {
            StartActivity(typeof(Calc));
        }

        [Export]
        public void HelpClicked(Android.Views.View v)
        {
            Toast.MakeText(this.SupportActionBar.ThemedContext, "help clicked", ToastLength.Short).Show();
        }

        [Export]
        public void AboutClicked(Android.Views.View v)
        {
            Toast.MakeText(this.SupportActionBar.ThemedContext, "about clicked", ToastLength.Short).Show();
        }
    }
}