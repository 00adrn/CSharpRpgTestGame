﻿using System.ComponentModel;
using System.Linq;
using Engine.EventArgs;

namespace Engine.viewModels;
using Engine.models;
using Engine.factories;

public class GameSession : BaseNotification
{
    public event EventHandler<GameMessageEventArgs> OnMessageRaised; 
    private Location _currentLocation;
    public Player currentPlayer { get; set; }
    public World currentWorld { get; set; }
    private Monster _currentMonster;
    public bool hasMonster => currentMonster != null;
    public Monster currentMonster
    {
        get { return _currentMonster; }
        set
        {
            _currentMonster = value;
            
            OnPropertyChanged(nameof(currentMonster));
            OnPropertyChanged(nameof(hasMonster));

            if (currentMonster != null)
            {
                RaiseMessage("");
                RaiseMessage($"You see a {currentMonster.name} here!");
            }
        }
    }

    public Location currentLocation
    {
        get { return _currentLocation; }
        set
        {
            _currentLocation = value;
            OnPropertyChanged(nameof(currentLocation));
            OnPropertyChanged(nameof(hasLocationNorth));
            OnPropertyChanged(nameof(hasLocationWest));
            OnPropertyChanged(nameof(hasLocationEast));
            OnPropertyChanged(nameof(hasLocationSouth));

            GivePlayerQuestsAtLocation();
            GetMonsterAtLocation();
        }
    }

    public bool hasLocationNorth
    {
        get
        {
            return currentWorld.LocationAt(currentLocation.xCoordinate, currentLocation.yCoordinate + 1) != null;
        }
    }
    public bool hasLocationWest
    {
        get
        {
            return currentWorld.LocationAt(currentLocation.xCoordinate - 1, currentLocation.yCoordinate) != null;
        }
    }
    public bool hasLocationEast
    {
        get
        {
            return currentWorld.LocationAt(currentLocation.xCoordinate + 1, currentLocation.yCoordinate) != null;
        }
    }
    public bool hasLocationSouth
    {
        get
        {
            return currentWorld.LocationAt(currentLocation.xCoordinate, currentLocation.yCoordinate - 1) != null;
        }
    }
    public GameSession()
    {
        currentPlayer = new Player {name="Altria", characterClass = "Saber", hitPoints = 100, experiencePoints = 0, gold = 10};
        
        currentWorld = WorldFactory.CreateWorld();
        
        currentLocation = currentWorld.LocationAt(0, -1);
        
    }
    public void MoveDirection(string direction)
    {
        switch (direction)
        {
            case "north":
                if (hasLocationNorth)
                {
                    currentLocation =
                        currentWorld.LocationAt(currentLocation.xCoordinate, currentLocation.yCoordinate + 1);
                }
                break;
            case "east":
                if (hasLocationEast)
                {
                    currentLocation =
                        currentWorld.LocationAt(currentLocation.xCoordinate + 1, currentLocation.yCoordinate);
                }
                break;
            case "south":
                if (hasLocationSouth)
                {
                    currentLocation =
                        currentWorld.LocationAt(currentLocation.xCoordinate, currentLocation.yCoordinate - 1);
                }
                break;
            case "west":
                if (hasLocationWest)
                {
                    currentLocation =
                        currentWorld.LocationAt(currentLocation.xCoordinate - 1, currentLocation.yCoordinate);
                }

                break;
        }
    }
    
    private void GivePlayerQuestsAtLocation()
    {
        foreach (Quest quest in currentLocation.QuestsAvailableHere)
        {
            if (!currentPlayer.quests.Any(q => q.playerQuest.questID == quest.questID))
            {
                currentPlayer.quests.Add(new QuestStatus(quest));
            }
        }
    }
    
    private void GetMonsterAtLocation()
    {
        currentMonster = currentLocation.GetMonster();
    }

    private void RaiseMessage(string message)
    {
        OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
    }
}