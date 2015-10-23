using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
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
        private const string defaut = "";
        private static SqlConnection conn;
        private static SqlCommand cmd;

        public static bool connect()
        {
            string dataSource = ConfigIni.GetString(conf, catSql, dataTitle, defaut);
            //DbProviderFactory dbpf = DbProviderFactories.GetFactory(provider);
            conn = new SqlConnection();
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

        public static bool manageFamille(String type,String nomFamille, int idFamille=0)
        {
            switch (type){
                case "create":
                    if (exists("Famille", nomFamille, "nom_famille"))
                        return false;
                    cmd = new SqlCommand("Insert into Famille(nom_famille) Value("+nomFamille+")", conn);
                    return cmd.ExecuteNonQuery().Equals(1);
                case "update":
                    if (!exists("Famille", idFamille.ToString(), "id_famille"))
                        return false;
                    cmd = new SqlCommand("Update Famille Set nom_famille =" + nomFamille + " WHERE id_famille="+idFamille, conn);
                    return cmd.ExecuteNonQuery().Equals(1);
                case "delete":
                    if (!exists("Famille", idFamille.ToString(), "id_famille"))
                        return false;
                    cmd = new SqlCommand("Delete from Famille where id_famille = " + idFamille, conn);
                    return cmd.ExecuteNonQuery().Equals(1);
                default:
                    return false;
            }
        }

        public static bool manageSousFamille()
        {
            return false;
        }

        public static bool manageArticle()
        {
            return false;
        }

        public static bool manageMarque(){
        
            return false;
        }

        public static bool exists(String table, String valeur, String colonne){
            cmd = new SqlCommand("Select count(*) from " + table + " WHERE "+colonne+"="+valeur, conn);
            
            return cmd.ExecuteScalar().Equals(1);
        }

    }
}
