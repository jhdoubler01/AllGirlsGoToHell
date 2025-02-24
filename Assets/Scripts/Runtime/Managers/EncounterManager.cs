using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Characters;
using TMPro;

namespace AGGtH.Runtime.Managers
{
    public class EncounterManager : MonoBehaviour
    {
        private EncounterManager() { }
        public static EncounterManager Instance { get; private set; }

        protected GameManager GameManager => GameManager.Instance;

        //redo this part later,, its just for now -- these should be in collectionmanager and ui manager
        public List<CardData> drawPile;
        public List<CardData> playerHandData;
        public List<CardBase> playerHand;
        public List<CardData> discardPile;
        [SerializeField] private TMP_Text drawPileText;
        [SerializeField] private TMP_Text discardPileText;


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
        private void SetUpDrawPile()
        {
            drawPile = GameManager.GameplayData.StarterDeck.CardList;
        }
        private void RefillDrawPile()
        {
            if(drawPile.Count != 0) { return; }
            drawPile = discardPile;
            discardPile.Clear();
        }
        private void DrawCards(int numCards)
        {
            int rand = 0;
            for(var i = 0; i < numCards; i++)
            {
                if (drawPile.Count == 0) { RefillDrawPile(); }
                rand = UnityEngine.Random.Range(0,drawPile.Count-1);
                Debug.Log(rand);
                playerHandData.Add(drawPile[rand]);
                drawPile.RemoveAt(rand);
            }
            playerHand = GameManager.InitializePlayerHand(playerHandData);
        }
        private void Start()
        {
            StartCombat();
        }
        public void StartCombat()
        {
            SetUpDrawPile();
            DrawCards(GameManager.GameplayData.DrawCount);
        }
        #endregion
    }
}

