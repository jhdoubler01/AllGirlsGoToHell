using UnityEngine;
using System.Collections.Generic;
using AGGtH.Runtime.Card;

namespace AGGtH.Runtime.Data.Collection.RewardData
{
    [CreateAssetMenu(fileName = "Card Reward Data", menuName = "AGGtH/Rewards/CardRW", order = 0)]

    public class CardRewardData : RewardDataBase
    {
        [SerializeField] private List<CardData> rewardCardList;
        public List<CardData> RewardCardList => rewardCardList;
    }
}
