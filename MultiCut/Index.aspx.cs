using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CLBL;
using CLModel;
using System.Drawing;

namespace MultiCut
{
    public partial class WebForm1 : Page
    {
        Repository repo = new Repository();        
        protected void Page_Load(object sender, EventArgs e)
        {
            CreateTable();            
        }
        
        public void CreateTable()
        {            
            string HalName = "Hal 9";
            HalNavn.Text = HalName; 
            
            List<ProductResult> lpr = repo.GetAll(HalName);

            foreach (ProductResult rp in lpr.GroupBy(x => x.EmnrNr).Select(x => x.FirstOrDefault()))
            {
                TableRow tr = new TableRow();
                tr.Height = 50;
                TableCell EmnrNr = new TableCell();
                EmnrNr.Font.Bold = true;
                EmnrNr.Font.Size = 100;
                EmnrNr.Text = rp.EmnrNr;
                tr.Cells.Add(EmnrNr);
                
                foreach (ProductResult pr in lpr.Where(c => c.EmnrNr == rp.EmnrNr))
                {                                     
                    TableCell Resultat = new TableCell();
                    Resultat.Width = 100;
                    switch (pr.Resultat)
                    {
                        case "Ja":
                            Resultat.BackColor = Color.ForestGreen;
                            break;
                        case "Nej":
                            Resultat.BackColor = Color.Red;
                            break;
                        case "Måske":
                            Resultat.BackColor = ColorTranslator.FromHtml("#ebeb00");
                            break;
                    }                                      
                    tr.Cells.Add(Resultat);                                     
                }
                Table1.Rows.Add(tr);
            }           
        }        
    }
}