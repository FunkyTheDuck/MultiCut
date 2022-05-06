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

namespace MultiCut
{
    public partial class WebForm1 : Page
    {
        Repository repo = new Repository();
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
                    TableCell Resultat = new TableCell();
                    Resultat.CssClass = "spaceBetweenTR";
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
                    Resultat.Text = pr.Tid.Replace(":", "\n");
                    Resultat.Font.Bold = true;
                    tr.Cells.Add(Resultat);
                }
                Table1.Rows.Add(tr);
                Table1.Rows.Add(trxkstra);
            }
        }
        public void FillComboBox()
        {
            List<string> halls = repo.GetHalls();
            HalNavnBox.DataSource = halls;
            HalNavnBox.DataBind();
        }
        protected void HalNavnBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateTable(HalNavnBox.SelectedValue);
        }
    }
}