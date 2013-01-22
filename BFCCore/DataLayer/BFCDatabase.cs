using BFCCore.BusinessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
    }
}
