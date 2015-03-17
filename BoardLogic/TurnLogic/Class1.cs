using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoardLogic
{
    public class Class1
    {
        public enum TileType
        {
            HIT        =  0 ,
            MISS       =  1 ,
            UNSELECTED =  2 ,
            BOARD_SIZE = 10 ,

        }

        public class BoardLogic
        {
            //Member variables
            private Tile[,] board;


            private bool playersTurn = true;
            public bool PlayersTurn
            {
                get { return playersTurn; }
                set { playersTurn = value; }
            }

            
            public void TurnUpdates(Array Tiles)
            {

            }

            public void TurnChange(int index)
            {
                playersTurn = !playersTurn;
                
                
                //board[index, index] = getTileType();

                //if (playersTurn)
                //{
                //    playersTurn = false;
                //}
                //else
                //{
                //    playersTurn = true;
                //}
            }


        }
    }
}
