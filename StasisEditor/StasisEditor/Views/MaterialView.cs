﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using StasisCore.Models;
using StasisEditor.Controllers;
using StasisEditor.Controls;
using StasisEditor.Models;

namespace StasisEditor.Views
{
    public partial class MaterialView : UserControl, IMaterialView
    {
        private IMaterialController _controller;
        private List<MaterialResource>[] _materialCopies;
        private MaterialProperties _materialProperties;

        public MaterialView()
        {
            int numMaterialTypes = Enum.GetValues(typeof(MaterialType)).Length;
            _materialCopies = new List<MaterialResource>[numMaterialTypes];

            InitializeComponent();

            // Set material types
            foreach (string materialType in Enum.GetNames(typeof(MaterialType)))
                materialTypesListBox.Items.Add(materialType);
        }

        // setController
        public void setController(IMaterialController controller)
        {
            _controller = controller;
        }

        // copyMaterials
        public void copyMaterials()
        {
            // Set material copies
            int numMaterialTypes = Enum.GetValues(typeof(MaterialType)).Length;
            for (int i = 0; i < numMaterialTypes; i++)
                _materialCopies[i] = MaterialResource.copyFrom(_controller.getMaterials((MaterialType)i));
        }

        // setChangesMade
        public void setChangesMade(bool status)
        {
            saveButton.Enabled = true;
        }

        // setAutoUpdatePreview -- this will trigger an event
        public void setAutoUpdatePreview(bool status)
        {
            autoUpdatePreview.Checked = status;
        }

        // getSelectedMaterial
        public MaterialResource getSelectedMaterial()
        {
            return materialsListBox.SelectedItem as MaterialResource;
        }

        // openProperties
        private void openProperties(MaterialResource material)
        {
            _materialProperties = new MaterialProperties(_controller, material);
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

        /*
        // Close button clicked
        private void closeButton_Click(object sender, EventArgs e)
        {
            if (_controller.getChangesMade())
            {
                if (MessageBox.Show("Are you sure you want to close the Materials View? Any unsaved changes will be lost.", "Close Materials", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    Close();
            }
            else
                Close();
        }*/

        // Material type selection changed
        private void materialTypesListBox_SelectedValueChanged(object sender, EventArgs e)
        {
            ListBox listBox = (sender as ListBox);
            MaterialType type = (MaterialType)Enum.Parse(typeof(MaterialType), listBox.SelectedItem as string);
            materialsListBox.DataSource = _materialCopies[(int)type];
        }

        // Selected materials changed
        private void materialsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Update preview button
            previewButton.Enabled = materialsListBox.SelectedItem != null;
            
            // Open material properties
            closeProperties();
            openProperties(materialsListBox.SelectedItem as MaterialResource);
        }

        // Material property changed
        private void materialProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            // Changes are being made
            setChangesMade(true);

            // Refresh the materials list
            (materialsListBox as RefreshingListBox).RefreshItems();
        }

        // Preview material
        private void previewButton_Click(object sender, EventArgs e)
        {
            _controller.preview(materialsListBox.SelectedItem as MaterialResource);
        }

        // Auto update changed
        private void autoUpdatePreview_CheckedChanged(object sender, EventArgs e)
        {
            _controller.setAutoUpdatePreview(autoUpdatePreview.Checked);
        }

        /*
        // Form closed
        private void MaterialView_FormClosed(object sender, FormClosedEventArgs e)
        {
            _controller.viewClosed();
        }*/
    }
}
