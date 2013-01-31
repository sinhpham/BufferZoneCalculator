using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;

namespace BFCAndroid.View
{
    public class AboutFragment : Fragment
    {
        public override Android.Views.View OnCreateView(LayoutInflater p0, ViewGroup p1, Bundle p2)
        {
            return p0.Inflate(Resource.Layout.AboutFragment, p1, false);
        }
    }
}