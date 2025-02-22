using UnityEngine;
using UnityEditor;

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

        [MenuItem("Window/Card Creator")]
        public static void ShowWindow()
        {
            GetWindow<Editor>("Card Creator");
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
            if (GUILayout.Button("Create Card"))
            {
                CreateCard();
            }
        }

        private void CreateCard()
        {
            CardData newCard = ScriptableObject.CreateInstance<CardData>();
            newCard.Id = id;
            newCard.CardName = cardName;
            newCard.EnergyCost = energyCost;
            newCard.CardSprite = cardSprite;
            newCard.CardLoveLanguageType = cardLoveLanguageType;
            newCard.RarityType = rarityType;
            newCard.UsableWithoutTarget = usableWithoutTarget;
            newCard.ExhaustAfterPlay = exhaustAfterPlay;
            newCard.CardActionType = cardActionType;

            string path = "Assets/Data/CardData/" + id + ".asset";
            AssetDatabase.CreateAsset(newCard, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newCard;

            Debug.Log("Created Card: " + id);
        }
    }
}
