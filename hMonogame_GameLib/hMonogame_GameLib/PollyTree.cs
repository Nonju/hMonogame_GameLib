using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace hMonogame_GameLib {
    class GameObjects {

        protected Texture2D texture;
        protected Vector2 pos;
        protected float width, height;
        protected Rectangle rec;
        protected Color color;

        public GameObjects(Texture2D texture, Vector2 pos, float width, float height) {
            this.texture = texture;
            this.pos = pos;
            this.width = width;
            this.height = height;

            rec = new Rectangle((int)pos.X, (int)pos.Y, (int)width, (int)height);
        }
        public virtual void Update() { }
        public virtual void Draw(SpriteBatch spriteBatch) { }

        MouseState mState;
        public virtual bool OnHover() { //if mouse is on top of object
            mState = Mouse.GetState();
            if (mState.X > pos.X && mState.X < (pos.X + width)) {
                if (mState.Y > pos.Y && mState.Y < (pos.Y + height)) {
                    return true;
                }
            }
            return false;
        }
        bool objectIsClicked = false;
        public virtual bool IsClicked() { //checks if object is clicked
            mState = Mouse.GetState();
            if (mState.LeftButton == ButtonState.Pressed && !objectIsClicked) {
                objectIsClicked = true;
                if (mState.X > pos.X && mState.X < (pos.X + width)) {
                    if (mState.Y > pos.Y && mState.Y < (pos.Y + height)) {
                        return true; //object is clicked!
                    }
                }
            }
            else if (mState.LeftButton == ButtonState.Released) { objectIsClicked = false; }
            return false; // == not Clicked
        }

        //Properties
        public Texture2D Texture { set { texture = value; } }
        public Vector2 Pos { get { return pos; } set { pos = value; } }
        public float Width {
            get { return width; }
            set {
                if (value <= 1) { width = 1; }
                else { width = value; }
            }
        }
        public float Height {
            get { return height; }
            set {
                if (value <= 1) { height = 1; }
                else { height = value; }
            }
        }

    }//end GameObjects

    class PhysicalObjects : GameObjects {

        protected bool isAlive = true;

        public PhysicalObjects(Texture2D texture, Vector2 pos, float width, float height) : base(texture, pos, width, height) { }

        public bool CollisionDetection(Rectangle otherRec) {
            return rec.Intersects(otherRec);
        }

        //Properties
        public bool IsAlive { get { return isAlive; } set { isAlive = value; } }
    }//end PhysicalObjects

    class MovingObjects : PhysicalObjects {

        protected Vector2 speed;

        public MovingObjects(Texture2D texture, Vector2 pos, float width, float height, Vector2 speed)
            : base(texture, pos, width, height) {
            this.speed = speed;
        }

    }//end MovingObjects

    class Buttons : GameObjects {

        protected Color baseColor;

        public Buttons(Texture2D texture, Vector2 pos, float width, float height, Color baseColor) : base(texture, pos, width, height) {
            this.baseColor = baseColor;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Color onHoverColor) {
            if (OnHover()) { color = onHoverColor; }
            else { color = baseColor; }
            spriteBatch.Draw(texture, rec, color);
        }

    }//end Buttons

}
