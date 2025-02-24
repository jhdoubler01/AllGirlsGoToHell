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

        //all of these card methods and stuff should be in cardCollectionManager im just being lazy rn
        private void SetUpDrawPile()
        {
            //drawPile = GameManager.GameplayData.StarterDeck.CardList;
            foreach(CardData card in GameManager.GameplayData.StarterDeck.CardList)
            {
                drawPile.Add(card);
            }
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
        public void MoveCardToDiscardPile(CardBase card)
        {
            CardData cardData = card.CardData;
            discardPile.Add(cardData);
            playerHand.Remove(card);
            Destroy(card.gameObject);
        }

        private void Start()
        {
            StartCombat();
            StartPlayerTurn();
        }
        public void StartCombat()
        {
            SetUpDrawPile();
        }
        private void StartPlayerTurn()
        {
            DrawCards(GameManager.GameplayData.DrawCount);
            GameManager.ResetPlayerEnergy();
        }
        public void EndPlayerTurn()
        {
            if(playerHand.Count > 0)
            {
                int num = playerHand.Count;
                for(var i=num-1; i>=0;i--)
                {
                    MoveCardToDiscardPile(playerHand[i]);
                }
            }
            StartCoroutine(EnemyTurnDelay());
        }
        private void EndEnemyTurn()
        {

        }

        private IEnumerator EnemyTurnDelay()
        {
            Debug.Log("enemy turn- pretend smth happens here");
            yield return new WaitForSeconds(3);
            StartPlayerTurn();
        }
        #endregion
    }
}

