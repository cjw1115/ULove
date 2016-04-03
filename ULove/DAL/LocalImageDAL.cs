using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ULove.Models;

namespace ULove.DAL
{
    public class LocalImageDAL<T>where T:class
    {
        private static String DB_NAME = "Local.db";
        

        public string DbPath { get; set; }
        public SQLite.Net.SQLiteConnection DbConnection { get; set; }
        private LocalImageDAL()
        {
            string databasePath = Path.Combine(DbPath, DB_NAME);
            DbConnection = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), databasePath);
        }
        public LocalImageDAL(string dbpath)
        {
            DbPath = dbpath;
            string databasePath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, DB_NAME);

            DbConnection = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), databasePath);
            if (DbConnection.GetTableInfo(typeof(T).Name).Count <= 0)
                CreateTable();

        }
        public void CreateTable()
        { 
            if (DbConnection == null)
            {
                return;
            }
            int re=DbConnection.CreateTable<T>();
        }
        public void Add(T data)
        {
            if (DbConnection == null)
                return;
            var entity = data;
            DbConnection.Insert(entity);
        }
        public void Delete(int id)
        {
            if (DbConnection == null)
                return;
            //DbConnection.Table<T>().Delete(m => m.ID == id);
            var cmd=DbConnection.CreateCommand($"delete from {typeof(T).Name} where ID={id}");
            cmd.ExecuteNonQuery();

            //数据库性能机制，防止多度的磁盘IO
            //cmd = DbConnection.CreateCommand($"Vacuum {typeof(T).Name}");
            //cmd.ExecuteNonQuery();

        }

        public IList<T> SelectAll()
        {
            IList<T> list = new List<T>();
            if (DbConnection == null)
                return list;
            var query=DbConnection.Table<T>();
            var t= query.ToList();
            return t;
        }
        
    }
}
