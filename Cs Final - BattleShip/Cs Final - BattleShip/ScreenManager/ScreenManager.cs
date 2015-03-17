using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;         

namespace Cs_Final___BattleShip.Screens
{
    public class ScreenManager
    {
        static private ScreenManager m_ScreenManager;


        static private GraphicsDevice m_GraphicsDevice;
        public static GraphicsDevice GraphicsDevice { get { return ScreenManager.m_GraphicsDevice; } set { ScreenManager.m_GraphicsDevice = value; } }


        static private GraphicsDeviceManager m_GraphicsDeviceManager;
        public static GraphicsDeviceManager GraphicsDeviceManager { get { return ScreenManager.m_GraphicsDeviceManager; } set { ScreenManager.m_GraphicsDeviceManager = value; } }


        static private SpriteBatch m_SpriteBatch;
        public static SpriteBatch SpriteBatch { get { return ScreenManager.m_SpriteBatch; } set { ScreenManager.m_SpriteBatch = value; } }


        static private GameFramework m_GameFramework;
        public static GameFramework GameFramework { get { return ScreenManager.m_GameFramework; } set { ScreenManager.m_GameFramework = value; } }


        private List<Screen> m_Screens;
        public List<Screen> Screens { get { return m_Screens; } set { m_Screens = value; } }


        private Screen m_CurrentScreen;
        public Screen CurrentScreen { get { return m_CurrentScreen; } set { m_CurrentScreen = value; } }


        static public ScreenManager getInstance()
        {
            if (m_ScreenManager == null)
            {
                m_ScreenManager = new ScreenManager();

            }

            return m_ScreenManager;

        }

        public enum ScreenEnum
        {
            BattleShipBoard, BattleshipGame, GameOver
        }

        private ScreenManager()
        {
            m_Screens = new List<Screen>();
            m_Screens.Add(new BattleShipBoard());
            m_Screens.Add(new BattleShipGame());
            m_Screens.Add(new GameOverScreen());

            m_CurrentScreen = m_Screens[(int)ScreenEnum.BattleShipBoard];
        }

        public void draw()
        {
            m_CurrentScreen.draw();
        }

        public void update()
        {
            m_CurrentScreen.update();
        }

        public void switchScreen(ScreenEnum screen)
        {
            Screen tempScreen = null;
            switch (screen)
            {
                case ScreenEnum.BattleShipBoard:
                    tempScreen = m_Screens[(int)ScreenEnum.BattleShipBoard];
                    break;

                case ScreenEnum.BattleshipGame:
                    tempScreen = m_Screens[(int)ScreenEnum.BattleshipGame];
                    break;

                case ScreenEnum.GameOver:
                    tempScreen = m_Screens[(int)ScreenEnum.GameOver];
                    break;

                default:
                    tempScreen = null;
                    break;
            }

            if (tempScreen == null) { return; }

            GameFramework.passGraphicsManager().PreferredBackBufferHeight = tempScreen.ScreenHeight;
            GameFramework.passGraphicsManager().PreferredBackBufferWidth = tempScreen.ScreenWidth;
            GameFramework.passGraphicsManager().ApplyChanges();
            m_CurrentScreen = tempScreen;
        }

        public Screen getScreen(ScreenEnum screenEnum)
        {
            Screen tempScreen = null;
            switch (screenEnum)
            {
                case ScreenEnum.BattleShipBoard:
                    tempScreen = m_Screens[(int)ScreenEnum.BattleShipBoard];
                    break;

                case ScreenEnum.BattleshipGame:
                    tempScreen = m_Screens[(int)ScreenEnum.BattleshipGame];
                    break;

                case ScreenEnum.GameOver:
                    tempScreen = m_Screens[(int)ScreenEnum.GameOver];
                    break;

                default:
                    tempScreen = null;
                    break;
            }

            return tempScreen;
        }
    }
}
