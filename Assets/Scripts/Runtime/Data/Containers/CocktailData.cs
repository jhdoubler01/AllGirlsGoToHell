using UnityEngine;
using AGGtH.Runtime.Enums;
using System.Collections.Generic;

namespace AGGtH.Runtime.Data.Containers
{
    [CreateAssetMenu(fileName = "Cocktail Data", menuName = "AGGtH/Items/CocktailData")]
    public class CocktailData : ScriptableObject
    {
        [Header("Base")]
        [SerializeField] private string cocktailName;
        [SerializeField] private string description;

        [Header("Crafting Variables")]
        [SerializeField] private Dictionary<CocktailIngredientType,int> ingredientDict = new Dictionary<CocktailIngredientType,int>();
    }
}
