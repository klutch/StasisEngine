﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace StasisGame.UI
{
    abstract public class Screen
    {
        protected List<IUIComponent> _UIComponents;
        protected int _selectedIndex = -1;
        protected GamePadState _newGamepadState;
        protected GamePadState _oldGamepadState;
        protected KeyboardState _newKeyState;
        protected KeyboardState _oldKeyState;
        protected MouseState _newMouseState;
        protected MouseState _oldMouseState;

        public Screen()
        {
            _UIComponents = new List<IUIComponent>();
        }

        public void addComponent(IUIComponent component)
        {
            _UIComponents.Add(component);
        }

        public void removeComponent(IUIComponent component)
        {
            _UIComponents.Remove(component);
        }

        public void select(IUIComponent component)
        {
            if (component.selectable && _UIComponents.Contains(component))
            {
                if (_selectedIndex != -1)
                    _UIComponents[_selectedIndex].onDeselect();

                _selectedIndex = _UIComponents.IndexOf(component);
                component.onSelect();
            }
        }

        public void selectNextComponent()
        {
            bool selectableComponentExists = false;

            for (int i = 0; i < _UIComponents.Count; i++)
            {
                if (_UIComponents[i].selectable)
                {
                    selectableComponentExists = true;
                    break;
                }
            }

            if (selectableComponentExists)
            {
                bool foundNextSelectableComponent = false;
                int index = _selectedIndex;

                while (!foundNextSelectableComponent)
                {
                    index++;

                    if (index >= _UIComponents.Count)
                        index = 0;

                    if (_UIComponents[index].selectable)
                    {
                        foundNextSelectableComponent = true;
                        _UIComponents[_selectedIndex].onDeselect();
                        _selectedIndex = index;
                        _UIComponents[index].onSelect();
                    }
                }
            }
        }

        public void selectPreviousComponent()
        {
            bool selectableComponentExists = false;

            for (int i = 0; i < _UIComponents.Count; i++)
            {
                if (_UIComponents[i].selectable)
                {
                    selectableComponentExists = true;
                    break;
                }
            }

            if (selectableComponentExists)
            {
                bool foundNextSelectableComponent = false;
                int index = _selectedIndex;

                while (!foundNextSelectableComponent)
                {
                    index--;

                    if (index < 0)
                        index = _UIComponents.Count - 1;

                    if (_UIComponents[index].selectable)
                    {
                        foundNextSelectableComponent = true;
                        _UIComponents[_selectedIndex].onDeselect();
                        _selectedIndex = index;
                        _UIComponents[index].onSelect();
                    }
                }
            }
        }

        virtual public void update()
        {
            for (int i = 0; i < _UIComponents.Count; i++)
            {
                _UIComponents[i].UIUpdate();
            }
        }

        virtual public void draw()
        {
            for (int i = 0; i < _UIComponents.Count; i++)
            {
                _UIComponents[i].UIDraw();
            }
        }

    }
}
