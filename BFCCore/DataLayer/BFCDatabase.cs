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
    }
}
