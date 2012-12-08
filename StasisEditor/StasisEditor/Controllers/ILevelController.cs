﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using StasisCore.Models;
using StasisEditor.Controllers.Actors;
using StasisEditor.Views;

namespace StasisEditor.Controllers
{
    public interface ILevelController : IController
    {
        void handleXNADraw();
        void update();
        void resizeGraphicsDevice(int width, int height);

        void createNewLevel();
        void closeLevel();
        LevelResource getLevel();

        void setShapeRenderer(ShapeRenderer shapeRenderer);
        ShapeRenderer getShapeRenderer();

        void createActorControllerFromToolbar(string buttonName);
        void addActorController(ActorResourceController actorController);
        void removeActorController(ActorResourceController actorController);
        void selectSubController(ActorSubController subController);
        void deselectSubController(ActorSubController subController);

        List<ActorResourceController> getActorControllers();

        void mouseMove(MouseEventArgs e);
        void mouseLeave();
        void mouseEnter();
        void mouseDown(MouseEventArgs e);
        void mouseUp(MouseEventArgs e);

        Vector2 getWorldOffset();
        Vector2 getWorldMouse();
        bool getIsMouseOverView();
        float getScale();
    }
}
