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
using System.Threading;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Reflection;

namespace MultiCut
{
    public partial class WebForm1 : Page
    {
        Repository repo = new Repository();
        bool first = true;
        List<ProductResult> oldList;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillComboBox();
            }
        }
        
        public void CreateTable(string HalName)
        {
            List<ProductResult> lpr = repo.GetAll(HalName);
            if(lpr == null)
            {
                lpr = oldList; //query fejlede
            }
            else
            {
                oldList = lpr;
            }
            if (lpr == null)
                return;
            foreach (ProductResult rp in lpr.GroupBy(x => x.EmnrNr).Select(x => x.FirstOrDefault()))
            {
                TableRow trxkstra = new TableRow();
                trxkstra.Height = 20;
                trxkstra.CssClass = "trxkstraBorder";
                TableRow tr = new TableRow();                
                tr.Height = 16;
                tr.CssClass = "trBorder";
                TableCell EmnrNr = new TableCell();
                EmnrNr.Font.Bold = true;
                EmnrNr.Font.Size = 33;
                EmnrNr.Text = rp.EmnrNr + ":";
                EmnrNr.Width = 6;
                
                tr.Cells.Add(EmnrNr);              
                foreach (ProductResult pr in lpr.Where(c => c.EmnrNr == rp.EmnrNr))
                {
                    DateTime resultTime = DateTime.Parse(pr.Tid).AddHours(2);
                    TableCell Resultat = new TableCell();
                    Resultat.CssClass = "spaceBetweenTR";
                    switch (pr.Resultat.ToLower())
                    {
                        case "ja":
                            Resultat.BackColor = Color.LawnGreen;
                            break;
                        case "nej":
                            Resultat.BackColor = Color.Red;
                            break;
                        case "måske":
                            Resultat.BackColor = ColorTranslator.FromHtml("#ebeb00");
                            break;
                        case "måling igang":
                            Resultat.BackColor = Color.White;
                            break;
                    }
                    string testString = string.Empty;
                    string timeString = resultTime.ToString();
                    for (int i = 0; i < 16; i++)
                    {
                        if(i == 5 || i == 6 || i == 7 || i == 8 || i == 9)
                        {
                            if(i == 5)
                            {
                                testString += "\n";
                            }
                        } else
                        {
                            testString += timeString[i];
                        }
                    }
                    Resultat.Text = testString.Replace("-", "/");
                    Resultat.Font.Bold = true;
                    if(tr.Cells.Count < 31)
                    {
                        tr.Cells.Add(Resultat);
                    }
                }
                if(tr.Cells.Count > 1)
                {
                    if(!first)
                    {
                        Table1.Rows.Add(trxkstra);
                    }
                    first = false;
                    Table1.Rows.Add(tr);
                }
            }
        }
        public void FillComboBox()
        {
            List<string> halls = repo.GetHalls();
            if (halls == null)
                return;
            halls.Add("*Vælg hal*");
            halls.Reverse();
            HalNavnBox.DataSource = halls;
            HalNavnBox.DataBind();
        }
        protected void HalNavnBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateTable(HalNavnBox.SelectedValue);
        }
    }
}