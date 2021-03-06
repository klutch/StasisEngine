﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore.Models;

namespace StasisEditor.Views.Controls
{
    using Keys = System.Windows.Forms.Keys;

    public class EditBlueprintSocketsGraphics : GraphicsDeviceControl
    {
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;
        private Blueprint _blueprint;
        private Vector2 _mouse;
        private Vector2 _screenCenter;
        private EditBlueprintSocketsView _view;
        private bool _ctrl;

        public Blueprint blueprint { get { return _blueprint; } set { _blueprint = value; } }
        private Vector2 offset { get { return _screenCenter + new Vector2(Width, Height) / 2; } }

        public EditBlueprintSocketsGraphics()
            : base()
        {
        }

        // Initialize
        protected override void Initialize()
        {
            if (!DesignMode)
            {
                _view = Parent as EditBlueprintSocketsView;

                // Resources
                _spriteBatch = new SpriteBatch(GraphicsDevice);
                _pixel = new Texture2D(GraphicsDevice, 1, 1);
                _pixel.SetData<Color>(new[] { Color.White });

                // Input
                System.Windows.Forms.Application.Idle += delegate { Invalidate(); };
                MouseMove += new System.Windows.Forms.MouseEventHandler(EditBlueprintSocketsGraphics_MouseMove);
                MouseDown += new System.Windows.Forms.MouseEventHandler(EditBlueprintSocketsGraphics_MouseDown);
                FindForm().KeyPreview = true;
                FindForm().KeyDown += new System.Windows.Forms.KeyEventHandler(Parent_KeyDown);
                FindForm().KeyUp += new System.Windows.Forms.KeyEventHandler(EditBlueprintSocketsGraphics_KeyUp);
            }
        }

        // Key up
        void EditBlueprintSocketsGraphics_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
                _ctrl = false;
        }

        // Key down
        void Parent_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control || e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey)
                _ctrl = true;
        }

        // Mouse down
        void EditBlueprintSocketsGraphics_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Hit test scraps
            BlueprintScrap target = null;
            for (int i = 0; i < _blueprint.scraps.Count; i++)
            {
                if (_blueprint.scraps[i].hitTest(_mouse - offset))
                {
                    target = _blueprint.scraps[i];
                    break;
                }
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (_view.selectedScrap == null)
                {
                    // Select scrap
                    _view.selectedScrap = target;
                }
                else
                {
                    // Place selected scrap
                    _view.selectedScrap = null;
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (_view.socketTargetA == null)
                {
                    // Set first socket target
                    _view.socketTargetA = target;
                }
                else
                {
                    // Create socket on first target
                    Vector2 relativePoint = target.currentCraftPosition - _view.socketTargetA.currentCraftPosition;
                    BlueprintSocket firstSocket = new BlueprintSocket(_view.socketTargetA, target, relativePoint);
                    _blueprint.sockets.Add(firstSocket);

                    // Create socket on second target
                    BlueprintSocket secondSocket = new BlueprintSocket(target, _view.socketTargetA, -relativePoint);
                    _blueprint.sockets.Add(secondSocket);

                    // Set opposing sockets
                    firstSocket.opposingSocket = secondSocket;
                    secondSocket.opposingSocket = firstSocket;

                    // Form connection
                    _view.socketTargetA.connectScrap(target);
                    target.connectScrap(_view.socketTargetA);

                    // Clear socket target
                    _view.socketTargetA = null;
                }
            }
        }

        // Mouse move
        void EditBlueprintSocketsGraphics_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Vector2 oldMouse = _mouse;
            _mouse = new Vector2(e.X, e.Y);
            Vector2 delta = _mouse - oldMouse;

            if (_ctrl)
            {
                // Move screen
                _screenCenter += delta;
            }
            else
            {
                // Move selected scrap
                if (_view.selectedScrap != null)
                    _view.selectedScrap.move(delta, true);
                //if (_view.selectedScrap != null)
                //    _view.selectedScrap.currentCraftPosition += delta;
            }
        }

        // Draw
        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.Black);

            if (_blueprint != null)
            {
                _spriteBatch.Begin();

                // Draw scraps
                for (int i = _blueprint.scraps.Count - 1; i >= 0; i--)
                    _spriteBatch.Draw(_blueprint.scraps[i].scrapTexture, _blueprint.scraps[i].currentCraftPosition + offset, _blueprint.scraps[i].scrapTexture.Bounds, Color.White, 0f, _blueprint.scraps[i].textureCenter, 1f, SpriteEffects.None, 0);

                // Draw scrap sockets
                foreach (BlueprintSocket socket in _blueprint.sockets)
                {
                    drawLine(
                        socket.scrapA.currentCraftPosition,
                        socket.scrapA.currentCraftPosition + socket.relativePoint,
                        Color.Green);
                }

                // Draw mouse position
                _spriteBatch.Draw(_pixel, new Vector2(_mouse.X, _mouse.Y), new Rectangle(0, 0, 4, 4), Color.Yellow, 0, new Vector2(2, 2), 1f, SpriteEffects.None, 0);

                if (_view.socketTargetA != null)
                {
                    // Draw socket line
                    drawLine(_view.socketTargetA.currentCraftPosition, _mouse - offset, Color.Blue);
                }

                _spriteBatch.End();
            }
        }

        // drawLine
        private void drawLine(Vector2 pointA, Vector2 pointB, Color color)
        {
            Vector2 relative = pointB - pointA;
            float length = relative.Length();
            float angle = (float)Math.Atan2(relative.Y, relative.X);
            Rectangle rect = new Rectangle(0, 0, (int)length, 2);
            _spriteBatch.Draw(_pixel, pointA + offset, rect, color, angle, new Vector2(0, 1), 1f, SpriteEffects.None, 0);
        }
    }
}
