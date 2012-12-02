﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisEditor.Controllers;
using StasisEditor.Controls;

namespace StasisEditor.Views
{
    public partial class TextureView : Form, ITextureView
    {
        private ITextureController _controller;

        public TextureView()
        {
            InitializeComponent();
            textureDataGrid.DataSource = TextureResources.getAllResources();
        }

        // setController
        public void setController(ITextureController controller)
        {
            _controller = controller;
        }

        // Form closed
        private void TextureView_FormClosed(object sender, FormClosedEventArgs e)
        {
            _controller.viewClosed();
        }

        // Add texture resource
        private void addTextureButton_Click(object sender, EventArgs e)
        {
            NewTextureResource newResource = new NewTextureResource();
            if (newResource.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("should add a new texture resource.");
            }
        }
    }
}
