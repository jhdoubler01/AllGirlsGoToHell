using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Enums;
using System.IO;

namespace AGGtH.Editor.CardEditor
{
    public class CardEditorWindow : EditorWindow
    {
        private string id;
        private string cardName;
        private int energyCost;
        private Sprite cardSprite;
        private CardLoveLanguageType cardLoveLanguageType;
        private RarityType rarityType;
        private bool usableWithoutTarget;
        private bool exhaustAfterPlay;
        private CardActionType cardActionType;
        private List<CardActionData> cardActionDataList = new List<CardActionData>();

        private const string cardDataPath = "Assets/Data/CardData/";

        [MenuItem("Window/Card Creator")]
        public static void ShowWindow()
        {
            GetWindow<CardEditorWindow>("Card Creator");
        }

        private void OnGUI()
        {
            GUILayout.Label("Create a New Card", EditorStyles.boldLabel);

            id = EditorGUILayout.TextField("ID", id);
            cardName = EditorGUILayout.TextField("Card Name", cardName);
            energyCost = EditorGUILayout.IntField("Energy Cost", energyCost);
            cardSprite = (Sprite)EditorGUILayout.ObjectField("Card Sprite", cardSprite, typeof(Sprite), false);
            cardLoveLanguageType = (CardLoveLanguageType)EditorGUILayout.EnumPopup("Card Love Language Type", cardLoveLanguageType);
            rarityType = (RarityType)EditorGUILayout.EnumPopup("Rarity Type", rarityType);
            usableWithoutTarget = EditorGUILayout.Toggle("Usable Without Target", usableWithoutTarget);
            exhaustAfterPlay = EditorGUILayout.Toggle("Exhaust After Play", exhaustAfterPlay);
            //cardActionType = (CardActionType)EditorGUILayout.EnumPopup("Card Action Type", cardActionType);

            GUILayout.Label("Card Actions", EditorStyles.boldLabel);

            if (GUILayout.Button("Add Action"))
            {
                cardActionDataList.Add(new CardActionData());
            }

            for (int i = 0; i < cardActionDataList.Count; i++)
            {
                GUILayout.BeginVertical("box");
                GUILayout.Label($"Action {i + 1}", EditorStyles.miniBoldLabel);

                cardActionDataList[i].CardActionType = (CardActionType)EditorGUILayout.EnumPopup("Action Type", cardActionDataList[i].CardActionType);
                cardActionDataList[i].ActionTargetType = (ActionTargetType)EditorGUILayout.EnumPopup("Target", cardActionDataList[i].ActionTargetType); // 🔹 Added target selection

                switch (cardActionDataList[i].CardActionType)
                {
                    cardActionDataList[i] = new CardActionData();
                }

                GUILayout.BeginHorizontal();
                cardActionDataList[i] = new CardActionData()
                {
                    CardActionType = (CardActionType)EditorGUILayout.EnumPopup("Action Type", cardActionDataList[i].CardActionType),
                    ActionValue  = EditorGUILayout.IntField("Action Value", cardActionDataList[i].ActionValue),
                };

                if (GUILayout.Button("Remove"))
                {
                    cardActionDataList.RemoveAt(i);
                }
                GUILayout.EndVertical();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Create Card"))
            {
                CreateCard();
            }
        }

        private void CreateCard()
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                Debug.LogError("Card ID cannot be empty.");
                return;
            }

            if (!Directory.Exists(cardDataPath))
            {
                Directory.CreateDirectory(cardDataPath);
                AssetDatabase.Refresh();
            }

            string path = "Assets/Data/CardData/" + id + ".asset";

            if (AssetDatabase.LoadAssetAtPath<CardData>(path) != null)
            {
                Debug.LogError("Card with this ID already exists.");
                return;
            }

            CardData newCard = ScriptableObject.CreateInstance<CardData>();
            newCard.Id = id;
            newCard.CardName = cardName;
            newCard.EnergyCost = energyCost;
            newCard.CardSprite = cardSprite;
            newCard.CardLoveLanguageType = cardLoveLanguageType;
            newCard.Rarity = rarityType;
            newCard.UsableWithoutTarget = usableWithoutTarget;
            newCard.ExhaustAfterPlay = exhaustAfterPlay;
            //newCard.CardActionType = cardActionType;
            newCard.CardActionDataList = new List<CardActionData>(cardActionDataList);

            AssetDatabase.CreateAsset(newCard, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newCard;

            Debug.Log("Created Card: " + id);
        }
    }
}
