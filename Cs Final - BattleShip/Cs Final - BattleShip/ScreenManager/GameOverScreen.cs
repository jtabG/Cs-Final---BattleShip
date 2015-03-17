using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Cs_Final___BattleShip.Screens
{
    class GameOverScreen : Screen
    {
        public override void update()
        {
            base.update();
        }

        public override void draw()
        {
            drawBackTexture();

            base.draw();
        }

        public override void reset()
        {

        }

        private Vector2 buttonBuffer = new Vector2(10, 10);
        private Vector2 buttonSize = new Vector2(200, 50);

        private GameStateEnum gameResult = GameStateEnum.RUNNING;
        public void setGameResult(GameStateEnum result) { gameResult = result; }

        public GameOverScreen()
        {
            ScreenWidth = 1024;
            ScreenHeight = 768;

            //populate the button list
            Vector2 buttonPosition;
            //(ScreenManger.a_GraphicsDeviceManager.PreferredBackBufferWidth/2) - buttonSize.X/2;
            // buttons
            buttonPosition.X = (ScreenWidth / 2) - (buttonSize.X / 2);

            // exit button
            buttonPosition.Y = ScreenHeight - (buttonSize.Y + buttonBuffer.Y);

            m_Buttons.Add(new Button(new Vector2(buttonPosition.X, buttonPosition.Y), //Position
                                        buttonSize,  //Frame size
                                        new Vector2(0, 0),  //Offset
                                        ScreenManager.GameFramework.Content.Load<Texture2D>("quitbutton"), //Texture
                                        quit /* click fucntion */ ));
        }

        private void quit()
        {
            ScreenManager.GameFramework.Exit();
        }

        private void drawBackTexture()
        {
            string backTextureName = gameResult == GameStateEnum.WIN ? "winScreen" : "loseScreen";

            Texture2D backTexture = ScreenManager.GameFramework.Content.Load<Texture2D>(backTextureName);

            Rectangle destinationRectangle = new Rectangle(0, 0, ScreenWidth, ScreenHeight);

            ScreenManager.SpriteBatch.Draw( backTexture,
                                            destinationRectangle,
                                            null,
                                            Microsoft.Xna.Framework.Color.White,
                                            0,
                                            new Vector2(0, 0),
                                            0,
                                            0.0f);
        }
    }
}
