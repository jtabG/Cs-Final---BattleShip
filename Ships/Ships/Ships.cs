using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ships
{
    public abstract class Ship
    {
        private bool isAlive = true;
        public bool IsAlive         { get { return isAlive; } set { isAlive = value; } }


        private int hitsTaken = 0;
        public int HitsTaken        { get { return hitsTaken; } set { hitsTaken = value; } }


        private int size = 0;
        public int Size        { get { return size; } set { size = value; } }


        private bool isVertical = true;
        public bool IsVertical        { get { return isVertical; } set { isVertical = value; } }


        private int startLocationIndexX = 0;
        public int StartLocationIndexX        { get { return startLocationIndexX; } set { startLocationIndexX = value; } }


        private int startLocationIndexY = 0;
        public int StartLocationIndexY        { get { return startLocationIndexY; } set { startLocationIndexY = value; } }


        private bool[] hitLocations = null;
        public bool[] HitLocations        { get { return hitLocations; } set { hitLocations = value; } }

        //private string[] coordinateLocations = null;

        //public string[] CoordinateLocations
        //{
        //    get { return coordinateLocations; }
        //    set { coordinateLocations = value; }
        //}

        public abstract string getShipType(); //override
        //{
        //    return "Undefined";
        //}


        public bool shotHitShip(int locationX, int locationY) //override
        {
            if (IsVertical)
            {
                hitLocations[locationY - StartLocationIndexY] = true;
            }
            else
            {
                hitLocations[locationX - StartLocationIndexX] = true;
            }

            HitsTaken++;

            return (HitsTaken >= Size) ? true : false;
        }

        public bool shotAtShip(int locationX, int locationY)
        {
            bool result = false;

            if (IsVertical)
            {
                if (locationX != StartLocationIndexX) { return false; }
                result = (locationY - StartLocationIndexY) < size && (locationY - StartLocationIndexY) > -1 ? true : false;
            }
            else
            {
                if (locationY != StartLocationIndexY) { return false; }
                result = (locationX - StartLocationIndexX) < size && (locationX - StartLocationIndexX) > -1 ? true : false;
            }

            return result;
        }


    }
    /*
     * #                        size
     * 1    aircraft carrier    5
     * 1    battleship          4
     * 1    cruiser             3
     * 2    destroyer           2
     * 2    submarine           1
    */

    public class Carrier : Ship
    {
        public override string getShipType()
        {
            return "Carrier";
        }

        public Carrier()
        {
            Size = 5;
            HitLocations = new bool[] { false, false, false, false, false };
        }

    }

    public class Battleship : Ship
    {
        public override string getShipType()
        {
            return "Battleship";
        }

        public Battleship()
        {
            Size = 4;
            HitLocations = new bool[] { false, false, false, false };
        }

    }

    public class Cruiser : Ship
    {
        public override string getShipType()
        {
            return "Cruiser";
        }

        public Cruiser()
        {
            Size = 3;
            HitLocations = new bool[] { false, false, false };
        }

    }

    public class Destroyer : Ship
    {
        public override string getShipType()
        {
            return "Destroyer";
        }

        public Destroyer()
        {
            Size = 2;
            HitLocations = new bool[] { false, false };
        }

    }

    public class Submarine : Ship
    {
        public override string getShipType()
        {
            return "Submarine";
        }

        public Submarine()
        {
            Size = 1;
            HitLocations = new bool[] { false };
        }

    }

}