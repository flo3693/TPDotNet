using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace Projet_Hamon_Tissier_mercure
{
    static class ExcelDataController
    {
        private const string conf = "Jet.txt";
        private const string catXls = "xls";
        private const string provTitle = "Provider";
        private const string dataTitle = "Data Source";
        private const string propTitle = "Extended Properties";
        private const string sheetTitle = "Sheet";
        private const string defaut = "";
        
        private static OleDbConnection cnx;
        private static OleDbDataAdapter adapt;
        private static System.Data.DataSet dataSet = new System.Data.DataSet();
        public static System.Data.DataSet DataSet{
            get{return dataSet;}
            set{dataSet = value;}
        }

        private static OleDbCommand cmd;

        public static bool connect()
        {
            string provider = ConfigIni.GetString(conf, catXls, provTitle, defaut);
            string dataSource = ConfigIni.GetString(conf, catXls, dataTitle, defaut);
            string properties = ConfigIni.GetString(conf, catXls, propTitle, defaut);

            if (provider == defaut || dataSource == defaut || properties == defaut)
                return false;

            cnx = new OleDbConnection();
            try
            {
                cnx.ConnectionString = provTitle + "=" + provider + ";"
                                          + dataTitle + "=" + dataSource + ";"
                                          + propTitle + "=" + properties;

                cnx.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("Une exeption a ete detecte : " + e.Message);
                return false;
            }

            return true;
        }

        public static bool readXls()
        {
            if (connect())
            {
                string sheet = ConfigIni.GetString(conf, catXls, sheetTitle, defaut);
                if (sheet == defaut)
                    return false;

                cmd = new OleDbCommand("Select * from [" + sheet + "$]", cnx);
                adapt = new OleDbDataAdapter();
                adapt.SelectCommand = cmd;
                adapt.Fill(dataSet, "XLData");

                bool ret = false;

                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                    ret = true;

                disconnect();
                cmd.Dispose();
                adapt.Dispose();

                return ret;
            }
            return false;
        }

        public static void disconnect()
        {
            cnx.Close();
        }

    }
}
