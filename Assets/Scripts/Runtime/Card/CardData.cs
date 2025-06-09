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
    [CreateAssetMenu(fileName = "New Card", menuName = "AGGtH/Card/CardData")]

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
        [SerializeField] private List<SpecialKeywords> specialKeywordsList;
        [SerializeField] private Color positiveModifierColor = new Color(0.36675f, 0.54f, 0.243f);
        [SerializeField] private Color negativeModifierColor = new Color(0.76f, 0.266f, 0.3071668f);

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

        private EncounterManager EncounterManager => EncounterManager.Instance;


        #endregion

        #region Methods
        public void UpdateDescription()
        {
            for(int i = 0; i< cardActionDataList.Count; i++)
            {
                if(i == 0)
                {
                    MyDescription = GetActionDescription(cardActionDataList[i]);
                }
                else
                {
                    MyDescription = MyDescription + " " + GetActionDescription(cardActionDataList[i]);
                }
            }

        }
        //public string GetModifiedValue(CardData cardData)
        //{
        //    if (cardData.CardActionDataList.Count <= 0) return "";

        //    if (ModifiedActionValueIndex >= cardData.CardActionDataList.Count)
        //        modifiedActionValueIndex = cardData.CardActionDataList.Count - 1;

        //    if (ModifiedActionValueIndex < 0)
        //        modifiedActionValueIndex = 0;

        //    var str = new StringBuilder();
        //    float value = cardData.CardActionDataList[ModifiedActionValueIndex].ActionValue;
        //    float modifer = 0;
        //    if (EncounterManager)
        //    {
        //        var player = EncounterManager.Player;

        //        if (player)
        //        {
        //            modifer = player.CharacterStats.StatusDict[ModiferStats].StatusValue;
        //            value += modifer;

        //            if (modifer != 0)
        //            {
        //                if (usePrefixOnModifiedValue)
        //                    str.Append(modifiedValuePrefix);
        //            }
        //        }
        //    }

        //    str.Append(value * 2);

        //    if (EnableOverrideColor)
        //    {
        //        if (OverrideColorOnValueScaled)
        //        {
        //            if (modifer != 0)
        //                str.Replace(str.ToString(), ColorExtensions.ColorString(str.ToString(), OverrideColor));
        //        }
        //        else
        //        {
        //            str.Replace(str.ToString(), ColorExtensions.ColorString(str.ToString(), OverrideColor));
        //        }

        //    }
        //    Debug.Log("value is " + str.ToString());
        //    return str.ToString();
        //}
        public string GetActionDescription(CardActionData actionData)
        {
            string MyDescription = string.Empty;
            CardActionType actionType = actionData.CardActionType;
            float value = actionData.ActionValue;
            string valueStr = string.Empty;
            StatusType statusEffect = actionData.StatusType;
            ActionTargetType targetType = actionData.ActionTargetType;
            if (EncounterManager)
            {
                var player = EncounterManager.Player;
                if (player)
                {
                    float modifier = CardActionModifierStats.GetCardActionModifier(actionType, player);
                    value += modifier;
                    value *= 2;
                    if (modifier > 0)
                    {
                        valueStr = ColorExtensions.ColorString(value.ToString(), positiveModifierColor);
                    }
                    else if (modifier < 0)
                    {
                        valueStr = ColorExtensions.ColorString(value.ToString(), negativeModifierColor);
                    }
                    else
                    {
                        valueStr = value.ToString();
                    }
                    //if modifier is less than zero text should be red, if it's more than zero it should be green
                }
            }
            switch (actionType)
            {
                case CardActionType.Attack:
                    MyDescription = $"Deal {valueStr} damage";
                    break;
                case CardActionType.Heal:
                    MyDescription = $"Heal {valueStr} health";
                    break;
                case CardActionType.Block:
                    MyDescription = $"Gain {valueStr} block";
                    break;
                case CardActionType.ApplyBuff:
                    MyDescription = $"Apply a buff of {valueStr} {statusEffect}";
                    break;
                case CardActionType.ApplyDebuff:
                    MyDescription = $"Apply a debuff of {valueStr} {statusEffect}";
                    break;
                case CardActionType.DrawCard:
                    MyDescription = $"Draw {valueStr} cards.";
                    break;
                case CardActionType.GainEnergy:
                    MyDescription = $"Gain {valueStr} energy.";
                    break;
                case CardActionType.Exhaust:
                    MyDescription = "Exhaust this card.";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
            }

            switch (targetType)
            {
                case ActionTargetType.Player:
                    MyDescription += " on yourself.";
                    break;
                case ActionTargetType.Enemy:
                    MyDescription += " to a target.";
                    break;
                case ActionTargetType.AllEnemies:
                    MyDescription += " to all targets.";
                    break;
                case ActionTargetType.RandomEnemy:
                    MyDescription += " to a random target.";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null);
            }

            return MyDescription;
        }
        #endregion
    }

    [Serializable]
    public class CardActionData
    {
        [SerializeField] private ActionTargetType actionTargetType;
        [SerializeField] private CardActionType cardActionType;
        [SerializeField] private StatusType statusType;
        [SerializeField] private float actionValue;
        [SerializeField] private float actionDelay;

        public ActionTargetType ActionTargetType { get => actionTargetType; set => actionTargetType = value; }
        public CardActionType CardActionType { get => cardActionType; set => cardActionType = value; }
        public StatusType StatusType { get => statusType; set => statusType = value; }
        public float ActionValue { get => actionValue; set => actionValue = value; }
        public float ActionDelay { get => actionDelay; set => actionDelay = value; }
    }
}


