using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;


namespace hMonogame_GameLib {
    public class SmokeGenerator {

        public enum SmokeState { Off, Thin, Medium, Thick }; //add "fire-state" ???
        public SmokeState currentSmokeState;
        private SmokeState lastSmokeState;

        List<SmokeParticle> particleList;
        Texture2D smokeTexture;
        Vector2 basePos;
        float baseWidth;
        int nrOfLenghtUnits, particlesPerLenghtUnit, maxNrOfParticles;
        float chanceToSpawn;
        Random smokeRand;

        SpriteFont sf;


        public SmokeGenerator(ContentManager content, Texture2D smokeTexture, Vector2 baseSmokePos, float baseSmokeWidth) {
            currentSmokeState = lastSmokeState = SmokeState.Off; //sets default smokeState

            particleList = new List<SmokeParticle>();
            this.smokeTexture = smokeTexture;
            this.basePos = baseSmokePos;
            this.baseWidth = baseSmokeWidth;
            nrOfLenghtUnits = (int)(baseWidth / StandardMeasurements.WidthUnit); //calculates the nr of particles to spawn based on the smoketrail width (baseSmokeWidth)
            particlesPerLenghtUnit = 0; //default value
            maxNrOfParticles = 0; //default value
            chanceToSpawn = 0.5f; //dafault value
            smokeRand = new Random();
            SetSmokeThickness();

            //spritefont
            sf = content.Load<SpriteFont>("Images/SpriteFont");
        }

        public void UpdateSmoke(GameTime gameTime) {
            if (lastSmokeState != currentSmokeState) { //state of smoke has been changed
                lastSmokeState = currentSmokeState;
                SetSmokeThickness();
            }

            //Spawn more particles if under max
            if (particleList.Count < maxNrOfParticles) {
                int tempRandX;
                if (smokeRand.NextDouble() <= chanceToSpawn) { //spawns 3 particles at once
                    for (int i = 0; i < nrOfLenghtUnits + 1; i++) {
                        tempRandX = smokeRand.Next((int)basePos.X, (int)(basePos.X + baseWidth));
                        SmokeParticle temp = new SmokeParticle(smokeTexture, new Vector2(tempRandX, basePos.Y), new Vector2(0, smokeRand.Next(1, 2)), 2, 2, 1050);
                        tempRandX = smokeRand.Next((int)basePos.X, (int)(basePos.X + baseWidth));
                        SmokeParticle temp2 = new SmokeParticle(smokeTexture, new Vector2(tempRandX, basePos.Y), new Vector2(0, smokeRand.Next(1, 3)), 2, 2, 1060);
                        tempRandX = smokeRand.Next((int)basePos.X, (int)(basePos.X + baseWidth));
                        SmokeParticle temp3 = new SmokeParticle(smokeTexture, new Vector2(tempRandX, basePos.Y), new Vector2(0, smokeRand.Next(1, 3)), 2, 2, 1040);

                        //add to list
                        particleList.Add(temp);
                        particleList.Add(temp2);
                        particleList.Add(temp3);
                    } //end for-loop

                }

            }

            //Run particleUpdate
            foreach (SmokeParticle sP in particleList.ToArray()) {
                sP.Update(gameTime);
                //remove dead particles
                if (!sP.IsAlive) { particleList.Remove(sP); }
            }


        }

        public void Draw(SpriteBatch spriteBatch) {
            foreach (SmokeParticle sP in particleList) {
                sP.Draw(spriteBatch);
            }
            //spritefonts
            spriteBatch.DrawString(sf, "Nr of particles: " + particleList.Count.ToString(), Vector2.Zero, Color.Red);
            spriteBatch.DrawString(sf, "Current smokeState: " + currentSmokeState.ToString(), new Vector2(0, 20), Color.Pink);
        }


        private void SetSmokeThickness() { //Changes the smokes thickness
            switch (currentSmokeState) {
                case SmokeState.Off:
                    particlesPerLenghtUnit = 0;
                    chanceToSpawn = 0;
                    break;
                case SmokeState.Thin:
                    particlesPerLenghtUnit = 60;
                    chanceToSpawn = 0.5f;
                    break;
                case SmokeState.Medium:
                    particlesPerLenghtUnit = 120;
                    chanceToSpawn = 0.8f;
                    break;
                case SmokeState.Thick:
                    particlesPerLenghtUnit = 200;
                    chanceToSpawn = 1f;
                    break;
                default:
                    break;
            }

            maxNrOfParticles = (int)Math.Round((double)(particlesPerLenghtUnit * (baseWidth / 10)), 0);
            if (particleList.Count > maxNrOfParticles) { //removes particles above allowed particleLimit
                foreach (SmokeParticle sP in particleList.ToArray()) {
                    if (particleList.IndexOf(sP) >= maxNrOfParticles) { particleList.Remove(sP); }
                }
            }
        }

        //Properties
        public Vector2 SetBasePos { get { return basePos; } set { basePos = value; } }

    } //end class SmokeGenerator

    internal class SmokeParticle {

        Texture2D texture;
        Vector2 pos, speed;
        float width, height;
        Rectangle rec;

        bool isAlive;

        int currentColor;
        Random rand;
        int lifetimeInMS;
        float spawnTime, currentTime;
        bool firstTimeUpdateBool;


        public SmokeParticle(Texture2D texture, Vector2 pos, Vector2 speed, float width, float height, int lifetimeInMS) {
            this.texture = texture;
            this.pos = pos;
            this.speed = speed;
            this.width = width;
            this.height = height;

            rand = new Random();
            this.lifetimeInMS = (lifetimeInMS - rand.Next(501)); //lifetime can difference in ~ 300 ms
            firstTimeUpdateBool = true;

            currentColor = 100; //default color value for R, G and B

            rec = new Rectangle((int)pos.X, (int)pos.Y, (int)width, (int)height);

            isAlive = true;

        }

        public void Update(GameTime gameTime) {
            if (firstTimeUpdateBool) { //sets "spawntime" on first update-loop
                firstTimeUpdateBool = false;
                spawnTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            currentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds; //currentTime retriever

            //isAlive check
            if (currentTime >= this.lifetimeInMS + spawnTime) { this.isAlive = false; }

            //Movement
            pos.Y -= speed.Y;

            //currentColor
            currentColor++;

            //Update Recs
            rec.X = (int)pos.X;
            rec.Y = (int)pos.Y;
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rec, new Color(currentColor, currentColor, currentColor));
        }

        //Properties
        public bool IsAlive { get { return isAlive; } }

    } //end class SmokeParticle
}
