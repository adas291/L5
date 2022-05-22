﻿using System;
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
        
        const string CFR1 = @"Rezultatas.txt";
        public static bool flag = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if(!flag)
            //{
            //    Session["Directory"] = null;
            //    Session["ServerSpec"] = null;
            //    Session["Idle"] = null;
            //}
            //List<Server> servers;
            //List<Server> emails;
            //List<Tuple<string, DateTime, int>> idle;

            //if (Session["Directory"] != null)
            //{
            //}
            //if (Session["ServerSpec"] != null)
            //{
            //    servers = (List<Server>)Session["ServerSpec"];
            //    InOut.DrawSpecificationsTable(servers, form1);

            //}
            //if (Session["EmailData"] != null)
            //{
            //    emails = (List<Server>)Session["EmailData"];
            //    InOut.DisplayServerData(emails, form1);
            //}
            //if (Session["Idle"] != null)
            //{
            //    idle = (List<Tuple<string, DateTime, int>>)Session["Idle"];
            //    InOut.PrintIdleHours(idle, form1);
            //}
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

            Session["directory"] = directory;

            directory.Create();

            string path = Path.Combine(directory.FullName + "Spec.txt");

            InOut.SaveFilesToDirectory(directory, FileUpload1, Table1);

            List<Server> Servers;
            List<Server> EmailData;

            try
            {
                Servers = InOut.ReadServerInfo("Spec.txt", directory);
                Session["ServerSpec"] = Servers;
            }
            catch (Exception ex)
            {
                InOut.FillTable(new List<string> { ex.Message }, Table1);
                return;
            }

            List<string> Errors = new List<string>();

            EmailData = InOut.ReadEmailData(directory, "Spec.txt", Errors, Servers);
            Session["EmailData"] = EmailData;

            if (Errors.Count > 0)
            {
                InOut.FillTable(Errors, Table1);
            }



            InOut.DrawSpecificationsTable(Servers, form1);
            InOut.DisplayServerData(EmailData, form1);


            TaskUtils.AssignDuration(EmailData, Servers, Table1);

            var idleInfo = TaskUtils.FindIdleHours(EmailData).OrderBy(s => s.Item1).ThenByDescending(s => s.Item2).ToList();
            Session["Idle"] = idleInfo;



            InOut.PrintIdleHours(idleInfo, form1);

            Button2.Visible = true;
            flag = true;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            try
            {
                List<Server> serverInfo = (List<Server>)Session["ServerSpec"];
                List<Server> emails = (List<Server>)Session["EmailData"];
                var Idle = (List<Tuple<string, DateTime, int>>)Session["Idle"];
                DirectoryInfo directory = (DirectoryInfo)Session["Directory"];
                InOut.InputDataToTxt(serverInfo, emails, directory.FullName + @"/Rezultatas.txt");
                InOut.PrintResultsToTxt(Idle, directory.FullName + CFR1);
            }
            catch (Exception ex)
            {
                Label3.Text = $"{ex.Message}";
                Label3.ForeColor = System.Drawing.Color.Red;
                Label3.Visible = true;
                return;
            }
            Label3.Text = $"Rezultatai sėkmingai išsaugoti {@"Rezultatas.txt"}.";
            Label3.Visible = true;
        }
    }
}