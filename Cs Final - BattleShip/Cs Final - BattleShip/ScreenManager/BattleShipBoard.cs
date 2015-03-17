using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public class BattleShipBoard : Screen
    {
        //error catchin
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        private MouseState currentMouseState;

        private List<Ship> ListOfShips = new List<Ship>();
        public List<Ship> a_ListOfShips        { get { return ListOfShips; } set { ListOfShips = value; } }

        private bool[] m_AvailableSlots = new bool[100];
        public bool[] AvailableSlots { get { return m_AvailableSlots; } set { m_AvailableSlots = value; } }

        private bool placingVertical = false;
        public bool PlacingVertical { get { return placingVertical; } set { placingVertical = value; } }
        
        private Ship selectedShip = null;
        public Ship SelectedShip { get { return selectedShip; } set { selectedShip = value; } }

        private Vector2 gridOffset = new Vector2(50, 50);
        private Vector2 gridWidthHeight;
        private Rectangle placementGrid;

        private Vector2 buttonBuffer = new Vector2(10, 10);
        private Vector2 buttonSize = new Vector2(200, 50);

        private bool gameCanStart = false;
        private SpriteFont m_Font;

        public readonly string horizontalGridCoorinates = "A B C D E F G H I J";
        public readonly string [] verticalGridCoorinates = {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"};

        private bool playingGame = false;

        private Board placementBoard;
        public Board PlacementBoard { get { return placementBoard; } set { placementBoard = value; } }


        private int[] numberOfShipsByType = { 0, 0, 0, 0, 0};


        enum ShipButtonEnum
        {
            Carrier = 1, Battleship, Cruiser, Destroyer, Submarine, error
        }

        public enum ShipTypeEnum
        {//smallest to largest to allow easy comparison to ship size
            Submarine, Destroyer, Cruiser, Battleship, Carrier, NumberOfShipTypes
        }

        //overrides from screen
        public override void update()
        {
            base.update();

            MouseState lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (currentMouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
            {
                //left mouse up events
                if (placementGrid.Contains(new Point(currentMouseState.X, currentMouseState.Y)) && selectedShip != null)
                {
                    //MessageBox(new IntPtr(0), "in gid", "Error info", 0);
                    placeShip(currentMouseState.X, currentMouseState.Y);
                }
            }

            if (currentMouseState.RightButton == ButtonState.Released && lastMouseState.RightButton == ButtonState.Pressed)
            {//swap vertical placement bool on right mouse up
                placingVertical = !placingVertical;
            }
        }

        public override void draw()
        {
            //if not playing game, draw buttons
            if (!playingGame) { base.draw(); }

            drawTiles();
            drawPlacedShips();
            drawGridCoorindates();

            if (selectedShip != null)   { drawShipOnMouse(); }
        }

        public override void reset()
        {
            ScreenManager.GameFramework.IsMouseVisible = true;
            ListOfShips.Clear();
            selectedShip = null;
            gameCanStart = false;    //reset to false after test
            playingGame = false;

            for (int i = 0; i < (int)ShipTypeEnum.NumberOfShipTypes; i++)
            {
                numberOfShipsByType[i] = 0;
            }

            for (int i = 0; i < 100; i++)
            {
                AvailableSlots[i] = true;
            }
        }

        //Button functions
        private void quit()
        {
            ScreenManager.GameFramework.Exit();
        }

        public void selectAShip()
        {
            //Carrier = 1, Battleship, Cruiser, Destroyer, Submarine
            ShipButtonEnum foundship = ShipButtonEnum.error;
            for (int i = 1; i < 6; i++)
            {
                if (currentMouseState.Y >= (i * buttonSize.Y) + (buttonBuffer.Y * (4 * i)) &&
                    currentMouseState.Y <= (i * buttonSize.Y) + (buttonBuffer.Y * (4 * i)) + buttonSize.Y)
                {
                    foundship = (ShipButtonEnum)i;
                    break;
                }
            }

            switch (foundship)
            {  //each time a ship is selected, check to see if limit is reached for that shiptype
                case ShipButtonEnum.Battleship:
                    selectedShip = numberOfShipsByType[(int)ShipTypeEnum.Battleship] < 1 ? new Battleship() : null;
                    break;

                case ShipButtonEnum.Carrier:
                    selectedShip = numberOfShipsByType[(int)ShipTypeEnum.Carrier] < 1 ? new Carrier() : null;
                    break;

                case ShipButtonEnum.Cruiser:
                    selectedShip = numberOfShipsByType[(int)ShipTypeEnum.Cruiser] < 1 ? new Cruiser() : null;
                    break;

                case ShipButtonEnum.Destroyer:
                    selectedShip = numberOfShipsByType[(int)ShipTypeEnum.Destroyer] < 2 ? new Destroyer() : null;
                    break;

                case ShipButtonEnum.Submarine:
                    selectedShip = numberOfShipsByType[(int)ShipTypeEnum.Submarine] < 2 ? new Submarine() : null;
                    break;

                default:
                    //MessageBox(new IntPtr(0), "Problem at ship assignment switch", "Error info", 0);
                    selectedShip = null;
                    break;
            }

            bool holdingShip = selectedShip == null ? true : false;
            ScreenManager.GameFramework.IsMouseVisible = holdingShip;
        }

        void startGame()
        {
            if (!gameCanStart)
            {
                return;
            }

            //MessageBox(new IntPtr(0), "you started the game", "the game", 0);
            BattleShipGame temp = (BattleShipGame)ScreenManager.getInstance().getScreen(ScreenManager.ScreenEnum.BattleshipGame);
            temp.setPlayerBoard(this);
            playingGame = true;
            ScreenManager.getInstance().switchScreen(ScreenManager.ScreenEnum.BattleshipGame);
        }

        // set up
        public BattleShipBoard()
        {
            reset();

            m_Font = ScreenManager.GameFramework.Content.Load<SpriteFont>("GridFont");

            ScreenWidth = 1024;
            ScreenHeight = 768;

            try
            {
                placementBoard = new Board((int)gridOffset.X, (int)gridOffset.Y);
            }
            catch (Exception ex)
            {
                MessageBox(new IntPtr(0), ex.ToString(), "Error info", 0);
                quit();
            }

            //define the size of the grid
            gridWidthHeight = new Vector2(  (int)BoardEnum.SIZE_OF_BOARD * (int)BoardEnum.SIZE_OF_TILE,
                                            (int)BoardEnum.SIZE_OF_BOARD * (int)BoardEnum.SIZE_OF_TILE);
            //used to see if mouse is in bounds of placement grid
            placementGrid = new Rectangle((int)gridOffset.X, (int)gridOffset.Y, (int)gridWidthHeight.X, (int)gridWidthHeight.Y);

            populateButtons();

        }

        private void populateButtons()
        {
            //populate the button list
            Vector2 buttonPosition;

            // buttons
            buttonPosition.X = ScreenManager.GraphicsDeviceManager.PreferredBackBufferWidth - (buttonSize.X + buttonBuffer.X);

            // Carrier ship button          //Carrier = 1, Battleship, Cruiser, Destroyer, Submarine
            buttonPosition.Y = ((int)ShipButtonEnum.Carrier * buttonSize.Y) + (buttonBuffer.Y * (4 * (int)ShipButtonEnum.Carrier));

            m_Buttons.Add(new Button(new Vector2(buttonPosition.X, buttonPosition.Y), //Position
                                        buttonSize,  //Frame size
                                        new Vector2(0, 0),  //Offset
                                        ScreenManager.GameFramework.Content.Load<Texture2D>("carrierbutton"), //Texture
                                        selectAShip /* click fucntion */ ));

            // Battleship ship button          //Carrier = 1, Battleship, Cruiser, Destroyer, Submarine
            buttonPosition.Y = ((int)ShipButtonEnum.Battleship * buttonSize.Y) + (buttonBuffer.Y * (4 * (int)ShipButtonEnum.Battleship));

            m_Buttons.Add(new Button(new Vector2(buttonPosition.X, buttonPosition.Y), //Position
                                        buttonSize,  //Frame size
                                        new Vector2(0, 0),  //Offset
                                        ScreenManager.GameFramework.Content.Load<Texture2D>("battleshipbutton"), //Texture
                                        selectAShip /* click fucntion */ ));

            // Cruiser ship button          //Carrier = 1, Battleship, Cruiser, Destroyer, Submarine
            buttonPosition.Y = ((int)ShipButtonEnum.Cruiser * buttonSize.Y) + (buttonBuffer.Y * (4 * (int)ShipButtonEnum.Cruiser));

            m_Buttons.Add(new Button(new Vector2(buttonPosition.X, buttonPosition.Y), //Position
                                        buttonSize,  //Frame size
                                        new Vector2(0, 0),  //Offset
                                        ScreenManager.GameFramework.Content.Load<Texture2D>("cruiserbutton"), //Texture
                                        selectAShip /* click fucntion */ ));

            // Destroyer ship button          //Carrier = 1, Battleship, Cruiser, Destroyer, Submarine
            buttonPosition.Y = ((int)ShipButtonEnum.Destroyer * buttonSize.Y) + (buttonBuffer.Y * (4 * (int)ShipButtonEnum.Destroyer));

            m_Buttons.Add(new Button(new Vector2(buttonPosition.X, buttonPosition.Y), //Position
                                        buttonSize,  //Frame size
                                        new Vector2(0, 0),  //Offset
                                        ScreenManager.GameFramework.Content.Load<Texture2D>("destroyerbutton"), //Texture
                                        selectAShip /* click fucntion */ ));

            // Submarine ship button          //Carrier = 1, Battleship, Cruiser, Destroyer, Submarine
            buttonPosition.Y = ((int)ShipButtonEnum.Submarine * buttonSize.Y) + (buttonBuffer.Y * (4 * (int)ShipButtonEnum.Submarine));

            m_Buttons.Add(new Button(new Vector2(buttonPosition.X, buttonPosition.Y), //Position
                                        buttonSize,  //Frame size
                                        new Vector2(0, 0),  //Offset
                                        ScreenManager.GameFramework.Content.Load<Texture2D>("submarinebutton"), //Texture
                                        selectAShip /* click fucntion */ ));


            // start button
            buttonPosition.Y = buttonBuffer.Y/2;

            m_Buttons.Add(new Button(new Vector2(buttonPosition.X, buttonPosition.Y), //Position
                                        buttonSize,  //Frame size
                                        new Vector2(0, 0),  //Offset
                                        ScreenManager.GameFramework.Content.Load<Texture2D>("startgamebutton"), //Texture
                                        startGame /* click fucntion */ ));

            // reset button
            buttonPosition.Y = ScreenManager.GraphicsDeviceManager.PreferredBackBufferHeight - (buttonSize.Y * 2 + buttonBuffer.Y * 2);

            m_Buttons.Add(new Button(new Vector2(buttonPosition.X, buttonPosition.Y), //Position
                                        buttonSize,  //Frame size
                                        new Vector2(0, 0),  //Offset
                                        ScreenManager.GameFramework.Content.Load<Texture2D>("resetbutton"), //Texture
                                        reset /* click fucntion */ ));

            // exit button
            buttonPosition.Y = ScreenManager.GraphicsDeviceManager.PreferredBackBufferHeight - (buttonSize.Y + buttonBuffer.Y);

            m_Buttons.Add(new Button(new Vector2(buttonPosition.X, buttonPosition.Y), //Position
                                        buttonSize,  //Frame size
                                        new Vector2(0, 0),  //Offset
                                        ScreenManager.GameFramework.Content.Load<Texture2D>("quitbutton"), //Texture
                                        quit /* click fucntion */ ));

        }

        
        public void checkTiles()
        {
            //MouseState lastMouseState = currentMouseState;
            //currentMouseState = Mouse.GetState();

            //if (currentMouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
            //{
                List<List<Tile>> currentTiles = placementBoard.a_Tiles;

                for (int y = 0; y < (int)BoardEnum.SIZE_OF_BOARD; y++)
                {
                    for (int x = 0; x < (int)BoardEnum.SIZE_OF_BOARD; x++)
                    {
                        currentTiles[x][y].mouseLeftClickUpEvent(currentMouseState.X, currentMouseState.Y);
                    }
                }
            //}

        }

        public bool placeShip(int mouseX, int mouseY)
        {
            Tile foundTile = placementBoard.findTileFromCoordinate(mouseX, mouseY);
            int indexX = foundTile.a_IndexX;
            int indexY = foundTile.a_IndexY;

            if (placingVertical)
            {
                if (indexY + selectedShip.Size < 11)
                {
                    // if the position is already occupied
                    // moved here due to out of index possibility if checked before checking range
                    if (!spaceAvailable(indexX, indexY))
                    {
                        return false;
                    }

                    //place ship, set its members variables and add it to list
                    selectedShip.IsVertical = placingVertical;
                    selectedShip.StartLocationIndexX = indexX;
                    selectedShip.StartLocationIndexY = indexY;

                    for (int i = 0; i < selectedShip.Size; i++)
                    {
                        AvailableSlots[(indexY + i) * 10 + indexX] = false;
                    }

                    ListOfShips.Add(selectedShip);

                    // get the type of ship placed based on it's size and increment the number
                    // of ships of that type that have been placed
                    int shipTypeIndex = selectedShip.Size - 1;
                    numberOfShipsByType[shipTypeIndex]++;

                    selectedShip = null;

                    if (ListOfShips.Count >= 7)
                    {
                        gameCanStart = true;
                    }
                }
                else { return false; }
            }
            else
            {
                if (indexX + selectedShip.Size < 11)
                {
                    // if the position is already occupied
                    // moved here due to out of index possibility if checked before checking range
                    if (!spaceAvailable(indexX, indexY))
                    {
                        return false;
                    }
                    //place ship, set its members variables and add it to list
                    selectedShip.IsVertical = placingVertical;
                    selectedShip.StartLocationIndexX = indexX;
                    selectedShip.StartLocationIndexY = indexY;

                    for (int i = 0; i < selectedShip.Size; i++)
                    {
                        AvailableSlots[indexY * 10 + indexX + i] = false;
                    }

                    ListOfShips.Add(selectedShip);

                    // get the type of ship placed based on it's size and increment the number
                    // of ships of that type that have been placed
                    int shipTypeIndex = selectedShip.Size - 1;
                    numberOfShipsByType[shipTypeIndex]++;

                    selectedShip = null;

                    if (ListOfShips.Count >= 7)
                    {
                        gameCanStart = true;
                    }
                }
                else { return false; }
            }

            bool notHoldingShip = selectedShip == null ? true : false;
            ScreenManager.GameFramework.IsMouseVisible = notHoldingShip;
            return true;
        }

        bool spaceAvailable(int indexX, int indexY)
        {
            bool available = true;

            if (placingVertical)
            {
                for (int i = 0; i < selectedShip.Size; i++)
                {
                    if (AvailableSlots[(indexY + i) * 10 + indexX] == false)
                    {
                        available = false;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < selectedShip.Size; i++)
                {
                    if (AvailableSlots[indexY * 10 + indexX + i] == false)
                    {
                        available = false;
                        break;
                    }
                }
            }
            return available;
        }


        // drawing functions
        void drawPlacedShips()
        {
            foreach (Ship ship in ListOfShips)
            {
                string shipType = ship.getShipType();
                //shipType = "TestButton";    //remove when have textures for ships
                Texture2D selectedShipTexture = ScreenManager.GameFramework.Content.Load<Texture2D>(shipType);

                int w = (int)BoardEnum.SIZE_OF_TILE * ship.Size;
                int h = (int)BoardEnum.SIZE_OF_TILE;

                int x = placementBoard.a_Tiles[ship.StartLocationIndexX][ship.StartLocationIndexY].a_X;
                x = ship.IsVertical ? x + (int)BoardEnum.SIZE_OF_TILE : x ;
                
                int y = placementBoard.a_Tiles[ship.StartLocationIndexX][ship.StartLocationIndexY].a_Y;

                Rectangle destinationRectangle = new Rectangle(x, y, w, h);

                float rotation = ship.IsVertical ? (MathHelper.Pi / 2.0f) : 0.0f;

                ScreenManager.SpriteBatch.Draw( selectedShipTexture,
                                                destinationRectangle,
                                                null,
                                                Microsoft.Xna.Framework.Color.White,
                                                rotation,
                                                new Vector2(0, 0),
                                                0,
                                                0.0f);
                
                //reset for drawing hit locations
                w = (int)BoardEnum.SIZE_OF_TILE;
                Texture2D HitTexture = ScreenManager.GameFramework.Content.Load<Texture2D>("shotHit");
                for (int i = 0; i < ship.Size; i++)
                {
                    x = placementBoard.a_Tiles[ship.StartLocationIndexX][ship.StartLocationIndexY].a_X;
                    y = placementBoard.a_Tiles[ship.StartLocationIndexX][ship.StartLocationIndexY].a_Y;

                    if (ship.HitLocations[i])
                    {
                        if (ship.IsVertical)
                        {   y += h * i;   }
                        else
                        {   x += w * i;   }

                        destinationRectangle = new Rectangle(x, y, w, h);

                        ScreenManager.SpriteBatch.Draw( HitTexture,
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
        }

        void drawGridCoorindates()
        {
            //TOP ROW COORDINATES
            Vector2 textOffset = gridOffset;
            textOffset.Y -= 35;
            textOffset.X *= 1.6f;
            m_Font.Spacing = 26;
            ScreenManager.SpriteBatch.DrawString(m_Font, horizontalGridCoorinates, textOffset, Color.GhostWhite);

            //SIDE COORDINATES
            textOffset = gridOffset;
            textOffset.Y += 20;
            textOffset.X -= 30;
            m_Font.Spacing = 0;
            for (int i = 0; i < 10; i++)
            {
                ScreenManager.SpriteBatch.DrawString(m_Font, verticalGridCoorinates[i], textOffset, Color.GhostWhite);
                textOffset.Y += 70;
            }
        }

        public void drawTiles()
        {
            List<List<Tile>> currentTiles = placementBoard.a_Tiles;
            
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

        public void drawShipOnMouse()
        {
            string shipType = selectedShip.getShipType();
            //shipType = "TestButton";    //remove when have textures for ships
            Texture2D selectedShipTexture = ScreenManager.GameFramework.Content.Load<Texture2D>(shipType);

            int tileOffset = (int)BoardEnum.SIZE_OF_TILE / 2;
            int w = (int)BoardEnum.SIZE_OF_TILE * selectedShip.Size;
            int h = (int)BoardEnum.SIZE_OF_TILE;// -tileOffset;

            //w = placingVertical ? (w + tileOffset) : (w - tileOffset);

            //draw centered on mouse (change of plans)
            //Rectangle destinationRectangle = new Rectangle(currentMouseState.X - w/2, currentMouseState.Y - h/2, w, h);
            int x = placingVertical ? (currentMouseState.X + tileOffset) : (currentMouseState.X - tileOffset);
            //int x = placingVertical ? (currentMouseState.X + h / 2) : (currentMouseState.X - h / 2);
            int y = currentMouseState.Y - tileOffset;

            Rectangle destinationRectangle = new Rectangle(x, y, w, h);

            float rotation = placingVertical ? (MathHelper.Pi / 2.0f) : 0.0f;

            ScreenManager.SpriteBatch.Draw(selectedShipTexture,
                                            destinationRectangle,
                                            null,
                                            Microsoft.Xna.Framework.Color.White,
                                            rotation,
                                            new Vector2(0, 0),
                                            0,
                                            0.0f);
        }

    }
}
