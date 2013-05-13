﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StasisGame.Managers;

namespace StasisGame.UI
{
    public class LoadGameScreen : Screen
    {
        private LoderGame _game;
        private Texture2D _background;
        private Texture2D _logo;
        private Texture2D _savesContainer;
        private SpriteFont _santaBarbaraNormal;
        private SpriteFont _arial;
        private ContentManager _content;

        public LoadGameScreen(LoderGame game)
            : base(ScreenType.LoadGameMenu)
        {
            _game = game;
            _content = new ContentManager(game.Services);
            _content.RootDirectory = "Content";
            _background = _content.Load<Texture2D>("main_menu/bg");
            _logo = _content.Load<Texture2D>("main_menu/logo");
            _savesContainer = _content.Load<Texture2D>("load_game_menu/saves_container");
            _santaBarbaraNormal = _content.Load<SpriteFont>("santa_barbara_normal");
            _arial = _content.Load<SpriteFont>("arial");

            createUIComponents();
        }

        ~LoadGameScreen()
        {
            _content.Unload();
        }

        private void createUIComponents()
        {
            List<XElement> playerSaves = DataManager.loadPlayerSaves();
            List<TextButton> saveButtons = new List<TextButton>();
            Vector2 initialPosition = new Vector2(-200, 300);

            foreach (XElement playerSave in playerSaves)
            {
                int slot = int.Parse(playerSave.Attribute("slot").Value);
                string text = slot.ToString() + " - " + playerSave.Attribute("name").Value;
                TextButton button = new TextButton(
                    _game.spriteBatch,
                    _arial,
                    Color.White,
                    (int)initialPosition.X,
                    (int)(initialPosition.Y) + saveButtons.Count * 24,
                    text,
                    UIComponentAlignment.TopCenter,
                    (component) =>
                    {
                        _game.closeLoadGameMenu();
                        _game.loadGame(slot);
                    });
                saveButtons.Add(button);
                _UIComponents.Add(button);
            }
        }

        public override void update()
        {
            _oldGamepadState = _newGamepadState;
            _oldKeyState = _newKeyState;
            _oldMouseState = _newMouseState;

            _newGamepadState = GamePad.GetState(PlayerIndex.One);
            _newKeyState = Keyboard.GetState();
            _newMouseState = Mouse.GetState();

            // Mouse input
            for (int i = 0; i < _UIComponents.Count; i++)
            {
                IUIComponent component = _UIComponents[i];

                if (component.selectable)
                {
                    ISelectableUIComponent selectableComponent = component as ISelectableUIComponent;
                    if (selectableComponent.hitTest(new Vector2(_newMouseState.X, _newMouseState.Y)))
                    {
                        if (_oldMouseState.X - _newMouseState.X != 0 || _oldMouseState.Y - _newMouseState.Y != 0)
                            select(selectableComponent);

                        if (_oldMouseState.LeftButton == ButtonState.Released && _newMouseState.LeftButton == ButtonState.Pressed)
                            selectableComponent.activate();
                    }
                }
            }

            // Gamepad input
            if (_newGamepadState.IsConnected)
            {
                bool movingUp = (_oldGamepadState.ThumbSticks.Left.Y < 0.25f && _newGamepadState.ThumbSticks.Left.Y > 0.25f) ||
                    (_oldGamepadState.DPad.Up == ButtonState.Released && _newGamepadState.DPad.Up == ButtonState.Pressed);
                bool movingDown = (_oldGamepadState.ThumbSticks.Left.Y > -0.25f && _newGamepadState.ThumbSticks.Left.Y < -0.25f) ||
                    (_oldGamepadState.DPad.Down == ButtonState.Released && _newGamepadState.DPad.Down == ButtonState.Pressed);
                bool activate = _oldGamepadState.Buttons.A == ButtonState.Released && _newGamepadState.Buttons.A == ButtonState.Pressed;

                if (movingUp)
                    selectPreviousComponent();
                else if (movingDown)
                    selectNextComponent();

                if (activate && _selectedComponent != null)
                {
                    _selectedComponent.activate();
                }
            }

            base.update();
        }

        public override void draw()
        {
            float scale = (float)_game.GraphicsDevice.Viewport.Height / (float)_background.Height;
            _game.spriteBatch.Draw(_background, Vector2.Zero, _background.Bounds, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            _game.spriteBatch.Draw(_logo, new Vector2((int)(_game.GraphicsDevice.Viewport.Width / 2f), 100f), _logo.Bounds, Color.White, 0, new Vector2(_logo.Width, _logo.Height) / 2, 0.75f, SpriteEffects.None, 0);
            _game.spriteBatch.Draw(_savesContainer, new Vector2((int)(_game.GraphicsDevice.Viewport.Width / 2f), 150f), _savesContainer.Bounds, Color.White, 0f, new Vector2((int)(_savesContainer.Width / 2f), 0), 1f, SpriteEffects.None, 0f);

            base.draw();
        }
    }
}