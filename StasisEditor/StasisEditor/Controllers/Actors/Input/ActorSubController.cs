﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisEditor.Controllers.Actors
{
    abstract public class ActorSubController
    {
        private bool _selected;
        public bool selected { get { return _selected; } set { _selected = value; } }

        public ActorSubController()
        {
        }

        // hitTest
        abstract public bool hitTest(Vector2 worldMouse);

        // handleMouseMove
        virtual public void handleMouseMove(Vector2 worldDelta)
        {
        }

        // handleLeftMouseDown
        virtual public void handleLeftMouseDown()
        {
        }

        // checkXNAKeys
        virtual public void checkXNAKeys()
        {
        }
    }
}
