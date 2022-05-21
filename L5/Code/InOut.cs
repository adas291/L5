using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;


namespace L5.Code
{
    public class InOut
    {
        public static void SaveFilesToDirectory(DirectoryInfo directory, FileUpload fileUpload, Table table)
        {
            List<string> errors = new List<string>();

            foreach (var file in fileUpload.PostedFiles)
            {
                try
                {
                    if (file.FileName.EndsWith(".txt"))
                    {
                        file.SaveAs(Path.Combine(directory.FullName + file.FileName));
                    }
                    else
                    {
                        throw new Exception($"Format of {file.FileName} is not correct!");
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                }
            }
            if (errors.Count > 0)
            {
                InOut.FillTable(errors, table);
            }
        }
        public static List<Server> ReadServerInfo(string fileName, DirectoryInfo directory)
        {
            if (File.Exists(directory.FullName + fileName))
            {
                List<Server> servers = new List<Server>();
                foreach (var item in directory.GetFiles())
                {
                    if (item.Name.CompareTo(fileName) == 0)
                    {
                        using (StreamReader sr = new StreamReader(item.FullName))
                        {

                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                string[] parts = line.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                                string name = parts[0];
                                double speed = Convert.ToDouble(parts[1]);
                                Server server = new Server(name, speed);
                                servers.Add(server);
                            }
                            return servers;
                        }
                    }
                }
                throw new Exception($"{fileName} is missing from directory");
            }
            else
            {
                throw new Exception($"There is no file named {fileName} ");
            }

        }
        public static List<Server> ReadEmailData(DirectoryInfo directory, List<Server> servers, string specFile)
        {
            List<Server> serverList = new List<Server>();
            foreach (var file in directory.GetFiles())
            {
                if (file.Name != specFile)
                {
                    using (StreamReader sr = new StreamReader(file.FullName))
                    {

                        string[] firstLine = sr.ReadLine().Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                        int year = Convert.ToInt32(firstLine[0]);
                        int month = Convert.ToInt32(firstLine[1]);
                        int day = Convert.ToInt32(firstLine[2]);
                        string serverName = firstLine[3];
                        DateTime date = new DateTime(year, month, day);
                        string line;

                        List<Email> emails = new List<Email>();
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] parts = line.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                            int hour = Convert.ToInt32(parts[0]);
                            int min = Convert.ToInt32(parts[1]);
                            string senderAddress = parts[2];
                            string receiverAddress = parts[3];
                            int emailSize = Convert.ToInt32(parts[4]);
                            DateTime emailDate = new DateTime(year, month, day, hour, min, 1);
                            //emailDate.AddHours(hour);
                            //emailDate.AddMinutes(min);
                            Email email = new Email(senderAddress, receiverAddress, emailSize, serverName, emailDate);
                            emails.Add(email);
                        }

                        Server temp = new Server(serverName, emails, date);
                        serverList.Add(temp);
                    }
                }
            }
            return serverList;
        }
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
        public static void DisplayServerData(List<Server> servers, System.Web.UI.HtmlControls.HtmlForm forma1)
        {
            if (servers.Count > 0)
            {

                foreach (var server in servers)
                {


                    Label label = new Label();
                    label.Text = server.ToString();

                    TableCell sender = new TableCell();
                    TableCell receiver = new TableCell();
                    TableCell targetServer = new TableCell();
                    TableCell Date = new TableCell();

                    sender.Text = "Siuntėjas";
                    receiver.Text = "Gavėjas";
                    targetServer.Text = "Serveris į kurį siunčiama";
                    Date.Text = "Data";

                    TableRow row = new TableRow();
                    row.Cells.Add(sender);
                    row.Cells.Add(receiver);
                    row.Cells.Add(targetServer);
                    row.Cells.Add(Date);

                    Table table = new Table();
                    table.Rows.Add(row);

                    foreach (var email in server.EmailTraffic)
                    {
                        sender = new TableCell();
                        receiver = new TableCell();
                        targetServer = new TableCell();
                        Date = new TableCell();

                        sender.Text = email.SenderAddress;
                        receiver.Text = email.ReceiverAddress;
                        targetServer.Text = email.TargeServer;
                        Date.Text = email.SendTime.ToString();

                        row = new TableRow();
                        row.Cells.Add(sender);
                        row.Cells.Add(receiver);
                        row.Cells.Add(targetServer);
                        row.Cells.Add(Date);

                        table.Rows.Add(row);

                    }
                    forma1.Controls.Add(table);

                }
            }

        }
        public static void DrawSpecificationsTable(List<Server> servers, System.Web.UI.HtmlControls.HtmlForm forma1)
        {
            if (servers.Count > 0)
            {
                Table table = new Table();
                foreach (var item in servers)
                {
                    TableCell name = new TableCell();
                    TableCell speed = new TableCell();

                    name.Text = item.Name;
                    speed.Text = item.InterfaceSpeed.ToString();

                    TableRow row = new TableRow();
                    row.Cells.Add(name);
                    row.Cells.Add(speed);
                    table.Rows.Add(row);
                }
                forma1.Controls.Add(table);
            }
        }
        public static void DisplayServerData(List<Server> servers, System.Web.UI.HtmlControls.HtmlForm forma1, bool check)
        {
            if (servers.Count > 0)
            {
                foreach (var server in servers)
                {


                    Label label = new Label();
                    label.Text = server.ToString();

                    TableCell sender = new TableCell();
                    TableCell receiver = new TableCell();
                    TableCell targetServer = new TableCell();
                    TableCell Date = new TableCell();
                    TableCell duration = new TableCell();

                    sender.Text = "Siuntėjas";
                    receiver.Text = "Gavėjas";
                    targetServer.Text = "Serveris į kurį siunčiama";
                    duration.Text = "Trukem";

                    Date.Text = "Data";

                    TableRow row = new TableRow();
                    row.Cells.Add(sender);
                    row.Cells.Add(receiver);
                    row.Cells.Add(targetServer);
                    row.Cells.Add(Date);
                    row.Cells.Add(duration);

                    Table table = new Table();
                    table.Rows.Add(row);

                    foreach (var email in server.EmailTraffic)
                    {
                        sender = new TableCell();
                        receiver = new TableCell();
                        targetServer = new TableCell();
                        Date = new TableCell();
                        duration = new TableCell();

                        sender.Text = email.SenderAddress;
                        receiver.Text = email.ReceiverAddress;
                        targetServer.Text = email.TargeServer;
                        Date.Text = email.SendTime.ToString();
                        duration.Text = email.TransferDuration.ToString();

                        row = new TableRow();
                        row.Cells.Add(sender);
                        row.Cells.Add(receiver);
                        row.Cells.Add(targetServer);
                        row.Cells.Add(Date);
                        row.Cells.Add(duration);

                        table.Rows.Add(row);

                    }
                    forma1.Controls.Add(table);
                }
            }
        }
        public static void PrintIdleHours(List<Tuple<string, DateTime>> servers, System.Web.UI.HtmlControls.HtmlForm forma1)
        {
            if(servers.Count > 0)
            {
                Table table = new Table();
                foreach (var item in servers)
                {
                    TableCell name = new TableCell();
                    TableCell hour = new TableCell();
                    TableCell date = new TableCell();

                    //name.Text = item.Item1;
                    //hour.Text = item.Item2.Hour.ToString();
                    //date.Text = item.Item2.ToShortDateString();

                    date.Text = item.Item2.ToString();



                    TableRow row = new TableRow();
                    row.Cells.Add(name);
                    row.Cells.Add(date);
                    row.Cells.Add(hour);

                    table.Rows.Add(row);
                }
                forma1.Controls.Add(table);
            }
            
        }
    }
}
