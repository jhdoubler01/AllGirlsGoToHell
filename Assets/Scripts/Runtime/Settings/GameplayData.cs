using System.Collections.Generic;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Characters;
using UnityEngine;

namespace AGGtH.Runtime.Settings
{
    [CreateAssetMenu(fileName = "Gameplay Data", menuName = "Assets/Data/Settings/GameplayData", order = 0)]
    public class GameplayData : ScriptableObject
    {
        [Header("Gameplay Settings")]
        [SerializeField] private int drawCount = 4;
        [SerializeField] private int maxEnergy = 3;

        [Header("Decks")]
        [SerializeField] private DeckData initalDeck;
        [SerializeField] private int maxCardOnHand;

        [Header("Card Settings")]
        [SerializeField] private List<CardData> allCardsList;
        [SerializeField] private CardBase cardPrefab;


        //[Header("Customization Settings")]
        //[SerializeField] private string defaultName = "Nue";
        //[SerializeField] private bool useStageSystem;

        [Header("Modifiers")]
        [SerializeField] private bool isRandomHand = false;
        [SerializeField] private int randomCardCount;

        #region Encapsulation
        public int DrawCount => drawCount;
        public int MaxEnergy => maxEnergy;
        public bool IsRandomHand => isRandomHand;
        public DeckData StarterDeck => initalDeck;
        public int RandomCardCount => randomCardCount;
        public int MaxCardOnHand => maxCardOnHand;
        public List<CardData> AllCardsList => allCardsList;
        public CardBase CardPrefab => cardPrefab;
        //public string DefaultName => defaultName;
        //public bool UseStageSystem => useStageSystem;
        #endregion
    }
}