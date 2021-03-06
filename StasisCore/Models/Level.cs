﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    abstract public class Level
    {
        protected Vector2 _gravity;
        protected Vector2 _wind;
        protected string _name;
        protected string _backgroundUID;

        public Vector2 gravity { get { return _gravity; } set { _gravity = value; } }
        public Vector2 wind { get { return _wind; } set { _wind = value; } }
        virtual public string name { get { return _name; } set { _name = value; } }
        public string backgroundUID { get { return _backgroundUID; } set { _backgroundUID = value; } }

        // Create new
        public Level(string name)
        {
            _name = name;
            _gravity = new Vector2(0, 32f);
            _wind = new Vector2(0, 0);
            _backgroundUID = "default_background";
        }

        // Level
        public Level(XElement data)
        {
            _gravity = Loader.loadVector2(data.Attribute("gravity"), new Vector2(0, 32));
            _wind = Loader.loadVector2(data.Attribute("wind"), Vector2.Zero);
            _name = Loader.loadString(data.Attribute("name"), "new_level");
            _backgroundUID = Loader.loadString(data.Attribute("background_uid"), "default_background");
        }
    }
}
