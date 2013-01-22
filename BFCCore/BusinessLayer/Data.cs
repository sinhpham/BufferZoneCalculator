using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BFCCore.BusinessLayer
{
    public class Manufacturer
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Nozzle
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ManufacturerId { get; set; }
        public string Name { get; set; }
    }

    public class WaterFlow
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int NozzleId { get; set; }
        public string Value { get; set; }
    }

    public class Pressure
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int NozzleId { get; set; }
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
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class WindSpeed
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }

    public class LabelSprayQuality
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BoomHeight
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Multiplier
    {
        [Indexed]
        public int WindSpeedId { get; set; }
        [Indexed]
        public int LabelSprayQualityId { get; set; }
        [Indexed]
        public int SprayQualityId { get; set; }
        [Indexed]
        public int BoomHeightId { get; set; }
        public double Value { get; set; }
    }
}
