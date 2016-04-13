using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimpleSystem
{
    class SceneManager
    {
        private static SceneManager sceneManager;
        private Form1 mainMenu;
        private Registry registry;
        private Load load;


        private SceneManager(){}

        public enum Scenes
        {
            MainMenu,
            Registry,
            Load
        }

        public static SceneManager Instance
        {
            get
            {
                if (sceneManager == null) sceneManager = new SceneManager();

                return sceneManager;
            }
        }

        public void ChangeScene(Form currForm, Scenes scene)
        {
            currForm.Visible = false;

            switch(scene)
            {
                case Scenes.MainMenu:
                    if (mainMenu == null) mainMenu = new Form1();
                                          mainMenu.Visible = true; break;
                case Scenes.Registry: 
                    if (registry == null) registry = new Registry();
                                         registry.Visible = true; break;
                case Scenes.Load:
                    if (load == null) load = new Load();
                                      load.Visible = true; break;    
            }
            
        }
    }
}