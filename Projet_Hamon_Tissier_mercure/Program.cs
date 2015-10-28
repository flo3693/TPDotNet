using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Projet_Hamon_Tissier_mercure
{
    class Program
    {
        static void Main(string[] args)
        {
            if (importSqlFromExcel())
                Console.WriteLine("importation reussie");
            else
                Console.WriteLine("Echec lors de l'importation");
        }



        static bool importSqlFromExcel()
        {
            if (!ExcelDataController.connect())
            {
                Console.WriteLine("Probleme ouverture fichier xls");
                return false;
            }
            if (!ExcelDataController.readXls())
            {
                Console.WriteLine("Probleme lecture fichier xls");
                ExcelDataController.disconnect();
                return false;
            }
            DataSet data = ExcelDataController.DataSet;
            ExcelDataController.disconnect();

            if (!SQLDataController.connect())
            {
                Console.WriteLine("Probleme connexion bdd");
                return false;
            }

            foreach (DataRow row in data.Tables[0].Rows)
            {
                int idFamille = SQLDataController.createFamille(row[3].ToString());
                int idMarque = SQLDataController.createMarque(row[2].ToString());
                if (idFamille == 0)
                {
                    Console.WriteLine("Probleme creation famille bdd");
                    SQLDataController.disconnect();
                    return false;
                }
                int idSsFamille = SQLDataController.createSousFamille(row[4].ToString(), idFamille);
                if (idSsFamille == 0 || idMarque == 0)
                {
                    Console.WriteLine("Probleme creation marque ou sousFamille bdd");
                    SQLDataController.disconnect();
                    return false;
                }
                int idArticle = SQLDataController.createArticle(idMarque, row[1].ToString(), row[5].ToString().Replace(",", "."), row[0].ToString(), idSsFamille);
                if (idArticle == 0)
                {
                    Console.WriteLine("Probleme creation article bdd");
                    SQLDataController.disconnect();
                    return false;
                }
            }

            SQLDataController.disconnect();
            return true;
        }

    }
}
