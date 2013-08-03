﻿using System;
using System.Collections.Generic;
using StasisGame.Managers;
using StasisGame.UI;
using StasisCore;

namespace StasisGame.Systems
{
    public class ScreenSystem : ISystem
    {
        private SystemManager _systemManager;
        private List<Screen> _screens;
        private List<Transition> _transitions;
        private bool _paused;
        private bool _singleStep;

        public SystemType systemType { get { return SystemType.Screen; } }
        public int defaultPriority { get { return 90; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }
        public int count { get { return _screens.Count; } }

        public ScreenSystem(SystemManager systemManager)
        {
            _systemManager = systemManager;
            _screens = new List<Screen>();
            _transitions = new List<Transition>();
        }

        public void addScreen(Screen screen)
        {
            _screens.Add(screen);
            Logger.log(string.Format("Added {0} screen to ScreenSystem.", screen.screenType));
        }

        /*
        public void removeScreen(ScreenType screenType)
        {
            Screen screenToRemove = null;

            for (int i = 0; i < _screens.Count; i++)
            {
                if (_screens[i].screenType == screenType)
                    screenToRemove = _screens[i];
            }

            if (screenToRemove != null)
            {
                removeScreen(screenToRemove);
            }
            else
            {
                Logger.log(string.Format("Could not remove {0} screen from ScreenSystem.", screenType));
            }
        }*/

        public void removeScreen(Screen screen)
        {
            _screens.Remove(screen);
            Logger.log(string.Format("Removed {0} screen from ScreenSystem.", screen.screenType));
        }

        public void addTransition(Transition transition)
        {
            _transitions.Add(transition);
        }

        public void update()
        {
            if (!_paused || _singleStep)
            {
                // Update screen
                for (int i = 0; i < _screens.Count; i++)
                {
                    _screens[i].update();
                }

                // Update transition (one at a time)
                if (_transitions.Count > 0)
                {
                    Transition transition = _transitions[0];

                    if (transition.finished)
                    {
                        transition.end();
                        _transitions.Remove(transition);
                    }
                    else if (transition.starting)
                    {
                        transition.begin();
                        transition.update();
                    }
                    else
                    {
                        transition.update();
                    }
                }
            }
            _singleStep = false;
        }

        public void draw()
        {
            // Draw screens
            for (int i = 0; i < _screens.Count; i++)
            {
                _screens[i].draw();
            }

            // Draw transition
            if (_transitions.Count > 0)
            {
                _transitions[0].draw();
            }
        }
    }
}
