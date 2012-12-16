﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisCore.Models
{
    public enum TerrainLayerType
    {
        Texture = 0,
        Noise,
        DecalSpatter,
        Group,
        Root
    };

    public enum TerrainBlendType
    {
        Opaque = 0,
        Overlay,
        Additive
    };

    abstract public class TerrainLayerResource
    {
        protected TerrainLayerType _type;
        protected LayerProperties _properties;
        protected bool _enabled;

        public TerrainLayerType type { get { return _type; } }
        public LayerProperties properties { get { return _properties; } set { _properties = value; } }
        public bool enabled { get { return _enabled; } set { _enabled = value; } }

        // create
        public static TerrainLayerResource create(TerrainLayerType layerType)
        {
            switch (layerType)
            {
                case TerrainLayerType.Texture:
                    return new TerrainTextureLayerResource();
                case TerrainLayerType.Noise:
                    return new TerrainNoiseLayerResource();
                case TerrainLayerType.DecalSpatter:
                    return new TerrainDecalSpatterLayerResource();
            }
            return null;
        }

        // toXML
        abstract public XElement toXML();

        // copyFrom -- clones a list
        public static List<TerrainLayerResource> copyFrom(List<TerrainLayerResource> list)
        {
            List<TerrainLayerResource> copy = new List<TerrainLayerResource>();
            foreach (TerrainLayerResource layer in list)
                copy.Add(layer.clone());
            return copy;
        }

        // Constructor
        public TerrainLayerResource(bool enabled)
        {
            _enabled = enabled;
        }

        // clone
        abstract public TerrainLayerResource clone();
    }
}
