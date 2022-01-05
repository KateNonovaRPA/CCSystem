using Microsoft.EntityFrameworkCore;
using Models.Context;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Models.Services
{
    public abstract class BaseService
    {
        protected MainContext db;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DbSet<T> All<T>() where T : class
        {
            return db.Set<T>();
        }

        public IQueryable<T> All<T>(Expression<Func<T, bool>> search) where T : class
        {
            return db.Set<T>().Where(search);
        }

        public bool DeleteRange<T>(Expression<Func<T, bool>> whatToDelete) where T : class
        {
            try
            {
                var setOfT = db.Set<T>();
                var forDelete = setOfT.Where(whatToDelete);
                setOfT.RemoveRange(forDelete);
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }

        public T Find<T>(object id) where T : class
        {
            return db.Set<T>().Find(id);
        }

        public static string[] SplitString(string data)
        {
            string[] subs = data.Split('-');
            return subs;
        }

        public static DataTable ExportToDataTable(string filePath)
        {
            DataTable dt = new DataTable();
            string path = Path.Combine(Environment.CurrentDirectory, filePath);
            try
            {
                //TODO: Make a template and change the sheet name
                string sqlquery = "Select * From [Sheet1$]";
                DataSet ds = new DataSet();
                string constring = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
                OleDbConnection con = new OleDbConnection(constring + "");
                OleDbDataAdapter da = new OleDbDataAdapter(sqlquery, con);
                da.Fill(ds);
                dt = ds.Tables[0];
            }
            catch
            {
                throw new Exception("Fail export excel to data table");
            }
            return dt;
        }
    }
}