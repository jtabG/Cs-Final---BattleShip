using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

//error catching
using System.Runtime.InteropServices;

//custom libs
using Ships;    //justin
using BoardLogic;   //mark
using BattleShipLogic;  //tanner

namespace Cs_Final___BattleShip.Screens
{
    public enum GameStateEnum
    {
        RUNNING, WIN, LOSE
    }

    public class BattleShipGame : Screen
    {
        //error catchin
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        private BattleShipBoard m_PlayerBoard = null;

        private Board targetGrid = null;

        private Vector2 buttonBuffer = new Vector2(10, 10);
        private Vector2 buttonSize = new Vector2(200, 50);

        private Vector2 targetGridOffset = new Vector2(50, 50);
        private Vector2 targetGridWidthHeight;
        private Rectangle targetGridArea;

        private SpriteFont m_Font;
        private string reticleTextureName = "TargetReticle";
        private MouseState currentMouseState;

        private bool isPlayerTurn = true;

        private int numberOfEnemyShips = 7;
        private int numberOfPlayerShips = 7;

        private Opponent AIPlayer = null;

        //threads
        Thread AIThread = null;
        Thread AITargetThread = null;

        private GameStateEnum currentGameState = GameStateEnum.RUNNING;

        private BattleShipBoard enemyBoard;

        // override from screen
        public override void reset()
        {
            numberOfEnemyShips = 7;
            numberOfPlayerShips = 7;
            isPlayerTurn = true;
            currentGameState = GameStateEnum.RUNNING;
        }

