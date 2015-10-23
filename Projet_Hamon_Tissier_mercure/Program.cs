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
            if (SQLDataController.connect())
            {
                if (SQLDataController.manageFamille("create", "pc"))
                    Console.WriteLine("famille pc ajoutee");
                else
                    Console.WriteLine("pb creation famille pc");
            }
            else
                Console.WriteLine("pb connection");
        }
    }
}
