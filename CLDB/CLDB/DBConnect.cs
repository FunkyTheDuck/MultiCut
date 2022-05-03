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
        public List<ProductResult> GetAll(string Hal)
        {
            SecureString pssword = new SecureString();
            string siteUrl = "https://365herningsholm.sharepoint.com/sites/SharePointData";
            ClientContext ctx = new ClientContext(siteUrl);
            Web web = ctx.Web;
            foreach (char c in "igd94ndi".ToCharArray())
            {
                pssword.AppendChar(c);
            }
            ctx.Credentials = new SharePointOnlineCredentials("nick978g@herningsholm.dk", pssword);
            List list = web.Lists.GetByTitle("ProductionList");
            CamlQuery caml = CamlQuery.CreateAllItemsQuery();
            ListItemCollection listItems = list.GetItems(caml);

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
    }
}
