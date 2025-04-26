using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using System.Collections;
using AGGtH.Runtime.Managers;
using AGGtH.Runtime.Extensions;
using AGGtH.Runtime.Enums;

namespace AGGtH.Runtime.Card
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card/CardData")]

    public class CardData : ScriptableObject
    {
        [Header("Card Profile")]
        [SerializeField] private string id;
        [SerializeField] private string cardName;
        [SerializeField] private int energyCost;
        [SerializeField] private Sprite cardSprite;
        [SerializeField] private RarityType rarity;
        [SerializeField] private CardLoveLanguageType cardLoveLanguageType;

        [Header("Action Settings")]
        [SerializeField] private bool usableWithoutTarget;
        [SerializeField] private bool exhaustAfterPlay;
        [SerializeField] private List<CardActionData> cardActionDataList = new List<CardActionData>();

        [Header("Description")]
        [SerializeField] private List<CardDescriptionData> cardDescriptionDataList;
        [SerializeField] private List<SpecialKeywords> specialKeywordsList;

        [Header("Dialogue")]
        [SerializeField] private List<string> dialogueOptions = new List<string>();

        #region Cache
        public string Id { get => id; set => id = value; }
        public string CardName { get => cardName; set => cardName = value; }
        public int EnergyCost { get => energyCost; set => energyCost = value; }
        public Sprite CardSprite { get => cardSprite; set => cardSprite = value; }
        public bool UsableWithoutTarget { get => usableWithoutTarget; set => usableWithoutTarget = value; }
        public bool ExhaustAfterPlay { get => exhaustAfterPlay; set => exhaustAfterPlay = value; }
        public List<CardActionData> CardActionDataList { get => cardActionDataList; set => cardActionDataList = value; }
        public RarityType Rarity { get => rarity; set => rarity = value; }
        public CardLoveLanguageType CardLoveLanguageType { get => cardLoveLanguageType; set => cardLoveLanguageType = value; }
        public CardActionType CardActionType { get; set; }
        public string MyDescription { get; set; }
        public List<string> DialogueOptions { get => dialogueOptions; set => dialogueOptions = value; }

        #endregion

        #region Methods
        public void UpdateDescription()
        {
            var str = new StringBuilder();

            foreach (var descriptionData in cardDescriptionDataList)
            {
                str.Append(descriptionData.UseModifier
                    ? descriptionData.GetModifiedValue(this)
                    : descriptionData.GetDescription());
            }

            MyDescription = str.ToString();
        }
        #endregion
    }

    [Serializable]
    public class CardActionData
    {
        [SerializeField] private ActionTargetType actionTargetType;
        [SerializeField] private CardActionType cardActionType;
        [SerializeField] private BuffType buffType;
        [SerializeField] private DebuffType debuffType;
        [SerializeField] private float actionValue;
        [SerializeField] private float actionDelay;

        public ActionTargetType ActionTargetType { get => actionTargetType; set => actionTargetType = value; }
        public CardActionType CardActionType { get => cardActionType; set => cardActionType = value; }
        public BuffType BuffType { get => buffType; set => buffType = value; }
        public DebuffType DebuffType { get => debuffType; set => debuffType = value; }
        public float ActionValue { get => actionValue; set => actionValue = value; }
        public float ActionDelay { get => actionDelay; set => actionDelay = value; }
    }
    [Serializable]
    public class CardDescriptionData
    {
        [Header("Text")]
        [SerializeField] private string descriptionText;
        [SerializeField] private bool enableOverrideColor;
        [SerializeField] private Color overrideColor = Color.black;

        [Header("Modifer")]
        [SerializeField] private bool useModifier;
        [SerializeField] private int modifiedActionValueIndex;
        [SerializeField] private StatusType modiferStats;
        [SerializeField] private bool usePrefixOnModifiedValue;
        [SerializeField] private string modifiedValuePrefix = "*";
        [SerializeField] private bool overrideColorOnValueScaled;

        public string DescriptionText => descriptionText;
        public bool EnableOverrideColor => enableOverrideColor;
        public Color OverrideColor => overrideColor;
        public bool UseModifier => useModifier;
        public int ModifiedActionValueIndex => modifiedActionValueIndex;
        public StatusType ModiferStats => modiferStats;
        public bool UsePrefixOnModifiedValue => usePrefixOnModifiedValue;
        public string ModifiedValuePrefix => modifiedValuePrefix;
        public bool OverrideColorOnValueScaled => overrideColorOnValueScaled;

        private EncounterManager EncounterManager => EncounterManager.Instance;

        public string GetDescription()
        {
            var str = new StringBuilder();

            str.Append(DescriptionText);

            if (EnableOverrideColor && !string.IsNullOrEmpty(str.ToString()))
                str.Replace(str.ToString(), ColorExtensions.ColorString(str.ToString(), OverrideColor));

            return str.ToString();
        }

        public string GetModifiedValue(CardData cardData)
        {
            if (cardData.CardActionDataList.Count <= 0) return "";

            if (ModifiedActionValueIndex >= cardData.CardActionDataList.Count)
                modifiedActionValueIndex = cardData.CardActionDataList.Count - 1;

            if (ModifiedActionValueIndex < 0)
                modifiedActionValueIndex = 0;

            var str = new StringBuilder();
            var value = cardData.CardActionDataList[ModifiedActionValueIndex].ActionValue;
            var modifer = 0;
            if (EncounterManager)
            {
                var player = EncounterManager.Player;

                if (player)
                {
                    modifer = player.CharacterStats.StatusDict[ModiferStats].StatusValue;
                    value += modifer;

                    if (modifer != 0)
                    {
                        if (usePrefixOnModifiedValue)
                            str.Append(modifiedValuePrefix);
                    }
                }
            }

            str.Append(value);

            if (EnableOverrideColor)
            {
                if (OverrideColorOnValueScaled)
                {
                    if (modifer != 0)
                        str.Replace(str.ToString(), ColorExtensions.ColorString(str.ToString(), OverrideColor));
                }
                else
                {
                    str.Replace(str.ToString(), ColorExtensions.ColorString(str.ToString(), OverrideColor));
                }

            }

            return str.ToString();
        }

        #region Editor
#if UNITY_EDITOR
        
        public string GetDescriptionEditor()
        {
            var str = new StringBuilder();
            
            str.Append(DescriptionText);
            
            return str.ToString();
        }

        public string GetModifiedValueEditor(CardData cardData)
        {
            if (cardData.CardActionDataList.Count <= 0) return "";
            
            if (ModifiedActionValueIndex>=cardData.CardActionDataList.Count)
                modifiedActionValueIndex = cardData.CardActionDataList.Count - 1;

            if (ModifiedActionValueIndex<0)
                modifiedActionValueIndex = 0;
            
            var str = new StringBuilder();
            var value = cardData.CardActionDataList[ModifiedActionValueIndex].ActionValue;
            if (EncounterManager)
            {
                var player = EncounterManager.Player;
                if (player)
                {
                    var modifer =player.CharacterStats.StatusDict[ModiferStats].StatusValue;
                    value += modifer;
                
                    if (modifer!= 0)
                        str.Append("*");
                }
            }
           
            str.Append(value);
          
            return str.ToString();
        }
        
        public void EditDescriptionText(string newText) => descriptionText = newText;
        public void EditEnableOverrideColor(bool newStatus) => enableOverrideColor = newStatus;
        public void EditOverrideColor(Color newColor) => overrideColor = newColor;
        public void EditUseModifier(bool newStatus) => useModifier = newStatus;
        public void EditModifiedActionValueIndex(int newIndex) => modifiedActionValueIndex = newIndex;
        public void EditModiferStats(StatusType newStatusType) => modiferStats = newStatusType;
        public void EditUsePrefixOnModifiedValues(bool newStatus) => usePrefixOnModifiedValue = newStatus;
        public void EditPrefixOnModifiedValues(string newText) => modifiedValuePrefix = newText;
        public void EditOverrideColorOnValueScaled(bool newStatus) => overrideColorOnValueScaled = newStatus;

#endif
        #endregion
    }
}


