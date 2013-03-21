﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisGame.Components;
using StasisGame.Managers;

namespace StasisGame.Systems
{
    public class EquipmentSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;

        public SystemType systemType { get { return SystemType.Equipment; } }
        public int defaultPriority { get { return 10; } }

        public EquipmentSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void assignItemToToolbar(ItemComponent itemComponent, ToolbarComponent toolbarComponent, int toolbarSlot)
        {
            toolbarComponent.inventory[toolbarSlot] = itemComponent;
        }

        public void selectToolbarSlot(ToolbarComponent toolbarComponent, int slot)
        {
            ItemComponent itemComponent = toolbarComponent.selectedItem;
            if (itemComponent != null && itemComponent.hasAiming)
            {
                _entityManager.removeComponent(toolbarComponent.entityId, ComponentType.Aim);
            }

            toolbarComponent.selectedIndex = slot;

            itemComponent = toolbarComponent.selectedItem;
            if (itemComponent != null && itemComponent.hasAiming)
            {
                Console.WriteLine("Adding aim component");
                _entityManager.addComponent(toolbarComponent.entityId, new AimComponent(0f, 10f));
            }
        }

        public void update()
        {
            PlayerSystem playerSystem = _systemManager.getSystem(SystemType.Player) as PlayerSystem;
            List<int> toolbarEntities = _entityManager.getEntitiesPosessing(ComponentType.Toolbar);

            // Player
            if (playerSystem != null)
            {
                int playerId = playerSystem.playerId;
                ToolbarComponent playerToolbar = _entityManager.getComponent(playerId, ComponentType.Toolbar) as ToolbarComponent;
                WorldPositionComponent playerPositionComponent = _entityManager.getComponent(playerId, ComponentType.WorldPosition) as WorldPositionComponent;
                ItemComponent selectedItem = playerToolbar.selectedItem;

                if (selectedItem != null)
                {
                    bool mouseLeftDown = InputSystem.newMouseState.LeftButton == ButtonState.Pressed && InputSystem.oldMouseState.LeftButton == ButtonState.Released;
                    bool mouseRightDown = InputSystem.newMouseState.RightButton == ButtonState.Pressed && InputSystem.oldMouseState.RightButton == ButtonState.Released;
                    bool leftTriggerDown = InputSystem.newGamepadState.IsConnected && InputSystem.newGamepadState.Triggers.Left > 0.5f && InputSystem.oldGamepadState.Triggers.Left <= 0.5f;
                    bool rightTriggerDown = InputSystem.newGamepadState.IsConnected && InputSystem.newGamepadState.Triggers.Right > 0.5f && InputSystem.oldGamepadState.Triggers.Right <= 0.5f;
                    AimComponent aimComponent = _entityManager.getComponent(playerId, ComponentType.Aim) as AimComponent;

                    if (mouseLeftDown || leftTriggerDown)
                    {
                        selectedItem.primaryAction = true;
                    }
                    if (mouseRightDown || rightTriggerDown)
                    {
                        selectedItem.secondaryAction = true;
                    }

                    if (selectedItem.hasAiming && aimComponent != null)
                    {
                        Vector2 worldPosition = (_entityManager.getComponent(playerId, ComponentType.WorldPosition) as WorldPositionComponent).position;

                        if (InputSystem.newGamepadState.IsConnected)
                        {
                            Vector2 aim = InputSystem.newGamepadState.ThumbSticks.Right * selectedItem.maxRange;
                            aim.Y *= -1;
                            aimComponent.angle = (float)Math.Atan2(aim.Y, aim.X);
                            aimComponent.length = aim.Length();
                        }
                        else
                        {
                            Vector2 relative = (InputSystem.worldMouse - worldPosition);
                            aimComponent.angle = (float)Math.Atan2(relative.Y, relative.X);
                            aimComponent.length = Math.Min(relative.Length(), selectedItem.maxRange);
                        }
                    }
                }
            }

            // All toolbars
            for (int i = 0; i < toolbarEntities.Count; i++)
            {
                ToolbarComponent toolbarComponent = _entityManager.getComponent(toolbarEntities[i], ComponentType.Toolbar) as ToolbarComponent;
                ItemComponent selectedItem = toolbarComponent.selectedItem;

                if (selectedItem != null)
                {
                    if (selectedItem != null && (selectedItem.primaryAction || selectedItem.secondaryAction))
                    {
                        if (selectedItem.primaryAction)
                            Console.WriteLine("primary action");
                        if (selectedItem.secondaryAction)
                            Console.WriteLine("secondary action");

                        switch (selectedItem.itemType)
                        {
                            case ItemType.RopeGun:
                                Console.WriteLine("Rope gun");
                                break;

                            case ItemType.Blueprint:
                                Console.WriteLine("Blueprint");
                                break;

                            case ItemType.BlueprintScrap:
                                Console.WriteLine("Blueprint scrap");
                                break;
                        }

                        selectedItem.primaryAction = false;
                        selectedItem.secondaryAction = false;
                    }
                }
            }
        }
    }
}