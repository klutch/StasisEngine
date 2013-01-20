﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StasisEditor.Models
{
    public class CircuitConnectionProperties : ActorProperties
    {
        private string _message;

        public string message { get { return _message; } set { _message = value; } }
        [Browsable(false)]
        public XAttribute[] data
        {
            get
            {
                return new XAttribute[] { new XAttribute("message", _message) };
            }
        }

        // Create new
        public CircuitConnectionProperties(string message)
            : base()
        {
            _message = message;
        }

        // Load from xml
        public CircuitConnectionProperties(XElement data)
        {
            _message = data.Attribute("message").Value;
        }

        // Clone properties
        public override ActorProperties clone()
        {
            return new CircuitConnectionProperties(_message);
        }
    }
}