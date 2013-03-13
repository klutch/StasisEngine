﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using StasisGame.Components;
using StasisGame.Managers;
using StasisCore;

namespace StasisGame.Systems
{
    public class PlayerSystem : ISystem
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private int _playerId;

        public int defaultPriority { get { return 30; } }
        public SystemType systemType { get { return SystemType.Player; } }
        public int playerId { get { return _playerId; } set { _playerId = value; } }

        public PlayerSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void update()
        {
            InputComponent inputComponent = _entityManager.getComponent(_playerId, ComponentType.Input) as InputComponent;
            CharacterMovementComponent characterMovementComponent = _entityManager.getComponent(_playerId, ComponentType.CharacterMovement) as CharacterMovementComponent;

            if (inputComponent.usingGamepad)
            {
                if (inputComponent.newGamepadState.ThumbSticks.Left.X < 0)
                    characterMovementComponent.walkLeft = true;
                else if (inputComponent.newGamepadState.DPad.Left == ButtonState.Pressed)
                    characterMovementComponent.walkLeft = true;
                else
                    characterMovementComponent.walkLeft = false;

                if (inputComponent.newGamepadState.ThumbSticks.Left.X > 0)
                    characterMovementComponent.walkRight = true;
                else if (inputComponent.newGamepadState.DPad.Right == ButtonState.Pressed)
                    characterMovementComponent.walkRight = true;
                else
                    characterMovementComponent.walkRight = false;

                characterMovementComponent.jump = inputComponent.newGamepadState.Buttons.A == ButtonState.Pressed;
            }
            else
            {
                characterMovementComponent.walkLeft = inputComponent.newKeyState.IsKeyDown(Keys.A) || inputComponent.newKeyState.IsKeyDown(Keys.Left);
                characterMovementComponent.walkRight = inputComponent.newKeyState.IsKeyDown(Keys.D) || inputComponent.newKeyState.IsKeyDown(Keys.Right);
                characterMovementComponent.jump = inputComponent.newKeyState.IsKeyDown(Keys.Space);
            }
        }
    }
}
