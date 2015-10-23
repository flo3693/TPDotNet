using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Hamon_Tissier_mercure
{
    public static class ConfigIni
    {
        public static string GetString(string fileName, string section, string name, string defaut)
        {
            string line;
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(fileName);
                while ((line = file.ReadLine()) != null)
                {
                    if (line.TrimStart(' ').StartsWith("[" + section + "]"))
                        break;
                }
                while ((line = file.ReadLine()) != null)
                {
                    if (line.TrimStart(' ').StartsWith("["))
                        break;
                    if (line.TrimStart(' ').StartsWith(name + "="))
                    {
                        int indexEgal = line.IndexOf('=') + 1;
                        return line.Substring(indexEgal , line.Length - indexEgal);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Une exeption a ete detecte : "+e.Message);
            }
            return defaut;
        }

        public static int GetInteger(string fileName, string section, string name, int defaut)
        {
            string strVal = GetString(fileName, section, name, "");
            int val;
            if(strVal == "" || !int.TryParse(strVal, out val))
                return defaut;
            return val;
        }

        public static void WriteString(string fileName, string section, string name, string value)
        {
            try
            {
                System.IO.StreamReader fileR = new System.IO.StreamReader(fileName);
                List<string> tempFile = new List<string>();
                Boolean sectionExiste = false;
                Boolean paramExiste = false;
                string line;
                while (!sectionExiste && (line = fileR.ReadLine()) != null)
                {
                    tempFile.Add(line);
                    if (line.TrimStart(' ').StartsWith("[" + section + "]"))
                    {
                        sectionExiste = true;
                        break;
                    }
                }
                if (sectionExiste)
                {
                    while (((line = fileR.ReadLine()) != null) && !line.TrimStart(' ').StartsWith("["))
                    {
                        if (line.TrimStart(' ').StartsWith(name + "="))
                        {
                            tempFile.Add(name + "=" + value);
                            paramExiste = true;
                        }
                        if (!paramExiste)
                            tempFile.Add(line);
                    }
                    if (!paramExiste)
                        tempFile.Add(name + "=" + value);
                    if (line != null)
                        tempFile.Add(line);
                    while ((line = fileR.ReadLine()) != null)
                        tempFile.Add(line);
                }
                else
                {
                    tempFile.Add("[" + section + "]");
                    tempFile.Add(name + "=" + value);
                }
                fileR.Close();

                System.IO.StreamWriter fileW = new System.IO.StreamWriter(fileName, false);
                foreach (string l in tempFile.ToArray())
                    fileW.WriteLine(l);
                fileW.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("Erreur survenue lors du traitement du fichier : " + e.Message);
            }

        }

        public static void WriteInteger(string fileName, string section, string name, int value)
        {
            WriteString(fileName, section, name, value.ToString());
        }
    }
}
