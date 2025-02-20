﻿using Engine.models;
namespace Engine.factories;

internal class WorldFactory
{
    internal World CreateWorld()
    {
        World newWorld = new World();
        
        newWorld.AddLocation(0, -1, "Home", "Home, sweet home.", "/WPFUI;component/images/locations/Home.png");
        newWorld.AddLocation(0, 0, "Spider Forest", "Beware of the creepy crawlies.", "/WPFUI;component/images/locations/SpiderForest.png");
        return newWorld;
    }
}