﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StasisCore.Models;
using StasisEditor.Controllers;

namespace StasisEditor.Views.Controls
{
    public partial class TerrainLayersControl : UserControl
    {
        private MaterialController _controller;
        private TerrainMaterialResource _material;
        //private List<TerrainLayerResource> _layers;

        public TerrainLayersControl(MaterialController controller, TerrainMaterialResource material)
        {
            _controller = controller;
            _material = material;

            // Controls
            InitializeComponent();
            Dock = DockStyle.Fill;
            upButton.Text = char.ConvertFromUtf32(0x000002c4);
            downButton.Text = char.ConvertFromUtf32(0x000002c5);
        }

        // populate tree view
        public void populateTreeView(TerrainRootLayerResource rootLayer)
        {
            SuspendLayout();

            List<TerrainLayerResource> layers = rootLayer.layers;

            // Clear existing nodes
            layersTreeView.Nodes.Clear();

            // Build nodes
            LayerNode rootNode = recursiveBuildNode(rootLayer);

            // Set tree view to root node
            layersTreeView.Nodes.Add(rootNode);

            // Expand all
            layersTreeView.ExpandAll();

            ResumeLayout();
        }

        // recursiveBuildNode
        private LayerNode recursiveBuildNode(TerrainLayerResource layer)
        {
            LayerNode node = new LayerNode(layer, layer.enabled);
            switch (layer.type)
            {
                case TerrainLayerType.Root:
                    TerrainRootLayerResource rootLayer = layer as TerrainRootLayerResource;
                    foreach (TerrainLayerResource childLayer in rootLayer.layers)
                        node.Nodes.Add(recursiveBuildNode(childLayer));
                    break;

                case TerrainLayerType.Group:
                    TerrainGroupLayerResource groupLayer = layer as TerrainGroupLayerResource;
                    foreach (TerrainLayerResource childLayer in groupLayer.layers)
                        node.Nodes.Add(recursiveBuildNode(childLayer));
                    break;
            }
            return node;
        }

        // Select layer
        public void selectLayer(TerrainLayerResource layer)
        {
            Debug.Assert(layersTreeView.Nodes[0] is LayerNode);
            LayerNode targetNode = recursiveGetNode(layersTreeView.Nodes[0] as LayerNode, layer);
            layersTreeView.SelectedNode = targetNode;
        }

        // recursiveGetNode
        private LayerNode recursiveGetNode(LayerNode startNode, TerrainLayerResource layer)
        {
            // Check this node's layer
            if (startNode.layer == layer)
                return startNode;

            if (startNode.layer.type == TerrainLayerType.Root || startNode.layer.type == TerrainLayerType.Group)
            {
                // Spawn more searches, checking results
                foreach (TreeNode node in startNode.Nodes)
                {
                    LayerNode result = recursiveGetNode(node as LayerNode, layer);
                    if (result != null && result.layer == layer)
                        return result;
                }
            }

            return null;
        }

        // Move layer up
        private void upButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(layersTreeView.SelectedNode is LayerNode);
            LayerNode node = layersTreeView.SelectedNode as LayerNode;

            // Move layer up
            TerrainGroupLayerResource parent = (node.Parent as LayerNode).layer as TerrainGroupLayerResource;
            _controller.moveTerrainLayerUp(parent, node.layer);

            // Refresh tree view
            populateTreeView(_material.rootLayer);

            // Select repositioned layer
            selectLayer(node.layer);

            // Set changes made
            _controller.setChangesMade(true);
        }

        // Move layer down
        private void downButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(layersTreeView.SelectedNode is LayerNode);
            LayerNode node = layersTreeView.SelectedNode as LayerNode;

            // Move layer down
            TerrainGroupLayerResource parent = (node.Parent as LayerNode).layer as TerrainGroupLayerResource;
            _controller.moveTerrainLayerDown(parent, node.layer);

            // Refresh tree view
            populateTreeView(_material.rootLayer);

            // Select repositioned layer
            selectLayer(node.layer);

