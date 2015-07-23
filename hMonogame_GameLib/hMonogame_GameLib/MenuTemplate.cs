using System;
using System.Runtime;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace hMonogame_GameLib {
    class MenuItem : Buttons {

        int currentState;

        public MenuItem(Texture2D texture, Vector2 pos, float width, float height, Color baseColor, int currentState)
            : base(texture, pos, width, height, baseColor) {
            this.currentState = currentState;
        }

        //Properties
        public int State { get { return currentState; } }

    }//end class MenuItem

    public class MenuTemplate {

        /* Could be that "selected" and "lastChange" aren't needed
         * since menuinteraction is done by clicking
         * instead of navigating the menu. 
         * 
         * Re-add if ever adding arrowkey-controlled menu-navigation.
         */

        List<MenuItem> menu;
        //int selected = 0; //First menuselection is always on location "0" in menuList

        float currentHeight = 0; //menuItems different heights
        //double lastChange = 0; //time since last menuSelection
        int defaultMenuState;
        Vector2 basePos;

        public MenuTemplate(int defaultMenuState, Vector2 basePos) {
            menu = new List<MenuItem>();
            this.defaultMenuState = defaultMenuState;
            this.basePos = basePos;
        }

        public void AddItem(Texture2D itemTexture, float textureWidth, float textureHeight, Color itemColor, int state) {
            //ItemHeights
            float X = basePos.X;
            float Y = basePos.Y + currentHeight;
            currentHeight += textureHeight + StandardMeasurements.HeightUnit; //height + spacing

            MenuItem temp = new MenuItem(itemTexture, new Vector2(X, Y), textureWidth, textureHeight, itemColor, state);
            menu.Add(temp);
        }

        public int Update(GameTime gameTime) {

            foreach (MenuItem mI in menu) {
                if (mI.IsClicked()) { return mI.State; }
            }

            //if no selection hsa been made, stay in menu
            return defaultMenuState;
        }

        //Properties
        //public int SetSelection { set { selected = value; } }

    }//end class MenuTemplate
}
