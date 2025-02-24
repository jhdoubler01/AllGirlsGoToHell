using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Characters;

namespace AGGtH.Runtime.Managers
{
    public class EncounterManager : MonoBehaviour
    {
        private EncounterManager() { }
        public static EncounterManager Instance { get; private set; }

        protected GameManager GameManager => GameManager.Instance;

        //redo this part later just for now
        public List<CardBase> playerDeck;

        [Header("References")]
        [SerializeField] private List<Transform> enemyPosList;

        #region Cache
        public List<EnemyBase> CurrentEnemiesList { get; private set; } 
        public PlayerBase Player { get; private set; }

        public Action OnPlayerTurnStarted;
        public Action OnEnemyTurnStarted;

        public List<Transform> EnemyPosList => enemyPosList;

        //public EnemyEncounter CurrentEncounter { get; private set; }
        #endregion
        private CombatStateType _currentCombatStateType;

        public CombatStateType CurrentCombatStateType
        {
            get => _currentCombatStateType;
            private set
            {

            }
        }

        #region Setup
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
                CurrentCombatStateType = CombatStateType.PrepareCombat;
            }
        }
        private void Start()
        {
            StartCombat();
        }
        public void StartCombat()
        {
            playerDeck = GameManager.InitializePlayerDeck();
        }
        #endregion
    }
}

