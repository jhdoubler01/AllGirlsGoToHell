using UnityEngine;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Settings;
using System.Collections.Generic;


namespace AGGtH.Runtime.Managers
{
    public class GameManager : MonoBehaviour
    {
        public GameManager() { }
        public static GameManager Instance { get; private set; }

        [SerializeField] private CardBase cardPrefab;
        [SerializeField] private Transform cardParentTransform;
        [SerializeField] private CardCollectionManager cardCollectionManager;
        [SerializeField] private GameplayData gameplayData;

        public GameplayData GameplayData => gameplayData;

        public CardBase BuildAndGetCard(CardData targetData, Transform parent)
        {
            var clone = Instantiate(GameplayData.CardPrefab, parent);
            clone.SetCard(targetData);
            return clone;
        }
        public List<CardBase> InitializePlayerDeck()
        {
            List<CardBase> deck = new List<CardBase>();
            foreach(CardData cardData in GameplayData.StarterDeck.CardList)
            {
                deck.Add(BuildAndGetCard(cardData, cardParentTransform));
            }
            return deck;
        }

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }
        }

    }
}
