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

        public static bool manageSousFamille(String type, String nomSsFamille, int idSsFamille = 0, int idFamille = 0)
        {
            switch (type)
            {
                case "create":
                    if (exists("SousFamille", nomSsFamille, "nom_ssfamille"))
                        return false;
                    cmd = new SqlCommand("Insert into SousFamille(nom_ssfamille, id_famille) Value(" + nomSsFamille + ","+ idFamille +")", conn);
                    return cmd.ExecuteNonQuery().Equals(1);
                case "updateNom":
                    if (!exists("SousFamille", idSsFamille.ToString(), "id_ssfamille"))
                        return false;
                    cmd = new SqlCommand("Update SousFamille Set nom_ssfamille =" + nomSsFamille + " WHERE id_ssfamille=" + idSsFamille, conn);
                    return cmd.ExecuteNonQuery().Equals(1);
                case "updateFamille":
                    if (!exists("SousFamille", idSsFamille.ToString(), "id_ssfamille"))
                        return false;
                    cmd = new SqlCommand("Update SousFamille Set id_famille =" + idFamille + " WHERE id_ssfamille=" + idSsFamille, conn);
                    return cmd.ExecuteNonQuery().Equals(1);
                case "delete":
                    if (!exists("SousFamille", idSsFamille.ToString(), "id_ssfamille"))
                        return false;
                    cmd = new SqlCommand("Delete from SousFamille where id_ssfamille = " + idSsFamille, conn);
                    return cmd.ExecuteNonQuery().Equals(1);
                default:
                    return false;
            }
        }

        public static bool manageArticle(String type, String nomArticle, int idArticle = 0, int idMarque = 0)
        {
            switch (type)
            {
                case "create":
                    if (exists("Article", nomArticle, "nom_article"))
                        return false;
                    cmd = new SqlCommand("Insert into Article(nom_article, id_marque) Value(" + nomArticle + "," + idMarque + ")", conn);
                    return cmd.ExecuteNonQuery().Equals(1);
                case "updateNom":
                    if (!exists("Article", idArticle.ToString(), "id_article"))
                        return false;
                    cmd = new SqlCommand("Update Article Set nom_article =" + nomArticle + " WHERE id_article=" + idArticle, conn);
                    return cmd.ExecuteNonQuery().Equals(1);
                case "updateMarque":
                    if (!exists("Article", idArticle.ToString(), "id_article"))
                        return false;
                    cmd = new SqlCommand("Update Article Set id_marque =" + idMarque + " WHERE id_article=" + idArticle, conn);
                    return cmd.ExecuteNonQuery().Equals(1);
                case "delete":
                    if (!exists("Article", idArticle.ToString(), "id_article"))
                        return false;
                    cmd = new SqlCommand("Delete from Article where id_article = " + idArticle, conn);
                    return cmd.ExecuteNonQuery().Equals(1);
                default:
                    return false;
            }
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
