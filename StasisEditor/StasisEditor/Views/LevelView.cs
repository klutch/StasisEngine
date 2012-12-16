﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using StasisEditor.Controllers;
using StasisEditor.Controllers.Actors;

namespace StasisEditor.Views
{
    public partial class LevelView : UserControl
    {
        private LevelController _controller;

        public LevelView()
        {
            InitializeComponent();
        }

        // setController
        public void setController(LevelController controller)
        {
            _controller = controller;
        }

        // hookToXNA
        public void hookToXNA()
        {
            // Hook to XNA
            XNAResources.graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(preparingDeviceSettings);
            Microsoft.Xna.Framework.Input.Mouse.WindowHandle = surface.FindForm().Handle;

            // Resize graphics device when the surface is resized
            Resize += new EventHandler(surface_Resize);

            // Temporary -- Force resize
            _controller.resizeGraphicsDevice(surface.Width, surface.Height);
        }

        // unhookFromXNA
        public void unhookFromXNA()
        {
            // Unhook from XNA
            XNAResources.graphics.PreparingDeviceSettings -= new EventHandler<PreparingDeviceSettingsEventArgs>(preparingDeviceSettings);

            // Remove resize event handler
            Resize -= new EventHandler(surface_Resize);
        }

        // Surface resize event handler
        void surface_Resize(object sender, EventArgs e)
        {
            _controller.resizeGraphicsDevice(surface.Width, surface.Height);
        }

        // Set the graphics device window handle to the surface handle
        private void preparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = surface.Handle;
        }

        // getWidth
        public int getWidth()
        {
            return surface.Width;
        }

        // getHeight
        public int getHeight()
        {
            return surface.Height;
        }

        // handleXNADraw
        public void handleXNADraw()
        {
            // Draw grid
            drawGrid();

            // Draw mouse position
            drawMousePosition();

            // Draw actor controllers
            drawActorControllers();
        }

        // drawGrid
        private void drawGrid()
        {
            // Draw grid
            int screenWidth = surface.Width;
            int screenHeight = surface.Height;
            Vector2 worldOffset = _controller.getWorldOffset();
            float scale = _controller.getScale();
            Microsoft.Xna.Framework.Rectangle destRect = new Microsoft.Xna.Framework.Rectangle(0, 0, (int)(screenWidth + scale), (int)(screenHeight + scale));
            Vector2 gridOffset = new Vector2((worldOffset.X * scale) % scale, (worldOffset.Y * scale) % scale);
            Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color(new Vector3(0.2f, 0.2f, 0.2f));

            // Vertical grid lines
            for (float x = 0; x < destRect.Width; x += scale)
                XNAResources.spriteBatch.Draw(XNAResources.pixel, new Microsoft.Xna.Framework.Rectangle((int)(x + gridOffset.X), 0, 1, screenHeight), color);

            // Horizontal grid lines
            for (float y = 0; y < destRect.Height; y += scale)
                XNAResources.spriteBatch.Draw(XNAResources.pixel, new Microsoft.Xna.Framework.Rectangle(0, (int)(y + gridOffset.Y), screenWidth, 1), color);
        }

        // drawMousePosition
        private void drawMousePosition()
        {
            float scale = _controller.getScale();
            Vector2 worldOffset = _controller.getWorldOffset();
            Vector2 worldMouse = _controller.getWorldMouse();

            XNAResources.spriteBatch.Draw(
                XNAResources.pixel,
                (worldMouse + worldOffset) * scale,
                new Microsoft.Xna.Framework.Rectangle(0, 0, 8, 8),
                Microsoft.Xna.Framework.Color.Yellow, 0, new Vector2(4, 4),
                1f,
                Microsoft.Xna.Framework.Graphics.SpriteEffects.None,
                0);

        }

        // drawActorControllers
        private void drawActorControllers()
        {
            List<ActorResourceController> actorControllers = _controller.getActorControllers();
            foreach (ActorResourceController actorController in actorControllers)
                actorController.draw();
        }

        // Surface mouse down
        private void surface_MouseDown(object sender, MouseEventArgs e)
        {
            _controller.handleMouseDown(e);
        }
    }
}
