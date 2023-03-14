using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint;
using System.Data;
using CLModel;
using System.Runtime.Remoting.Contexts;

namespace CLDB
{
    public class DBConnect
    {
        //laver en secure string som bruges som kodeord når programmet skal tilgå SharePointet
        SecureString pssword = new SecureString();
        //laver en string som har værdien af URL'en til SharePointes home screen
        string siteUrl = "https://multicutdk.sharepoint.com/sites/MulticutResultat";

        ClientContext ctx;

        Web web;

        //dette er en Microsoft.SharePoint.Client List og ikke System.collections.Generic List
        List list;

        CamlQuery caml;

        ListItemCollection listItems;

        //en public methode som skal returner en Liste af modellen ProductResult. Methoden har en string i parameteren som skal bruges til at sortere data'en som hentes
        public List<ProductResult> GetAll(string Hal)
        {
            //opretter en connection til URL'en
            ctx = new ClientContext(siteUrl);
            //tager connectionen og henter Web delen af den
            web = ctx.Web;
            //laver et foreach loop som looper igennem en CharArray (CharArray skal være ligmed brugerens adgangskode)
            foreach (char c in "Gm27Libm!2xKv?da*".ToCharArray())
            {
                //for hver Char i loopet tages den Char og den tilføjes til SecureString'en pssword
                pssword.AppendChar(c);
            }
            //sætter connectionen Credentials til at være ligmed brugerens Email og Kodeord
            ctx.Credentials = new SharePointOnlineCredentials("herningsholm@multicut.dk", pssword);
            //sætter listen til at være ligmed listen i SharePointet hvor navnet er "ProductionList"
            list = web.Lists.GetByTitle("ProductionList");
            //laver en Query som skal hente all items i en liste
            caml = CamlQuery.CreateAllItemsQuery();
            //sætter ListItems til at være ligmed den fundet liste i SharePointets værdierer ved hjælp af Query'en caml
            listItems = list.GetItems(caml);
            //tilføjer listItems til ctx så den kan køres
            ctx.Load(listItems);

            //indtil videre er der ikke noget data som er blevet hentet, men alt er opsat og skal til at køres inde i denne Try Catch
            try
            {
                //køre Query som henter data'en fra SharePointet hvis denne 
                ctx.ExecuteQuery();
            }
            catch
            {
                //hvis Query'en fejlede returneres ingenting. Største årsager til Query fejl er fejl i email, kodeord eller navnet på listen
                return null;
            }

            //laver en ny liste af modellen ProductResult som får værdierne fra det hentede data efter det er blevet sorteret
            List<ProductResult> products = new List<ProductResult>();

            //laver en foreach loop som køre for hver item i SharePoint listen hvor Hal i modellen er ligmed Hal i parameteren og Resultat i modellen ikke er Skal måles 
            // og data'en er sorteret på Modified som er en dato i SharePointet
            foreach (ListItem item in listItems.Where(c => c["Hal"].ToString() == Hal && c["Resultat"].ToString() != "Skal måles").OrderByDescending(x => x["Modified"]))
            {
                //for hver items laves en ny ProductResult model som for værdierne fra itemet som bliver checket på
                ProductResult result = new ProductResult();
                try
                {
                    result = new ProductResult
                    {
                        EmnrNr = item["Title"].ToString(),
                        Afdeling = item["Afdeling"]?.ToString(),
                        Resultat = item["Resultat"].ToString(),
                        Hal = item["Hal"].ToString(),
                        Tekinker = item["Tekniker"].ToString(),
                        Tid = item["Modified"].ToString(),
                        OrderNummer = item["Ordrenr"].ToString(),
                        SerieNummer = item["S_x002f_N"].ToString() // "S_x002f_N" står for S/N 
                    };
                    result.EmnrNr = $"{result.EmnrNr} / {result.OrderNummer}";
                    //efter tilføjes den nye model til listen products
                    products.Add(result);
                }
                catch
                {

                }
            }
            //methoden slutter med at returner hele listen products
            return products;
        }

        //en public methode som skal returner en Liste af modellen ProductResult
        public List<string> GetHalls()
        {
            //opretter en connection til URL'en
            ctx = new ClientContext(siteUrl);
            //tager connectionen og henter Web delen af den
            web = ctx.Web;
            //laver et foreach loop som looper igennem en CharArray (CharArray skal være ligmed brugerens adgangskode)
            foreach (char c in "Gm27Libm!2xKv?da*".ToCharArray())
            {
                //for hver Char i loopet tages den Char og den tilføjes til SecureString'en pssword
                pssword.AppendChar(c);
            }
            //sætter connectionen Credentials til at være ligmed brugerens Email og Kodeord
            ctx.Credentials = new SharePointOnlineCredentials("herningsholm@multicut.dk", pssword);
            //sætter listen til at være ligmed listen i SharePointet hvor navnet er "ProductionList"
            list = web.Lists.GetByTitle("ProductionList");
            //laver en Query som skal hente all items i en liste
            caml = CamlQuery.CreateAllItemsQuery();
            //sætter ListItems til at være ligmed den fundet liste i SharePointets værdierer ved hjælp af Query'en caml
            listItems = list.GetItems(caml);
            //tilføjer listItems til ctx så den kan køres
            ctx.Load(listItems);

            //indtil videre er der ikke noget data som er blevet hentet, men alt er opsat og skal til at køres inde i denne Try Catch
            try
            {
                //køre Query som henter data'en fra SharePointet hvis denne 
                ctx.ExecuteQuery();
            }
            catch
            {
                //hvis Query'en fejlede returneres ingenting
                return null;
            }
            //en ny liste af strings som skal opbevarer hall navnene
            List<string> halls = new List<string>();
            //en foreach loop som køre igennem hvert item i listen i SharePointet
            foreach (ListItem item in listItems)
            {
                //tilføjer hallen fra itemet til string listen
                halls.Add(item["Hal"].ToString());
            }
            //returnere listen halls efter at have sorteret den så der kun er en af hver og formatteret den til en liste igen
            return halls.Distinct().ToList();
        }
        public List<string> GetAfdelinger(string hal)
        {
            //en ny liste af strings som skal opbevarer hall navnene
            List<string> afdelinger = new List<string>();
            if(listItems != null)
            {
                //en foreach loop som køre igennem hvert item i listen i SharePointet
                foreach (ListItem item in listItems.Where(x => x["Hal"].ToString() == hal))
                {
                    if (item["Afdeling"] != null)
                    {
                        //tilføjer hallen fra itemet til string listen
                        afdelinger.Add(item["Afdeling"].ToString());
                    }
                }
            }
            if(afdelinger != null)
            {
                //returnere listen halls efter at have sorteret den så der kun er en af hver og formatteret den til en liste igen
                return afdelinger.Distinct().ToList();
            }
            return null;
        }
    }
}