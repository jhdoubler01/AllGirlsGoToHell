using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using AGGtH.Runtime.Data.Save;
using AGGtH.Runtime.Managers;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Characters;
using AGGtH.Runtime.Data.Containers;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Extensions;


namespace AGGtH.Runtime.Managers
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }
        private string saveFilePath;

        void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                saveFilePath = Path.Combine(Application.persistentDataPath, "gameSave.json");
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SaveGame()
        {
            var gameData = new GameData()
            {
                currentStageId = GameManager.Instance.PersistentGameplayData.CurrentStageId,
                currentEncounterId = GameManager.Instance.PersistentGameplayData.CurrentEncounterId,
                isFinalEncounter = GameManager.Instance.PersistentGameplayData.IsFinalEncounter,
                currentHealth = GameManager.Instance.PersistentGameplayData.PlayerHealthData.CurrentHealth,
                maxHealth = GameManager.Instance.PersistentGameplayData.PlayerHealthData.MaxHealth,
                currentEnergy = GameManager.Instance.PersistentGameplayData.CurrentEnergy,
                maxEnergy = GameManager.Instance.PersistentGameplayData.MaxEnergy,
                currentGold = GameManager.Instance.PersistentGameplayData.CurrentGold,
                currentCardsList = GameManager.Instance.PersistentGameplayData.CurrentCardsList,
                drawPile = CardCollectionManager.Instance.DrawPile,
                discardPile = CardCollectionManager.Instance.DiscardPile,
                exhaustPile = CardCollectionManager.Instance.ExhaustPile,
                volume = AudioListener.volume,
                isFullScreen = Screen.fullScreen,
                resolution = Screen.currentResolution.ToString(),
                unlockedAchievements = new List<string>(), // Populate as needed
                lastSaveTime = DateTime.Now,
                currentEncounter = EncounterManager.Instance.CurrentEncounter,
                currentEnemiesList = EncounterManager.Instance.CurrentEnemiesList,
                canUseCards = GameManager.Instance.PersistentGameplayData.CanUseCards,
                canSelectCards = GameManager.Instance.PersistentGameplayData.CanSelectCards,
                isRandomHand = GameManager.Instance.PersistentGameplayData.IsRandomHand
            };

            string json = JsonUtility.ToJson(gameData, true);
            File.WriteAllText(saveFilePath, json);

            Debug.Log("Game saved to " + saveFilePath);

        }

        public bool LoadGame()
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                GameData gameData = JsonUtility.FromJson<GameData>(json);

                GameManager.Instance.PersistentGameplayData.CurrentStageId = gameData.currentStageId;
                GameManager.Instance.PersistentGameplayData.CurrentEncounterId = gameData.currentEncounterId;
                GameManager.Instance.PersistentGameplayData.IsFinalEncounter = gameData.isFinalEncounter;
                GameManager.Instance.PersistentGameplayData.PlayerHealthData.CurrentHealth = gameData.currentHealth;
                GameManager.Instance.PersistentGameplayData.PlayerHealthData.MaxHealth = gameData.maxHealth;
                GameManager.Instance.PersistentGameplayData.CurrentEnergy = gameData.currentEnergy;
                GameManager.Instance.PersistentGameplayData.MaxEnergy = gameData.maxEnergy;
                GameManager.Instance.PersistentGameplayData.CurrentGold = gameData.currentGold;
                GameManager.Instance.PersistentGameplayData.CurrentCardsList = gameData.currentCardsList;
                CardCollectionManager.Instance.DrawPile = gameData.drawPile;
                CardCollectionManager.Instance.DiscardPile = gameData.discardPile;
                CardCollectionManager.Instance.ExhaustPile = gameData.exhaustPile;
                AudioListener.volume = gameData.volume;
                Screen.fullScreen = gameData.isFullScreen;
                EncounterManager.Instance.CurrentEncounter = gameData.currentEncounter;
                EncounterManager.Instance.CurrentEnemiesList = gameData.currentEnemiesList;
                GameManager.Instance.PersistentGameplayData.CanUseCards = gameData.canUseCards;
                GameManager.Instance.PersistentGameplayData.CanSelectCards = gameData.canSelectCards;
                GameManager.Instance.PersistentGameplayData.IsRandomHand = gameData.isRandomHand;
                
                Debug.Log("Game loaded from " + saveFilePath);
                return true;
            }
            else
            {
                Debug.LogWarning("Save file not found at " + saveFilePath);
                return false;
            }
        }

        public void DeleteSave()
        {
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
                Debug.Log("Save file deleted at " + saveFilePath);
            }
            else
            {
                Debug.LogWarning("No save file to delete at " + saveFilePath);
            }
        }
    }
}
