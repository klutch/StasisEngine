﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using StasisCore;
using StasisCore.Models;
using StasisGame.Managers;
using StasisGame.Systems;

namespace StasisGame.UI
{
    public class WorldMapScreen : Screen
    {
        private LoderGame _game;
        private SystemManager _systemManager;
        private float _scale;
        private Vector2 _currentScreenCenter;
        private Vector2 _targetScreenCenter;
        private Vector2 _halfScreenSize;
        private Texture2D _pathTexture;
        private Vector2 _pathTextureOrigin;
        private ContentManager _content;
        private Effect _fogEffect;
        private Texture2D _antiFogBrush;
        private Vector2 _antiFogBrushOrigin;
        private RenderTarget2D _fogRT;
        private RenderTarget2D _antiFogRT;
        private Vector2 _levelSelectPosition;
        private Texture2D _levelSelectIcon;
        private Vector2 _levelSelectIconHalfSize;
        private float _levelSelectAngle;
        private Color _levelSelectIconColor;
        private Color _levelSelectIconSelectedColor;
        private Color _levelSelectIconDeselectedColor;
        private bool _allowNewLevelSelection = true;
        private SpriteFont _levelSelectTitleFont;
        private SpriteFont _levelSelectDescriptionFont;
        private LevelIconDefinition _selectedLevelIcon;

        public WorldMapScreen(LoderGame game, SystemManager systemManager)
            : base(game.screenSystem, ScreenType.WorldMap)
        {
            _game = game;
            _systemManager = systemManager;
            _scale = 1f;

            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            _fogEffect = _content.Load<Effect>("fog_effect");
            _pathTexture = _content.Load<Texture2D>("world_map/path");
            _pathTextureOrigin = new Vector2(_pathTexture.Width, _pathTexture.Height) / 2f;
            _antiFogBrush = _content.Load<Texture2D>("world_map/anti_fog_brush");
            _antiFogBrushOrigin = new Vector2(_antiFogBrush.Width, _antiFogBrush.Height) / 2f;
            _levelSelectIcon = _content.Load<Texture2D>("world_map/level_select_icon");
            _levelSelectIconHalfSize = new Vector2(_levelSelectIcon.Width, _levelSelectIcon.Height) / 2f;
            _levelSelectIconSelectedColor = Color.Yellow;
            _levelSelectIconDeselectedColor = Color.White * 0.8f;
            _levelSelectIconColor = _levelSelectIconDeselectedColor;
            _levelSelectTitleFont = _content.Load<SpriteFont>("world_map/level_select_title");
            _levelSelectDescriptionFont = _content.Load<SpriteFont>("world_map/level_select_description");
            _halfScreenSize = new Vector2(_spriteBatch.GraphicsDevice.Viewport.Width, _spriteBatch.GraphicsDevice.Viewport.Height) / 2f;
            _fogRT = new RenderTarget2D(_spriteBatch.GraphicsDevice, _spriteBatch.GraphicsDevice.Viewport.Width, _spriteBatch.GraphicsDevice.Viewport.Height);
            _antiFogRT = new RenderTarget2D(_spriteBatch.GraphicsDevice, _spriteBatch.GraphicsDevice.Viewport.Width, _spriteBatch.GraphicsDevice.Viewport.Height);
        }

        ~WorldMapScreen()
        {
            _content.Unload();
        }

        private LevelIconDefinition hitTestLevelIcons(Vector2 mouseWorld, float tolerance)
        {
            float shortest = 9999999f;
            LevelIconDefinition result = null;

            foreach (WorldMapDefinition worldMapDefinition in DataManager.worldMapManager.worldMapDefinitions)
            {
                foreach (LevelIconDefinition levelIconDefinition in worldMapDefinition.levelIconDefinitions)
                {
                    LevelIconState state = DataManager.worldMapManager.getLevelIconState(worldMapDefinition.uid, levelIconDefinition.uid);

                    if (state != null && state.discovered)
                    {
                        float distance = (mouseWorld - levelIconDefinition.position).Length();

                        if (distance <= tolerance)
                        {
                            shortest = distance;
                            result = levelIconDefinition;
                        }
                    }
                }
            }
            return result;
        }

