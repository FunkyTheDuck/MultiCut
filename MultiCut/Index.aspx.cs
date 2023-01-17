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
        //en instans af klassen Repository
        Repository repo = new Repository();

        string afdelingsNavn = string.Empty;
        //en bool som bliver brugt til at checke om noget køre første gang
        bool first = true;
        //laver en backup liste af modellen oldList
        List<ProductResult> oldList;
        protected void Page_Load(object sender, EventArgs e)
        {
            //et if statement til at checke om det er første gang siden loader 
            if (!IsPostBack)
            {
                //hvis det er første gang køres methoden FillComboBox();
                FillComboBox();
            }
        }
        
        //en private method som laver UI'en til resulterne og har en string i parameteren
        private void CreateTable(string HalName)
        {
            List<ProductResult> lpr;
            //første tjekkes på om der er valgt en afdeling som skal sortes på
            if (!string.IsNullOrEmpty(afdelingsNavn))
            {
                //hvis der er valgt en afdeling så hentes alle værdierne fra SharePointet
                lpr = repo.GetAll(HalName);
                //bagefter bliver der sorteret på hvor kun de resultater med den rigtige afdeling bliver
                if(lpr != null)
                {
                    lpr = lpr.Where(x => x.Afdeling == afdelingsNavn).ToList();
                }
            }
            else
            {
                //hvis der ikke er valgt nogen afdeling bliver alle værdier fra SharePointet hentet
                lpr = repo.GetAll(HalName);
            }
            //laver en liste af modellen ProductResult som for sin værdi fra methoden GetAll i klassen Repository med denne methodes parameter i den parameter
            //laver et if statement som checker på om listen lpr er ligmed null (hvis den er ligmed null har den fejlede i at hente dataen)
            if (lpr == null)
            {
                //hvis lpr er ligmed null, så for lpr sin værdi fra Listen oldList 
                lpr = oldList; 
            }
            else
            {
                //hvis lpr fik hentet noget data så "kopieres" det over i oldList for at gemme værdierne hvis lpr skulle fejle næste gang i at hente data
                oldList = lpr;
            }
            //hvis lpr stadig ikke har nogen værdier er det fordi den fejlede første gang så stoppes methoden med at køre
            if (lpr == null)
                return;
            
            //laver et foreach loop som samler itemsne på EmnrNr og bagefter vælge den første (rp is the row)
            foreach (ProductResult rp in lpr.GroupBy(x => x.EmnrNr).Select(x => x.FirstOrDefault()))
            {
                //laver et HTML TableRow
                TableRow trxkstra = new TableRow();
                //sætter TableRow'et højte til 20px
                trxkstra.Height = 20;
                //tilføjer class'en trxkstraBroder til TableRow'et
                trxkstra.CssClass = "trxkstraBorder";
                //laver endnu et TableRow
                TableRow tr = new TableRow();
                //sætter dens højte til 16px
                tr.Height = 16;
                //tilføjer class'en trBorder til TableRow'et
                tr.CssClass = "trBorder";
                //laver et HTML TableCell
                TableCell EmnrNr = new TableCell();
                //gør TableCell font til bold
                EmnrNr.Font.Bold = true;
                EmnrNr.CssClass = "test";
                //gør TableCell font size til 33px
                EmnrNr.Font.Size = 33;
                //sætter TableCell'ens tekst til at være ligmed modellen rp EmnrNr + :
                EmnrNr.Text = rp.EmnrNr + ":";
                //og tilføjer den til TableRowet tr
                tr.Cells.Add(EmnrNr);           
                
                //laver et foreach loop som køre for hver item i lpr hvor deres EmnrNr er ligmed den EmnrNr i rp modellen (pr is the color/result in the row)
                foreach (ProductResult pr in lpr.Where(c => c.EmnrNr == rp.EmnrNr))
                {
                    //opretter en DateTime som for værdien fra modellens Tid + 2 timer
                    DateTime resultTime = DateTime.Parse(pr.Tid).AddHours(1);
                    //laver et HTML TableCell
                    TableCell Resultat = new TableCell();
                    //og tilføjer den en class som hedder spaceBetweenTR
                    Resultat.CssClass = "spaceBetweenTR";
                    //laver en switch case og checker på modellens Resultat i lower cases
                    switch (pr.Resultat.ToLower())
                    {
                        //der er 5 cases og hver af dem ændre bare backgrunds farven til en anden farce
                        case "ok":
                            Resultat.BackColor = Color.LawnGreen;
                            break;
                        case "not ok":
                            Resultat.BackColor = Color.Red;
                            break;
                        case "måske":
                            Resultat.BackColor = ColorTranslator.FromHtml("#ebeb00"); //gør den gul
                            break;
                        case "måling igang":
                            Resultat.BackColor = Color.White;
                            break;
                        case "pta":
                            Resultat.BackColor = ColorTranslator.FromHtml("#32B4EC"); //gør det lyseblå
                            break;
                    }
                    //laver en string som for værdien fra en sorteret string
                    string testString = string.Empty;
                    //laver en string som for værdien fra DateTime'en resultTime og den skal sorteres
                    string timeString = resultTime.ToString();
                    //køre et for loop 16 gange
                    for (int i = 0; i < 16; i++)
                    {
                        //hvis i er ligmed 6, 7, 8, 9 skal ingenting ske, men hvis i er ligmed 5 skal der tilføjes \n for at skifte ny linje
                        if(i == 5 || i == 6 || i == 7 || i == 8 || i == 9)
                        {
                            if(i == 5)
                            {
                                testString += "\n";
                            }
                        } else //hvis i ikke er ligmed 5, 6, 7, 8 eller 9 så tilføjes en char fra timeString til testString
                        {
                            testString += timeString[i];
                        }
                    }
                    testString += " \n " + pr.SerieNummer;
                    //efter skiftes alle "-" ud med "/" fordi "/" ser bedre ud
                    Resultat.Text = testString.Replace("-", "/");
                    //så ændres fonten til at være bold
                    Resultat.Font.Bold = true;
                    //så checkes på TableRow'et tr child counter og hvis den er mere end 20 skal TableCell'en ikke tilføjes. Så kun de første 20 bliver addet
                    if(tr.Cells.Count < 21)
                    {
                        tr.Cells.Add(Resultat);
                    }
                }
                //laver et hvis statement som køre kun når TableRow'et child counter er mere end 1. Så tilføjes der ikke noget før data'en er ude på UI'et
                if(tr.Cells.Count > 1)
                {
                    //laver et if statement som checker på og det ikke er første gang. Hvis det ikke er første gang bliver trxkstra tilføjet til Table1
                    if(!first)
                    {
                        Table1.Rows.Add(trxkstra);
                    }
                    //efter bliver first til false og tr addes til Table1
                    first = false;
                    Table1.Rows.Add(tr);
                }
            }
        }

        //en private methode om skal køres for at fulde comboboxen op med værdier
        private void FillComboBox()
        {
            //laver en list af strings som for sin værdier fra methoden GetHalls i klassen Repository
            List<string> halls = repo.GetHalls();
            //hvis halls er null fejlede Query'en og methoden stopper
            if (halls == null)
                return;
            //hvis ikke tilføjes en stand ind værdi
            halls.Add("*Vælg hal*");
            //også vendes listen om 
            halls.Reverse();
            //så for comboboxen sine værdier fra listen halls
            HalNavnBox.DataSource = halls;
            HalNavnBox.DataBind();
        }
        //en methode som køres hver gang brugeren ændre værdien i comboboxen
        protected void HalNavnBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //methoden kalder bare en anden methode
            CreateTable(HalNavnBox.SelectedValue);
            //efter at en hals data et blevet hentet bliver dens afdelinger hentet
            FillAfdelingCombobox(HalNavnBox.SelectedValue);
        }
        //en methode som køres hver gang brugeren ændre værdien i afdelings comboboxen
        protected void AfdelingNavnBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //methoden starter med at tjekke på om den standarde værdi er valgt 
            if(AfdelingNavnBox.SelectedValue == "*afdeling*")
            {
                //hvis den er det så ændres stringen afdelingsNavn til at være tom, så der ikke bliver sorteret på afdelinger
                afdelingsNavn = string.Empty;
            }
            //hvis den bliver ændret til en af afdelingerne
            else
            {
                //så bliver stringen afdelingsNavn så til den valgte afdeling, så der bliver sorteret på den
                afdelingsNavn = AfdelingNavnBox.SelectedValue;
            }
            //så kaldes CreateTable med hal navnet for at hente alt i hallen under den valgte afdeling
            CreateTable(HalNavnBox.SelectedValue);
        }
        //en methode til at hente alle afdelinger inden i en hal som tages med i parameteren
        private void FillAfdelingCombobox(string hal)
        {
            //opretter en liste som for sin værdier fra GetAfdelinger 
            List<string> afdelinger = repo.GetAfdelinger(hal);
            //hvis der ikke blev hentet nogen værdier så stopper methoden i at køre videre
            if (afdelinger == null)
                return;
            //hvis der er værdier i listen, så tilføjes en værdi
            afdelinger.Add("*afdeling*");
            //for at få den nye tilføjet værdi overest vejnes der rundt på listen
            afdelinger.Reverse();
            //så sættes DataSource i comboboxen til at være ligmed listen af strings
            AfdelingNavnBox.DataSource = afdelinger;
            AfdelingNavnBox.DataBind();
        }
    }
}