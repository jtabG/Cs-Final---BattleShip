using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Cs_Final___BattleShip.Screens
{
    public class Button
    {
        private Vector2 m_Position;
        public Vector2 Position { get { return m_Position; } set { m_Position = value; } }


        private Vector2 m_UVCoord;
        public Vector2 UVCoord { get { return m_UVCoord; } set { m_UVCoord = value; } }


        private Vector2 m_FrameSize;
        public Vector2 FrameSize { get { return m_FrameSize; } set { m_FrameSize = value; } }


        private Vector2 m_DrawOffset;
        public Vector2 DrawOffset { get { return m_DrawOffset; } set { m_DrawOffset = value; } }


        private bool m_IsClicked;
        public bool IsClicked { get { bool clicked = m_IsClicked; m_IsClicked = false; return clicked; } set { m_IsClicked = value; } }


        private Texture2D m_Texture;
        public Texture2D Texture { get { return m_Texture; } set { m_Texture = value; } }

        enum ButtonStateEnum
        {
            NORMAL, MOUSEOVER, SELECTED
        }

        private MouseState currentMouseState;

        //Pointer to a function
        public delegate void buttonFunction();
        protected event buttonFunction m_ClickEvent;

        //Member functions
        public virtual void update()
        {
            // store the previous mouse state and get the mouse state relevant for this frame
            MouseState lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            Rectangle buttonRectangle = new Rectangle((int)m_Position.X, (int)m_Position.Y,
                                                      (int)m_FrameSize.X, (int)m_FrameSize.Y);

            if (buttonRectangle.Contains(new Point(currentMouseState.X, currentMouseState.Y)))
            {
                m_UVCoord.X = m_FrameSize.X * (int)ButtonStateEnum.MOUSEOVER;

                if (currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    m_UVCoord.X = m_FrameSize.X * (int)ButtonStateEnum.SELECTED;
                }
                else if (currentMouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
                {
                    if (m_ClickEvent != null)
                    {
                        m_ClickEvent();
                    }
                }
            }
            else { m_UVCoord.X = m_FrameSize.X * (int)ButtonStateEnum.NORMAL; }


        }

        public virtual void draw()
        {
            //see http://msdn.microsoft.com/en-us/library/ff433992.aspx for more information
            Rectangle destinationRectangle = new Rectangle( (int)m_Position.X, (int)m_Position.Y,
                                                            (int)m_FrameSize.X, (int)m_FrameSize.Y ); //target

            Rectangle sourceRectangle = new Rectangle(  (int)m_UVCoord.X, (int)m_UVCoord.Y,
                                                        (int)m_FrameSize.X, (int)m_FrameSize.Y ); //origin

            ScreenManager.SpriteBatch.Draw( m_Texture,
                                            destinationRectangle,
                                            sourceRectangle,
                                            Microsoft.Xna.Framework.Color.White,
                                            0,
                                            m_DrawOffset,
                                            0,
                                            0.0f );
        }

        // constructor without a behavior
        public Button(Vector2 position, Vector2 frameSize, Vector2 drawOffset, Texture2D texture)
        {
            m_Position = position;
            m_UVCoord = new Vector2(0, 0);
            m_FrameSize = frameSize;
            m_DrawOffset = drawOffset;
            m_IsClicked = false;
            m_Texture = texture;
            m_ClickEvent = null;

        }

        // constructor passed a function dictating it's click behavior
        public Button(Vector2 position, Vector2 frameSize, Vector2 drawOffset, Texture2D texture, buttonFunction clickEvent)
        {
            m_Position = position;
            m_UVCoord = new Vector2(0, 0);
            m_FrameSize = frameSize;
            m_DrawOffset = drawOffset;
            m_IsClicked = false;
            m_Texture = texture;
            m_ClickEvent = clickEvent;
        }

    }
}
