﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Box2D.XNA;
using StasisGame.Managers;
using StasisGame.Components;

namespace StasisGame.Systems
{
    public class CharacterMovementSystem : ISystem
    {
        public const float MAX_WALK_SPEED = 7;
        public const float WALK_FORCE = 12;
        public const float JUMP_FORCE = 10.5f;
        public const float CLIMB_SPEED = 0.05f;
        private SystemManager _systemManager;
        private EntityManager _entityManager;
        private bool _paused;
        private bool _singleStep;

        public int defaultPriority { get { return 40; } }
        public SystemType systemType { get { return SystemType.CharacterMovement; } }
        public bool paused { get { return _paused; } set { _paused = value; } }
        public bool singleStep { get { return _singleStep; } set { _singleStep = value; } }

        public CharacterMovementSystem(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        public void update()
        {
            List<int> characterEntities = _entityManager.getEntitiesPosessing(ComponentType.CharacterMovement);

            for (int i = 0; i < characterEntities.Count; i++)
            {
                PhysicsComponent physicsComponent = (PhysicsComponent)_entityManager.getComponent(characterEntities[i], ComponentType.Physics);
                CharacterMovementComponent characterMovementComponent = (CharacterMovementComponent)_entityManager.getComponent(characterEntities[i], ComponentType.CharacterMovement);
                RopeGrabComponent ropeGrabComponent = (RopeGrabComponent)_entityManager.getComponent(characterEntities[i], ComponentType.RopeGrab);
                Body body = physicsComponent.body;
                Vector2 averageNormal = Vector2.Zero;
                float modifier = characterMovementComponent.walkSpeedModifier;
                bool applyForce =
                    (characterMovementComponent.walkLeft && characterMovementComponent.allowLeftMovement) ||
                    (characterMovementComponent.walkRight && characterMovementComponent.allowRightMovement);
                Vector2 characterVelocity = physicsComponent.body.GetLinearVelocity();
                float characterSpeed = characterVelocity.Length();
                float characterHorizontalSpeed = Math.Abs(characterVelocity.X);

                characterMovementComponent.calculateMovementAngle();

                if (characterMovementComponent.onSurface)
                {
                    // Check speed limit
                    if (Math.Abs(body.GetLinearVelocity().Length()) > MAX_WALK_SPEED)
                        applyForce = false;

                    // Pull harder if grabbing
                    //if (grabbing)
                    //    modifier = 2;

                    // Apply movement force
                    if (applyForce)
                    {
                        Vector2 movement = new Vector2((float)Math.Cos(characterMovementComponent.movementAngle), (float)Math.Sin(characterMovementComponent.movementAngle));
                        movement *= characterMovementComponent.walkLeft ? -1 : 1;
                        movement *= WALK_FORCE * modifier;
                        body.ApplyForce(movement, body.GetPosition());
                    }

                    // Fake friction when not moving
                    if ((body.GetLinearVelocity().X > 0 && characterMovementComponent.walkLeft) ||
                        (body.GetLinearVelocity().X < 0 && characterMovementComponent.walkRight) ||
                        (!characterMovementComponent.walkLeft && !characterMovementComponent.walkRight) &&
                        !characterMovementComponent.jump)
                    {
                        // All conditions necessary for damping have been met
                        if (Math.Abs(body.GetLinearVelocity().Y) < 1 || Math.Abs(body.GetLinearVelocity().Length()) < 10)
                            body.ApplyForce(new Vector2(-body.GetLinearVelocity().X * 10, -body.GetLinearVelocity().Y * 5), body.GetPosition());
                    }
                }
                else
                {
                    if (characterMovementComponent.walkLeft || characterMovementComponent.walkRight)
                    {
                        if (ropeGrabComponent != null)
                        {
                            // Swing
                            float swingForce = (characterMovementComponent.walkLeft ? -WALK_FORCE : WALK_FORCE) / 2;
                            Vector2 movement = new Vector2((float)Math.Cos(characterMovementComponent.movementAngle), (float)Math.Sin(characterMovementComponent.movementAngle));
                            physicsComponent.body.ApplyForce(movement * swingForce, body.GetPosition());
                        }
                        else
                        {
                            // Air walk
                            float airWalkForce = (characterMovementComponent.walkLeft ? -WALK_FORCE : WALK_FORCE) / 4;

                            // Check speed limit
                            if (Math.Abs(body.GetLinearVelocity().X) > MAX_WALK_SPEED / 2)
                            {
                                if (body.GetLinearVelocity().X < -MAX_WALK_SPEED / 2 && airWalkForce < 0)
                                    applyForce = false;
                                else if (body.GetLinearVelocity().X > MAX_WALK_SPEED / 2 && airWalkForce > 0)
                                    applyForce = false;
                            }

                            // Apply movement force
                            if (applyForce)
                            {
                                Vector2 movement = new Vector2((float)Math.Cos(characterMovementComponent.movementAngle), (float)Math.Sin(characterMovementComponent.movementAngle));
                                movement *= airWalkForce;
                                body.ApplyForce(movement, body.GetPosition());
                            }
                        }
                    }
                }

                // Jump
                if (characterMovementComponent.jump)
                {
                    if (ropeGrabComponent != null)
                    {
                        ropeGrabComponent.detachBody(physicsComponent.body);
                        _entityManager.removeComponent(characterEntities[i], ropeGrabComponent);
                        ropeGrabComponent = null;
                    }

                    if (!characterMovementComponent.alreadyJumped && (characterMovementComponent.allowLeftMovement || characterMovementComponent.allowRightMovement))
                    {
                        if (characterMovementComponent.onSurface)
                        {
                            characterMovementComponent.alreadyJumped = true;
                            body.SetLinearVelocity(new Vector2(body.GetLinearVelocity().X, -JUMP_FORCE));
                        }
                    }
                }

                // Climbing
                if (ropeGrabComponent != null)
                {
                    float climbSpeed = characterMovementComponent.climbAmount * CLIMB_SPEED;
                    if (characterMovementComponent.climbUp)
                    {
                        ropeGrabComponent.moveAttachedBody(physicsComponent.body, climbSpeed);
                    }
                    else if (characterMovementComponent.climbDown)
                    {
                        ropeGrabComponent.moveAttachedBody(physicsComponent.body, -climbSpeed);
                    }
                }

                characterMovementComponent.collisionNormals.Clear();
            }
        }
    }
}
