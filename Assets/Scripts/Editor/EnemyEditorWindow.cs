using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using AGGtH.Runtime.Characters.Enemy;
using AGGtH.Runtime.Enums;

namespace AGGtH.Editor
{
    public class EnemyEditorWindow : EditorWindow
    {
        private string id;
        private string enemyName;
        private int healthAmt;
        private Sprite enemySprite;
        private EnemyLoveLanguageType enemyLoveLanguageType;


        [MenuItem("Window/Enemy Creator")]
        public static void ShowWindow()
        {
            GetWindow<EnemyEditorWindow>("Enemy Creator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Create a New Enemy", EditorStyles.boldLabel);

            id = EditorGUILayout.TextField("ID", id);
            enemyName = EditorGUILayout.TextField("Enemy Name", enemyName);
            healthAmt = EditorGUILayout.IntField("Health", healthAmt);
            enemySprite = (Sprite)EditorGUILayout.ObjectField("Enemy Sprite", enemySprite, typeof(Sprite), false);
            enemyLoveLanguageType = (EnemyLoveLanguageType)EditorGUILayout.EnumPopup("Enemy Love Language Type", enemyLoveLanguageType);
 
/*             GUILayout.Label("Card Action Type", EditorStyles.boldLabel);

            if (GUILayout.Button("Add Action"))
            {
                cardActionDataList.Add(new CardActionData());
            }

            for (int i = 0; i < cardActionDataList.Count; i++)
            {
                if (cardActionDataList[i] == null)
                {
                    cardActionDataList[i] = new CardActionData();
                }

                GUILayout.BeginHorizontal();
                cardActionDataList[i] = new CardActionData()
                {
                    CardActionType = (CardActionType)EditorGUILayout.EnumPopup("Action Type", cardActionDataList[i].CardActionType),
                    DamageAmt = EditorGUILayout.IntField("Damage Amount", cardActionDataList[i].DamageAmt),
                    HealAmt = EditorGUILayout.IntField("Heal Amount", cardActionDataList[i].HealAmt),
                    BlockAmt = EditorGUILayout.IntField("Block", cardActionDataList[i].BlockAmt),
                };

                if (GUILayout.Button("Remove"))
                {
                    cardActionDataList.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
            } */

            if (GUILayout.Button("Create Enemy"))
            {
                CreateEnemy();
            }
        }

        private void CreateEnemy()
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Debug.LogError("Card ID cannot be empty.");
                return;
            }

            string path = "Assets/Data/EnemyData/" + id + ".asset";

            if (AssetDatabase.LoadAssetAtPath<EnemyData>(path) != null)
            {
                Debug.LogError("Enemy with this ID already exists.");
                return;
            }

            EnemyData newEnemy = ScriptableObject.CreateInstance<EnemyData>();
            newEnemy.Id = id;
            newEnemy.EnemyName = enemyName;
            newEnemy.HealthAmt = healthAmt;
            newEnemy.EnemySprite = enemySprite;
            newEnemy.EnemyLoveLanguageType = enemyLoveLanguageType;

            AssetDatabase.CreateAsset(newEnemy, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newEnemy;

            Debug.Log("Created Enemy: " + id);
        }
    }
}
