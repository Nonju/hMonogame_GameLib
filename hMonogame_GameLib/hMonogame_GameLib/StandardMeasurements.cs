using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace hMonogame_GameLib {
    public static class StandardMeasurements {

        static float widthUnit, heightUnit;
        static GameWindow gameWindow;

        public static void Load(GameWindow window) {
            UpdateMeasurements(window);
        }

        public static void UpdateMeasurements(GameWindow window) {
            gameWindow = window;
            widthUnit = window.ClientBounds.Width * 0.05f;
            heightUnit = window.ClientBounds.Height * 0.05f;
        }

        //properties
        public static float WidthUnit { get { return widthUnit; } }
        public static float HeightUnit { get { return heightUnit; } }
        public static GameWindow Window { get { return gameWindow; } }

    }
}
