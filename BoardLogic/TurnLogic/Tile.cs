using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoardLogic
{
    public enum TileEnum
    {
        //State values
        UNREVEALED = 0,
        HIT        = 1,
        NOT_HIT    = 2

    }

    public class Tile
    {
        //Member variables
        protected int   m_X        ;
        protected int   m_Y        ;
        protected int   m_Width    ;
        protected int   m_Height   ;
        protected short m_State    ;
        protected bool  m_TargetHit;

        protected int m_IndexX;
        protected int m_IndexY;

        //Accessors
        public int   a_X         { get {return m_X         ;} set { m_X          = value ;} }
        public int   a_Y         { get {return m_Y         ;} set { m_Y          = value ;} }
        public int   a_Width     { get {return m_Width     ;} set { m_Width      = value ;} }
        public int   a_Height    { get {return m_Height    ;} set { m_Height     = value ;} }
        public short a_State     { get {return m_State     ;} set { m_State      = value ;} }
        public bool  a_TargetHit { get {return m_TargetHit ;} set { m_TargetHit  = value ;} }

        public int a_IndexX      { get {return m_IndexX    ;} set { m_IndexX     = value ;} }
        public int a_IndexY      { get { return m_IndexY   ;} set { m_IndexY     = value; } }

        //Member functions
        public void mouseLeftClickUpEvent(int positionX, int positionY)
        {
            if (
                    positionX >= m_X && positionX <= (m_X + m_Width) &&//range along X axis
                    positionY >= m_Y && positionY <= (m_Y + m_Height)   //range along Y axis
                )
            {
                if (m_State == 1)
                {
                    m_State = (short)TileEnum.HIT;
                }
                else
                {
                    m_State = (short)TileEnum.NOT_HIT;
                }

            }

        }

        //Constructors
        public Tile(int aX, int aY, int aWidth, int aHeight)
        {
            //Assigning argument values
            m_X       = aX       ;
            m_Y       = aY       ;
            m_Width   = aWidth   ;
            m_Height  = aHeight  ;

            //Assigning hard values
            m_State = (short)TileEnum.UNREVEALED;

        }

        public Tile(int aX, int aY, int aWidth, int aHeight, int indexX, int indexY)
        {
            //Assigning argument values
            m_X = aX;
            m_Y = aY;
            m_Width = aWidth;
            m_Height = aHeight;

            m_IndexX = indexX;
            m_IndexY = indexY;

            //Assigning hard values
            m_State = (short)TileEnum.UNREVEALED;

        }

    }

}
