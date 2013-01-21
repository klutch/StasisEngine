﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore.Resources;

namespace StasisCore.Models
{
    public class MaterialRadialScatterLayer : MaterialScatterLayer
    {
        protected float _a;
        protected float _b;
        protected float _intersections;
        protected float _maxRadius;
        protected int _arms;
        protected bool _twinArms;
        protected bool _flipArms;
        protected float _jitter;
        protected float _centerJitter;
        protected Vector2 _centerOffset;
        protected Color _baseColor;
        protected int _randomRed;
        protected int _randomGreen;
        protected int _randomBlue;
        protected int _randomAlpha;

        public float a { get { return _a; } set { _a = value; } }
        public float b { get { return _b; } set { _b = value; } }
        public float intersections { get { return _intersections; } set { _intersections = value; } }
        public float maxRadius { get { return _maxRadius; } set { _maxRadius = value; } }
        public int arms { get { return _arms; } set { _arms = value; } }
        public bool twinArms { get { return _twinArms; } set { _twinArms = value; } }
        public bool flipArms { get { return _flipArms; } set { _flipArms = value; } }
        public float jitter { get { return _jitter; } set { _jitter = value; } }
        public float centerJitter { get { return _centerJitter; } set { _centerJitter = value; } }
        public Vector2 centerOffset { get { return _centerOffset; } set { _centerOffset = value; } }
        virtual public Color baseColor { get { return _baseColor; } set { _baseColor = value; } }
        public int randomRed { get { return _randomRed; } set { _randomRed = value; } }
        public int randomGreen { get { return _randomGreen; } set { _randomGreen = value; } }
        public int randomBlue { get { return _randomBlue; } set { _randomBlue = value; } }
        public int randomAlpha { get { return _randomAlpha; } set { _randomAlpha = value; } }
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("a", _a);
                d.SetAttributeValue("b", _b);
                d.SetAttributeValue("intersections", _intersections);
                d.SetAttributeValue("max_radius", _maxRadius);
                d.SetAttributeValue("arms", _arms);
                d.SetAttributeValue("twin_arms", _twinArms);
                d.SetAttributeValue("flip_arms", _flipArms);
                d.SetAttributeValue("jitter", _jitter);
                d.SetAttributeValue("center_jitter", _centerJitter);
                d.SetAttributeValue("center_offset", _centerOffset);
                d.SetAttributeValue("base_color", _baseColor);
                d.SetAttributeValue("random_red", _randomRed);
                d.SetAttributeValue("random_green", _randomGreen);
                d.SetAttributeValue("random_blue", _randomBlue);
                d.SetAttributeValue("random_alpha", _randomAlpha);
                return d;
            }
        }

        public MaterialRadialScatterLayer()
            : base("radial_scatter")
        {
            _a = 1f;
            _b = 1f;
            _intersections = 32f;
            _maxRadius = 64f;
            _arms = 9;
            _twinArms = false;
            _flipArms = false;
            _jitter = 0f;
            _centerJitter = 0f;
            _centerOffset = Vector2.Zero;
            _baseColor = Color.White;
        }

        public MaterialRadialScatterLayer(XElement data)
            : base(data)
        {
            _a = float.Parse(data.Attribute("a").Value);
            _b = float.Parse(data.Attribute("b").Value);
            _intersections = float.Parse(data.Attribute("intersections").Value);
            _maxRadius = float.Parse(data.Attribute("max_radius").Value);
            _arms = int.Parse(data.Attribute("arms").Value);
            _twinArms = bool.Parse(data.Attribute("twin_arms").Value);
            _flipArms = bool.Parse(data.Attribute("flip_arms").Value);
            _jitter = float.Parse(data.Attribute("jitter").Value);
            _centerJitter = float.Parse(data.Attribute("center_jitter").Value);
            _centerOffset = XmlLoadHelper.getVector2(data.Attribute("center_offset").Value);
            _baseColor = XmlLoadHelper.getColor(data.Attribute("base_color").Value);
            _randomRed = int.Parse(data.Attribute("random_red").Value);
            _randomGreen = int.Parse(data.Attribute("random_green").Value);
            _randomBlue = int.Parse(data.Attribute("random_blue").Value);
            _randomAlpha = int.Parse(data.Attribute("random_alpha").Value);
        }
    }
}