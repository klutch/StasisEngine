﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StasisGame.Components;
using StasisGame.Managers;
using StasisCore;
using StasisCore.Models;

namespace StasisGame.Systems
{
    public class EquipmentSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private bool _paused;
        private bool _singleStep;
        private RopeMaterial _defaultRopeMaterial;
        private Random _rng;

        public SystemType systemType { get { return SystemType.Equipment; } }
        public int defaultPriority { get { return 10; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }

        public EquipmentSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
            _defaultRopeMaterial = new RopeMaterial(ResourceManager.getResource("default_rope_material"));
            _rng = new Random();
        }

        // assignItemToToolbar
        public void assignItemToToolbar(string levelUid, ItemComponent itemComponent, ToolbarComponent toolbarComponent, int toolbarSlot)
        {
            toolbarComponent.inventory[toolbarSlot] = itemComponent;
            selectToolbarSlot(levelUid, toolbarComponent, toolbarComponent.selectedIndex);
        }

        // selectToolbarSlot
        public void selectToolbarSlot(string levelUid, ToolbarComponent toolbarComponent, int slot)
        {
            ItemComponent itemComponent = toolbarComponent.selectedItem;
            if (itemComponent != null && itemComponent.definition.hasAimingComponent)
            {
                _entityManager.removeComponent(levelUid, toolbarComponent.entityId, ComponentType.Aim);
            }

            toolbarComponent.selectedIndex = slot;

            itemComponent = toolbarComponent.selectedItem;
            if (itemComponent != null && itemComponent.definition.hasAimingComponent)
            {
                Console.WriteLine("Adding aim component");
                _entityManager.addComponent(levelUid, toolbarComponent.entityId, new AimComponent(new Vector2(10f, 0f), 0f, 10f));
            }
        }

        // getInventorySlot
        public int getInventorySlot(InventoryComponent inventoryComponent, ItemComponent itemComponent)
        {
            foreach (KeyValuePair<int, ItemComponent> slotItemPair in inventoryComponent.inventory)
            {
                if (slotItemPair.Value == itemComponent)
                {
                    return slotItemPair.Key;
                }
            }
            return -1;
        }

        // getInventoryItem
        public ItemComponent getInventoryItem(InventoryComponent inventoryComponent, int i)
        {
            ItemComponent itemComponent;

            inventoryComponent.inventory.TryGetValue(i, out itemComponent);

            return itemComponent;
        }

        // addInventoryItem
        public void addInventoryItem(InventoryComponent inventoryComponent, ItemComponent item)
        {
            for (int i = 0; i < inventoryComponent.slots; i++)
            {
                if (!inventoryComponent.inventory.ContainsKey(i))
                {
                    inventoryComponent.inventory.Add(i, item);
                    return;
                }
            }
        }

        // removeInventoryItem
        public void removeInventoryItem(InventoryComponent inventoryComponent, ItemComponent item)
        {
            int index = -1;
            foreach (KeyValuePair<int, ItemComponent> pair in inventoryComponent.inventory)
            {
                if (pair.Value == item)
                {
                    index = pair.Key;
                    break;
                }
            }

            if (index != -1)
            {
                inventoryComponent.inventory.Remove(index);
            }
        }

