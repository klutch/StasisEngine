﻿namespace StasisEditor.Views.Controls
{
    partial class CreateBlueprintScrapShapeButton
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
            this.createBlueprintScrapButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // createBlueprintScrapButton
            // 
            this.createBlueprintScrapButton.Location = new System.Drawing.Point(0, 17);
            this.createBlueprintScrapButton.Margin = new System.Windows.Forms.Padding(0);
            this.createBlueprintScrapButton.Name = "createBlueprintScrapButton";
            this.createBlueprintScrapButton.Size = new System.Drawing.Size(75, 23);
            this.createBlueprintScrapButton.TabIndex = 0;
            this.createBlueprintScrapButton.Text = "Edit Points";
            this.createBlueprintScrapButton.UseVisualStyleBackColor = true;
            this.createBlueprintScrapButton.Click += new System.EventHandler(this.createBlueprintScrapButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Blueprint Scrap Shape";
            // 
            // CreateBlueprintScrapShapeButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.createBlueprintScrapButton);
            this.Name = "CreateBlueprintScrapShapeButton";
            this.Size = new System.Drawing.Size(194, 56);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button createBlueprintScrapButton;
        private System.Windows.Forms.Label label1;
    }
}
