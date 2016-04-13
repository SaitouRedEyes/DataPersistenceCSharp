using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SimpleSystem
{    
    public class Character
    {
        private string name;
        private string _class;
        private int level;        

        public Character() { }
        public Character(string name, string _class, int level)
        {
            this.name = name;
            this._class = _class;
            this.level = level;
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        [XmlElement("Class")]
        public string _Class
        {
            get { return this._class; }
            set { this._class = value; }
        }

        public int Level
        {
            get { return this.level; }
            set { this.level = value; }
        }        
    }
}
