using System;
using System.Collections.Generic;
using AGGtH.Runtime.Characters;
using AGGtH.Runtime.Card;
using UnityEngine;

namespace AGGtH.Runtime.Settings
{
    [Serializable]
    public class PersistentGameplayData
    {
        private readonly GameplayData _gameplayData;
        [SerializeField] private int currentGold;
        [SerializeField] private int drawCount;
        [SerializeField] private int maxEnergy;
        [SerializeField] private int currentEnergy;
        [SerializeField] private bool canUseCards;
        [SerializeField] private bool canSelectCards;
        [SerializeField] private bool isRandomHand;
        [SerializeField] private PlayerBase player;
        [SerializeField] private int currentStageId;
        [SerializeField] private int currentEncounterId;
        [SerializeField] private bool isFinalEncounter;
        [SerializeField] private List<CardData> currentCardsList;
        [SerializeField] private PlayerHealthData playerHealthData;

        public PersistentGameplayData(GameplayData gameplayData)
        {
            _gameplayData = gameplayData;

            InitData();
        }

        public void SetPlayerHealthData(string id, int newCurrentHealth, int newMaxHealth)
        {
            var newData = new PlayerHealthData();
            newData.CharacterId = id;
            newData.CurrentHealth = newCurrentHealth;
            newData.MaxHealth = newMaxHealth;
            PlayerHealthData = newData;

        }
        private void InitData()
        {
            DrawCount = _gameplayData.DrawCount;
            MaxEnergy = _gameplayData.MaxEnergy;
            CurrentEnergy = MaxEnergy;
            CanUseCards = true;
            CanSelectCards = true;
            IsRandomHand = _gameplayData.IsRandomHand;
            Player = _gameplayData.Player;
            CurrentEncounterId = 0;
            CurrentStageId = 0;
            CurrentGold = 0;
            CurrentCardsList = new List<CardData>();
            IsFinalEncounter = false;
        }

        #region Encapsulation

        public int DrawCount
        {
            get => drawCount;
            set => drawCount = value;
        }

        public int MaxEnergy
        {
            get => maxEnergy;
            set => maxEnergy = value;
        }

        public int CurrentEnergy
        {
            get => currentEnergy;
            set => currentEnergy = value;
        }

        public bool CanUseCards
        {
            get => canUseCards;
            set => canUseCards = value;
        }

        public bool CanSelectCards
        {
            get => canSelectCards;
            set => canSelectCards = value;
        }

        public bool IsRandomHand
        {
            get => isRandomHand;
            set => isRandomHand = value;
        }

        public PlayerBase Player
        {
            get => player;
            set => player = value;
        }

        public int CurrentStageId
        {
            get => currentStageId;
            set => currentStageId = value;
        }

        public int CurrentEncounterId
        {
            get => currentEncounterId;
            set => currentEncounterId = value;
        }

        public bool IsFinalEncounter
        {
            get => isFinalEncounter;
            set => isFinalEncounter = value;
        }

        public List<CardData> CurrentCardsList
        {
            get => currentCardsList;
            set => currentCardsList = value;
        }

        public PlayerHealthData PlayerHealthData
        {
            get => playerHealthData;
            set => playerHealthData = value;
        }
        public int CurrentGold
        {
            get => currentGold;
            set => currentGold = value;
        }

        #endregion
    }
}
