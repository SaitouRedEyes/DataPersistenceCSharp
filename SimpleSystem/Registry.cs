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

namespace SimpleSystem
{
    public partial class Registry : Form
    {
        private const string PATH_TXT = "Registry.txt";
        private const string PATH_XML = "Registry.xml";
        private const string PATH_XML_SERIALIZABLE = "RegistrySer.xml";

        public Registry()
        {
            InitializeComponent();
        }        

        private void RegistryButtonClick(object sender, EventArgs e)
        {
            Button b = (Button) sender;

            if (!txtBoxName.Text.Equals("") && !cmbBoxClass.Text.Equals("") &&
                   !txtBoxLevel.Text.Equals(""))
            {
                if (b.Text.Equals("Registry TXT")) RegistryTXT();
                else if (b.Text.Equals("Registry XML")) RegistryXML();
                else RegistryXMLSerializable();
            }
            else Console.WriteLine("É necessário preencher todos os campos!!");

        }

        /**Registry in a txt file
         * **/
        private void RegistryTXT()
        {
            using (StreamWriter file = new StreamWriter(PATH_TXT, true, Encoding.UTF8))
            {
                file.WriteLine(DateTime.Now);
                file.WriteLine("Name:" + txtBoxName.Text);
                file.WriteLine("Class:" + cmbBoxClass.Text);
                file.WriteLine("Level:" + txtBoxLevel.Text);
                Console.WriteLine("Registro no txt realizado com sucesso!!");

                SceneManager.Instance.ChangeScene(this, SceneManager.Scenes.MainMenu);
            }            
        }

        /**Registry in a xml file
         * **/
        private void RegistryXML()
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();

                if (!File.Exists(PATH_XML) || new FileInfo(PATH_XML).Length == 0)
                {
                    File.Create(PATH_XML).Close();

                    XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
                    XmlElement rootNode = xmlDocument.CreateElement("Characters");
                    xmlDocument.InsertBefore(xmlDeclaration, xmlDocument.DocumentElement);
                    xmlDocument.AppendChild(rootNode);
                    xmlDocument.Save(PATH_XML);
                }

                xmlDocument.Load(PATH_XML);
                XmlElement element, newElem = xmlDocument.CreateElement("Character");

                Dictionary<string, string> elementStructure = new Dictionary<string, string>()
                {
                    {"Name", txtBoxName.Text}, {"Class", cmbBoxClass.Text}, {"Level", txtBoxLevel.Text}
                };

                for (int i = 0; i < elementStructure.Count; i++)
                {
                    element = xmlDocument.CreateElement(elementStructure.Keys.ToArray()[i]);
                    element.InnerText = elementStructure.Values.ToArray()[i];
                    newElem.AppendChild(element);
                }

                xmlDocument.DocumentElement.AppendChild(newElem);                
                xmlDocument.Save(PATH_XML);
                Console.WriteLine("Registro no XML realizado com sucesso!!");

                SceneManager.Instance.ChangeScene(this, SceneManager.Scenes.MainMenu);
            }
            catch (Exception e) { Console.WriteLine("Problemas para salvar o arquivo xml: " + e.Message); }
        }

        /**Registry in a xml file a serializable object.
         * **/
        private void RegistryXMLSerializable()
        {            
            Character character = new Character();
            character.Name = txtBoxName.Text;
            character._Class = cmbBoxClass.Text;
            character.Level = int.Parse(txtBoxLevel.Text);
            
            try
            {                
                XmlSerializerNamespaces emptyNamepsaces = new XmlSerializerNamespaces(new[] 
                { 
                    new XmlQualifiedName(string.Empty, string.Empty) 
                });

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                XmlSerializer serializer = new XmlSerializer(typeof(Character));
                using (StreamWriter stream = new StreamWriter(PATH_XML_SERIALIZABLE, true, Encoding.UTF8))
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, character, emptyNamepsaces);
                    //stream.WriteLine();
                }

                Console.WriteLine("Registro no XML serializado realizado com sucesso!!");

                SceneManager.Instance.ChangeScene(this, SceneManager.Scenes.MainMenu);
            }
            catch (Exception e) { Console.WriteLine("Problemas para salvar o arquivo xml serializado: " + e.Message); }
        }

        private void CloseForm(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}