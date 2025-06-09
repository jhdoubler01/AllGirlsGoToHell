using UnityEngine;

namespace AGGtH.Runtime.Data.Collection.RewardData
{
    [CreateAssetMenu(fileName="Chips Reward Data", menuName = "AGGtH/Rewards/ChipsRW", order=0)]
    public class ChipsRewardData : RewardDataBase
    {
        [SerializeField] private int minChips;
        [SerializeField] private int maxChips;
        public int MinChips => minChips;
        public int MaxChips => maxChips;
    }
}
