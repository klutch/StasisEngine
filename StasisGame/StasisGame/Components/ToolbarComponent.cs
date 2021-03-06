﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.Components
{
    public class ToolbarComponent : IComponent
    {
        private Dictionary<int, ItemComponent> _inventory;
        private int _slots;
        private int _selectedIndex;
        private int _entityId;

        public ComponentType componentType { get { return ComponentType.Toolbar; } }
        public Dictionary<int, ItemComponent> inventory { get { return _inventory; } }
        public int slots { get { return _slots; } }
        public int selectedIndex { get { return _selectedIndex; } set { _selectedIndex = value; } }
        public ItemComponent selectedItem { get { return _inventory[_selectedIndex]; } }
        public int entityId { get { return _entityId; } set { _entityId = value; } }

        public ToolbarComponent(int slots, int entityId)
        {
            _slots = slots;
            _entityId = entityId;
            _inventory = new Dictionary<int, ItemComponent>();

            for (int i = 0; i < _slots; i++)
            {
                _inventory.Add(i, null);
            }
        }
    }
}
