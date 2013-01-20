﻿using System;
using System.Xml.Linq;

namespace StasisCore.Models
{
    abstract public class MaterialLayer
    {
        protected bool _enabled;
        protected string _type;

        public bool enabled { get { return _enabled; } set { _enabled = value; } }
        public string type { get { return _type; } }

        virtual public XElement data
        {
            get
            {
                return new XElement("Layer",
                    new XAttribute("type", _type),
                    new XAttribute("enabled", enabled));
            }
        }

        // Create new
        public MaterialLayer(string type, bool enabled)
        {
            _type = type;
            _enabled = enabled;
        }

        // Create from xml
        public MaterialLayer(XElement data)
        {
            _type = data.Attribute("type").Value;
            _enabled = bool.Parse(data.Attribute("enabled").Value);
        }

        public static MaterialLayer load(XElement data)
        {
            MaterialLayer layer = null;
            switch (data.Attribute("type").Value)
            {
                case "root":
                    layer = new MaterialGroupLayer(data);
                    break;

                case "group":
                    layer = new MaterialGroupLayer(data);
                    break;

                case "texture":
                    layer = new MaterialTextureLayer(data);
                    break;

                case "noise":
                    layer = new MaterialNoiseLayer(data);
                    break;

                case "uniform_scatter":
                    layer = new MaterialUniformScatterLayer(data);
                    break;
            }
            return layer;
        }

        public static MaterialLayer create(string type)
        {
            MaterialLayer layer = null;
            switch (type)
            {
                case "group":
                    layer = new MaterialGroupLayer();
                    break;

                case "texture":
                    layer = new MaterialTextureLayer();
                    break;

                case "noise":
                    layer = new MaterialNoiseLayer();
                    break;

                case "uniform_scatter":
                    layer = new MaterialUniformScatterLayer();
                    break;
            }
            return layer;
        }
    }
}
