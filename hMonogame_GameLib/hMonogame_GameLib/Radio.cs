using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;


namespace hMonogame_GameLib {
    public static class Radio {

        //bools
        static bool isPlaying;
        static bool resetSong;
        static bool isVisible;

        //errormessages
        static List<string> errorMsgStrings;
        static SpriteFont errorMsgSF;
        static bool displayErrorMsg;

        //radio
        static Texture2D radioTexture;
        static Vector2 radioPos;
        static float radioWidth, radioHeight;
        static Rectangle radioRec;
        static List<string> songList; //list of songs
        static Song song; //song thats played through radio

        static Keys playKey;
        static Keys toggleVisibility;

        //songNameSpritefont
        static string currentSongName;
        static SpriteFont songNameSF;
        static Vector2 songNameSfPos;

        //buttons
        static Buttons prevSongBtn;
        static Texture2D prevSongBtnTexture;
        static Vector2 prevSongBtnPos;
        static float prevSongBtnWidth, prevSongBtnHeight;

        static Buttons playPauseBtn;
        static Texture2D playPauseBtnTexture;
        static Texture2D playBtnTexture, pauseBtnTexture;
        static Vector2 playPauseBtnPos;
        static float playPauseBtnWidth, playPauseBtnHeight;

        static Buttons nextSongBtn;
        static Texture2D nextSongBtnTexture;
        static Vector2 nextSongBtnPos;
        static float nextSongBtnWidth, nextSongBtnHeight;

        static Color btnColor;

        static ContentManager Content;
        public static void StandardSetupLoad(ContentManager content, Vector2 basePos, float baseWidth, float baseHeight) {
            Content = content;
            //bools
            isPlaying = false;
            resetSong = false;
            isVisible = true;
            displayErrorMsg = true;

            //errormessages
            errorMsgStrings = new List<string>();
            displayErrorMsg = true;
            errorMsgSF = content.Load<SpriteFont>("Images/SpriteFont");

            //radio measurements and stanard values
            radioTexture = content.Load<Texture2D>(@"Images/Radio/StandardRadioBackground.png");
            radioPos = basePos; //radioPos
            if (baseWidth < 1) { radioWidth = 1; } //radioWidth
            else { radioWidth = baseWidth; }
            if (baseHeight < 1) { radioHeight = 1; } //radioHeight
            else { radioHeight = baseHeight; }
            radioRec = new Rectangle((int)radioPos.X, (int)radioPos.Y, (int)radioWidth, (int)radioHeight);
            songList = new List<string>();
            MediaPlayer.Volume = 0.5f; //standard volume value
            song = content.Load<Song>("Sounds/Radio/BackInBlack"); //Default song

            //songNameSpritefont
            currentSongName = "Back in Black";
            songNameSF = content.Load<SpriteFont>("Images/SpriteFont");
            songNameSfPos = new Vector2(radioPos.X + (radioWidth * 0.4f), radioPos.Y + (radioHeight * 0.18f));

            //BUTTONS
            float radioBtnHeight = radioHeight * 0.5f;
            btnColor = Color.LightGreen;
            //prevSongBtn
            prevSongBtnTexture = content.Load<Texture2D>("Images/Radio/PrevBtn.png");
            prevSongBtnPos = new Vector2(radioPos.X + (radioWidth * 0.2f), radioPos.Y + radioBtnHeight);
            prevSongBtnWidth = radioWidth * 0.15f;
            prevSongBtnHeight = radioHeight * 0.3f;
            prevSongBtn = new ButtonsFolder.CounterBtn(prevSongBtnTexture, prevSongBtnPos, prevSongBtnWidth, prevSongBtnHeight, Color.White);

            //play/pause-btn
            playPauseBtnTexture = playBtnTexture = content.Load<Texture2D>("Images/Radio/PlayBtn.png");
            pauseBtnTexture = content.Load<Texture2D>("Images/Radio/PauseBtn.png");
            playPauseBtnPos = new Vector2(radioPos.X + (radioWidth * 0.45f), radioPos.Y + radioBtnHeight);
            playPauseBtnWidth = radioWidth * 0.15f;
            playPauseBtnHeight = radioHeight * 0.3f;
            playPauseBtn = new ButtonsFolder.BasicBtn(playPauseBtnTexture, playPauseBtnPos, playPauseBtnWidth, playPauseBtnHeight, Color.White);

            //nextSongBtn
            nextSongBtnTexture = content.Load<Texture2D>("Images/Radio/NextBtn.png");
            nextSongBtnPos = new Vector2(radioPos.X + (radioWidth * 0.7f), radioPos.Y + radioBtnHeight);
            nextSongBtnWidth = radioWidth * 0.15f;
            nextSongBtnHeight = radioHeight * 0.3f;
            nextSongBtn = new ButtonsFolder.CounterBtn(nextSongBtnTexture, nextSongBtnPos, nextSongBtnWidth, nextSongBtnHeight, Color.White);

        }

        //public static void CustomLoad() { } //flytta om så att denna blir en CustomLoad och den andra bara en snabb StandardLoad

