using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using Microsoft.SharePoint.Client;
using System.Data;
using CLModel;
using System.Runtime.Remoting.Contexts;

namespace CLDB
{
    public class DBConnect
    {

        SecureString pssword = new SecureString();
        string siteUrl = "https://365herningsholm.sharepoint.com/sites/SharePointData";
        ClientContext ctx;
        Web web;
        List list;
        CamlQuery caml;
        ListItemCollection listItems;
        public List<ProductResult> GetAll(string Hal)
        {
            ctx = new ClientContext(siteUrl);
            web = ctx.Web;

            foreach (char c in "igd94ndi".ToCharArray())
            {
                pssword.AppendChar(c);
            }
            ctx.Credentials = new SharePointOnlineCredentials("nick978g@herningsholm.dk", pssword);
            list = web.Lists.GetByTitle("ProductionList");
            caml = CamlQuery.CreateAllItemsQuery();
            listItems = list.GetItems(caml);

            ctx.Load(listItems);
            ctx.ExecuteQuery();

            List<ProductResult> products = new List<ProductResult>();

            foreach (ListItem item in listItems.Where(c => c["Hal"].ToString() == Hal))
            {
                ProductResult result = new ProductResult {
                    EmnrNr = item["Title"].ToString(),
                    Resultat = item["Resultat"].ToString(),
                    Hal = item["Hal"].ToString(),
                    Tekinker = item["Tekniker"].ToString(),
                    Tid = item["Tid"].ToString()
                };

                products.Add(result);
            }
            return products;
        }  
        
        public List<string> GetHalls()
        {
            ctx = new ClientContext(siteUrl);
            web = ctx.Web;

            foreach (char c in "igd94ndi".ToCharArray())
            {
                pssword.AppendChar(c);
            }
            ctx.Credentials = new SharePointOnlineCredentials("nick978g@herningsholm.dk", pssword);
            list = web.Lists.GetByTitle("ProductionList");
            caml = CamlQuery.CreateAllItemsQuery();
            listItems = list.GetItems(caml);

            ctx.Load(listItems);
            ctx.ExecuteQuery();

            List<string> halls = new List<string>();

            foreach (ListItem item in listItems)
            {
                halls.Add(item["Hal"].ToString());
            }
            return halls.Distinct().ToList();
        }
    }
}
