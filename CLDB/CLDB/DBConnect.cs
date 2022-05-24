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
        SecureString pssword = new SecureString();
        string siteUrl = "https://multicutdk.sharepoint.com/sites/MulticutResultat";
        ClientContext ctx;
        Web web;
        List list;
        FieldChoice everyHall;
        CamlQuery caml;
        ListItemCollection listItems;
        public List<ProductResult> GetAll(string Hal)
        {
            ctx = new ClientContext(siteUrl);
            web = ctx.Web;
            foreach (char c in "Gm27Libm!2xKv?da*".ToCharArray())
            {
                pssword.AppendChar(c);
            }
            ctx.Credentials = new SharePointOnlineCredentials("herningsholm@multicut.dk", pssword);
            list = web.Lists.GetByTitle("ProductionList");
            caml = CamlQuery.CreateAllItemsQuery();
            listItems = list.GetItems(caml);
            ctx.Load(listItems);
            try
            {
                ctx.ExecuteQuery();
            }
            catch
            {
                return null;
            }
            List<ProductResult> products = new List<ProductResult>();
            foreach (ListItem item in listItems.Where(c => c["Hal"].ToString() == Hal))
            {
                ProductResult result = new ProductResult {
                    EmnrNr = item["Title"].ToString(),
                    Resultat = item["Resultat"].ToString(),
                    Hal = item["Hal"].ToString(),
                    Tekinker = item["Tekniker"].ToString(),
                    Tid = item["Created"].ToString()
                };
                products.Add(result);
            }
            products.Reverse();
            return products;
        }  
        public List<string> GetHalls()
        {
            ctx = new ClientContext(siteUrl);
            web = ctx.Web;

            foreach (char c in "Gm27Libm!2xKv?da*".ToCharArray())
            {
                pssword.AppendChar(c);
            }
            ctx.Credentials = new SharePointOnlineCredentials("herningsholm@multicut.dk", pssword);
            list = web.Lists.GetByTitle("ProductionList");
            caml = CamlQuery.CreateAllItemsQuery();
            listItems = list.GetItems(caml);

            ctx.Load(listItems);
            
            try
            {
                ctx.ExecuteQuery();
            }
            catch
            {
                return null;  //den fejlede i at hente halls
            }

            List<string> halls = new List<string>();
            foreach (ListItem item in listItems)
            {
                halls.Add(item["Hal"].ToString());
            }
            return halls.Distinct().ToList();
        }

        //testing down under


        public List<string> GetHallsTEST()
        {
            ctx = new ClientContext(siteUrl);
            web = ctx.Web;

            foreach (char c in "Gm27Libm!2xKv?da*".ToCharArray())
            {
                pssword.AppendChar(c);
            }
            ctx.Credentials = new SharePointOnlineCredentials("herningsholm@multicut.dk", pssword);
            list = web.Lists.GetByTitle("ProductionList");
            caml = CamlQuery.CreateAllItemsQuery();
            listItems = list.GetItems(caml);

            ctx.Load(listItems);
            try
            {
                ctx.ExecuteQuery();
            }
            catch
            {
                return null;  //den fejlede i at hente halls
            }
            
            //everyHall = (FieldChoice)list.Fields[0].InternalName;
            string test = list.Fields[0].InternalName;



            //for (int i = 0; i < everyHall.Choices.Count; i++)
            //{

            //}

            List<string> halls = new List<string>();
            foreach (ListItem item in listItems)
            {
                halls.Add(item["Hal"].ToString());
            }
            return halls;
        }
    }
}