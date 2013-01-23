using BFCCore.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BFCAndroid
{
    static class BFCAndroidGlobal
    {
        public static Manufacturer SelectedManufacturer { get; set; }
        public static Nozzle SelectedNozzle { get; set; }
        public static Pressure SelectedPressure { get; set; }
        public static WaterFlow SelectedWaterFlow { get; set; }

        public static SprayQuality SelectedSprayQuality { get; set; }
        public static LabelSprayQuality SelectedLabelSprayQuality { get; set; }
        public static BoomHeight SelectedBoomHeight { get; set; }
        public static WindSpeed SelectedWindSpeed { get; set; }
    }
}