        // update
        public void update(GameTime gameTime)
        {
            if (_singleStep || !_paused)
            {
                string levelUid = LevelSystem.currentLevelUid;
                LevelSystem levelSystem = _systemManager.getSystem(SystemType.Level) as LevelSystem;

                if (levelSystem.finalized)
                {
                    PlayerSystem playerSystem = _systemManager.getSystem(SystemType.Player) as PlayerSystem;
                    RopeSystem ropeSystem = _systemManager.getSystem(SystemType.Rope) as RopeSystem;
                    PhysicsComponent playerPhysicsComponent = _entityManager.getComponent(levelUid, PlayerSystem.PLAYER_ID, ComponentType.Physics) as PhysicsComponent;
                    List<int> toolbarEntities = _entityManager.getEntitiesPosessing(levelUid, ComponentType.Toolbar);

                    // Player equipment
                    if (playerSystem != null)
                    {
                        ToolbarComponent playerToolbar = _entityManager.getComponent(levelUid, PlayerSystem.PLAYER_ID, ComponentType.Toolbar) as ToolbarComponent;
                        WorldPositionComponent playerPositionComponent = _entityManager.getComponent(levelUid, PlayerSystem.PLAYER_ID, ComponentType.WorldPosition) as WorldPositionComponent;
                        ItemComponent selectedItem = playerToolbar.selectedItem;

                        if (selectedItem != null)
                        {
                            selectedItem.primaryContinuousAction = InputSystem.newMouseState.LeftButton == ButtonState.Pressed;
                            selectedItem.primarySingleAction = selectedItem.primaryContinuousAction && InputSystem.oldMouseState.LeftButton == ButtonState.Released;
                            selectedItem.secondaryContinuousAction = InputSystem.newMouseState.RightButton == ButtonState.Pressed;
                            selectedItem.secondarySingleAction = selectedItem.secondaryContinuousAction && InputSystem.oldMouseState.RightButton == ButtonState.Released;
                            //bool leftTriggerDown = InputSystem.usingGamepad && InputSystem.newGamepadState.Triggers.Left > 0.5f && InputSystem.oldGamepadState.Triggers.Left <= 0.5f;
                            //bool rightTriggerDown = InputSystem.usingGamepad && InputSystem.newGamepadState.Triggers.Right > 0.5f && InputSystem.oldGamepadState.Triggers.Right <= 0.5f;
                            AimComponent aimComponent = _entityManager.getComponent(levelUid, PlayerSystem.PLAYER_ID, ComponentType.Aim) as AimComponent;

                            if (selectedItem.definition.hasAimingComponent && aimComponent != null)
                            {
                                WorldPositionComponent worldPositionComponent = _entityManager.getComponent(levelUid, PlayerSystem.PLAYER_ID, ComponentType.WorldPosition) as WorldPositionComponent;

                                if (worldPositionComponent != null)
                                {
                                    Vector2 worldPosition = worldPositionComponent.position;
                                    if (InputSystem.usingGamepad)
                                    {
                                        Vector2 vector = InputSystem.newGamepadState.ThumbSticks.Left * selectedItem.state.currentRangeLimit;
                                        vector.Y *= -1;
                                        aimComponent.angle = (float)Math.Atan2(vector.Y, vector.X);
                                        aimComponent.length = vector.Length();
                                        aimComponent.vector = vector;
                                    }
                                    else
                                    {
                                        Vector2 relative = (InputSystem.worldMouse - worldPosition);
                                        aimComponent.angle = (float)Math.Atan2(relative.Y, relative.X);
                                        aimComponent.length = Math.Min(relative.Length(), selectedItem.state.currentRangeLimit);
                                        aimComponent.vector = relative;
                                    }
                                }
                            }
                        }
                    }

                    // All toolbars
                    for (int i = 0; i < toolbarEntities.Count; i++)
                    {
                        ToolbarComponent toolbarComponent = _entityManager.getComponent(levelUid, toolbarEntities[i], ComponentType.Toolbar) as ToolbarComponent;
                        ItemComponent selectedItem = toolbarComponent.selectedItem;

                        if (selectedItem != null)
                        {
                            if (selectedItem.secondarySingleAction)
                                Console.WriteLine("secondary action");

                            switch (selectedItem.definition.uid)
                            {
                                // RopeGun
                                case "ropegun":
                                    if (selectedItem.primarySingleAction)
                                    {
                                        AimComponent aimComponent = _entityManager.getComponent(levelUid, toolbarEntities[i], ComponentType.Aim) as AimComponent;
                                        Vector2 initialPointA = (_entityManager.getComponent(levelUid, toolbarEntities[i], ComponentType.WorldPosition) as WorldPositionComponent).position;
                                        Vector2 initialPointB = initialPointA + new Vector2((float)Math.Cos(aimComponent.angle), (float)Math.Sin(aimComponent.angle)) * aimComponent.length;
                                        int ropeEntityId = _entityManager.factory.createSingleAnchorRope(levelUid, initialPointA, initialPointB, _defaultRopeMaterial, true);

                                        if (ropeEntityId != -1)
                                        {
                                            RopeGrabComponent ropeGrabComponent = _entityManager.getComponent(levelUid, toolbarComponent.entityId, ComponentType.RopeGrab) as RopeGrabComponent;
                                            RopeComponent ropeComponent = _entityManager.getComponent(levelUid, ropeEntityId, ComponentType.Rope) as RopeComponent;
                                            PhysicsComponent physicsComponent = _entityManager.getComponent(levelUid, toolbarEntities[i], ComponentType.Physics) as PhysicsComponent;
                                            RopeGrabComponent newRopeGrabComponent = null;
                                            Vector2 initialVelocity = physicsComponent.body.LinearVelocity;
                                            RopeNode currentNode = null;
                                            int ropeSegmentCount;

                                            if (physicsComponent == null)
                                                break;

                                            // Handle initial velocity
                                            currentNode = ropeComponent.ropeNodeHead;
                                            ropeSegmentCount = currentNode.count;
                                            System.Diagnostics.Debug.Assert(ropeSegmentCount != 0);
                                            int count = ropeSegmentCount;
                                            while (currentNode != null)
                                            {
                                                float weight = (float)count / (float)ropeSegmentCount;

                                                currentNode.body.LinearVelocity = currentNode.body.LinearVelocity + initialVelocity * weight;

                                                count--;
                                                currentNode = currentNode.next;
                                            }

                                            // Handle previous grabs
                                            if (ropeGrabComponent != null)
                                            {
                                                RopeComponent previouslyGrabbedRope = _entityManager.getComponent(levelUid, ropeGrabComponent.ropeEntityId, ComponentType.Rope) as RopeComponent;
                                                ropeSystem.releaseRope(ropeGrabComponent, physicsComponent.body);

                                                if (previouslyGrabbedRope.destroyAfterRelease)
                                                    previouslyGrabbedRope.timeToLive = 100;
                                                _entityManager.removeComponent(levelUid, toolbarComponent.entityId, ropeGrabComponent);
                                                ropeGrabComponent = null;
                                            }

                                            newRopeGrabComponent = new RopeGrabComponent(ropeEntityId, ropeComponent.ropeNodeHead, 0f, ropeComponent.reverseClimbDirection);
                                            ropeSystem.grabRope(newRopeGrabComponent, physicsComponent.body);
                                            _entityManager.addComponent(levelUid, toolbarComponent.entityId, newRopeGrabComponent);
                                        }
                                    }
                                    break;

                                // Dynamite
                                case "dynamite":
                                    if (selectedItem.primarySingleAction)
                                    {
                                        AimComponent aimComponent = _entityManager.getComponent(levelUid, toolbarEntities[i], ComponentType.Aim) as AimComponent;

                                        _entityManager.factory.createDynamite(levelUid, playerPhysicsComponent.body.Position, aimComponent.vector * 80f);
                                    }
                                    break;

                                // Water gun
                                case "watergun":
                                    if (selectedItem.primaryContinuousAction)
                                    {
                                        FluidSystem fluidSystem = _systemManager.getSystem(SystemType.Fluid) as FluidSystem;
                                        AimComponent aimComponent = _entityManager.getComponent(levelUid, toolbarEntities[i], ComponentType.Aim) as AimComponent;
                                        Vector2 aimUnitVector = Vector2.Normalize(aimComponent.vector);
                                        Vector2 particlePosition =
                                            playerPhysicsComponent.body.Position +
                                            aimUnitVector +
                                            new Vector2(StasisMathHelper.floatBetween(-0.1f, 0.1f, _rng), StasisMathHelper.floatBetween(-0.1f, 0.1f, _rng));
                                        Vector2 particleVelocity = aimUnitVector * 0.4f;

                                        fluidSystem.createParticle(particlePosition, particleVelocity);
                                    }
                                    break;
                            }

                            selectedItem.primarySingleAction = false;
                            selectedItem.secondarySingleAction = false;
                            selectedItem.primaryContinuousAction = false;
                            selectedItem.secondaryContinuousAction = false;
                        }
                    }
                }
            }
            _singleStep = false;
        }
    }
}
