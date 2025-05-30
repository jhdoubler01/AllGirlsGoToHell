using UnityEngine;
using AGGtH.Runtime.Enums;

namespace AGGtH.Runtime.Data.Collection.RewardData
{
    [CreateAssetMenu(fileName = "IngredientRewardData", menuName = "Assets/Data/RewardData/IngredientReward")]
    public class IngredientRewardData : RewardDataBase
    {
        [SerializeField] private CocktailIngredientType ingredientType;
        public CocktailIngredientType IngredientType => ingredientType;
    }
}
