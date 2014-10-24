using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DragonUIEditor
{
    class EditorPreferences
    {
        public string filename;

        public void Open()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 2)
                filename = args[1];

            JSONTable preferences = null;
            try
            {
                preferences = JSONTable.parseFile("preferences.json");
            }
            catch (Exception) { }

            if (preferences != null)
            {
                if (filename == null)
                {
                    filename = preferences.getString("lastOpened", null);
                }
            }
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Save()
        {
            StreamWriter writer = new StreamWriter(File.OpenWrite("preferences.json"));
            writer.WriteLine("{\"lastOpened\":\""+JSONTable.escapeString(filename)+"\"}");
            writer.Close();
        }
    }
}
