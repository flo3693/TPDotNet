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
            if(ExcelDataController.readXls()){
                DataSet dataSet = ExcelDataController.DataSet;
                foreach (DataRow row in dataSet.Tables[0].Rows) {
                    Console.WriteLine(row[1].ToString()); // row[0] = 1ere colonne, row[1] = 2eme colonne
                }
            }
        }
    }
}
