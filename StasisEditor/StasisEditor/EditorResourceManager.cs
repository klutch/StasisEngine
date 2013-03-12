﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using StasisCore;
using StasisCore.Models;

namespace StasisEditor
{
    public class EditorResourceManager
    {
        public const string RESOURCE_SOURCE_PATH = @"D:\StasisResources";

        // Checks to see if a resource exists
        public static bool exists(string uid)
        {
            try
            {
                // Check to see if the resource is loaded already
                if (ResourceManager.isResourceLoaded(uid))
                    return true;

                // Check materials
                using (FileStream fs = new FileStream(RESOURCE_SOURCE_PATH + ResourceManager.materialPath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement materialData in data.Elements("Material"))
                    {
                        if (materialData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check items
                using (FileStream fs = new FileStream(RESOURCE_SOURCE_PATH + ResourceManager.itemPath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement itemData in data.Elements("Item"))
                    {
                        if (itemData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check blueprints
                using (FileStream fs = new FileStream(RESOURCE_SOURCE_PATH + ResourceManager.blueprintPath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement blueprintData in data.Elements("Item"))
                    {
                        if (blueprintData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check characters
                using (FileStream fs = new FileStream(RESOURCE_SOURCE_PATH + ResourceManager.characterPath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement characterData in data.Elements("Character"))
                    {
                        if (characterData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check dialogue
                using (FileStream fs = new FileStream(RESOURCE_SOURCE_PATH + ResourceManager.dialoguePath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement dialogueData in data.Elements("Dialogue"))
                    {
                        if (dialogueData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check circuits
                using (FileStream fs = new FileStream(RESOURCE_SOURCE_PATH + ResourceManager.dialoguePath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement circuitData in data.Elements("Circuit"))
                    {
                        if (circuitData.Attribute("uid").Value == uid)
                            return true;
                    }
                }

                // Check backgrounds
                using (FileStream fs = new FileStream(RESOURCE_SOURCE_PATH + ResourceManager.backgroundPath, FileMode.Open))
                {
                    XElement data = XElement.Load(fs);
                    foreach (XElement backgroundData in data.Elements("Background"))
                    {
                        if (backgroundData.Attribute("uid").Value == uid)
                            return true;
                    }
                }
            }
            catch (XmlException e)
            {
                throw new InvalidResourceException();
            }
            catch (FileNotFoundException e)
            {
                throw new ResourceNotFoundException(uid);
            }

            return false;
        }

        // Save material resources
        public static void saveMaterialResources(List<Material> materials, bool backup)
        {
            // Backup materials
            if (backup)
            {
                string backupFile = ResourceManager.materialPath + ".bak";
                if (File.Exists(backupFile))
                    File.Delete(backupFile);
                File.Move(ResourceManager.materialPath, backupFile);
            }

            // Save materials
            XDocument doc = new XDocument(new XElement("Materials"));
            foreach (Material material in materials)
                doc.Element("Materials").Add(material.data);
            doc.Save(ResourceManager.materialPath);

            // Reload materials
            FileStream fs = new FileStream(EditorResourceManager.RESOURCE_SOURCE_PATH + ResourceManager.materialResources, FileMode.Open);
            ResourceManager.loadAllMaterials(fs);
        }

        // Save blueprint resources
        public static void saveBlueprintResources(List<Blueprint> blueprints, bool backup)
        {
            // Backup blueprints
            if (backup)
            {
                string backupFile = ResourceManager.blueprintPath + ".bak";
                if (File.Exists(backupFile))
                    File.Delete(backupFile);
                File.Move(ResourceManager.blueprintPath, backupFile);
            }

            // Save blueprints
            XDocument doc = new XDocument(new XElement("Items"));
            foreach (Blueprint blueprint in blueprints)
                doc.Element("Items").Add(blueprint.data);
            doc.Save(ResourceManager.blueprintPath);

            // Reload blueprints
            FileStream fs = new FileStream(EditorResourceManager.RESOURCE_SOURCE_PATH + ResourceManager.blueprintResources, FileMode.Open);
            ResourceManager.loadAllItems(fs);
        }

        // Save circuit resources
        public static void saveCircuitResources(List<Circuit> circuits, bool backup)
        {
            // Backup circuits
            if (backup)
            {
                string backupFile = ResourceManager.circuitPath + ".bak";
                if (File.Exists(backupFile))
                    File.Delete(backupFile);
                File.Move(ResourceManager.circuitPath, backupFile);
            }

            // Save circuits
            XDocument doc = new XDocument(new XElement("Circuits"));
            foreach (Circuit circuit in circuits)
                doc.Element("Circuits").Add(circuit.data);
            doc.Save(ResourceManager.circuitPath);

            // Reload circuits
            ResourceManager.loadAllCircuits(new FileStream(EditorResourceManager.RESOURCE_SOURCE_PATH + ResourceManager.circuitPath, FileMode.Open));
        }

        // Save background resources
        public static void saveBackgroundResources(XElement data, bool backup)
        {
            // Backup backgrounds
            if (backup)
            {
                string backupFile = ResourceManager.backgroundPath + ".bak";
                if (File.Exists(backupFile))
                    File.Delete(backupFile);
                File.Move(ResourceManager.backgroundPath, backupFile);
            }

            // Save backgrounds
            XDocument doc = new XDocument(data);
            doc.Save(ResourceManager.backgroundPath);

            // Reload background
            ResourceManager.loadAllBackgrounds(new FileStream(RESOURCE_SOURCE_PATH + ResourceManager.backgroundPath, FileMode.Open));
        }
    }
}