        public void loadTextures()
        {
            foreach (WorldMapDefinition worldMapDefinition in DataManager.worldMapManager.worldMapDefinitions)
            {
                if (worldMapDefinition.texture == null)
                {
                    worldMapDefinition.texture = ResourceManager.getTexture(worldMapDefinition.textureUid);
                }
                foreach (LevelIconDefinition levelIconDefinition in worldMapDefinition.levelIconDefinitions)
                {
                    if (levelIconDefinition.finishedTexture == null)
                    {
                        levelIconDefinition.finishedTexture = ResourceManager.getTexture(levelIconDefinition.finishedTextureUid);
                    }
                    if (levelIconDefinition.unfinishedTexture == null)
                    {
                        levelIconDefinition.unfinishedTexture = ResourceManager.getTexture(levelIconDefinition.unfinishedTextureUid);
                    }
                }
            }
        }

        public override void update()
        {
            Vector2 mouseDelta;
            Vector2 mousePosition;
            Vector2 viewOffset = _halfScreenSize - _currentScreenCenter;
            bool wasLevelIconPreviouslySelected = _selectedLevelIcon != null;

            base.update();

            mouseDelta = new Vector2(_newMouseState.X - _oldMouseState.X, _newMouseState.Y - _oldMouseState.Y);
            mousePosition = new Vector2(_newMouseState.X, _newMouseState.Y) - viewOffset;
            _allowNewLevelSelection = mouseDelta.Length() > 2f;
            _levelSelectAngle = MathHelper.WrapAngle(_levelSelectAngle + 0.05f);

            if (_allowNewLevelSelection)
                _selectedLevelIcon = hitTestLevelIcons(mousePosition, 100f);

            if (_selectedLevelIcon == null && wasLevelIconPreviouslySelected)
            {
                _levelSelectPosition = new Vector2(_oldMouseState.X, _oldMouseState.Y);
                _levelSelectIconColor = _levelSelectIconDeselectedColor;
            }
            else if (_selectedLevelIcon != null)
            {
                _levelSelectPosition = _selectedLevelIcon.position + viewOffset;
                _levelSelectIconColor = _levelSelectIconSelectedColor;
                _targetScreenCenter += (_selectedLevelIcon.position - _currentScreenCenter) / 100f;
            }
            
            if (_selectedLevelIcon == null)
            {
                _levelSelectPosition += mouseDelta;
            }

            /*
            if (InputSystem.usingGamepad)
            {
                _targetScreenCenter += _newGamepadState.ThumbSticks.Left * 7 * new Vector2(1, -1);
                _targetScreenCenter += _newGamepadState.ThumbSticks.Right * 7 * new Vector2(1, -1);

                _scale = Math.Max(0.5f, _scale - _newGamepadState.Triggers.Left / 500f);
                _scale = Math.Min(1f, _scale + _newGamepadState.Triggers.Right / 500f);
            }*/

            if (_newKeyState.IsKeyDown(Keys.Left) || _newKeyState.IsKeyDown(Keys.A))
                _targetScreenCenter += new Vector2(-7, 0);
            if (_newKeyState.IsKeyDown(Keys.Right) || _newKeyState.IsKeyDown(Keys.D))
                _targetScreenCenter += new Vector2(7, 0);
            if (_newKeyState.IsKeyDown(Keys.Up) || _newKeyState.IsKeyDown(Keys.W))
                _targetScreenCenter += new Vector2(0, -7);
            if (_newKeyState.IsKeyDown(Keys.Down) || _newKeyState.IsKeyDown(Keys.S))
                _targetScreenCenter += new Vector2(0, 7);

            if (_newMouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
            {
                if (_selectedLevelIcon != null)
                {
                    _game.closeWorldMap();
                    _game.loadLevel(_selectedLevelIcon.levelUid);
                }
            }

            //_levelSelectPosition += mouseDelta;

            //_targetScreenCenter = Vector2.Max(_topLeft, Vector2.Min(_bottomRight, _targetScreenCenter));
            _currentScreenCenter += (_targetScreenCenter - _currentScreenCenter) / 11f;

            base.update();
        }

        // Pre process (occurs before Draw())
        public void preProcess()
        {
            Vector2 viewOffset = -_currentScreenCenter + _halfScreenSize;
            float antiFogTextureScale = _scale * 0.5f;

            // Draw anti fog points (points where the anti fog brush texture will be drawn
            _spriteBatch.GraphicsDevice.SetRenderTarget(_antiFogRT);
            _spriteBatch.GraphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            foreach (WorldMapDefinition worldMap in DataManager.worldMapManager.worldMapDefinitions)
            {
                foreach (LevelPathDefinition levelPath in worldMap.levelPathDefinitions)
                {
                    if (DataManager.worldMapManager.isLevelPathDiscovered(worldMap.uid, levelPath.id))
                    {
                        foreach (LevelPathKey key in levelPath.pathKeys)
                        {
                            float increment = 0.1f;
                            for (float i = increment; i < 1f; i += increment)
                            {
                                Vector2 point = Vector2.Zero;
                                StasisMathHelper.bezier(ref key.p0, ref key.p1, ref key.p2, ref key.p3, i, out point);
                                _spriteBatch.Draw(_antiFogBrush, viewOffset + point, _antiFogBrush.Bounds, Color.White, 0f, _antiFogBrushOrigin, antiFogTextureScale, SpriteEffects.None, 0f);
                            }
                        }
                    }
                }
                foreach (LevelIconDefinition levelIcon in worldMap.levelIconDefinitions)
                {
                    LevelIconState state = DataManager.worldMapManager.getLevelIconState(worldMap.uid, levelIcon.uid);

                    if (state != null && state.discovered)
                    {
                        _spriteBatch.Draw(_antiFogBrush, viewOffset + levelIcon.position, _antiFogBrush.Bounds, Color.White, 0f, _antiFogBrushOrigin, antiFogTextureScale, SpriteEffects.None, 0f);
                    }
                }
            }
            _spriteBatch.End();
            _spriteBatch.GraphicsDevice.SetRenderTarget(_fogRT);
            _spriteBatch.GraphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, _fogEffect);
            _spriteBatch.Draw(_antiFogRT, _antiFogRT.Bounds, Color.White);
            _spriteBatch.End();
            _spriteBatch.GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.GraphicsDevice.Clear(Color.Black);
        }

