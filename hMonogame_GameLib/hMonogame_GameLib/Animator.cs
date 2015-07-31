using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace hMonogame_GameLib {
    public class Animator {

        Texture2D texture;
        float textureWidth, textureHeight; //width/height on whats drawn
        int frameWidth, frameHeight; //width/height on frames
        int staticFrame = 0; //default value for which frame to display when no commands are given to object
        int startFrameY = 0; //startframe in Y-axis
        int startFrameX = 0;
        bool isMoving = true;
        int timeToNext = 50; //default time until frameSwitch
        int xRow = 1, yRow = 1; //default values on number of frames in spreadsheet
        float elapsedTime; //clock
        int currentFrame; // what frame you're on (X-axis)
        Rectangle sourceRec;

        public Animator(Texture2D texture, float textureWidth, float textureHeight, int frameWidth, int frameHeight) {
            this.texture = texture;
            this.textureWidth = textureWidth;
            this.textureHeight = textureHeight;
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            currentFrame = 0;

            sourceRec = new Rectangle(frameWidth * currentFrame, startFrameY, frameWidth, frameHeight);
        }

        public Rectangle Update(GameTime gameTime) {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (!isMoving) { currentFrame = staticFrame; } //Object doesn't move
            if (isMoving) {
                if (elapsedTime >= timeToNext) {
                    if (currentFrame >= xRow - 1) { currentFrame = startFrameX; }
                    else { currentFrame++; }
                    elapsedTime = 0; //resets clock
                }
            }
            else { isMoving = false; }

            //Updating Rectangles
            sourceRec.X = frameWidth * currentFrame;
            sourceRec.Y = startFrameY;
            return sourceRec;
        }

        //Properties
        public int SetStaticFrame { set { staticFrame = value; } }
        public int StartFrameX { set { startFrameX = value; } }
        public int StartFrameY { set { startFrameY = value; } }
        public bool IsMoving { get { return isMoving; } set { isMoving = value; } }
        public int SetTimeToNextFrame { set { timeToNext = value; } }
        public int SetNrOfXFrames { set { xRow = value; } }
        public int SetNrOfYFrames { set { yRow = value; } }
        public int GetCurrentXFrame { get { return currentFrame; } }

    }
}
