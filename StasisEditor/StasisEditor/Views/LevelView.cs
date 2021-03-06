﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisCore.Models;
using StasisEditor.Controllers;
using StasisEditor.Models;
using StasisEditor.Views.Controls;

namespace StasisEditor.Views
{
    public class LevelView : GraphicsDeviceControl
    {
        private LevelController _controller;
        private ContentManager _contentManager;
        private ContentManager _coreContentManager;
        private Texture2D _playerSpawnIcon;
        private Texture2D _itemIcon;
        private Texture2D _circuitIcon;
        private Texture2D _revoluteIcon;
        private Texture2D _prismaticIcon;
        private Texture2D _decalIcon;
        private Texture2D _tooltipIcon;
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;
        private Texture2D _circle;
        private SpriteFont _font;
        private Effect _primitivesEffect;
        private bool _draw = true;
        private bool _keysEnabled = true;
        private BackgroundRenderer _backgroundRenderer;

        public bool active
        {
            get { return _draw && _keysEnabled; }
            set { _draw = value; _keysEnabled = value; }
        }
        public SpriteBatch spriteBatch { get { return _spriteBatch; } }
        public BackgroundRenderer backgroundRenderer { get { return _backgroundRenderer; } }

        // setController
        public void setController(LevelController controller)
        {
            _controller = controller;
        }

        // Initialize
        protected override void Initialize()
        {
            // Resources
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _backgroundRenderer = new BackgroundRenderer(_spriteBatch);
            _contentManager = new ContentManager(Services, "Content");
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new[] { Color.White });
            _circle = _contentManager.Load<Texture2D>("circle");
            _playerSpawnIcon = _contentManager.Load<Texture2D>("actor_icons\\player_spawn");
            _itemIcon = _contentManager.Load<Texture2D>("actor_icons\\item");
            _circuitIcon = _contentManager.Load<Texture2D>("actor_icons\\circuit");
            _revoluteIcon = _contentManager.Load<Texture2D>("actor_icons\\revolute");
            _prismaticIcon = _contentManager.Load<Texture2D>("actor_icons\\prismatic");
            _decalIcon = _contentManager.Load<Texture2D>("actor_icons\\decal");
            _tooltipIcon = _contentManager.Load<Texture2D>("actor_icons/tooltip");
            _font = _contentManager.Load<SpriteFont>("arial");

            _coreContentManager = new ContentManager(Services, "Content");
            _primitivesEffect = _coreContentManager.Load<Effect>("effects\\primitives");

            // Draw loop
            Application.Idle += delegate { Invalidate(); };

            // Input
            MouseDown += new MouseEventHandler(LevelView_MouseDown);
            MouseEnter += new EventHandler(LevelView_MouseEnter);
            MouseLeave += new EventHandler(LevelView_MouseLeave);
            MouseWheel += new MouseEventHandler(LevelView_MouseWheel);
        }

        // Destructor
        ~LevelView()
        {
            _contentManager.Unload();
            _coreContentManager.Unload();
        }

        // Mouse wheel
        void LevelView_MouseWheel(object sender, MouseEventArgs e)
        {
            _controller.zoom(e.Delta);
        }

        // Mouse leave
        void LevelView_MouseLeave(object sender, EventArgs e)
        {
            _controller.mouseOverView = false;
        }

        // Mouse enter
        void LevelView_MouseEnter(object sender, EventArgs e)
        {
            _controller.mouseOverView = true;
        }

        // Mouse down
        void LevelView_MouseDown(object sender, MouseEventArgs e)
        {
            Focus();
            _controller.handleMouseDown(e);
        }

