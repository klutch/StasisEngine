﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Resources
{
    public class RopeGunItemResource : ItemResource
    {
        private bool _doubleAnchor;
        private float _range;

        public bool doubleAnchor { get { return _doubleAnchor; } set { _doubleAnchor = value; } }
        public float range { get { return _range; } set { _range = value; } }

        public RopeGunItemResource(string tag, int quantity, string worldTextureTag, string inventoryTextureTag, bool doubleAnchor, float range)
            : base(tag, quantity, worldTextureTag, inventoryTextureTag)
        {
            _doubleAnchor = doubleAnchor;
            _range = range;
            _type = ItemType.RopeGun;
        }

        // fromXML
        public static RopeGunItemResource fromXML(XElement element)
        {
            return new RopeGunItemResource(
                element.Attribute("tag").Value,
                int.Parse(element.Attribute("quantity").Value),
                element.Attribute("worldTextureTag").Value,
                element.Attribute("inventoryTextureTag").Value,
                bool.Parse(element.Attribute("doubleAnchor").Value),
                float.Parse(element.Attribute("range").Value));
        }

        // toXML
        public override XElement toXML()
        {
            return new XElement("Item",
                new XAttribute("type", _type),
                new XAttribute("tag", _tag),
                new XAttribute("quantity", _quantity),
                new XAttribute("worldTextureTag", _worldTextureTag),
                new XAttribute("inventoryTextureTag", _inventoryTextureTag),
                new XAttribute("doubleAnchor", _doubleAnchor),
                new XAttribute("range", _range));
        }

        // clone
        public override ItemResource clone()
        {
            return new RopeGunItemResource(_tag, _quantity, _worldTextureTag, _inventoryTextureTag, _doubleAnchor, _range);
        }
    }
}
