using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLModel;
using CLDB;

namespace CLBL
{
    public class Repository
    {
        //en instans på klassen DBConnect 
        DBConnect dbcon = new DBConnect();

        //en public methode som returner en Liste af modellen ProductResult. Methoden har en string i parameteren som skal bruges til at sortere data'en som hentes
        public List<ProductResult> GetAll(string Hal)
        {
            //returner returværdien fra methoden GetAll i klassen DBConnect og har denne methodes parameter i dens parameter
            return dbcon.GetAll(Hal);
        }
        //en public methode som returner en Liste af modellen ProductResult
        public List<string> GetHalls()
        {
            //returner returværdien fra methoden GetHalls i klassen DBConnect
            return dbcon.GetHalls();
        }
        //en public methode som retuner en liste af strings
        public List<string> GetAfdelinger(string hal)
        {
            try
            {
                return dbcon.GetAfdelinger(hal);
            }
            catch
            {
                return null;
            }
        }
    }
}