        // Draw
        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.Black);

            if (_draw && _controller.level != null)
            {
                if (_backgroundRenderer.background != null)
                {
                    _backgroundRenderer.update(_controller.scale, _controller.screenCenter);

                    // Draw first half of background layers
                    _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                    _backgroundRenderer.drawFirstHalf();
                    _spriteBatch.End();
                }
                
                _spriteBatch.Begin();

                // Draw grid
                drawGrid();

                // Draw level
                _controller.level.draw();

                _spriteBatch.End();

                _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

                // Draw actor controllers
                foreach (List<EditorActor> actors in _controller.level.sortedActors.Values)
                {
                    foreach (EditorActor actor in actors)
                        actor.draw();
                }

                _spriteBatch.End();

                if (_backgroundRenderer.background != null)
                {
                    // Draw second half of background layers
                    _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                    _backgroundRenderer.drawSecondHalf();
                    _spriteBatch.End();
                }

                _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

                // Draw mouse position
                drawMousePosition();

                _spriteBatch.End();
            }
        }

        // drawGrid
        private void drawGrid()
        {
            // Draw grid
            int screenWidth = Width;
            int screenHeight = Height;
            Vector2 worldOffset = _controller.worldOffset;
            float scale = _controller.scale;
            Microsoft.Xna.Framework.Rectangle destRect = new Microsoft.Xna.Framework.Rectangle(0, 0, (int)(screenWidth + scale), (int)(screenHeight + scale));
            Vector2 gridOffset = new Vector2((worldOffset.X * scale) % scale, (worldOffset.Y * scale) % scale);
            Microsoft.Xna.Framework.Color color = new Microsoft.Xna.Framework.Color(new Vector3(0.2f, 0.2f, 0.2f));

            // Vertical grid lines
            for (float x = 0; x < destRect.Width; x += scale)
                _spriteBatch.Draw(_pixel, new Vector2(x + gridOffset.X, 0), new Microsoft.Xna.Framework.Rectangle(0, 0, 1, screenHeight), color, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            // Horizontal grid lines
            for (float y = 0; y < destRect.Height; y += scale)
                _spriteBatch.Draw(_pixel, new Vector2(0, y + gridOffset.Y), new Microsoft.Xna.Framework.Rectangle(0, 0, screenWidth, 1), color, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        // drawMousePosition
        private void drawMousePosition()
        {
            float scale = _controller.scale;
            Vector2 worldOffset = _controller.worldOffset;
            Vector2 worldMouse = _controller.worldMouse;
            string text = String.Format("X: {0}\nY: {1}", worldMouse.X, worldMouse.Y);

            // Draw dot
            _spriteBatch.Draw(
                _pixel,
                (worldMouse + worldOffset) * scale,
                new Microsoft.Xna.Framework.Rectangle(0, 0, 4, 4),
                Microsoft.Xna.Framework.Color.Yellow, 0, new Vector2(2, 2),
                1f,
                Microsoft.Xna.Framework.Graphics.SpriteEffects.None,
                0);

            // Draw position as string
            drawString(text, worldMouse, Color.White, 0f);
            //_spriteBatch.DrawString(_font, text, (worldMouse + worldOffset) * scale + new Vector2(17, 17), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0000000001f);
            //_spriteBatch.DrawString(_font, text, (worldMouse + worldOffset) * scale + new Vector2(16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

        // drawBox
        public void drawBox(Vector2 position, float halfWidth, float halfHeight, float angle, Color color, float layerDepth)
        {
            float scale = _controller.scale;
            Rectangle rectangle = new Rectangle(0, 0, (int)(halfWidth * 2 * scale), (int)(halfHeight * 2 * scale));
            _spriteBatch.Draw(_pixel, (position + _controller.worldOffset) * scale, rectangle, color, angle, new Vector2(halfWidth, halfHeight) * scale, 1, SpriteEffects.None, layerDepth);
        }

        // drawLine
        public void drawLine(Vector2 pointA, Vector2 pointB, Color color, float layerDepth)
        {
            float scale = _controller.scale;
            Vector2 relative = pointB - pointA;
            float length = relative.Length();
            float angle = (float)Math.Atan2(relative.Y, relative.X);
            Rectangle rect = new Rectangle(0, 0, (int)(length * scale), 2);
            _spriteBatch.Draw(_pixel, (pointA + _controller.worldOffset) * scale, rect, color, angle, new Vector2(0, 1), 1f, SpriteEffects.None, layerDepth);
        }

        // drawPoint
        public void drawPoint(Vector2 position, Color color, float layerDepth)
        {
            drawCircle(position, 4f / _controller.scale, color, layerDepth);
        }

        // drawCircle
        public void drawCircle(Vector2 position, float radius, Color color, float layerDepth)
        {
            float circleScale = radius / (((float)_circle.Width / 2) / _controller.scale);
            _spriteBatch.Draw(_circle, (position + _controller.worldOffset) * _controller.scale, _circle.Bounds, color, 0, new Vector2(_circle.Width, _circle.Height) / 2, circleScale, SpriteEffects.None, layerDepth);
        }

        // drawIcon
        public void drawIcon(ActorType actorType, Vector2 position, float layerDepth)
        {
            Texture2D texture = _pixel;

            switch (actorType)
            {
                case ActorType.PlayerSpawn:
                    texture = _playerSpawnIcon;
                    break;

                case ActorType.Item:
                    texture = _itemIcon;
                    break;

                case ActorType.Circuit:
                    texture = _circuitIcon;
                    break;

                case ActorType.Revolute:
                    texture = _revoluteIcon;
                    break;

                case ActorType.Prismatic:
                    texture = _prismaticIcon;
                    break;

                case ActorType.Decal:
                    texture = _decalIcon;
                    break;

                case ActorType.Tooltip:
                    texture = _tooltipIcon;
                    break;
            }

            Rectangle rect = texture == _pixel ? new Rectangle(0, 0, 24, 24) : texture.Bounds;

            _spriteBatch.Draw(texture, (position + _controller.worldOffset) * _controller.scale, rect, Color.White, 0, new Vector2(rect.Width, rect.Height) / 2, 1f, SpriteEffects.None, layerDepth);
        }

        // drawString
        public void drawString(string text, Vector2 position, Color color, float layerDepth)
        {
            _spriteBatch.DrawString(_font, text, (position + _controller.worldOffset) * _controller.scale + new Vector2(1, 1), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, layerDepth + 0.0001f);
            _spriteBatch.DrawString(_font, text, (position + _controller.worldOffset) * _controller.scale, color, 0, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
        }

        // drawTexture
        public void drawTexture(Texture2D texture, Vector2 position, float angle, Vector2 origin, float layerDepth)
        {
            _spriteBatch.Draw(texture, (position + _controller.worldOffset) * _controller.scale, texture.Bounds, Color.White * 0.5f, angle, origin, _controller.scale / LevelController.ORIGINAL_SCALE, SpriteEffects.None, layerDepth);
        }

        // Render polygon
        public Texture2D renderPolygon(VertexPositionColorTexture[] _vertices, int primitiveCount)
        {
            Vector2 topLeft = new Vector2(_vertices[0].Position.X, _vertices[0].Position.Y);
            Vector2 bottomRight = topLeft;
            for (int i = 0; i < primitiveCount * 3; i++)
            {
                Vector2 vertex = new Vector2(_vertices[i].Position.X, _vertices[i].Position.Y);
                topLeft = Vector2.Min(topLeft, vertex);
                bottomRight = Vector2.Max(bottomRight, vertex);
            }
            int width = (int)Math.Ceiling((bottomRight.X - topLeft.X) * LevelController.ORIGINAL_SCALE);
            int height = (int)Math.Ceiling((bottomRight.Y - topLeft.Y) * LevelController.ORIGINAL_SCALE);

            RenderTarget2D renderTarget = new RenderTarget2D(GraphicsDevice, width, height);

            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Transparent);
            Matrix viewMatrix = Matrix.CreateTranslation(new Vector3(-topLeft, 0)) *
                Matrix.CreateScale(new Vector3(LevelController.ORIGINAL_SCALE, -LevelController.ORIGINAL_SCALE, 1f)) *
                Matrix.CreateTranslation(new Vector3(-width, height, 0) / 2f);
            Matrix projectionMatrix = Matrix.CreateOrthographic(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 1);

            _primitivesEffect.Parameters["world"].SetValue(Matrix.Identity);
            _primitivesEffect.Parameters["view"].SetValue(viewMatrix);
            _primitivesEffect.Parameters["projection"].SetValue(projectionMatrix);
            _primitivesEffect.CurrentTechnique.Passes["primitives"].Apply();
            GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _vertices, 0, primitiveCount, VertexPositionColorTexture.VertexDeclaration);
            GraphicsDevice.SetRenderTarget(null);

            Texture2D texture = new Texture2D(GraphicsDevice, width, height);
            Color[] data = new Color[width * height];
            renderTarget.GetData<Color>(data);
            texture.SetData<Color>(data);
            return texture;
        }
    }
}
