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

        static float standardWidthUnit, standardHeightUnit;

        public static void Load(GameWindow window) {
            UpdateMeasurements(window);
        }

        public static void UpdateMeasurements(GameWindow window) {
            standardWidthUnit = window.ClientBounds.Width * 0.05f;
            standardHeightUnit = window.ClientBounds.Height * 0.05f;
        }

        //properties
        public static float StandardWidthUnit { get { return standardWidthUnit; } }
        public static float StandardHeightUnit { get { return standardHeightUnit; } }

    }
}
