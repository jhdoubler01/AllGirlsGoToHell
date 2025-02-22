using UnityEngine;
using System.Collections.Generic;

namespace AGGtH.Runtime.Card
{
    [CreateAssetMenu(fileName = "Deck Data", menuName = "Assets/Data/DeckData", order = 1)]

    public class DeckData : ScriptableObject
    {
        [SerializeField] private string deckId;
        [SerializeField] private string deckName;

        [SerializeField] private List<CardData> cardList;

        public string DeckId => deckId;
        public string DeckName => deckName;
        public List<CardData> CardList => cardList;
    }
}
