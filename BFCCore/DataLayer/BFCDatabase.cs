using BFCCore.BusinessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace BFCCore.DataLayer
{
    public static class BFCDatabase
    {
        static string _dbName = "testdb";
        static SQLite.SQLiteConnection _db;
        static object _lock = new object();

        static BFCDatabase()
        {
            var docs = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var dbPath = Path.Combine(docs, _dbName);
            _db = new SQLite.SQLiteConnection(dbPath);
        }

        public static void CreateTables()
        {
            _db.CreateTable<Manufacturer>();
            _db.CreateTable<Nozzle>();
            _db.CreateTable<WaterFlow>();
            _db.CreateTable<Pressure>();
            _db.CreateTable<CalcSprayQuality>();
            _db.CreateTable<SprayQuality>();
            _db.CreateTable<WindSpeed>();
            _db.CreateTable<LabelSprayQuality>();
            _db.CreateTable<BoomHeight>();
            _db.CreateTable<Multiplier>();
        }

        public static void DropTables()
        {
            _db.DropTable<Manufacturer>();
            _db.DropTable<Nozzle>();
            _db.DropTable<WaterFlow>();
            _db.DropTable<Pressure>();
            _db.DropTable<CalcSprayQuality>();
            _db.DropTable<SprayQuality>();
            _db.DropTable<WindSpeed>();
            _db.DropTable<LabelSprayQuality>();
            _db.DropTable<BoomHeight>();
            _db.DropTable<Multiplier>();
        }

        public static void AddToDb<T>(IList<T> things)
        {
            lock (_lock)
            {
                _db.InsertAll(things);
            }
        }

        public static IList<T> GetTable<T>() where T : new()
        {
            var ret = new List<T>();
            var tab = _db.Table<T>();
            foreach (var r in tab)
            {
                ret.Add(r);
            }
            return ret;
        }

        public static IList<Nozzle> GetNozzleFor(Manufacturer man)
        {
            var ret = from n in _db.Table<Nozzle>()
                      where n.ManufacturerId == man.Id
                      select n;
            return ret.ToList();
        }

        public static IList<Pressure> GetPressureFor(Nozzle nozzle)
        {
            var ret = from p in _db.Table<Pressure>()
                      where p.NozzleId == nozzle.Id
                      select p;
            return ret.ToList();
        }

        public static IList<WaterFlow> GetWaterFlowFor(Nozzle nozzle)
        {
            var ret = from wf in _db.Table<WaterFlow>()
                      where wf.NozzleId == nozzle.Id
                      select wf;
            return ret.ToList();
        }

        public static SprayQuality GetSprayQualityFor(Pressure p, WaterFlow wf)
        {
            var retCsq = from csq in _db.Table<CalcSprayQuality>()
                         where csq.PressureId == p.Id && csq.WaterFlowId == wf.Id
                         select csq;
            Debug.Assert(retCsq.Count() == 1);

            // Work around, use local variable.
            var sqid = retCsq.First().SprayQualityId;
            var ret = from sq in _db.Table<SprayQuality>()
                      where sq.Id == sqid
                      select sq;
            Debug.Assert(ret.Count() == 1);

            return ret.First();
        }

        public static double? GetMultiplierFor(SprayQuality sq, LabelSprayQuality lsq, BoomHeight bh, WindSpeed ws)
        {
            // Work around, use local variable
            var sqId = sq.Id;
            var lsqId = lsq.Id;
            var bhId = bh.Id;
            var wsId = ws.Id;

            var ret = from m in _db.Table<Multiplier>()
                      where m.SprayQualityId == sqId && m.LabelSprayQualityId == lsqId && m.BoomHeightId == bhId && m.WindSpeedId == wsId
                      select m;
            Debug.Assert(ret.Count() == 1);
            var f = ret.FirstOrDefault();

            return f != null ? f.Value : (double?)null;
        }

        public static void CreateDummyData()
        {
            BFCDatabase.DropTables();
            BFCDatabase.CreateTables();

            var sprayQualitys = new List<SprayQuality> {
                new SprayQuality{Id = 1, Name = "Coarse"},
                new SprayQuality{Id = 2, Name = "Medium"},
                new SprayQuality{Id = 3, Name = "Fine"}
            };
            BFCDatabase.AddToDb(sprayQualitys);

            var windSpeeds = new List<WindSpeed> {
                new WindSpeed{Id = 1, Max = 8, Min = 1},
                new WindSpeed{Id = 2, Max = 16, Min = 9},
                new WindSpeed{Id = 3, Max = 25, Min = 17}
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
                new LabelSprayQuality{Id = 2, Name = "Medium"},
                new LabelSprayQuality{Id = 3, Name = "Fine"}
            };
            BFCDatabase.AddToDb(labelSprayQualitys);

            var manufacturers = new List<Manufacturer> {
                    new Manufacturer{Id = 1, Name = "Tee jet"}
                };
            BFCDatabase.AddToDb(manufacturers);

            var nozzle = new List<Nozzle> {
                    new Nozzle{Id = 1, ManufacturerId = 1, Name = "abc"},
                    new Nozzle{Id = 2, ManufacturerId = 1, Name = "bcd"},
                    new Nozzle{Id = 3, ManufacturerId = 1, Name = "cde"},
                };
            BFCDatabase.AddToDb(nozzle);

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

            var multi = new List<Multiplier> {
                new Multiplier{SprayQualityId = 1, LabelSprayQualityId = 1, BoomHeightId = 1, WindSpeedId = 1, Value = 2},
                new Multiplier{SprayQualityId = 2, LabelSprayQualityId = 2, BoomHeightId = 2, WindSpeedId = 2, Value = 4},
                new Multiplier{SprayQualityId = 3, LabelSprayQualityId = 3, BoomHeightId = 3, WindSpeedId = 3, Value = 8},
            };
            BFCDatabase.AddToDb(multi);
        }
    }
}
