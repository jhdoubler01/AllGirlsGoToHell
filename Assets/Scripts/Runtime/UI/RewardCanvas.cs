using UnityEngine;
using System;
using System.Collections.Generic;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Data.Collection;
using AGGtH.Runtime.Data.Containers;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Extensions;
using AGGtH.Runtime.UI.Reward;

namespace AGGtH.Runtime.UI
{
    public class RewardCanvas : CanvasBase
    {
        [Header("References")]
        [SerializeField] private RewardContainerData rewardContainerData;
        [SerializeField] private Transform rewardRoot;
        [SerializeField] private RewardContainer rewardContainerPrefab;
        [SerializeField] private Transform rewardPanelRoot;


        [Header("Choice")]
        [SerializeField] Transform choiceCardSpawnRoot;
        [SerializeField] CardBase choiceCardPrefab;
        [SerializeField] Transform choicePanel;

        private readonly List<RewardContainer> _currentRewardsList = new List<RewardContainer>();
        private readonly List<CardBase> _spawnedChoiceList = new List<CardBase>();
        private readonly List<CardData> _cardRewardList = new List<CardData>();

        public Transform ChoicePanel => choicePanel;

        #region Public Methods
        public void SetUpRewardData(RewardContainerData encounterRewardData)
        {
            rewardContainerData = encounterRewardData;
        }
        public void PrepareCanvas()
        {
            rewardPanelRoot.gameObject.SetActive(true);
        }
        public void BuildReward(RewardType rewardType)
        {
            if(rewardContainerData == null) { GameManager.SetUpRewards(); }
            var rewardClone = Instantiate(rewardContainerPrefab, rewardRoot);
            _currentRewardsList.Add(rewardClone);

            switch (rewardType)
            {
                case RewardType.Chips:
                    var rewardChips = rewardContainerData.GetRandomChipsReward(out var chipsRewardData);
                    rewardClone.BuildReward(chipsRewardData.RewardSprite, $"Get {rewardChips} chips", chipsRewardData.RewardType);
                    rewardClone.RewardButton.onClick.AddListener(() => GetChipsReward(rewardClone, rewardChips));
                    break;
                case RewardType.NewMove:
                    var rewardCardList = rewardContainerData.GetRandomCardRewardList(out var cardRewardData);
                    _cardRewardList.Clear();
                    foreach (var cardData in rewardCardList)
                        _cardRewardList.Add(cardData);
                    rewardClone.BuildReward(cardRewardData.RewardSprite, cardRewardData.RewardDescription, cardRewardData.RewardType);
                    rewardClone.RewardButton.onClick.AddListener(() => GetCardReward(rewardClone, 3));
                    break;
                case RewardType.Relic:
                    break;
                case RewardType.Ingredient:

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rewardType), rewardType, null);
            }
        }
        public override void ResetCanvas()
        {
            ResetRewards();
            ResetCanvas();
        }
        #endregion

        #region Private Methods
        private void ResetRewards()
        {
            foreach(var rewardContainer in _currentRewardsList)
            {
                Destroy(rewardContainer.gameObject);
            }
            _currentRewardsList?.Clear();
        }
        private void ResetChoice()
        {
            foreach(var choice in _spawnedChoiceList)
            {
                Destroy(choice.gameObject);
            }
            _spawnedChoiceList?.Clear();
            ChoicePanel.gameObject.SetActive(false);
            rewardRoot.gameObject.SetActive(true);
        }
        private void GetChipsReward(RewardContainer rewardContainer, int amount)
        {
            GameManager.PersistentGameplayData.CurrentChips += amount;
            _currentRewardsList.Remove(rewardContainer);
            //update chips amt text
            Destroy(rewardContainer.gameObject);
        }
        private void GetCardReward(RewardContainer rewardContainer, int amount = 3)
        {
            rewardRoot.gameObject.SetActive(false);
            ChoicePanel.gameObject.SetActive(true);

            for(int i=0; i < amount; i++)
            {
                Transform spawnTransform = choiceCardSpawnRoot;

                CardData reward = _cardRewardList.RandomItem();

                CardBase choice = Instantiate(GameManager.SelectCardPrefab(reward), spawnTransform, false);

                choice.BuildReward(reward);
                choice.OnCardChose += ResetChoice;

                _cardRewardList.Remove(reward);
                _spawnedChoiceList.Add(choice);
                _currentRewardsList.Remove(rewardContainer);
            }

            Destroy(rewardContainer.gameObject);
        }
        #endregion
    }
}
