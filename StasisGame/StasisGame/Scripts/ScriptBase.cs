﻿using System;
using System.Collections.Generic;
using StasisGame.Managers;
using StasisGame.Systems;

namespace StasisGame.Scripts
{
    abstract public class ScriptBase
    {
        protected SystemManager _systemManager;
        protected EntityManager _entityManager;

        public ScriptBase(SystemManager systemManager, EntityManager entityManager)
        {
            _systemManager = systemManager;
            _entityManager = entityManager;
        }

        virtual public bool doAction(string action)
        {
            return false;
        }

        virtual public void registerGoals(LevelSystem levelSystem)
        {
        }

        virtual public void onGoalComplete(LevelSystem levelSystem, Goal goal)
        {
        }

        virtual public void onLevelStart()
        {
        }

        virtual public void onLevelEnd()
        {
        }

        virtual public void onSwitchLevel(string from, string to)
        {
        }

        virtual public void onReturnToWorldMap(LevelSystem levelSystem)
        {
        }
    }
}
