﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using StasisCore.Models;
using StasisEditor.Models;
using StasisEditor.Controllers;
using StasisEditor.Views.Controls;

namespace StasisEditor.Views
{
    public partial class MaterialView : UserControl
    {
        private MaterialController _controller;
        private MaterialProperties _materialProperties;

        public MaterialView()
        {
            int numMaterialTypes = Enum.GetValues(typeof(MaterialType)).Length;

            InitializeComponent();

            // Set material types
            foreach (string materialType in Enum.GetNames(typeof(MaterialType)))
                materialTypesListBox.Items.Add(materialType);
        }

        // getController
        public MaterialController getController()
        {
            return _controller;
        }

        // setController
        public void setController(MaterialController controller)
        {
            _controller = controller;
        }

        // setChangesMade
        public void setChangesMade(bool status)
        {
            // Update button
            saveButton.Enabled = status;

            // Update selected material
            getSelectedMaterial().changed = status;
        }

        // setAutoUpdatePreview -- this will trigger an event
        public void setAutoUpdatePreview(bool status)
        {
            autoUpdatePreview.Checked = status;
        }

        // getSelectedMaterial
        public EditorMaterial getSelectedMaterial()
        {
            return materialsListBox.SelectedItem as EditorMaterial;
        }

        // openProperties
        private void openProperties(EditorMaterial material)
        {
            _materialProperties = new MaterialProperties(this, material);
            propertiesContainer.Controls.Add(_materialProperties);

            // Set material property grid's selected objects
            _materialProperties.PropertyGrid.SelectedObject = material;
        }

        // closeProperties
        private void closeProperties()
        {
            if (_materialProperties == null)
                return;

            propertiesContainer.Controls.Remove(_materialProperties);
            _materialProperties.Dispose();
            _materialProperties = null;
        }

        // Refresh material list
        public void refreshMaterialList()
        {
            materialsListBox.RefreshItems();
        }

        // Material type selection changed
        private void materialTypesListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            ListBox listBox = (sender as ListBox);
            MaterialType type = (MaterialType)Enum.Parse(typeof(MaterialType), listBox.SelectedItem as string);
            materialsListBox.DataSource = _controller.getMaterials(type);
        }

        // Selected materials changed
        private void materialsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update preview button
            previewButton.Enabled = materialsListBox.SelectedItem != null;

            // Update save button
            saveButton.Enabled = getSelectedMaterial().changed;
            
            // Open material properties
            closeProperties();
            openProperties(getSelectedMaterial());
        }

        // Material property changed
        private void materialProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // Changes are being made
            getSelectedMaterial().changed = true;

            // Refresh the materials list
            (materialsListBox as RefreshingListBox).RefreshItems();
        }

        // Preview material
        private void previewButton_Click(object sender, EventArgs e)
        {
            _controller.preview(getSelectedMaterial());
        }

        // Auto update changed
        private void autoUpdatePreview_CheckedChanged(object sender, EventArgs e)
        {
            _controller.setAutoUpdatePreview(autoUpdatePreview.Checked);
        }

        // Material save button
        private void saveButton_Click(object sender, EventArgs e)
        {
            getSelectedMaterial().changed = false;
            //_controller.saveResource(getSelectedMaterial().resource);
            saveButton.Enabled = false;
        }
    }
}
