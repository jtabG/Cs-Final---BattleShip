using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cs_Final___BattleShip.Screens
{
    abstract public class Screen
    {
        protected List<Button> m_Buttons;
        public List<Button> Buttons { get { return m_Buttons; } set { m_Buttons = value; } }

        private int m_ScreenWidth;
        public int ScreenWidth   { get { return m_ScreenWidth; } set { m_ScreenWidth = value; } }

        private int m_ScreenHeight;
        public int ScreenHeight  { get { return m_ScreenHeight; } set { m_ScreenHeight = value; } }

        public virtual void update()
        {
            for (int i = 0; i < m_Buttons.Count; i++)
            {
                m_Buttons[i].update();

                if (m_Buttons[i].IsClicked)
                {
                    return;
                }

            }

        }

        public virtual void draw()
        {
            for (int i = 0; i < m_Buttons.Count; i++)
            {
                m_Buttons[i].draw();
            }
        }

        public abstract void reset();

        public Screen()
        {
            m_Buttons = new List<Button>();
        }
        
    }

}