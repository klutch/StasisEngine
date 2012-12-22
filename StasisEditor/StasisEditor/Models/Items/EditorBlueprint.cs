﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.ComponentModel;
using StasisCore.Resources;

namespace StasisEditor.Models
{
    public class EditorBlueprint
    {
        private BlueprintItemResource _blueprintResource;

        [Browsable(false)]
        public BlueprintItemResource blueprintResource { get { return _blueprintResource; } }

        [CategoryAttribute("Blueprint Properties")]
        [DisplayName("Item Tag")]
        public string itemTag { get { return _blueprintResource.itemTag; } set { _blueprintResource.itemTag = value; } }

        public EditorBlueprint(ItemResource resource)
            : base()
        {
            _blueprintResource = resource as BlueprintItemResource;
        }

        // toXML
        public XElement toXML()
        {
            return _blueprintResource.toXML();
        }
    }
}