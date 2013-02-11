﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisGame.Systems;
using StasisGame.Managers;

namespace StasisGame
{
    public class Level
    {
        private SystemManager _systemManager;
        private EntityManager _entityManager;

        public Level(string filePath)
        {
            _systemManager = new SystemManager();
            _entityManager = new EntityManager();

            XElement data = null;
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                XDocument doc = XDocument.Load(stream);
                data = doc.Element("Level");
            }

            foreach (XElement actorData in data.Elements("Actor"))
            {
                switch (actorData.Attribute("type").Value)
                {
                    case "Box":
                        break;
                }
            }
        }
    }
}