using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using L5.Code;

namespace L5
{
    public partial class Forma1 : System.Web.UI.Page
    {
        const string CFD1 = @"~/App_Data/ServerSpecifications.txt";
        const string CFR1 = @"~/App_Data/Rezultatai.txt";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(Server.MapPath("~/App_Data/")))
                {
                    Directory.Delete(Server.MapPath("~/App_Data/"), true);
                }
            }
            catch (IOException)
            {
                Label2.Visible = true;
                Label2.Text = "Please close opened result file and try again.";
            }

            DirectoryInfo directory = new DirectoryInfo(Server.MapPath(@"~/App_Data/"));

            directory.Create();

            InOut.SaveFilesToDirectory(directory, FileUpload1, Table1);
            List<Server> Servers = InOut.ReadServerInfo("Specifications.txt", directory);





            foreach (var item in Servers)
            {
                Table table = new Table();
                TableRow row = new TableRow();
                TableCell cell1 = new TableCell();
                TableCell cell2 = new TableCell();


                cell1.Text = $"{item.Name}";
                cell2.Text = item.InterfaceSpeed.ToString();

                row.Cells.Add(cell1);
                row.Cells.Add(cell2);

                table.Rows.Add(row);

                table.BorderStyle = BorderStyle.Double;
                table.Attributes["gridlines"] = "both";

                form1.Controls.Add(table);

            }


            //string[] eilute = { "Vardas", "Pavarde", "Nr" };
            //for (int i = 0; i < 4; i++)
            //{
            //    Table table = new Table();
            //    TableRow row = new TableRow();

            //    foreach (var item in eilute)
            //    {
            //        TableCell cell = new TableCell();
            //        cell.Text = item;
            //        row.Cells.Add(cell);
            //        table.Rows.Add(row);
            //    }

            //    Label label = new Label();
            //    label.Text = $"Lentele nr{i}";
            //    label.Visible = true;
            //    row.Visible = true;

            //    table.BorderStyle = BorderStyle.Double;
            //    table.Attributes["gridlines"] = "both";
            //    form1.Controls.Add(table);
            //    form1.Controls.Add(label);
            //}
            
        }
    }
}