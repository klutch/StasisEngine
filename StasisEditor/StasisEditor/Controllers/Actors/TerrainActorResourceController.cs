﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore.Models;

namespace StasisEditor.Controllers.Actors
{
    public class TerrainActorResourceController : ActorResourceController, ILinkedPointSubControllable
    {
        private TerrainActorResource _terrainActorResource;
        private LinkedPointSubController _headLinkedPointController;

        public TerrainActorResourceController(ILevelController levelController, ActorResource actorResource = null)
            : base(levelController)
        {
            // Default actor
            if (actorResource == null)
                actorResource = new TerrainActorResource();

            // Set actor resources
            _actor = actorResource;
            _terrainActorResource = actorResource as TerrainActorResource;

            // Initialize points
            List<Vector2> actorResourcePoints = new List<Vector2>();
            if (_terrainActorResource.points.Count == 0)
            {
                actorResourcePoints.Add(_levelController.getWorldMouse() - new Vector2(1f, 0));
                actorResourcePoints.Add(_levelController.getWorldMouse() + new Vector2(1f, 0));
            }
            else
                actorResourcePoints = _terrainActorResource.points;

            // Create linked point controllers
            _headLinkedPointController = new LinkedPointSubController(actorResourcePoints[0], this);
            LinkedPointSubController current = _headLinkedPointController;
            for (int i = 1; i < actorResourcePoints.Count; i++)
            {
                current.next = new LinkedPointSubController(actorResourcePoints[i], this);
                current.next.previous = current;
                current = current.next;
            }
        }

        #region Linked Point Sub Controller Interface

        // setNewLinkedPointSubControllerHead
        public void setNewLinkedPointSubControllerHead(LinkedPointSubController newHead)
        {
            _headLinkedPointController = newHead;
        }

        #endregion

        #region Input

        // hitTest
        public override bool hitTest(Vector2 worldMouse)
        {
            // Hit test linked point sub controllers
            LinkedPointSubController current = _headLinkedPointController;
            while (current != null)
            {
                if (current.hitTest(worldMouse))
                {
                    _levelController.selectSubController(current);
                    return true;
                }

                current = current.next;
            }

            // Hit test link lines
            current = _headLinkedPointController;
            while (current.next != null)
            {
                if (current.linkHitTest(worldMouse))
                {
                    _levelController.selectSubController(current);
                    _levelController.selectSubController(current.next);
                    return true;
                }

                current = current.next;
            }

            return false;
        }

        // globalCheckKeys
        public override void globalCheckKey()
        {
            ////////////////////////////
            // Test for delete
            LinkedPointSubController current = _headLinkedPointController;
            bool anyLinkSelected = false;
            while (current.next != null)
            {
                if (current.selected)
                {
                    anyLinkSelected = true;
                    break;
                }
                current = current.next;
            }
            if (anyLinkSelected && Input.newKey.IsKeyDown(Keys.Delete) && Input.oldKey.IsKeyUp(Keys.Delete))
            {
                if (System.Windows.Forms.MessageBox.Show("Are you sure you want to delete this controller and all the points? \n (Use the minus key [-] to remove single points)", "Remove entire controller?", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    delete();
            }

            //////////////////////////////////////
            // Test for point insertion / removal
            bool plusPressed = Input.newKey.IsKeyDown(Keys.OemPlus) && Input.oldKey.IsKeyUp(Keys.OemPlus);
            bool minusPressed = Input.newKey.IsKeyDown(Keys.OemMinus) && Input.oldKey.IsKeyUp(Keys.OemMinus);
            Vector2 worldMouse = _levelController.getWorldMouse();

            // Only test for insertions if there are no links selected -- insertion while selected is handled in the sub controller's
            // checkXNAKeys method since this methods requires a hit test
            if (!anyLinkSelected && plusPressed)
            {
                // Hit test link line
                current = _headLinkedPointController;
                while (current.next != null)
                {
                    if (current.linkHitTest(worldMouse))
                    {
                        current.insertPoint(worldMouse);
                        return;
                    }
                    current = current.next;
                }
            }

            if (minusPressed)
            {
                // Hit test points
                current = _headLinkedPointController;
                while (current != null)
                {
                    if (current.hitTest(worldMouse))
                    {
                        current.removePoint();
                        return;
                    }

                    current = current.next;
                }
            }
        }

        #endregion

        #region Actor Resource Controller Methods

        // selectAllSubControllers
        public override void selectAllSubControllers()
        {
            LinkedPointSubController current = _headLinkedPointController;
            while (current != null)
            {
                _levelController.selectSubController(current);
                current = current.next;
            }
        }

        // deselectAllSubControllers
        public override void deselectAllSubControllers()
        {
            LinkedPointSubController current = _headLinkedPointController;
            while (current != null)
            {
                _levelController.deselectSubController(current);
                current = current.next;
            }
        }

        // draw
        public override void draw()
        {
            // Draw links
            LinkedPointSubController current = _headLinkedPointController;
            while (current.next != null)
            {
                _renderer.drawLine(current.position, current.next.position, Color.Orange);
                current = current.next;
            }

            // Draw points
            current = _headLinkedPointController;
            while (current != null)
            {
                _renderer.drawPoint(current.position, Color.Yellow);
                current = current.next;
            }
        }

        // clone
        public override ActorResourceController clone()
        {
            return new TerrainActorResourceController(_levelController, _actor.clone());
        }

        #endregion

    }
}