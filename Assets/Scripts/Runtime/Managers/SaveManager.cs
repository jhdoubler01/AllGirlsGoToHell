using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using AGGtH.Runtime.Data.Save;
using AGGtH.Runtime.Managers;
using AGGtH.Runtime.Card;
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
            if (Instance == null)
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
            var pgd = GameManager.Instance.PersistentGameplayData;
            var cm = CardCollectionManager.Instance;

            var gameData = new GameData()
            {
                currentStageId = pgd.CurrentStageId,
                currentEncounterId = pgd.CurrentEncounterId,
                isFinalEncounter = pgd.IsFinalEncounter,

                currentHealth = (int)pgd.PlayerHealthData.CurrentHealth,
                maxHealth = (int)pgd.PlayerHealthData.MaxHealth,

                currentEnergy = pgd.CurrentEnergy,
                maxEnergy = pgd.MaxEnergy,

                currentGold = pgd.CurrentGold,

                currentCardsList = pgd.CurrentCardsList,
                drawPile = cm.DrawPile,
                discardPile = cm.DiscardPile,
                exhaustPile = cm.ExhaustPile,

                volume = AudioListener.volume,
                isFullScreen = Screen.fullScreen,
                resolution = Screen.currentResolution.ToString(),

                lastSaveTime = DateTime.Now,

                canUseCards = pgd.CanUseCards,
                canSelectCards = pgd.CanSelectCards,
                isRandomHand = pgd.IsRandomHand
            };

            string json = JsonUtility.ToJson(gameData, true);
            File.WriteAllText(saveFilePath, json);

            Debug.Log("Game saved to " + saveFilePath);
        }

        public bool LoadGame()
        {
            if (!File.Exists(saveFilePath))
            {
                Debug.LogWarning("Save file not found at " + saveFilePath);
                return false;
            }

            var json = File.ReadAllText(saveFilePath);
            var gameData = JsonUtility.FromJson<GameData>(json);

            var pgd = GameManager.Instance.PersistentGameplayData;
            var cm = CardCollectionManager.Instance;

            pgd.CurrentStageId = gameData.currentStageId;
            pgd.CurrentEncounterId = gameData.currentEncounterId;
            pgd.IsFinalEncounter = gameData.isFinalEncounter;

            pgd.PlayerHealthData.CurrentHealth = gameData.currentHealth;
            pgd.PlayerHealthData.MaxHealth = gameData.maxHealth;

            pgd.CurrentEnergy = gameData.currentEnergy;
            pgd.MaxEnergy = gameData.maxEnergy;

            pgd.CurrentGold = gameData.currentGold;

            pgd.CurrentCardsList.Clear();
            pgd.CurrentCardsList.AddRange(gameData.currentCardsList);

            cm.DrawPile.Clear();
            cm.DrawPile.AddRange(gameData.drawPile);

            cm.DiscardPile.Clear();
            cm.DiscardPile.AddRange(gameData.discardPile);

            cm.ExhaustPile.Clear();
            cm.ExhaustPile.AddRange(gameData.exhaustPile);

            // Settings
            AudioListener.volume = gameData.volume;
            Screen.fullScreen = gameData.isFullScreen;

            pgd.CanUseCards = gameData.canUseCards;
            pgd.CanSelectCards = gameData.canSelectCards;
            pgd.IsRandomHand = gameData.isRandomHand;

            Debug.Log("Game loaded from " + saveFilePath);
            return true;
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
