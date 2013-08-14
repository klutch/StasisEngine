﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisGame.Components
{
    public class TooltipComponent : IComponent
    {
        private string _message;
        private Vector2 _position;
        private float _radius;
        private float _radiusSq;
        private bool _draw;

        public ComponentType componentType { get { return ComponentType.Tooltip; } }
        public string message { get { return _message; } set { _message = value; } }
        public Vector2 position { get { return _position; } set { _position = value; } }
        public float radius { get { return _radius; } set { _radius = value; } }
        public float radiusSq { get { return _radiusSq; } set { _radiusSq = value; } }
        public bool draw { get { return _draw; } set { _draw = value; } }

        public TooltipComponent(string message, Vector2 position, float radius)
        {
            _message = message;
            _position = position;
            _radius = radius;
            _radiusSq = radius * radius;
        }
    }
}
