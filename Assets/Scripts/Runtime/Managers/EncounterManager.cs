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

        protected UIManager UIManager => UIManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected CardCollectionManager CardCollectionManager => CardCollectionManager.Instance;

        //redo this part later,, its just for now -- these should be in collectionmanager and ui manager

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
        private CombatStateType currentCombatStateType;

        public CombatStateType CurrentCombatStateType
        {
            get => currentCombatStateType;
            private set
            {
                //ExecuteCombatState(value);
                //currentCombatStateType = value;
            }
        }


        #endregion


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
            StartPlayerTurn();
        }
        public void StartCombat()
        {
            //SetUpDrawPile();
        }
        private void StartPlayerTurn()
        {
            //DrawCards(GameManager.GameplayData.DrawCount);
            GameManager.ResetPlayerEnergy();
        }
        public void EndPlayerTurn()
        {
            //if(playerHand.Count > 0)
            //{
            //    int num = playerHand.Count;
            //    for(var i=num-1; i>=0;i--)
            //    {
            //        MoveCardToDiscardPile(playerHand[i]);
            //    }
            //}
            //StartCoroutine(EnemyTurnDelay());
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

        #region Runtime
        void FixedUpdate()
        {

        }
        #endregion
    }
}

