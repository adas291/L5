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

            string path = Path.Combine(directory.FullName + "Spec.txt");

            InOut.SaveFilesToDirectory(directory, FileUpload1, Table1);

            List<Server> Servers = InOut.ReadServerInfo("Spec.txt", directory);

            InOut.DrawSpecificationsTable(Servers, form1);

            List<Server> EmailData = InOut.ReadEmailData(directory, Servers, "Spec.txt");

            InOut.DisplayServerData(EmailData, form1);



            TaskUtils.AssignDuration(EmailData, Servers);
            InOut.DisplayServerData(EmailData, form1, true);

            InOut.PrintIdleHours(TaskUtils.FindIdleHours(EmailData), form1);

        }
    }
}