        public override void draw()
        {
            Vector2 viewOffset = -_currentScreenCenter + _halfScreenSize;

            foreach (WorldMapDefinition worldMap in DataManager.worldMapManager.worldMapDefinitions)
            {
                WorldMapState worldMapState = DataManager.worldMapManager.getWorldMapState(worldMap.uid);

                if (worldMapState.discovered)
                {
                    // World map texture
                    _spriteBatch.Draw(worldMap.texture, viewOffset, worldMap.texture.Bounds, Color.White, 0f, new Vector2(worldMap.texture.Width, worldMap.texture.Height) / 2f, _scale, SpriteEffects.None, 1f);

                    // Paths
                    foreach (LevelPathDefinition levelPath in worldMap.levelPathDefinitions)
                    {
                        if (DataManager.worldMapManager.isLevelPathDiscovered(worldMap.uid, levelPath.id))
                        {
                            foreach (LevelPathKey key in levelPath.pathKeys)
                            {
                                float increment = 0.001f;
                                for (float i = 0f; i < 1f; i += increment)
                                {
                                    Vector2 point = Vector2.Zero;
                                    StasisMathHelper.bezier(ref key.p0, ref key.p1, ref key.p2, ref key.p3, i, out point);
                                    _spriteBatch.Draw(_pathTexture, viewOffset + point, _pathTexture.Bounds, Color.Yellow, 0f, _pathTextureOrigin, _scale, SpriteEffects.None, 0.3f);
                                }
                            }
                        }
                    }

                    // Level icons
                    foreach (LevelIconDefinition levelIcon in worldMap.levelIconDefinitions)
                    {
                        LevelIconState state = DataManager.worldMapManager.getLevelIconState(worldMap.uid, levelIcon.uid);

                        if (state != null && state.discovered)
                        {
                            Texture2D texture = state.finished ? levelIcon.finishedTexture : levelIcon.unfinishedTexture;

                            _spriteBatch.Draw(texture, viewOffset + levelIcon.position, texture.Bounds, Color.White, 0f, new Vector2(texture.Width, texture.Height) / 2f, _scale, SpriteEffects.None, 0.2f);
                        }
                    }
                }
            }

            // Draw fog render target
            _spriteBatch.Draw(_fogRT, Vector2.Zero, _fogRT.Bounds, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0.1f);

            // Draw level select icon
            _spriteBatch.Draw(_levelSelectIcon, _levelSelectPosition, _levelSelectIcon.Bounds, _levelSelectIconColor, _levelSelectAngle, _levelSelectIconHalfSize, _scale, SpriteEffects.None, 0f);

            // Draw title text
            if (_selectedLevelIcon != null)
                _spriteBatch.DrawString(_levelSelectTitleFont, _selectedLevelIcon.title, new Vector2(32, 32), Color.White);

            // Draw description text
            if (_selectedLevelIcon != null)
            {
                string text = wrapText(_levelSelectDescriptionFont, _selectedLevelIcon.description, 420);
                _spriteBatch.DrawString(_levelSelectDescriptionFont, text, new Vector2(32, 96), Color.LightGray);
            }

            base.draw();
        }
    }
}
