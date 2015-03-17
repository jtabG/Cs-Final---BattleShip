using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoardLogic
{
    public enum BoardEnum
    {
        SIZE_OF_BOARD = 10,
        SIZE_OF_TILE  = 70

    }

    public class Board
    {
        //Member variables
        protected List<List<Tile>> m_Tiles;

        //Member functions
        public List<List<Tile>> a_Tiles { get {return m_Tiles;} set {m_Tiles = value;} }


        public Tile findTileFromCoordinate(int positionX, int positionY)
        {
            Tile foundTile = null;
            for (int y = 0; y < (int)BoardEnum.SIZE_OF_BOARD; y++)
            {
                for (int x = 0; x < (int)BoardEnum.SIZE_OF_BOARD; x++)
                {
                    if (
                        positionX >= m_Tiles[x][y].a_X && positionX <= (m_Tiles[x][y].a_X + m_Tiles[x][y].a_Width) &&//range along X axis
                        positionY >= m_Tiles[x][y].a_Y && positionY <= (m_Tiles[x][y].a_Y + m_Tiles[x][y].a_Height)   //range along Y axis
                        )
                    {
                        foundTile = m_Tiles[x][y];
                        //makeshift break from both loops
                        y = (int)BoardEnum.SIZE_OF_BOARD + 1;
                        break;
                    }
                }
            }

            return foundTile;
        }

        //public int findXCoordinateFromTile(Tile tile)
        //{
        //    tile.
        //}

        //Constructors
        //public Board()
        public Board(int startingX = 0, int startingY = 0)
        {            
            //Create and populate m_Tiles
            m_Tiles = new List<List<Tile>>();

            int xOffset = startingX;
            int yOffset = startingY;

            for (int y = 0; y < (int)BoardEnum.SIZE_OF_BOARD; y++)
            {
                m_Tiles.Add(new List<Tile>());
            }
            
            for (int y = 0; y < (int)BoardEnum.SIZE_OF_BOARD; y++)
            {
                for (int x = 0; x < (int)BoardEnum.SIZE_OF_BOARD; x++)
                {
                    m_Tiles[x].Add(new Tile(x * (int)BoardEnum.SIZE_OF_TILE + startingX,
                                            y * (int)BoardEnum.SIZE_OF_TILE + startingY,
                                            (int)BoardEnum.SIZE_OF_TILE,
                                            (int)BoardEnum.SIZE_OF_TILE,
                                            x, y));
                }

            }

            // caused runtime crash due to the initial list of lists going out of index

            //for (int y = 0; y < (int)BoardEnum.SIZE_OF_BOARD; y++)
            //{
            //    m_Tiles.Add(new List<Tile>());

            //    for (int x = 0; x < (int)BoardEnum.SIZE_OF_BOARD; x++)
            //    {
            //        m_Tiles[x].Add(new Tile(x * (int)BoardEnum.SIZE_OF_TILE,
            //                                y * (int)BoardEnum.SIZE_OF_TILE,
            //                                (int)BoardEnum.SIZE_OF_TILE,
            //                                (int)BoardEnum.SIZE_OF_TILE));                
            //    }

            //}

        }

    }

}
