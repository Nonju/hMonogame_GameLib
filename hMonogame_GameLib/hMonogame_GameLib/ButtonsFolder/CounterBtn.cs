using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace hMonogame_GameLib.ButtonsFolder {
    class CounterBtn : Buttons {

        int nrToAdd; //Number to add/subtract 

        public CounterBtn(Texture2D texture, Vector2 pos, float width, float height, Color baseColor)
            : base(texture, pos, width, height, baseColor) {
            nrToAdd = 1; //set to standard addition Nr
        }

        public int NrToAdd { get { return nrToAdd; } set { nrToAdd = value; } }

    }
}
