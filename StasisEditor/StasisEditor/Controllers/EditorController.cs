﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using StasisCore.Controllers;
using StasisCore.Resources;
using StasisEditor.Views;
using StasisEditor.Views.Controls;

namespace StasisEditor.Controllers
{
    public class EditorController : Controller
    {
        private MaterialController _materialController;
        private LevelController _levelController;
        private BlueprintController _blueprintController;

        private EditorView _editorView;
        private ActorToolbar _actorToolbar;

        private float _scale = 35f;

        public EditorView view { get { return _editorView; } }

        public EditorController(EditorView view)
        {
            // Initialize core resource controller
            ResourceController.initialize();

            _editorView = view;
            view.setController(this);

            // Initialize core resource controller
            //ResourceController.initialize(XNAResources.graphicsDevice);

            // Create editor view
            //_editorView = new EditorView();
            //_editorView.setController(this);

            // Create material controller
            _materialController = new MaterialController(this, _editorView.getMaterialView());

            // Create level controller
            _levelController = new LevelController(this, _editorView.getLevelView());

            // Create blueprint controller
            _blueprintController = new BlueprintController(this, _editorView.getBlueprintView());

        }

        // getScale
        public float getScale() { return _scale; }

        // setScale
        public void setScale(float value) { _scale = value; }

        // setActorToolbarEnabled
        public void setActorToolbarEnabled(bool status)
        {
            _actorToolbar.Enabled = status;
        }

        /*
        // enableLevelXNAInput
        public void enableLevelXNAInput(bool status)
        {
            _levelController.enableXNAInput(status);
        }

        // enableLevelXNADrawing
        public void enableLevelXNADrawing(bool status)
        {
            _levelController.enableXNADrawing(status);
        }
        */

        // createNewLevel
        public void createNewLevel()
        {
            _levelController.createNewLevel();

            // Modify menu items
            _editorView.enableNewLevel(false);
            _editorView.enableCloseLevel(true);
            _editorView.enableLoadLevel(false);
            _editorView.enableSaveLevel(true);

            // Add level settings
            _editorView.addLevelSettings(_levelController.level);

            // Create actor toolbar
            _actorToolbar = new ActorToolbar();
            _actorToolbar.setController(_levelController);
            _editorView.addActorToolbar(_actorToolbar);
        }

        // closeLevel
        public void closeLevel()
        {
            _levelController.closeLevel();

            // Modify menu
            _editorView.enableNewLevel(true);
            _editorView.enableCloseLevel(false);
            _editorView.enableLoadLevel(true);
            _editorView.enableSaveLevel(false);

            // Remove level settings
            _editorView.removeLevelSettings();

            // Remove actor toolbar
            _editorView.removeActorToolbar(_actorToolbar);
        }
        /*
        // handleXNADraw
        public void handleXNADraw()
        {
            // Level controller draw
            _levelController.handleXNADraw();

            // Blueprint controller draw
            _blueprintController.handleXNADraw();
        }
        */
        /*
        // update
        public void update()
        {
            // Handle mouse wheel event through XNA
            //if (Input.deltaScrollValue != 0)
            //    _scale += (float)Input.deltaScrollValue * 0.01f;
            
            // Level controller update
            _//levelController.update();

            // Blueprint controller update
            //_blueprintController.update();
        }
        */
        // exit
        public void exit()
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
