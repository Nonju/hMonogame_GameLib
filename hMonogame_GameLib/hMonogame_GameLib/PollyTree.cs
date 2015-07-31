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
        protected Rectangle rec, sourceRec;
        protected Color color;

        protected Vector2 origin;
        protected float rotationAngle;

        public GameObjects(Texture2D texture, Vector2 pos, float width, float height) {
            this.texture = texture;
            this.pos = pos;
            this.width = width;
            this.height = height;

            origin = Vector2.Zero; //default value
            rotationAngle = 0f; //default value

            rec = new Rectangle((int)pos.X, (int)pos.Y, (int)width, (int)height);
        }
        public virtual void Update() { }
        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rec, sourceRec, color, rotationAngle, origin, SpriteEffects.None, 0f);
        }

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
        public Color SetColor { set { color = value; } }
        public Vector2 Origin { get { return origin; } set { origin = value; } }
        public float RotationAngle { get { return rotationAngle; } set { rotationAngle = value; } }


    }//end GameObjects

    class PhysicalObjects : GameObjects {

        protected bool isAlive = true;
        protected Rectangle intersectRec;

        public PhysicalObjects(Texture2D texture, Vector2 pos, float width, float height)
            : base(texture, pos, width, height) {
            intersectRec = new Rectangle((int)pos.X, (int)(pos.Y + (height * 0.9f)), (int)width, (int)(height * 0.1f));
        }

        public bool CollisionDetection(PhysicalObjects otherObject) {
            //Update intersectRec
            UpdateIntersectRec();
            otherObject.UpdateIntersectRec();

            if (intersectRec.Intersects(otherObject.intersectRec)) { return true; }
            else { return false; }

        }

        protected void UpdateIntersectRec() {
            intersectRec.X = (int)pos.X;
            intersectRec.Y = (int)(pos.Y + (height * 0.9f));
        }

        //Properties
        public bool IsAlive { get { return isAlive; } set { isAlive = value; } }

    }//end class PhysicalObjects

    class MovingObjects : PhysicalObjects {

        protected Vector2 speed;
        protected bool isMoving;

        public MovingObjects(Texture2D texture, Vector2 pos, float width, float height, Vector2 speed)
            : base(texture, pos, width, height) {
            this.speed = speed;
            isMoving = true; //default value
        }

        //Properties
        public float SpeedX { get { return speed.X; } set { speed.X = value; } } //for altering the objects X-axis-speed
        public float SpeedY { get { return speed.Y; } set { speed.Y = value; } } //for altering the objects Y-axis-speed
        public bool IsMoving { get { return isMoving; } set { isMoving = value; } }

    }//end MovingObjects

    class Buttons : GameObjects {

        protected Color baseColor;

        public Buttons(Texture2D texture, Vector2 pos, float width, float height, Color baseColor) : base(texture, pos, width, height) {
            this.baseColor = baseColor;
        }

        public virtual void Draw(SpriteBatch spriteBatch, Color onHoverColor) {
            if (OnHover()) { color = onHoverColor; }
            else { color = baseColor; }
            //spriteBatch.Draw(texture, rec, color);
            spriteBatch.Draw(texture, rec, sourceRec, color, rotationAngle, origin, SpriteEffects.None, 0f);
        }

        public Color SetBaseColor { set { baseColor = value; } }

    }//end Buttons

    class Blocks : PhysicalObjects {

        protected string blockName;

        public Blocks(Texture2D texture, Vector2 pos, float width, float height, string blockName)
            : base(texture, pos, width, height) {
            if (blockName == null) { this.blockName = "EmptyBlock"; }
            else { this.blockName = blockName; }
        }


        //Properties
        public string BlockName { get { return blockName; } }

    }//end Blocks

}
