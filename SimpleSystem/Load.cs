using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace SimpleSystem
{
    public partial class Load : Form
    {
        private const string PATH_TXT = "Registry.txt";
        private const string PATH_XML = "Registry.xml";
        private const string PATH_XML_SERIALIZABLE = "RegistrySer.xml";

        public Load()
        {
            InitializeComponent();
        }

        private void CloseForm(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void LoadButtonClick(object sender, EventArgs e)
        {
            Button b = (Button) sender;

            if (!txtBoxName.Text.Equals(""))
            {
                if (b.Text.Equals("Load TXT")) LoadTXT();
                else if (b.Text.Equals("Load XML")) LoadXML();
                else LoadXMLSerializable();
            }
        }

        /** load a txt file
         * 
         * **/
        private void LoadTXT()
        {
            try
            {
                using (StreamReader sr = new StreamReader(PATH_TXT))
                {
                    while (sr.Peek() >= 0)
                    {
                        if (sr.ReadLine().Equals("Name:" + txtBoxName.Text))
                        {
                            string _class = sr.ReadLine();
                            string level = sr.ReadLine();

                            Character character = new Character(txtBoxName.Text,
                                                _class.Substring(_class.IndexOf(":") + 1),
                                                int.Parse(level.Substring(level.IndexOf(":") + 1)));

                            lblClass.Text = character._Class;
                            lblLevel.Text = character.Level.ToString();
                            break;
                        }
                        else lblClass.Text = lblLevel.Text = "";
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine("Arquivo não pode ser lido. Motivo: " + ex.Message); }
        }

        /** load a xml file
         * **/
        private void LoadXML()
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(PATH_XML);
                Character character = null;

                foreach (XmlNode node in xmlDocument.DocumentElement.ChildNodes)
                {
                    foreach (XmlNode child in node)
                    {
                        if (child.Name.Equals("Name") && child.InnerText.Equals(txtBoxName.Text))
                        {
                            character = new Character(node.SelectSingleNode("Name").InnerText,
                                                                node.SelectSingleNode("Class").InnerText,
                                                                int.Parse(node.SelectSingleNode("Level").InnerText));

                            lblClass.Text = character._Class;
                            lblLevel.Text = character.Level.ToString();
                            break;
                        }                        
                    }
                }

                if (character == null) lblClass.Text = lblLevel.Text = "";                
            }
            catch (Exception ex) { Console.WriteLine("Arquivo não pode ser lido. Motivo: " + ex.Message); }
        }

        /** load a xml file in a serializable object.
         * **/
        private void LoadXMLSerializable()
        {
            try
            {
                string document = "<root>" + File.ReadAllText(PATH_XML_SERIALIZABLE) + "</root>";
                XmlSerializer serializer = new XmlSerializer(typeof(Character));
                List<XElement> elements = XDocument.Parse(document).Descendants("Character").ToList();

                foreach (XElement characterElem in elements)
                {
                    Character character = (Character)serializer.Deserialize(characterElem.CreateReader());

                    if (character.Name.Equals(txtBoxName.Text))
                    {
                        lblClass.Text = character._Class;
                        lblLevel.Text = character.Level.ToString();
                        break;
                    }
                    else lblClass.Text = lblLevel.Text = "";
                }
            }
            catch (Exception ex) { Console.WriteLine("Arquivo não pode ser lido. Motivo: " + ex.Message); }
        }

        private void BackButtonClick(object sender, EventArgs e)
        {
            SceneManager.Instance.ChangeScene(this, SceneManager.Scenes.MainMenu);
        }
    }
}
