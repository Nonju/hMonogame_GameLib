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

    class AnimationMenuItem : Buttons {

        int currentState;
        Animator btnAnimator;

        public AnimationMenuItem(Texture2D texture, Vector2 pos, float width, float height, int frameWidth, int frameHeight, Color baseColor, int currentState)
            : base(texture, pos, width, height, baseColor) {
                btnAnimator = new Animator(texture, width, height, frameWidth, frameHeight);
                this.currentState = currentState;
        }

        public void ButtonAnimationUpdate(GameTime gameTime, bool buttonIsSelected) {
            sourceRec = btnAnimator.Update(gameTime);
            btnAnimator.IsMoving = buttonIsSelected;
        }

        //Properties
        public int State { get { return currentState; } }
        public Rectangle GetSourceRec { get { return sourceRec; } }

    }//end class AnimationMenuItem

    public class MenuTemplate {

        /* Could be that "selected" and "lastChange" aren't needed
         * since menuinteraction is done by clicking
         * instead of navigating the menu. 
         * 
         * Re-add if ever adding arrowkey-controlled menu-navigation.
         */

        List<MenuItem> staticMenu; //for static menuObjects
        List<AnimationMenuItem> animatedMenu; //for animated menuObjects
        //int selected = 0; //First menuselection is always on location "0" in menuList

        float currentHeight = 0; //menuItems different heights
        //double lastChange = 0; //time since last menuSelection
        int defaultMenuState;
        Vector2 basePos;
        Color onSelectColor;

        public MenuTemplate(int defaultMenuState, Vector2 basePos, Color onSelectColor) {
            staticMenu = new List<MenuItem>();
            animatedMenu = new List<AnimationMenuItem>();
            this.defaultMenuState = defaultMenuState;
            this.basePos = basePos;
            this.onSelectColor = onSelectColor;
        }

        public void AddStaticItem(Texture2D itemTexture, float textureWidth, float textureHeight, Color itemColor, int state) {
            //ItemHeights
            float X = basePos.X;
            float Y = basePos.Y + currentHeight;
            currentHeight += textureHeight + StandardMeasurements.HeightUnit; //height + spacing

            MenuItem temp = new MenuItem(itemTexture, new Vector2(X, Y), textureWidth, textureHeight, itemColor, state);
            staticMenu.Add(temp);
        }
        public void AddAnimatedItem(Texture2D itemSpredSheet, float textureWidth, float textureHeight, int frameWidth, int frameHeight, Color itemColor, int state) {
            //ItemHeights
            float X = basePos.X;
            float Y = basePos.Y + currentHeight;
            currentHeight += textureHeight + StandardMeasurements.HeightUnit; //height + spacing

            AnimationMenuItem temp = new AnimationMenuItem(itemSpredSheet, new Vector2(X, Y), textureWidth, textureHeight, frameWidth, frameHeight, itemColor, state);
            animatedMenu.Add(temp);
        }

        public int Update(GameTime gameTime) {

            foreach (MenuItem mI in staticMenu) {
                if (mI.IsClicked()) { return mI.State; }
            }
            foreach (AnimationMenuItem aMi in animatedMenu) {
                aMi.ButtonAnimationUpdate(gameTime, aMi.OnHover()); //animates the button when it's selected/hovered
                if (aMi.IsClicked()) { return aMi.State; }
            }

            //if no selection has been made, stay in menu
            return defaultMenuState;
        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (MenuItem mI in staticMenu) {
                mI.Draw(spriteBatch, onSelectColor);
            }
            foreach (AnimationMenuItem aMi in animatedMenu) {
                aMi.Draw(spriteBatch, onSelectColor);
            }
        }

        //Properties
        //public int SetSelection { set { selected = value; } }

    }//end class MenuTemplate
}
