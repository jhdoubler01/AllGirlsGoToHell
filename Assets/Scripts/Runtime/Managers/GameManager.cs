using UnityEngine;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Settings;
using System.Collections.Generic;
using AGGtH.Runtime.Extensions;

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


        #region Public Methods
        public void InitGameplayData()
        {
            PersistentGameplayData = new PersistentGameplayData(gameplayData);
        }
        // spawn card object in game and set its stats to a target CardData
        public CardBase BuildAndGetCard(CardData targetData, Transform parent)
        {
            var clone = Instantiate(GameplayData.CardPrefab, parent);
            clone.SetCard(targetData);
            return clone;
        }
        // spawn cards in players hand
        public void InitPlayerHand()
        {
            PersistentGameplayData.CurrentCardsList.Clear();

            if (PersistentGameplayData.IsRandomHand)
            {
                for(int i = 0; i < GameplayData.RandomCardCount; i++)
                {
                    PersistentGameplayData.CurrentCardsList.Add(GameplayData.AllCardsList.RandomItem());
                }

            }
            else
            {
                foreach (var cardData in GameplayData.InitialDeck.CardList)
                {
                    PersistentGameplayData.CurrentCardsList.Add(cardData);
                }
            }
        }
        public void ResetPlayerEnergy()
        {
            int oldEnergy = playerCurrentEnergy;
            playerCurrentEnergy = GameplayData.MaxEnergy;
            UIManager.Instance.UpdateEnergyDisplay(oldEnergy, playerCurrentEnergy, GameplayData.EnergyUpdateDuration);
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
            int oldEnergy = playerCurrentEnergy;
            playerCurrentEnergy -= amtToSubtract;
            Debug.Log("Current Energy: " + playerCurrentEnergy);
            if (playerCurrentEnergy < 0) 
            { 
                Debug.LogError("Not enough energy to play card");
                playerCurrentEnergy = 0; 
            }
            UIManager.UpdateEnergyDisplay(oldEnergy, playerCurrentEnergy, GameplayData.EnergyUpdateDuration);

        }
        public void GainEnergy(int amtToGain)
        {
            playerCurrentEnergy += amtToGain;
            Debug.Log("Current Energy: " + playerCurrentEnergy);
            if (playerCurrentEnergy > GameplayData.MaxEnergy) { playerCurrentEnergy = GameplayData.MaxEnergy; }
            UIManager.SetEnergyBoxText(playerCurrentEnergy);
        }

        #endregion
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
                DontDestroyOnLoad(gameObject);
                InitGameplayData();
                InitPlayerHand();
            }
        }

    }
}
