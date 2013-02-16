﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Box2D.XNA;
using StasisCore;

namespace StasisGame.Components
{
    public class CharacterMovementComponent : IComponent
    {
        private float _movementAngle;
        private List<Vector2> _collisionNormals;
        private Vector2 _movementNormal;
        private bool _walkLeft;
        private bool _walkRight;
        private Fixture _feetFixture;
        private bool _jump;
        private bool _alreadyJumped;
        private bool _allowJumpResetOnCollision;
        private bool _allowLeftMovement;
        private bool _allowRightMovement;

        public ComponentType componentType { get { return ComponentType.CharacterMovement; } }
        public float movementAngle { get { return _movementAngle; } }
        public List<Vector2> collisionNormals { get { return _collisionNormals; } }
        public Vector2 movementNormal { get { return _movementNormal; } }
        public bool walkLeft { get { return _walkLeft; } set { _walkLeft = value; } }
        public bool walkRight { get { return _walkRight; } set { _walkRight = value; } }
        public bool onSurface { get { return _collisionNormals.Count > 0; } }
        public Fixture feetFixture { get { return _feetFixture; } }
        public bool jump { get { return _jump; } set { _jump = value; } }
        public bool alreadyJumped { get { return _alreadyJumped; } set { _alreadyJumped = value; } }
        public bool allowJumpResetOnCollision { get { return _allowJumpResetOnCollision; } set { _allowJumpResetOnCollision = value; } }
        public bool allowLeftMovement { get { return _allowLeftMovement; } }
        public bool allowRightMovement { get { return _allowRightMovement; } }

        public CharacterMovementComponent(Fixture feetFixture)
        {
            _collisionNormals = new List<Vector2>();
            _feetFixture = feetFixture;
        }

        public void calculateMovementAngle()
        {
            if (_collisionNormals.Count > 0)
            {
                _movementNormal = Vector2.Zero;
                foreach (Vector2 normal in _collisionNormals)
                    _movementNormal += normal;
                _movementNormal = new Vector2(-_movementNormal.Y, _movementNormal.X) / _collisionNormals.Count; // convert to perpendicular, and take the average
                _movementAngle = (float)Math.Atan2(_movementNormal.Y, _movementNormal.X);
                _allowLeftMovement = _movementNormal.X > 0.3f;
                _allowRightMovement = _movementNormal.X > 0.3f;
            }
            else
            {
                _allowLeftMovement = true;
                _allowRightMovement = true;
                _movementAngle = 0;
                _movementNormal = new Vector2(1, 0);
            }
        }
    }
}