using UnityEngine;
using System;
using System.Collections.Generic;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Characters;
using AGGtH.Runtime.Data.Containers;

namespace AGGtH.Runtime.Data.Save
{
    public class GameData
    {
        //player progress
        public int currentStageId;
        public int currentEncounterId;
        public bool isFinalEncounter;

        //stats
        public int currentHealth;
        public int maxHealth;
        public int currentEnergy;
        public int maxEnergy;
        public int currentGold;

        //inventory and cards
        public List<CardData> currentCardsList;
        public List<CardData> drawPile;
        public List<CardData> discardPile;
        public List<CardData> exhaustPile;

        //settings
        public float volume;
        public bool isFullScreen;
        public string resolution;
        public List<string> unlockedAchievements;
        public DateTime lastSaveTime;

        //encounter data
        public EnemyEncounter currentEnemyEncounter;
        public List<EnemyBase> currentEnemiesList;
        public bool canUseCards;
        public bool canSelectCards;
        public bool isRandomHand;

    }
}
