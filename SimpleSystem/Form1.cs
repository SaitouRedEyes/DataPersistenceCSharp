using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace SimpleSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CloseForm(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void IOButtonClick(object sender, EventArgs eventArgs)
        {
            Button b = (Button)sender;

            if (b.Text.Equals("Registry")) SceneManager.Instance.ChangeScene(this, SceneManager.Scenes.Registry);
            else SceneManager.Instance.ChangeScene(this, SceneManager.Scenes.Load);             
        }

        private void ConnectButtonClick(object sender, EventArgs eventArgs)
        {
            try  
            {
                // Create a new HttpWebRequest object.
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost/calculator_server/requests.php");
                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.KeepAlive=false;
                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
              
                string parameters = "n1=5&n2=20";                                  
                byte[] bytes = Encoding.ASCII.GetBytes(parameters);

                myHttpWebRequest.ContentLength = bytes.Length;

                Stream os = myHttpWebRequest.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);
                os.Close();
                
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                if(myHttpWebResponse != null)
                {
                    Stream streamResponse = myHttpWebResponse.GetResponseStream();
                    StreamReader streamRead = new StreamReader( streamResponse );
                    Char[] readBuff = new Char[256];

                    int count = streamRead.Read( readBuff, 0, 256 );                    
                    while (count > 0) 
                    {
                        String outputData = new String(readBuff, 0, count);
                        Console.Write(outputData);
                        count = streamRead.Read(readBuff, 0, 256);
                    }
                    Console.WriteLine();
                    
                    streamResponse.Close();
                    streamRead.Close();                    
                    myHttpWebResponse.Close();
                }
            }
            catch(ArgumentException e)
            {
                Console.WriteLine("Problemas ao tentar conectar com o server: ",e.Message);
            }
            catch(WebException e)
            {
                Console.WriteLine("WebException raised!");
                Console.WriteLine("\n{0}",e.Message);
                Console.WriteLine("\n{0}",e.Status);
            } 
            catch(Exception e)
            {
                Console.WriteLine("Exception raised!");
                Console.WriteLine("Source :{0} " , e.Source);
                Console.WriteLine("Message :{0} " , e.Message);
            }
        }
    }
}