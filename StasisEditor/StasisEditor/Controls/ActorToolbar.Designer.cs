﻿namespace StasisEditor.Controls
{
    partial class ActorToolbar
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActorToolbar));
            this.anchorToolStrip = new System.Windows.Forms.ToolStrip();
            this.boxButton = new System.Windows.Forms.ToolStripButton();
            this.circleButton = new System.Windows.Forms.ToolStripButton();
            this.terrainButton = new System.Windows.Forms.ToolStripButton();
            this.movingPlatformButton = new System.Windows.Forms.ToolStripButton();
            this.pressurePlateButton = new System.Windows.Forms.ToolStripButton();
            this.objectSpawnerButton = new System.Windows.Forms.ToolStripButton();
            this.ropeButton = new System.Windows.Forms.ToolStripButton();
            this.timerButton = new System.Windows.Forms.ToolStripButton();
            this.fluidButton = new System.Windows.Forms.ToolStripButton();
            this.itemsButton = new System.Windows.Forms.ToolStripButton();
            this.plantsButton = new System.Windows.Forms.ToolStripButton();
            this.playerSpawnButton = new System.Windows.Forms.ToolStripButton();
            this.goalButton = new System.Windows.Forms.ToolStripButton();
            this.anchorToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // anchorToolStrip
            // 
            this.anchorToolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.anchorToolStrip.AutoSize = false;
            this.anchorToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.anchorToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.anchorToolStrip.ImageScalingSize = new System.Drawing.Size(22, 22);
            this.anchorToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boxButton,
            this.circleButton,
            this.terrainButton,
            this.movingPlatformButton,
            this.pressurePlateButton,
            this.objectSpawnerButton,
            this.ropeButton,
            this.timerButton,
            this.fluidButton,
            this.itemsButton,
            this.plantsButton,
            this.playerSpawnButton,
            this.goalButton});
            this.anchorToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.anchorToolStrip.Location = new System.Drawing.Point(0, 0);
            this.anchorToolStrip.Name = "anchorToolStrip";
            this.anchorToolStrip.Size = new System.Drawing.Size(206, 69);
            this.anchorToolStrip.TabIndex = 0;
            this.anchorToolStrip.Text = "Actor Toolbar";
            this.anchorToolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.anchorToolStrip_ItemClicked);
            // 
            // boxButton
            // 
            this.boxButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.boxButton.Image = ((System.Drawing.Image)(resources.GetObject("boxButton.Image")));
            this.boxButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.boxButton.Margin = new System.Windows.Forms.Padding(0);
            this.boxButton.Name = "boxButton";
            this.boxButton.Size = new System.Drawing.Size(26, 26);
            this.boxButton.Text = "Box";
            // 
            // circleButton
            // 
            this.circleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.circleButton.Image = ((System.Drawing.Image)(resources.GetObject("circleButton.Image")));
            this.circleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.circleButton.Margin = new System.Windows.Forms.Padding(0);
            this.circleButton.Name = "circleButton";
            this.circleButton.Size = new System.Drawing.Size(26, 26);
            this.circleButton.Text = "Circle";
            // 
            // terrainButton
            // 
            this.terrainButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.terrainButton.Image = ((System.Drawing.Image)(resources.GetObject("terrainButton.Image")));
            this.terrainButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.terrainButton.Margin = new System.Windows.Forms.Padding(0);
            this.terrainButton.Name = "terrainButton";
            this.terrainButton.Size = new System.Drawing.Size(26, 26);
            this.terrainButton.Text = "Terrain";
            // 
            // movingPlatformButton
            // 
            this.movingPlatformButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.movingPlatformButton.Image = ((System.Drawing.Image)(resources.GetObject("movingPlatformButton.Image")));
            this.movingPlatformButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.movingPlatformButton.Margin = new System.Windows.Forms.Padding(0);
            this.movingPlatformButton.Name = "movingPlatformButton";
            this.movingPlatformButton.Size = new System.Drawing.Size(26, 26);
            this.movingPlatformButton.Text = "Moving Platform (Signal Receiver)";
            // 
            // pressurePlateButton
            // 
            this.pressurePlateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pressurePlateButton.Image = ((System.Drawing.Image)(resources.GetObject("pressurePlateButton.Image")));
            this.pressurePlateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pressurePlateButton.Margin = new System.Windows.Forms.Padding(0);
            this.pressurePlateButton.Name = "pressurePlateButton";
            this.pressurePlateButton.Size = new System.Drawing.Size(26, 26);
            this.pressurePlateButton.Text = "Pressure Plate (Signal Transmitter)";
            // 
            // objectSpawnerButton
            // 
            this.objectSpawnerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.objectSpawnerButton.Enabled = false;
            this.objectSpawnerButton.Image = ((System.Drawing.Image)(resources.GetObject("objectSpawnerButton.Image")));
            this.objectSpawnerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.objectSpawnerButton.Name = "objectSpawnerButton";
            this.objectSpawnerButton.Size = new System.Drawing.Size(26, 26);
            this.objectSpawnerButton.Text = "Object Spawner (Signal Receiver)";
            // 
            // ropeButton
            // 
            this.ropeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ropeButton.Image = ((System.Drawing.Image)(resources.GetObject("ropeButton.Image")));
            this.ropeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ropeButton.Margin = new System.Windows.Forms.Padding(0);
            this.ropeButton.Name = "ropeButton";
            this.ropeButton.Size = new System.Drawing.Size(26, 26);
            this.ropeButton.Text = "Rope";
            // 
            // timerButton
            // 
            this.timerButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.timerButton.Enabled = false;
            this.timerButton.Image = ((System.Drawing.Image)(resources.GetObject("timerButton.Image")));
            this.timerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.timerButton.Margin = new System.Windows.Forms.Padding(0);
            this.timerButton.Name = "timerButton";
            this.timerButton.Size = new System.Drawing.Size(26, 26);
            this.timerButton.Text = "Timer (Signal Transmitter)";
            // 
            // fluidButton
            // 
            this.fluidButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.fluidButton.Image = ((System.Drawing.Image)(resources.GetObject("fluidButton.Image")));
            this.fluidButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fluidButton.Margin = new System.Windows.Forms.Padding(0);
            this.fluidButton.Name = "fluidButton";
            this.fluidButton.Size = new System.Drawing.Size(26, 26);
            this.fluidButton.Text = "Fluid";
            // 
            // itemsButton
            // 
            this.itemsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.itemsButton.Enabled = false;
            this.itemsButton.Image = ((System.Drawing.Image)(resources.GetObject("itemsButton.Image")));
            this.itemsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.itemsButton.Name = "itemsButton";
            this.itemsButton.Size = new System.Drawing.Size(26, 26);
            this.itemsButton.Text = "Items";
            // 
            // plantsButton
            // 
            this.plantsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.plantsButton.Enabled = false;
            this.plantsButton.Image = ((System.Drawing.Image)(resources.GetObject("plantsButton.Image")));
            this.plantsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.plantsButton.Margin = new System.Windows.Forms.Padding(0);
            this.plantsButton.Name = "plantsButton";
            this.plantsButton.Size = new System.Drawing.Size(26, 26);
            this.plantsButton.Text = "Plants";
            // 
            // playerSpawnButton
            // 
            this.playerSpawnButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.playerSpawnButton.Image = ((System.Drawing.Image)(resources.GetObject("playerSpawnButton.Image")));
            this.playerSpawnButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.playerSpawnButton.Margin = new System.Windows.Forms.Padding(0);
            this.playerSpawnButton.Name = "playerSpawnButton";
            this.playerSpawnButton.Size = new System.Drawing.Size(26, 26);
            this.playerSpawnButton.Text = "Player Spawn";
            // 
            // goalButton
            // 
            this.goalButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.goalButton.Enabled = false;
            this.goalButton.Image = ((System.Drawing.Image)(resources.GetObject("goalButton.Image")));
            this.goalButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.goalButton.Margin = new System.Windows.Forms.Padding(0);
            this.goalButton.Name = "goalButton";
            this.goalButton.Size = new System.Drawing.Size(26, 26);
            this.goalButton.Text = "Goal";
            // 
            // ActorToolbar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.anchorToolStrip);
            this.Name = "ActorToolbar";
            this.Size = new System.Drawing.Size(206, 69);
            this.anchorToolStrip.ResumeLayout(false);
            this.anchorToolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip anchorToolStrip;
        private System.Windows.Forms.ToolStripButton boxButton;
        private System.Windows.Forms.ToolStripButton circleButton;
        private System.Windows.Forms.ToolStripButton terrainButton;
        private System.Windows.Forms.ToolStripButton movingPlatformButton;
        private System.Windows.Forms.ToolStripButton pressurePlateButton;
        private System.Windows.Forms.ToolStripButton ropeButton;
        private System.Windows.Forms.ToolStripButton timerButton;
        private System.Windows.Forms.ToolStripButton fluidButton;
        private System.Windows.Forms.ToolStripButton playerSpawnButton;
        private System.Windows.Forms.ToolStripButton goalButton;
        private System.Windows.Forms.ToolStripButton itemsButton;
        private System.Windows.Forms.ToolStripButton plantsButton;
        private System.Windows.Forms.ToolStripButton objectSpawnerButton;
    }
}