using UnityEngine;
using AGGtH.Runtime.Enums;

namespace AGGtH.Runtime.Data.Collection.RewardData
{
    public class RewardDataBase : ScriptableObject
    {
        [SerializeField] private RewardType rewardType;
        [SerializeField] private Sprite rewardSprite;
        [TextArea] [SerializeField] private string rewardDescription;
        public RewardType RewardType => rewardType;
        public Sprite RewardSprite => rewardSprite;
        public string RewardDescription => rewardDescription;
    }
}
