using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;


namespace L5.Code
{
    /// <summary>
    /// Class for input and output of information
    /// </summary>
    public class InOut
    {
        /// <summary>
        /// Saves files from fileupload to server
        /// </summary>
        /// <param name="directory">Diretory to save</param>
        /// <param name="fileUpload">File upload object</param>
        /// <param name="table">error table</param>
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
        /// <summary>
        /// Reads information about server's specifications
        /// </summary>
        /// <param name="fileName">Specific file name</param>
        /// <param name="directory">Directory to read</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
                                string[] parts = line.Split(new string[] { "; " },
                                                            StringSplitOptions.RemoveEmptyEntries);
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
        /// <summary>
        /// Reads information from saved files about emails
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="specFile"></param>
        /// <param name="errors"></param>
        /// <param name="ServerSpecs"></param>
        /// <returns></returns>
        public static List<Server> ReadEmailData(DirectoryInfo directory, string specFile, 
                                            List<string> errors, List<Server> ServerSpecs)
        {
            List<Server> serverList = new List<Server>();
            foreach (var file in directory.GetFiles())
            {
                if (file.Name != specFile )
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(file.FullName))
                        {

                            string[] firstLine = sr.ReadLine().Split(new string[] { "; " },
                                                            StringSplitOptions.RemoveEmptyEntries);
                            int year = Convert.ToInt32(firstLine[0]);
                            int month = Convert.ToInt32(firstLine[1]);
                            int day = Convert.ToInt32(firstLine[2]);
                            string serverName = firstLine[3];

                            var temwtp = ServerSpecs.Find(s => s.Name == serverName);

                            if(temwtp == null)
                            {
                                errors.Add($"{serverName} server is not supported by given files.");
                                continue;
                            }


                            DateTime date = new DateTime(year, month, day);
                            string line;

                            List<Email> emails = new List<Email>();
                            while ((line = sr.ReadLine()) != null)
                            {
                                string[] parts = line.Split(new string[] { "; " }, 
                                                    StringSplitOptions.RemoveEmptyEntries);
                                int hour = Convert.ToInt32(parts[0]);
                                int min = Convert.ToInt32(parts[1]);
                                string senderAddress = parts[2];
                                string receiverAddress = parts[3];
                                double emailSize = Convert.ToDouble(parts[4]);
                                DateTime emailDate = new DateTime(year, month, day, hour, min, 1);


                                Email email = new Email(senderAddress, receiverAddress, 
                                                                    emailSize, serverName, emailDate);
                                emails.Add(email);
                            }
                            Server temp = new Server(serverName, emails, date);
                            serverList.Add(temp);
                        }
                    }
                    catch(Exception ex)
                    {
                        errors.Add($"{file.Name} format is corrupted please fix and try again.");
                        continue;
                    }
                }
            }
            return serverList;           
        }
        /// <summary>
        /// Fills error table
        /// </summary>
        /// <param name="errors">Error list</param>
        /// <param name="table">Table object</param>
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
       
        /// <summary>
        /// Displays information about server data
        /// </summary>
        /// <param name="servers">List of all servers</param>
        /// <param name="forma1">Form object</param>
        public static void DisplayServerData(List<Server> servers, 
                            System.Web.UI.HtmlControls.HtmlForm forma1)
        {
            if (servers.Count > 0)
            {

                foreach (var server in servers)
                {


                    Label label = new Label();
                    label.Text = $"{server.Name} {server.TransferDate.ToString("yyyy-M-d\n")}";

                    label.Attributes.Add("Class", "labelStyle");

                    TableCell sender = new TableCell();
                    TableCell receiver = new TableCell();
                    TableCell fileSize = new TableCell();
                    TableCell Date = new TableCell();

                    sender.Text = "Siuntėjas";
                    receiver.Text = "Gavėjas";
                    fileSize.Text = "Laiško dydis";

                    Date.Text = "Išsiuntimo laikas";

                    TableRow row = new TableRow();
                    row.Cells.Add(sender);
                    row.Cells.Add(receiver);
                    row.Cells.Add(fileSize);
                    row.Cells.Add(Date);

                    row.Attributes.Add("class", "HeaderRow");

                    Table table = new Table();
                    table.Rows.Add(row);
                    forma1.Controls.Add(label);

                    foreach (var email in server.EmailTraffic)
                    {
                        sender = new TableCell();
                        receiver = new TableCell();
                        Date = new TableCell();
                        fileSize = new TableCell();

                        sender.Text = email.SenderAddress;
                        receiver.Text = email.ReceiverAddress;
                        fileSize.Text = email.SizeInBytes.ToString();
                        Date.Text = email.SendTime.ToString("HH:m 'h' ");

                        row = new TableRow();
                        row.Cells.Add(sender);
                        row.Cells.Add(receiver);
                        row.Cells.Add(fileSize);
                        row.Cells.Add(Date);
                        table.Rows.Add(row);

                    }
                    forma1.Controls.Add(table);
                }
            }
        }
        /// <summary>
        /// Draws server's specification's table
        /// </summary>
        /// <param name="servers">All server's specifications list</param>
        /// <param name="forma1">Form object</param>
        public static void DrawSpecificationsTable(List<Server> servers,
                                            System.Web.UI.HtmlControls.HtmlForm forma1)
        {
            if (servers.Count > 0)
            {
                Table table = new Table();
                TableRow row = new TableRow();
                row.Attributes.Add("class", "HeaderRow");

                TableCell name = new TableCell();
                TableCell speed = new TableCell();

                name.Text = "Serverio pavadinimas";
                speed.Text = "Serverio sąsajos greitis";

                row.Cells.Add(name);
                row.Cells.Add(speed);

                table.Rows.Add(row);

                foreach (var item in servers)
                {
                    name = new TableCell();
                    speed = new TableCell();

                    name.Text = item.Name;
                    speed.Text = item.InterfaceSpeed.ToString();

                    row = new TableRow();
                    row.Cells.Add(name);
                    row.Cells.Add(speed);

                    table.Rows.Add(row);
                }
                forma1.Controls.Add(table);
            }
        }        
        public static void PrintIdleHours(List<Tuple<string, DateTime, int>> servers,
                                            System.Web.UI.HtmlControls.HtmlForm forma1)
        {
            if(servers.Count > 0)
            {
                TableRow row = new TableRow();
                Label label = new Label();
                label.Text = "Valandos kuriomis srveriai buvo nenaudojami";
                label.Attributes.Add("class", "labelStyle");

                TableCell Server = new TableCell();
                TableCell Date = new TableCell();
                TableCell Hour = new TableCell();


                Server.Text = "Serverio pavadinimas";
                Date.Text = "Data";
                Hour.Text = "Valanda";

                row.Cells.Add(Server);
                row.Cells.Add(Date);
                row.Cells.Add(Hour);

                row.Attributes.Add("class", "HeaderRow");

                Table table = new Table();
                table.Rows.Add(row);
                foreach (var item in servers)
                {
                    Server = new TableCell();
                    Hour = new TableCell();
                    Date = new TableCell();

                    Server.Text = item.Item1;
                    Date.Text = item.Item2.ToString("MM-dd");
                    Hour.Text = item.Item3.ToString();

                    row = new TableRow();
                    row.Cells.Add(Server);
                    row.Cells.Add(Date);
                    row.Cells.Add(Hour);
                    table.Rows.Add(row);
                }
                forma1.Controls.Add(label);
                forma1.Controls.Add(table);
            }
            
        }
        public static void InputDataToTxt(List<Server> servers, List<Server> emails, string path)
        {
            if(servers.Count > 0)
            {
                string longLine1 = new string('-', 47);
                string longLine2 = new string('-', 110);

                using (StreamWriter sr = new StreamWriter(path, true))
                {
                    sr.WriteLine("Serverių specifikacijos");
                    sr.WriteLine(longLine1);
                    sr.WriteLine($"|{"Serverio pavadinimas",-29}|{"Greitis(bps)",-15}|");
                    sr.WriteLine(longLine1);
                    foreach (var server in servers)
                    {
                        sr.WriteLine(server.ToString());
                    }
                    sr.WriteLine(longLine1);
                    sr.WriteLine('\n');
                    if(emails.Count > 0)
                    {
                        sr.WriteLine("Serveriu veikla atskiromis dienomis");
                        foreach (var server in emails)
                        {
                            sr.WriteLine($"{server.TransferDate.ToString("yyyy-MM-dd")} {server.Name}");
                            sr.WriteLine(longLine2);
                            sr.WriteLine($"|{"Išsiuntimo laikas",-20}|{"Siuntėjas",-35}" +
                                         $"|{"Gavėjas",-35}|{"Laiško dydis",-15}|");

                            sr.WriteLine(longLine2);
                            foreach (var email in server.EmailTraffic)
                            {
                                sr.WriteLine($"|{email.SendTime.ToString("H:m"), 20}" +
                                             $"|{email.SenderAddress, -35}" +
                                             $"|{email.ReceiverAddress, -35}|{email.SizeInBytes, 15}|");
                            }
                            sr.WriteLine(longLine2 + "\n");
                        }
                    }
                }
            }
            else
            {
                throw new Exception("There is no input data");
            }
            
        }
        /// <summary>
        /// Prints result to text file
        /// </summary>
        /// <param name="list">List of results</param>
        /// <param name="path">path to print data</param>
        public static void PrintResultsToTxt(List<Tuple<string, DateTime, int>> list, string path)
        {
            if(list.Count> 0)
            {
                string longLine = new string('-', 49);
                using(StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.WriteLine("Valandos kuriomis serveriai nebuvo naudojami");
                    sw.WriteLine(longLine);
                    sw.WriteLine($"|{"Serverio pavadinimas",-20}|{"Data",-15}|{"Valanda",-10}|");
                    sw.WriteLine(longLine);
                    foreach (var item in list)
                    {
                        sw.WriteLine($"|{item.Item1,20}|{item.Item2.ToString("MM-dd"),15}" +
                                     $"|{item.Item3,10}|");
                    }
                    sw.WriteLine(longLine);
                }
            }
            else
            {
                throw new Exception("Nėra rezultatų");
            }
        }
    }
}
