using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Json;
using BFCCore.BusinessLayer;

namespace BFCCore.ServiceAccessLayer
{
    public class BFCNetwork
    {
        public void GetSprayQuality(Action<IList<SprayQuality>> action)
        {
            DownloadAndParseJsonData("http://demeter.usask.ca/buffer_zone_multiplier/bufferzone_db_data.php?table=spray_quality", jValue =>
            {
                var ret = new List<SprayQuality>();
                foreach (var v in jValue)
                {
                    var j = (JsonValue)v;
                    var curr = new SprayQuality()
                    {
                        Id = j["spray_quality_id"],
                        Name = j["spray_quality_name"],
                    };
                    ret.Add(curr);
                }
                action(ret);
            });
        }

        public void GetLabelSprayQuality(Action<IList<LabelSprayQuality>> action)
        {
            DownloadAndParseJsonData("http://demeter.usask.ca/buffer_zone_multiplier/bufferzone_db_data.php?table=label_spray", jValue =>
            {
                var ret = new List<LabelSprayQuality>();
                foreach (var v in jValue)
                {
                    var j = (JsonValue)v;
                    var curr = new LabelSprayQuality()
                    {
                        Id = j["label_sparay_id"],
                        Name = j["label_sparay_name"],
                    };
                    ret.Add(curr);
                }
                action(ret);
            });
        }

        public void GetBoomHeight(Action<IList<BoomHeight>> action)
        {
            DownloadAndParseJsonData("http://demeter.usask.ca/buffer_zone_multiplier/bufferzone_db_data.php?table=boom_height", jValue =>
            {
                var ret = new List<BoomHeight>();
                foreach (var v in jValue)
                {
                    var j = (JsonValue)v;
                    var curr = new BoomHeight()
                    {
                        Id = j["boom_height_id"],
                        Name = j["boom_height_name"],
                    };
                    ret.Add(curr);
                }
                action(ret);
            });
        }

        public void GetWindSpeed(Action<IList<WindSpeed>> action)
        {
            DownloadAndParseJsonData("http://demeter.usask.ca/buffer_zone_multiplier/bufferzone_db_data.php?table=wind_speed", jValue =>
            {
                var ret = new List<WindSpeed>();
                foreach (var v in jValue)
                {
                    var j = (JsonValue)v;
                    var curr = new WindSpeed()
                    {
                        Id = j["wind_speed_id"],
                        Min = j["min"],
                        Max = j["max"]
                    };
                    ret.Add(curr);
                }
                action(ret);
            });
        }

        public void GetMultiplier(Action<IList<Multiplier>> action)
        {
            DownloadAndParseJsonData("http://demeter.usask.ca/buffer_zone_multiplier/bufferzone_db_data.php?table=buffer_zone_multiplier", jValue =>
            {
                var ret = new List<Multiplier>();
                foreach (var v in jValue)
                {
                    var j = (JsonValue)v;
                    var curr = new Multiplier()
                    {
                        SprayQualityId = j["spray_quality_id"],
                        LabelSprayQualityId = j["label_spray_id"],
                        BoomHeightId = j["boom_height_id"],
                        WindSpeedId = j["wind_speed_id"],
                        Value = j["value"]
                    };
                    ret.Add(curr);
                }
                action(ret);
            });
        }

        void DownloadAndParseJsonData(string url, Action<JsonValue> action)
        {
            var wc = new WebClient();

            wc.DownloadStringCompleted += (sender, arg) =>
            {
                if (arg.Error != null)
                {
                    // Handle network error here.
                    return;
                }

                var jValue = JsonObject.Parse(arg.Result);
                action(jValue);
            };
            wc.DownloadStringAsync(new Uri(url));
        }
    }
}
