using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
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
                        fileUpload.SaveAs(Path.Combine(directory.FullName + file.FileName));
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
                TaskUtils.FillTable(errors, table);
            }
        }

        public static List<Server> ReadServerInfo(string fileName, DirectoryInfo directory)
        {
            List<Server> servers = new List<Server>(); 
            foreach (var item in directory.GetFiles())
            {
                if (item.Name == fileName)
                {
                    string line;

                    using (StreamReader sr = new StreamReader(item.FullName))
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] parts = line.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                            string name = parts[0];
                            double speed = Convert.ToDouble(parts[1]);
                            Server server = new Server(name, speed);
                            servers.Add(server);
                        }
                    }

                }
                else
                {
                    throw new Exception($"There is no file named {fileName} ");
                }
            }
            return servers;
        }

       
    }

}
