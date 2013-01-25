using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BFCCore.BusinessLayer
{
    public class Manufacturer
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Nozzle
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int ManufacturerId { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class WaterFlow
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int NozzleId { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }

    public class Pressure
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int NozzleId { get; set; }
        public double Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class CalcSprayQuality
    {
        [Indexed]
        public int WaterFlowId { get; set; }
        [Indexed]
        public int PressureId { get; set; }
        public int SprayQualityId { get; set; }
    }

    public class SprayQuality
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class LabelSprayQuality
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BoomHeight
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class WindSpeed
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }

    public class Multiplier
    {
        [Indexed]
        public int SprayQualityId { get; set; }
        [Indexed]
        public int LabelSprayQualityId { get; set; }
        [Indexed]
        public int BoomHeightId { get; set; }
        [Indexed]
        public int WindSpeedId { get; set; }

        public double Value { get; set; }
    }
}
