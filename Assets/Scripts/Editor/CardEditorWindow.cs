using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Enums;

namespace AGGtH.Editor
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
            cardActionType = (CardActionType)EditorGUILayout.EnumPopup("Card Action Type", cardActionType);

            GUILayout.Label("Card Action Type", EditorStyles.boldLabel);

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
            }

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
            newCard.CardActionType = cardActionType;

            AssetDatabase.CreateAsset(newCard, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newCard;

            Debug.Log("Created Card: " + id);
        }
    }
}
