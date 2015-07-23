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
    public class Animator {

        Texture2D texture; 
        float textureWidth, textureHeight; //width/height on whats drawn
        int frameWidth, frameHeight; //width/height on frames
        int startFrameY = 0; //startframe in Y-axis
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

        public Rectangle Update(GameTime gameTime, int staticFrame, int startFrameX, int startFrameY, bool isMoving, int xRow, int yRow, int timeToNext) {
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

    }
}