        public override void update()
        {
            if (currentGameState != GameStateEnum.RUNNING) { endGame(); }
            
            base.update();

            MouseState lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (!isPlayerTurn) { AIFires(); }

            if (currentMouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
            {
                //left mouse up events
                if (isPlayerTurn && targetGridArea.Contains(new Point(currentMouseState.X, currentMouseState.Y)))
                {
                    fireOnGrid();

                    AITargetThread = new Thread(AITargettingThreadfunction);
                    AITargetThread.Start();     //ai begins target selection after each shot
                }
            }
        }

        public void AITargettingThreadfunction()
        {
            while (AIPlayer.AITarget == null)
            {
                AIPlayer.SelectTarget();
            }
        }

        public override void draw()
        {
            base.draw();
            if (m_PlayerBoard != null) { m_PlayerBoard.draw(); }
            drawTargetGrid();
            drawTargetGridCoorindates();
            drawGridLabels();

            if (targetGridArea.Contains(new Point(currentMouseState.X, currentMouseState.Y)))
            { drawReticle(); }
            else { ScreenManager.GameFramework.IsMouseVisible = true; }

        }

        public BattleShipGame()
        {
            ScreenWidth = 1900;
            ScreenHeight = 850;

            m_Font = ScreenManager.GameFramework.Content.Load<SpriteFont>("GridFont");
            
            // create ai and get reference to it's board
            AIPlayer = new Opponent();
            enemyBoard = AIPlayer.getBoard();
            // enemy places ship while player is placing ships
            AIThread = new Thread(AIPlayer.PlaceShips);

            // to place enemy grid to the right most side
            targetGridOffset.X = ScreenWidth - (((int)BoardEnum.SIZE_OF_BOARD + 1) * (int)BoardEnum.SIZE_OF_TILE);

            targetGrid = new Board((int)targetGridOffset.X, (int)targetGridOffset.Y);

            //define the size of the grid
            targetGridWidthHeight = new Vector2((int)BoardEnum.SIZE_OF_BOARD * (int)BoardEnum.SIZE_OF_TILE,
                                                (int)BoardEnum.SIZE_OF_BOARD * (int)BoardEnum.SIZE_OF_TILE);
            //used to see if mouse is in bounds of placement grid
            targetGridArea = new Rectangle((int)targetGridOffset.X, (int)targetGridOffset.Y, (int)targetGridWidthHeight.X, (int)targetGridWidthHeight.Y);

            AIThread.Start();

            ScreenManager.GameFramework.IsMouseVisible = true;

            populateButtons();
        }

        private void populateButtons()
        {
            //populate the button list
            Vector2 buttonPosition;
            //(ScreenManger.a_GraphicsDeviceManager.PreferredBackBufferWidth/2) - buttonSize.X/2;
            // buttons
            buttonPosition.X = (ScreenWidth / 2) - (buttonSize.X / 2);

            // exit button
            buttonPosition.Y = ScreenHeight - (buttonSize.Y + buttonBuffer.Y);

            m_Buttons.Add(new Button(   new Vector2(buttonPosition.X, buttonPosition.Y), //Position
                                        buttonSize,  //Frame size
                                        new Vector2(0, 0),  //Offset
                                        ScreenManager.GameFramework.Content.Load<Texture2D>("quitbutton"), //Texture
                                        quit /* click fucntion */ ));
        }

        private void quit()
        {
            ScreenManager.GameFramework.Exit();
        }

        public void setPlayerBoard(BattleShipBoard playerBoard)
        {
            m_PlayerBoard = playerBoard;
        }

        //draw functions
        public void drawTargetGrid()
        {
            List<List<Tile>> currentTiles = targetGrid.a_Tiles;

            for (int y = 0; y < (int)BoardEnum.SIZE_OF_BOARD; y++)
            {
                for (int x = 0; x < (int)BoardEnum.SIZE_OF_BOARD; x++)
                {
                    int xPos = currentTiles[x][y].a_X;
                    int yPos = currentTiles[x][y].a_Y;
                    int h = currentTiles[x][y].a_Height;
                    int w = currentTiles[x][y].a_Width;
                    short state = currentTiles[x][y].a_State;

                    string tileTextureName = "TileWater";
                    switch ((TileEnum)state)
                    {
                        case TileEnum.UNREVEALED:
                            tileTextureName = "TileWater";
                            break;

                        case TileEnum.NOT_HIT:
                            tileTextureName = "missed shot";
                            break;

                        case TileEnum.HIT:
                            tileTextureName = "shotHit";
                            break;

                        default:
                            tileTextureName = "TileWater";
                            break;
                    }
                    
                    Texture2D tileTexture = ScreenManager.GameFramework.Content.Load<Texture2D>(tileTextureName);

                    //see http://msdn.microsoft.com/en-us/library/ff433992.aspx for more information
                    Rectangle destinationRectangle = new Rectangle(xPos, yPos, w, h); //target

                    //Rectangle sourceRectangle = new Rectangle(0, 0, w, h); //origin

                    //sourceRectangle.X = w * state;

                    ScreenManager.SpriteBatch.Draw( tileTexture,
                                                    destinationRectangle,
                                                    null,//sourceRectangle,
                                                    Microsoft.Xna.Framework.Color.White,
                                                    0,
                                                    new Vector2(0, 0),
                                                    0,
                                                    0.0f);
                }
            }
        }

        public void drawTargetGridCoorindates()
        {
            //TOP ROW COORDINATES
            Vector2 textOffset = targetGridOffset;
            textOffset.Y -= 35;
            textOffset.X += 25;
            m_Font.Spacing = 26;
            ScreenManager.SpriteBatch.DrawString(m_Font, m_PlayerBoard.horizontalGridCoorinates, textOffset, Color.GhostWhite);

            //SIDE COORDINATES
            textOffset = targetGridOffset;
            textOffset.Y += 20;
            textOffset.X += targetGridWidthHeight.X + 20;
            m_Font.Spacing = 0;
            for (int i = 0; i < 10; i++)
            {
                ScreenManager.SpriteBatch.DrawString(m_Font, m_PlayerBoard.verticalGridCoorinates[i], textOffset, Color.GhostWhite);
                textOffset.Y += 70;
            }
        }

        public void drawGridLabels()
        {
            //Your ships
            Vector2 textOffset = new Vector2();
            textOffset.Y = targetGridOffset.Y + targetGridWidthHeight.Y + (int)BoardEnum.SIZE_OF_TILE/2;
            textOffset.X = (int)BoardEnum.SIZE_OF_TILE * ((int)BoardEnum.SIZE_OF_BOARD / 2);
            m_Font.Spacing = 0;
            ScreenManager.SpriteBatch.DrawString(m_Font, "Your Fleet", textOffset, Color.RoyalBlue);

            //target grid
            textOffset.X = targetGridOffset.X + (targetGridWidthHeight.X / 2) - (int)BoardEnum.SIZE_OF_TILE;
            ScreenManager.SpriteBatch.DrawString(m_Font, "Enemy Waters", textOffset, Color.Crimson);
        }

        public void drawReticle()
        {
            ScreenManager.GameFramework.IsMouseVisible = false;
            Texture2D reticleTexture = ScreenManager.GameFramework.Content.Load<Texture2D>(reticleTextureName);

            int w = (int)BoardEnum.SIZE_OF_TILE;
            int h = (int)BoardEnum.SIZE_OF_TILE;

            //draw centered on mouse
            Rectangle destinationRectangle = new Rectangle(currentMouseState.X - w/2, currentMouseState.Y - h/2, w, h);

            ScreenManager.SpriteBatch.Draw( reticleTexture,
                                            destinationRectangle,
                                            null,
                                            Microsoft.Xna.Framework.Color.White,
                                            0.0f,
                                            new Vector2(0, 0),
                                            0,
                                            0.0f);
        }

        void fireOnGrid()
        {
            //MessageBox(new IntPtr(0), "Firing at enemy waters boom boom", "Firing", 0);
            
            Tile foundTile = targetGrid.findTileFromCoordinate(currentMouseState.X,currentMouseState.Y);
            int indexX = foundTile.a_IndexX;
            int indexY = foundTile.a_IndexY;

            if (foundTile.a_State != (short)TileEnum.UNREVEALED) { return; }    //prevent firing at the same spot

            TileEnum resultOfShot = TileEnum.NOT_HIT;

            foreach (Ship ship in enemyBoard.a_ListOfShips)
            {
                if (ship.shotAtShip(indexX, indexY))
                {
                    resultOfShot = TileEnum.HIT;

                    //shotHitShip changes hit locations and increments hits taken, returns true if ship sunk
                    if (ship.shotHitShip(indexX, indexY))   
                    {
                        numberOfEnemyShips--;
                        //string you sunk my battlesharp o_0
                        currentGameState = (numberOfEnemyShips <= 0) ? GameStateEnum.WIN : GameStateEnum.RUNNING;
                    }
                }
            }

            foundTile.a_State = (short)resultOfShot;

            isPlayerTurn = !isPlayerTurn; // switch turns
        }

        public void AIFires()
        {
            if (AIPlayer.AITarget == null) { return; }

            int indexX = AIPlayer.AITarget.a_IndexX;
            int indexY = AIPlayer.AITarget.a_IndexY;

            TileEnum resultOfShot = TileEnum.NOT_HIT;

            foreach (Ship ship in m_PlayerBoard.a_ListOfShips)
            {
                if (ship.shotAtShip(indexX, indexY))
                {
                    resultOfShot = TileEnum.HIT;

                    //shotHitShip changes hit locations and increments hits taken, returns true if ship sunk
                    if (ship.shotHitShip(indexX, indexY))
                    {
                        numberOfPlayerShips--;
                        //string you sunk my battlesharp o_0
                        currentGameState = (numberOfPlayerShips <= 0) ? GameStateEnum.LOSE : GameStateEnum.RUNNING;
                    }
                }
            }

            AIPlayer.AITarget.a_State = (short)resultOfShot;
            //show result to player
            m_PlayerBoard.PlacementBoard.a_Tiles[indexX][indexY].a_State = (short)resultOfShot;

            AIPlayer.AITarget = null;

            isPlayerTurn = !isPlayerTurn; // switch turns
        }

        private void endGame()
        {
            GameOverScreen temp = (GameOverScreen)ScreenManager.getInstance().getScreen(ScreenManager.ScreenEnum.GameOver);
            temp.setGameResult(currentGameState);

            ScreenManager.GameFramework.IsMouseVisible = true;
            ScreenManager.getInstance().switchScreen(ScreenManager.ScreenEnum.GameOver);
        }
    }
}