        static KeyboardState kState;
        static bool playkeyIsPressed = false;
        static bool visibilityKeyIsPressed = false;
        public static void Update() {
            kState = Keyboard.GetState();

            //play/pause radio
            if (kState.IsKeyDown(playKey) && !playkeyIsPressed || playPauseBtn.IsClicked()) {
                if (kState.IsKeyDown(playKey)) { playkeyIsPressed = true; }
                isPlaying = !isPlaying;
                if (MediaPlayer.State == MediaState.Paused) {
                    try { MediaPlayer.Resume(); } // if "resume" fails, then just restart the song instead
                    catch { MediaPlayer.Play(song); }
                }
                else if (MediaPlayer.State == MediaState.Playing) {
                    MediaPlayer.Pause();
                }
                else {
                    try { MediaPlayer.Play(song); }
                    catch { MediaPlayer.Play(song); } //quickfix but works/prevents crashes from MediaPlayer
                }
            }
            if (kState.IsKeyUp(playKey)) { playkeyIsPressed = false; }

            if (isPlaying) { playPauseBtn.Texture = pauseBtnTexture; }
            else { playPauseBtn.Texture = playBtnTexture; }

            //toggle visibility for radio
            if (kState.IsKeyDown(toggleVisibility) && !visibilityKeyIsPressed) {
                visibilityKeyIsPressed = true;
                isVisible = !isVisible;
            }
            if (kState.IsKeyUp(toggleVisibility)) { visibilityKeyIsPressed = false; }

            //reset song
            if (resetSong) { //kanske lägg till "resetSongKey"
                resetSong = false;
                MediaPlayer.Play(song); //restarts song
            }

            //Next/previous Song
            if (nextSongBtn.IsClicked() || prevSongBtn.IsClicked()) { //ability to change songs is not yet added
                AddErrorMessage("function not yet added");
            }


        }

        static string tempMoveDownByString;
        public static void Draw(SpriteBatch spriteBatch) {
            if (isVisible) {
                spriteBatch.Draw(radioTexture, radioRec, Color.White);
                spriteBatch.DrawString(songNameSF, currentSongName, songNameSfPos, Color.White);

                //Buttons
                prevSongBtn.Draw(spriteBatch, btnColor);
                playPauseBtn.Draw(spriteBatch, Color.LightBlue);
                nextSongBtn.Draw(spriteBatch, btnColor);
            }
            if (displayErrorMsg) { //for displaying errormessages + conveying the user of other things that's not working as intended
                tempMoveDownByString = string.Empty; //resets "moveDownBy"-string
                for (int i = 0; i < errorMsgStrings.Count; i++) {
                    if (i == 0) { spriteBatch.DrawString(errorMsgSF, errorMsgStrings[i], Vector2.Zero, Color.Red); }
                    else {
                        tempMoveDownByString += "\n";
                        spriteBatch.DrawString(errorMsgSF, tempMoveDownByString + errorMsgStrings[i], Vector2.Zero, Color.Red);
                    }
                }
            }
        }

        static string[] tempStringArray;
        public static void SelectNewSong(string filePath) { //allows the user to change song
            try {
                if (song != null) {
                    song = Content.Load<Song>(@"" + filePath);
                    tempStringArray = filePath.Split('/');
                    currentSongName = tempStringArray[tempStringArray.Length - 1];
                }
            }
            catch {
                AddErrorMessage("Error loading song.");
            }
        }

        static bool messageFound;
        private static void AddErrorMessage(string message) { //adds errormessages to a displayable list
            messageFound = false;
            foreach (string s in errorMsgStrings) {
                if (s == message) { messageFound = true; break; }
            }
            if (!messageFound) {
                errorMsgStrings.Add(message);
            }
        }

        //Properties
        //bool//
        public static bool IsVisible { get { return isVisible; } set { isVisible = value; } }
        public static void ResetSong() { resetSong = true; isPlaying = true; }
        public static bool IsPlaying { get { return isPlaying; } set { isPlaying = value; } }
        //settings//
        public static Keys SetPlayKey { set { playKey = value; } }
        public static Keys SetToggleVsibilityKey { set { toggleVisibility = value; } }
        public static Color SetButtonColor { set { btnColor = value; } }
        public static float SetRadioVolume {
            set {
                if (value >= 0.91f) { MediaPlayer.Volume = 1.0f; }
                else if (value <= 0.09f) { MediaPlayer.Volume = 0.0f; }
                else { MediaPlayer.Volume = value; }
            }
        }
        public static bool DisplayErrorMessages { set { displayErrorMsg = value; } }
        public static void CleraErrorMessages() { errorMsgStrings.Clear(); }
        //measurements and textures//
        public static Texture2D SetRadioTexture { set { radioTexture = value; } }
        public static Texture2D SetPrevSongButtonTexture { set { prevSongBtnTexture = value; } }
        public static Texture2D SetNextSongButtonTexture { set { nextSongBtnTexture = value; } }
        public static Texture2D SetPlayButtonTexture { set { playBtnTexture = value; } }
        public static Texture2D SetPauseButtonTexture { set { pauseBtnTexture = value; } }
        public static Vector2 SetRadioPos { get { return radioPos; } set { radioPos = value; } }
        public static float SetRadioWidth {
            get { return radioWidth; }
            set {
                if (value > 1) { radioWidth = value; }
                else { radioWidth = 1; }
            }
        }
        public static float SetRadioHeight {
            get { return radioHeight; }
            set {
                if (value > 1) { radioHeight = value; }
                else { radioHeight = 1; }
            }
        }

    }
}
