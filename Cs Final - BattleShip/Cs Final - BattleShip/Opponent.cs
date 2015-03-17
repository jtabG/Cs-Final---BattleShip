using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BoardLogic;
using Ships;

namespace Cs_Final___BattleShip.Screens
{
    class Opponent
    {
        // Member Variables
        public int m_LastHitx = -1;
        public int m_LastHity = -1;
        public int[] AILimitOfShipPerType = { 2, 2, 1, 1, 1 };
        public Tile AITarget = null; // AI's intended target gets pasted over this over and over

        BattleShipBoard AIBoard = new BattleShipBoard();
        Board AIBoardForTargeting = new Board();

        //in place to have access for AI player info in game
        public BattleShipBoard getBoard() { return AIBoard; }

        public void PlaceShips()
        {
            //Board AIBoard = new Board(0, 0);

            for (int i = 0; i < (int)BattleShipBoard.ShipTypeEnum.NumberOfShipTypes; i++) //(int)____ where "____" is what int is cast as
            {
                Random randomForPlacement = new Random();

                for (int numOfShipsPlaced = 0; numOfShipsPlaced < AILimitOfShipPerType[i]; numOfShipsPlaced++)
                {
                    Ship tempShip = null;
                    switch ((BattleShipBoard.ShipTypeEnum)i)
                    {
                        case BattleShipBoard.ShipTypeEnum.Submarine:
                            tempShip = new Submarine();
                            break;
                        case BattleShipBoard.ShipTypeEnum.Destroyer:
                            tempShip = new Destroyer();
                            break;
                        case BattleShipBoard.ShipTypeEnum.Cruiser:
                            tempShip = new Cruiser();
                            break;
                        case BattleShipBoard.ShipTypeEnum.Battleship:
                            tempShip = new Battleship();
                            break;
                        case BattleShipBoard.ShipTypeEnum.Carrier:
                            tempShip = new Carrier();
                            break;
                        default:
                            tempShip = null;
                            break;
                    }

                    Random secondRandomForPlacement = new Random(randomForPlacement.Next());
                    bool useRandomOne = true;

                    bool tileFound = false;
                    while (!tileFound)
                    {
                        int randomTileX = useRandomOne ? randomForPlacement.Next(0, 10) : secondRandomForPlacement.Next(10);
                        int randomTileY = useRandomOne ? randomForPlacement.Next(0, 10) : secondRandomForPlacement.Next(10);

                        Tile tempTile = AIBoard.PlacementBoard.a_Tiles[randomTileX][randomTileY];
                        AIBoard.SelectedShip = tempShip;
                        AIBoard.PlacingVertical = randomForPlacement.Next(0, 1) == 0 ? true : false; // !!! inline if statement

                        tileFound = AIBoard.placeShip(tempTile.a_X, tempTile.a_Y);
                        useRandomOne = !useRandomOne;
                    }
                }

            }
        }
        public void StartTurn()
        {

        }


        public void SelectTarget()
        {
            // Choose Random Location
            Random randomForTarget = new Random();
            bool targetNotFound = true;
            while (targetNotFound)
            {
                int randomTileX = randomForTarget.Next(0, 10);
                int randomTileY = randomForTarget.Next(0, 10);
                Tile tempTile = AIBoardForTargeting.a_Tiles[randomTileX][randomTileY];

                if (tempTile.a_State == (short)TileEnum.UNREVEALED) // because a_State is a short
                {
                    targetNotFound = false;
                    AITarget = tempTile;
                }
            }
        }
        //public void Shoot()
        //{
        //    if (AITarget == null)
        //    {
        //        return;
        //    }
        //}
    }
}
