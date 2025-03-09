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

        protected UIManager UIManager => UIManager.Instance;

        private int playerCurrentEnergy;
        public int PlayerCurrentEnergy => playerCurrentEnergy;

        public GameplayData GameplayData => gameplayData;
        public PersistentGameplayData PersistentGameplayData { get; private set; }

        // spawn card object in game and set its stats to a target CardData
        public CardBase BuildAndGetCard(CardData targetData, Transform parent)
        {
            var clone = Instantiate(GameplayData.CardPrefab, parent);
            clone.SetCard(targetData);
            return clone;
        }
        // spawn cards in players hand
        public List<CardBase> InitializePlayerHand(List<CardData> handData)
        {
            List<CardBase> deck = new List<CardBase>();
            foreach(CardData cardData in handData)
            {
                deck.Add(BuildAndGetCard(cardData, cardParentTransform));
            }
            return deck;
        }
        public void ResetPlayerEnergy()
        {
            playerCurrentEnergy = GameplayData.MaxEnergy;
            UIManager.SetEnergyBoxText(playerCurrentEnergy);
        }
        //takes how much energy a card costs and returns true if the player has enough energy to play it
        public bool IsEnoughEnergyToPlayCard(int energyToPlayCard)
        {
            if(energyToPlayCard <= playerCurrentEnergy) { return true; }
            return false;
        }
        //subtracts card cost from player total energy
        public void SubtractFromCurrentEnergy(int amtToSubtract)
        {
            playerCurrentEnergy -= amtToSubtract;
            Debug.Log("Current Energy: " + playerCurrentEnergy);
            if(playerCurrentEnergy < 0) { Debug.LogError("Player energy cannot be negative"); }
            UIManager.SetEnergyBoxText(playerCurrentEnergy);

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
                transform.parent = null;
                Instance = this;
            }
        }

    }
}
