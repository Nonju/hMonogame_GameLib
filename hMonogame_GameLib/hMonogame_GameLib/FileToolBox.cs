using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

//Addons
using System.IO;


namespace hMonogame_GameLib {
    public static class FileToolBox {

        //Retieves files from specified folder and returns them as an string[]-array
        public static string[] RetrieveFiles(string folderPath) {
            string[] fileArray = Directory.GetFiles(folderPath);            
            return fileArray;
        }

        //Reads from specified textfile and returns it as a List<string>
        public static List<string> ReadFromTextFile(string filePath) {
            List<string> lines = new List<string>();
            StreamReader reader = new StreamReader(@"" + filePath, System.Text.Encoding.UTF8);
            string line;
            while ((line = reader.ReadLine()) != null) {
                lines.Add(line);
            }
            reader.Close();
            return lines;
        }

        /* 
         * Add more tools below
         * Add more tools below
         * Add more tools below
         */

    }
}
