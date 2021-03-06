﻿using Microsoft.Xna.Framework;

namespace StasisGame.Systems
{
    public enum SystemType
    {
        Render,
        Input,
        Physics,
        Fluid,
        Player,
        CharacterMovement,
        Camera,
        Tree,
        Event,
        Circuit,
        Screen,
        Equipment,
        Rope,
        Level,
        Explosion,
        AIBehavior,
        Animation,
        Dialogue
    };

    public interface ISystem
    {
        void update(GameTime gameTime);
        SystemType systemType { get; }
        int defaultPriority { get; }
        bool paused { get; set; }
        bool singleStep { get; set; }
    }
}
