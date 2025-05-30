using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using AGGtH.Runtime.Data.Collection;
using AGGtH.Runtime.Data.Collection.RewardData;
using AGGtH.Runtime.Extensions;
using AGGtH.Runtime.Card;

namespace AGGtH.Runtime.Data.Containers
{
    [CreateAssetMenu(fileName = "RewardContainerData", menuName = "Scriptable Objects/RewardContainerData")]
    public class RewardContainerData : ScriptableObject
    {
        [SerializeField] private List<CardRewardData> cardRewardDataList;
        [SerializeField] private List<ChipsRewardData> chipsRewardDataList;
        [SerializeField] private List<IngredientRewardData> ingredientRewardDataList;
        public List<CardRewardData> CardRewardDataList => cardRewardDataList;
        public List<ChipsRewardData> ChipsRewardDataList => chipsRewardDataList;
        public List<IngredientRewardData> IngredientRewardDataList => ingredientRewardDataList;

        public List<CardData> GetRandomCardRewardList(out CardRewardData rewardData)
        {
            rewardData = CardRewardDataList.RandomItem();

            List<CardData> cardList = new List<CardData>();

            foreach (var cardData in rewardData.RewardCardList)
                cardList.Add(cardData);

            return cardList;
        }
        public int GetRandomChipsReward(out ChipsRewardData rewardData)
        {
            rewardData = ChipsRewardDataList.RandomItem();
            var value = Random.Range(rewardData.MinChips, rewardData.MaxChips);

            return value;
        }
        public IngredientRewardData GetRandomIngredientReward(out IngredientRewardData rewardData)
        {
            rewardData = IngredientRewardDataList.RandomItem();
            return rewardData;
        }
    }
}
