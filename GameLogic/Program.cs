using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading; // Might need this later

// include external lib.
// include the namespace

using BoardLogic;

namespace BattleShipLogic
{
    class Program
    {
        public string[] CoordinatesArray = new string[100] 
       {"A1","A2","A3","A4","A5","A6","A7","A8","A9","A10",
        "B1","B2","B3","B4","B5","B6","B7","B8","B9","B10",
        "C1","C2","C3","C4","C5","C6","C7","C8","C9","C10",
        "D1","D2","D3","D4","D5","D6","D7","D8","D9","D10",
        "E1","E2","E3","E4","E5","E6","E7","E8","E9","E10",
        "F1","F2","F3","F4","F5","F6","F7","F8","F9","F10",
        "G1","G2","G3","G4","G5","G6","G7","G8","G9","G10",
        "H1","H2","H3","H4","H5","H6","H7","H8","H9","H10",
        "I1","I2","I3","I4","I5","I6","I7","I8","I9","I10",
        "J1","J2","J3","J4","J5","J6","J7","J8","J9","J10"};
        static void Main(string[] args)
        {

        }
    }

    class Player
    {
        //public void PlaceShips(float mousePosX, float mousePosY)
        //{
        //    // x and y coordinate has to be within the size of a tile
        //    const int shipsToPlace = 7;
        //    for (int i = 0; i < shipsToPlace; i++)
        //    {
        //        
        //    }
        //    // Track player mouse movement
        //    // Mouse click
        //    // Edit location of that ship in list
        //    // Display ship there
        //}
        public void StartTurn()
        {
            // OPTIONAL - AI thread is still running
            // Player is displayed what slots he's hit
        }
        public void FireShot()
        {
            // Track player mouse movement
            // Mouse click
            // Get enum for tile state from hashtable
            // change state
            // OPTIONAL - Play animation
        }
    }
    public class Opponent
    {
        public int m_LastHitx = -1;
        public int m_LastHity = -1;

        public void PlaceShips()
        {
            Board AIBoard = new Board(0, 0);
            Random random = new Random();
            int randomTile = random.Next(0, 100);
        }
        public void StartTurn()
        {

        }
        public void FireShot()
        {
            // 

            // Track player mouse movement
            // choose random locations
            // hasn't already been shot at (enum called state)
            // if hit, ready up next shot on adjacent tile

            // let AI know ship is SUNK
        }
    }

    class GameThread
    {
        public Thread th;
        public bool gameRunning = true; // ???
        ThreadPriority P = ThreadPriority.Normal;

        public void AIThread()  // return type void?
        {
            // initial ship placement
        }
        public void UserThread() // return type void?
        {

        }
        void run()
        {
            Console.WriteLine("i'm inside the thread now...");
            //for (int i = StartingPoint; i < 100; i++)
            //{
            //    Console.WriteLine(th.Name);
            //}
            //Console.WriteLine(th.Name + " is Done!");
        }
    }


}

