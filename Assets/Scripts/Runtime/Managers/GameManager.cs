using UnityEngine;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Settings;
using AGGtH.Runtime.Data.Containers;
using System.Collections.Generic;
using AGGtH.Runtime.Extensions;
using AGGtH.Runtime.EnemyBehavior;
using UnityEditor;
using System;
using AGGtH.Runtime.Enums;

namespace AGGtH.Runtime.Managers
{
    [DefaultExecutionOrder(-10)]
    public class GameManager : MonoBehaviour
    {
        public GameManager() { }
        public static GameManager Instance { get; private set; }

        [Header("Settings")]
        [SerializeField] private GameplayData gameplayData;
        [SerializeField] private EncounterData encounterData;
        //[SerializeField] private SceneData sceneData;

        #region Cache
        //public SceneData SceneData => sceneData;
        public EncounterData EncounterData => encounterData;
        public GameplayData GameplayData => gameplayData;
        public PersistentGameplayData PersistentGameplayData { get; private set; }
        protected UIManager UIManager => UIManager.Instance;
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
                transform.parent = null;
                Instance = this;
                DontDestroyOnLoad(gameObject);
                CardActionProcessor.Initialize();
                EnemyActionProcessor.Initialize();
                InitGameplayData();
                InitPlayerHand();
                SetUpRewards();
            }
        }
        #endregion

        #region Public Methods
        public void InitGameplayData()
        {
            PersistentGameplayData = new PersistentGameplayData(gameplayData);
            Debug.Log("init gameplay data");

        }
        // spawn card object in game and set its stats to a target CardData
        public CardBase BuildAndGetCard(CardData targetData, RectTransform parent)
        {
            CardBase clone = Instantiate(SelectCardPrefab(targetData), parent, false);
            clone.SetCard(targetData);
            return clone;
        }
        // spawn cards in players hand
        public void InitPlayerHand()
        {
            PersistentGameplayData.CurrentCardsList.Clear();

            if (PersistentGameplayData.IsRandomHand)
            {
                for(int i = 0; i < GameplayData.RandomCardCount; i++)
                {
                    PersistentGameplayData.CurrentCardsList.Add(GameplayData.AllCardsList.RandomItem());
                }

            }
            else
            {
                foreach (var cardData in GameplayData.InitialDeck.CardList)
                {
                    PersistentGameplayData.CurrentCardsList.Add(cardData);
                }
            }
        }
        public void NextEncounter()
        {
            PersistentGameplayData.CurrentEncounterId++;
            //if (PersistentGameplayData.CurrentEncounterId >= EncounterData.EnemyEncounterList[PersistentGameplayData.CurrentStageId].EnemyEncounterList.Count)
            //{
            //    PersistentGameplayData.CurrentEncounterId = Random.Range(0,
            //        EncounterData.EnemyEncounterList[PersistentGameplayData.CurrentStageId].EnemyEncounterList.Count);
            //}
            SetUpRewards();
        }
        public CardBase SelectCardPrefab(CardData data)
        {
            string type = Enum.GetName(typeof(CardLoveLanguageType), data.CardLoveLanguageType);
            string action = Enum.GetName(typeof(CardActionType), data.CardActionDataList[0].CardActionType);
            CardBase c = Resources.Load<CardBase>($"Prefabs/Cards/{type}_{action}_card");
            return c;
        }
        public void SetUpRewards()
        {
            UIManager.RewardCanvas.SetUpRewardData(EncounterData.EncounterRewards);
        }
        #endregion
        #region Private Methods

        #endregion

    }
}
