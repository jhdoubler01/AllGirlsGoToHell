using UnityEngine;
using System.Collections.Generic;

namespace AGGtH.Runtime.Enums
{

    public enum CardLoveLanguageType
    {
        Neutral,
        Suggestive,
        Funny,
        Cutesy,
        Physical,
    }

    public class LoveLanguageComparison
    {
        private static readonly Dictionary<CardLoveLanguageType, Dictionary<CardLoveLanguageType, float>> _elementTable = new Dictionary<CardLoveLanguageType, Dictionary<CardLoveLanguageType, float>> {
        {
            CardLoveLanguageType.Suggestive, new Dictionary<CardLoveLanguageType, float>() {
                { CardLoveLanguageType.Suggestive, 1.5f }, // Strong against itself
                { CardLoveLanguageType.Funny, 0.5f }, // Weak against funny
            }
        },
        {
            CardLoveLanguageType.Funny, new Dictionary<CardLoveLanguageType, float>() {
                { CardLoveLanguageType.Funny, 1.5f }, // Strong against itself
                { CardLoveLanguageType.Suggestive, 0.5f }, // Weak against suggestive
            }
        },
        {
            CardLoveLanguageType.Cutesy, new Dictionary<CardLoveLanguageType, float>() {
                { CardLoveLanguageType.Cutesy, 1.5f }, // Strong against itself
                { CardLoveLanguageType.Physical, 0.5f }, // Weak against physical
            }
        },
        {
            CardLoveLanguageType.Physical, new Dictionary<CardLoveLanguageType, float>() {
                { CardLoveLanguageType.Physical, 1.5f }, // Strong against itself
                { CardLoveLanguageType.Cutesy, 0.5f }, // Weak against cutesy
            }
        }
    };

        public static float CompareLoveLanguages(CardLoveLanguageType attackerLanguage, CardLoveLanguageType defenderLanguage)
        {
            if (_elementTable.ContainsKey(attackerLanguage) && _elementTable[attackerLanguage].ContainsKey(defenderLanguage))
            {
                return _elementTable[attackerLanguage][defenderLanguage];
            }
            else
            {
                return 1f; // Neutral (no damage modifier)
            }
        }
    }
}



