﻿using System;
using Box2D.XNA;

namespace StasisGame.Components
{
    public class GroundBodyComponent : IComponent
    {
        private Body _body;

        public ComponentType componentType { get { return ComponentType.GroundBody; } }
        public Body body { get { return _body; } }

        public GroundBodyComponent(Body body)
        {
            _body = body;
        }
    }
}
