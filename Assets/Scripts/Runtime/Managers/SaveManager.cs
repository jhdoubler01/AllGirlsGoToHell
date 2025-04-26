using UnityEngine;
using System.IO;
using System.Collections.Generic;
usinh UnityEditor.SceneManagement;

namespace AGGtH.Runtime
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
            var gameData = new GameData
            {
                // Populate game data here
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

                // Load game data here
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
