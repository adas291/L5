using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;

namespace L5.Code
{
    public class TaskUtils
    {
        public static void FillTable(List<string> errors, Table table)
        {
            foreach (var item in errors)
            {
                TableCell cell = new TableCell();
                TableRow row = new TableRow();

                cell.Text = item;
                row.Cells.Add(cell);
                table.Rows.Add(row);
            }
        }


        //public static void DrawTable(List<Server> servers)
        //{
        //    string[] eilute = { "Vardas", "Pavarde", "Nr" };
        //    for (int i = 0; i < 4; i++)
        //    {
        //        Table table = new Table();
        //        TableRow row = new TableRow();

        //        foreach (var item in eilute)
        //        {
        //            TableCell cell = new TableCell();
        //            cell.Text = item;
        //            row.Cells.Add(cell);
        //            table.Rows.Add(row);
        //        }

        //        Label label = new Label();
        //        label.Text = $"Lentele nr{i}";
        //        label.Visible = true;
        //        row.Visible = true;

        //        table.BorderStyle = BorderStyle.Double;
        //        table.Attributes["gridlines"] = "both";
        //        Forma1.Controls.Add(table);
        //        Forma1.Controls.Add(label);
        //    }
        //}
    }
}