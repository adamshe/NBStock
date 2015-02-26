using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net ;
using System.IO;
using System.Diagnostics;
namespace NB.StockStudio
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }
        private string  getMyIP()
        {
            //            IPHostEntry ip = Dns.GetHostByName ("www.vbcity.com"); 
            //IPAddress [] IpA = ipE.AddressList; 
            //for (int i = 0; i < IpA.Length; i++) 
            //{ 
            //    Console.WriteLine ("IP Address {0}: {1} ", i, IpA[i].ToString ()); 
            //} 

            //To get the local IP address 
            string sHostName = Dns.GetHostName();
            IPHostEntry ipE = Dns.GetHostEntry(sHostName);
            IPAddress[] IpA = ipE.AddressList;
            return IpA[0].ToString();
            //for (int i = 0; i < IpA.Length; i++)
            //{
            //   // Console.WriteLine("IP Address {0}: {1} ", i, IpA[i].ToString());
            //    return IpA[i].ToString();
            //}

        }
        private string getIPAddress()
        {
      //      string  command;
      //      StringBuilder outputFile;

                try
                {

                    ProcessStartInfo startInfo = new ProcessStartInfo("ipconfig.exe");
                    //	command = @" -classpath C:\jdk1.3.1\lib\fxjtools.jar com.torcn.stock.fxj.app.FxjEodTxt " + file ;//+ " > " + directory + @"\" + outputFile;
                    //	startInfo.Arguments = command;
                    startInfo.UseShellExecute = false;
                    startInfo.RedirectStandardOutput = true;
                    

                    //Process.Start(startInfo);
                    Process p = new Process();
                    p.StartInfo = startInfo;
                    p.Start();

                //    StringWriter sw = new StringWriter(outputFile);
                    string output;
                    int pos;
                    while (null != (output = p.StandardOutput.ReadLine()))
                    {
                        if (0 < (pos = output.IndexOf("IP Address")))
                        {
                            pos = output.IndexOf(":");
                            return output.Substring(1+pos);
                        }
                    }
                    
                    p.WaitForExit();
                    return "";
                }
                catch { return ""; }

        }
        private void btnIP_Click(object sender, EventArgs e)
        {

            this.lblIP.Text = getMyIP(); //this.getIPAddress();//getMyIP();
        }
    }
}