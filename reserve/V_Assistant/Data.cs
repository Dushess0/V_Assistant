using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Drawing; 
namespace V_Assistant
{   
    public class Data
    {
        const string FileName = "settings.json";

        internal JsonData storage = null;

        public Data()
        {
            storage = load_user_settings();
        }
         internal JsonData load_user_settings()
        {
            var screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            var storage = File.Exists(FileName) ? JsonConvert.DeserializeObject<JsonData>(File.ReadAllText(FileName)) : new JsonData
            {
                Username = get_username(),
                #region Menu
                MenuPos = new Point(0, 0),

                MenuHeight = 400,
                MenuWidth = 400,
                HotKeys = new List<string>(new string[] { "A#bubble" }),
                MenuCols = 2,
                MenuRows = 2,
                #endregion
                MenuList = new List<string>(new string[] { "Games", "Hotkeys", "Music", "Time", "Stopwatch", "Timer" }),

                #region Widgets Position
                TimePos = new Point(screen.Width / 2 - 200, 30),

                #endregion
                songs = new List<string>(),


            };
            return storage;
       
        }
        string get_username()
        {
            return "Dushess";
        }
        public void save_current_settings()
        {
            File.WriteAllText(FileName, JsonConvert.SerializeObject(storage));
        }



    }
    class JsonData
    {
        public string Username { get; set; }


        #region Menu
        public Point MenuPos { get; set; }
        public int MenuHeight { get; set; }
        public int MenuWidth { get; set; }

        public int MenuRows { get; set; }
        public int MenuCols { get; set; }

        public List<string> MenuList { get; set; }

        public List<string> HotKeys { get; set; }
        public List<string> songs { get; set; }

        #endregion

        #region Widgets Positions
        public Point TimePos { get; set; }
        #endregion





    }
}


