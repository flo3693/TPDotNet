using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace Projet_Hamon_Tissier_mercure
{
    class SQLDataController
    {
        private const string conf = "Jet2.txt";
        private const string catSql = "sql";
        private const string dataTitle = "Data Source";
        private const string provTitle = "Provider";
        private const string defaut = "";
        private static SqlCeConnection conn;
        private static SqlCeCommand cmd;

        public static bool connect()
        {
            string provider = ConfigIni.GetString(conf, catSql, provTitle, defaut);
            string dataSource = ConfigIni.GetString(conf, catSql, dataTitle, defaut);
            //DbProviderFactory dbpf = DbProviderFactories.GetFactory(provider);
            conn = new SqlCeConnection();
             try
            {
                conn.ConnectionString = dataTitle + "=" + dataSource +";";
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("Une exeption a ete detecte : " + e.Message);
                return false;
            }

            return true;
        }

        public static void disconnect()
        {
            conn.Close();
        }

        public static int createFamille(String nomFamille)
        {
            cmd = new SqlCeCommand("Select id_famille from Famille where nom_famille ='"+nomFamille+"'", conn);
            var res = cmd.ExecuteScalar();
            if(res != null)
                return (int)res;
            else
            {
                cmd = new SqlCeCommand("Insert into Famille(nom_famille) Values('" + nomFamille + "')", conn);
                if(!cmd.ExecuteNonQuery().Equals(1))
                    return 0;
                cmd = new SqlCeCommand("Select max(id_famille) From Famille", conn);
                var res2 = cmd.ExecuteScalar();
                if (res2 != null)
                    return (int)res2;
                else
                    return 0;
            }
        }

        public static int createSousFamille(String nomSsFamille, int idFamille)
        {
            cmd = new SqlCeCommand("Select id_ssFamille from SousFamille where nom_ssFamille ='" + nomSsFamille + "'", conn);
            var res = cmd.ExecuteScalar();
            if (res != null)
                return (int)res;
            else
            {
                cmd = new SqlCeCommand("Insert into SousFamille(nom_ssFamille, id_famille) Values('" + nomSsFamille + "'," + idFamille + ")", conn);
                if (!cmd.ExecuteNonQuery().Equals(1))
                    return 0;
                cmd = new SqlCeCommand("Select max(id_ssFamille) From SousFamille", conn);
                var res2 = cmd.ExecuteScalar();
                if (res2 != null)
                    return (int)res2;
                else
                    return 0;
            }
        }

        public static int createArticle(int idMarque, String refArticle, string prixHT, string description, int idSsFamille)
        {

            cmd = new SqlCeCommand("Select id_article from Article where ref_article ='" + refArticle + "'", conn);
            var res = cmd.ExecuteScalar();
            if (res != null)
                return (int)res;
            else
            {
                cmd = new SqlCeCommand("Insert into Article(ref_article, prixHT, description, id_marque, id_ssFamille) "
                                + " Values('" + refArticle + "'," + prixHT + ",'" + description + "'," + idMarque + "," + idSsFamille + ")", conn);
                if (!cmd.ExecuteNonQuery().Equals(1))
                    return 0;
                cmd = new SqlCeCommand("Select max(id_article) From Article", conn);
                var res2 = cmd.ExecuteScalar();
                if (res2 != null)
                    return (int)res2;
                else
                    return 0;
            }
        }

        public static int createMarque(String nomMarque)
        {
            cmd = new SqlCeCommand("Select id_marque from Marque where nom_marque ='" + nomMarque + "'", conn);
            var res = cmd.ExecuteScalar();
            if (res != null)
                return (int)res;
            else
            {
                cmd = new SqlCeCommand("Insert into Marque(nom_marque) Values('" + nomMarque + "')", conn);
                if (!cmd.ExecuteNonQuery().Equals(1))
                    return 0;
                cmd = new SqlCeCommand("Select max(id_marque) From Marque", conn);
                var res2 = cmd.ExecuteScalar();
                if (res2 != null)
                    return (int)res2;
                else
                    return 0;
            }
        }

        public static bool exists(String table, String valeur, String colonne)
        {
            cmd = new SqlCeCommand("Select count(*) from " + table + " WHERE " + colonne + "='" + valeur+"'", conn);
            return cmd.ExecuteScalar().Equals(1);
        }

        public static bool exists(String table, double valeur, String colonne)
        {
            cmd = new SqlCeCommand("Select count(*) from " + table + " WHERE " + colonne + "=" + valeur , conn);
            return cmd.ExecuteScalar().Equals(1);
        }


    }
}