            // Set changes made
            _controller.setChangesMade(true);
        }

        // Add layer button clicked
        private void addButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(layersTreeView.SelectedNode is LayerNode);

            NewTerrainLayerForm newLayerForm = new NewTerrainLayerForm();
            if (newLayerForm.ShowDialog() == DialogResult.OK)
            {
                LayerNode node = layersTreeView.SelectedNode as LayerNode;
                TerrainLayerResource newLayer = TerrainLayerResource.create(newLayerForm.getSelectedType());

                if (node.layer.type == TerrainLayerType.Root || node.layer.type == TerrainLayerType.Group)
                {
                    // Insert into group
                    _controller.addTerrainLayer(node.layer as TerrainGroupLayerResource, newLayer, node.Nodes.Count);
                }
                else
                {
                    // Create new layer
                    LayerNode parent = node.Parent as LayerNode;

                    // Add new layer to parent
                    _controller.addTerrainLayer(parent.layer as TerrainGroupLayerResource, newLayer, node.Index + 1);
                }

                // Refresh tree
                populateTreeView(_material.rootLayer);

                // Select layer
                selectLayer(newLayer);

                // Refocus on tree
                layersTreeView.Focus();

                // Set changes made
                _controller.setChangesMade(true);
            }
        }

        // Remove layer
        private void removeLayerButton_Click(object sender, EventArgs e)
        {
            Debug.Assert(layersTreeView.SelectedNode is LayerNode);
            LayerNode node = layersTreeView.SelectedNode as LayerNode;
            Debug.Assert(node.layer.type != TerrainLayerType.Root);

            // Display a warning that child nodes will be destroyed
            bool doRemove = false;
            if (node.layer.type == TerrainLayerType.Root || node.layer.type == TerrainLayerType.Group)
                doRemove = MessageBox.Show("Are you sure you want to remove this node and all its child nodes?", "Remove Nodes", MessageBoxButtons.YesNo) == DialogResult.Yes;
            else
                doRemove = true;

            if (doRemove)
            {
                // Remove layer
                TerrainGroupLayerResource parent = (node.Parent as LayerNode).layer as TerrainGroupLayerResource;
                _controller.removeTerrainLayer(parent, node.layer);

                // Refresh tree view
                populateTreeView(_material.rootLayer);

                // Set changes made
                _controller.setChangesMade(true);
            }
        }

        // recursiveEnableNode
        private void recursiveEnableNode(TreeNode startNode, bool enabled)
        {
            // This only fakes a disabled node by changing its forecolor
            startNode.ForeColor = enabled ? Color.Black : Color.Gray;

            // Call the chillens'
            foreach (TreeNode childNode in startNode.Nodes)
                recursiveEnableNode(childNode, enabled);
        }

        // Property value changed
        private void layerProperties_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            _controller.setChangesMade(true);
        }

        // Selected node changed
        private void layersTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Debug.Assert(e.Node is LayerNode);
            LayerNode node = e.Node as LayerNode;

            // Remove button's status
            removeLayerButton.Enabled = node.layer.type != TerrainLayerType.Root;

            // Add button's status
            addLayerButton.Enabled = true;

            // Up/down buttons' status
            if (node.layer.type == TerrainLayerType.Root)
            {
                upButton.Enabled = false;
                downButton.Enabled = false;
            }
            else
            {
                upButton.Enabled = e.Node.Index > 0;
                downButton.Enabled = e.Node.Index < e.Node.Parent.Nodes.Count - 1;
            }

            // Set layer's property grid
            layerProperties.SelectedObject = node.layer.properties;
        }

        // Node check changed
        private void layersTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            Debug.Assert(e.Node is LayerNode);
            LayerNode node = e.Node as LayerNode;
            Debug.Assert(node.layer.type != TerrainLayerType.Root);

            // Set layer's enabled status
            node.layer.enabled = node.Checked;

            // Recursively update child nodes' enabled status
            // (nodes only, not layers -- the renderer stops at the first disabled layer anyway, so it's effectively the same behavior as disabling the layers)
            foreach (TreeNode childNode in e.Node.Nodes)
                recursiveEnableNode(childNode, node.Checked);

            // Set changes made
            _controller.setChangesMade(true);
        }

        // Checkbox validation
        private void layersTreeView_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            // Prevent root node from being unchecked
            Debug.Assert(e.Node is LayerNode);
            LayerNode node = e.Node as LayerNode;
            if (node.layer.type == TerrainLayerType.Root)
                e.Cancel = true;
        }
    }
}
