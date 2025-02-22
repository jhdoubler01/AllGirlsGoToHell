using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using System.Collections;
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
        //public CardActionType CardActionType { get; set; }

        #endregion

       /*  #region Editor Methods
        public void EditCardName(string newName) => cardName = newName;
        public void EditId(string newId) => id = newId;
        public void EditEnergyCost(int newEnergyCost) => energyCost = newEnergyCost;
        public void EditCardSprite(Sprite newSprite) => cardSprite = newSprite;
        public void EditUsableWithoutTarget(bool newStat) => usableWithoutTarget = newStat;
        public void EditExhaustAfterPlay(bool newStat) => exhaustAfterPlay = newStat;
        public void EditCardActionDataList(List<CardActionData> newList) => cardActionDataList = newList;
        public void EditCardRarity(RarityType newRarity) => rarity = newRarity;
        #endregion */
    }

    [Serializable]
    public class CardActionData
    {
        [SerializeField] private ActionTargetType actionTargetType;
        [SerializeField] private CardActionType cardActionType;
        [SerializeField] private BuffType buffType;
        [SerializeField] private DebuffType debuffType;
        [SerializeField] private int damageAmt;
        [SerializeField] private int healAmt;
        [SerializeField] private int blockAmt;
        [SerializeField] private int drawCardAmt;
        [SerializeField] private int energyGainAmt;

        public ActionTargetType ActionTargetType { get => actionTargetType; set => actionTargetType = value; }
        public CardActionType CardActionType { get => cardActionType; set => cardActionType = value; }
        public BuffType BuffType { get => buffType; set => buffType = value; }
        public DebuffType DebuffType { get => debuffType; set => debuffType = value; }
        public int DamageAmt { get => damageAmt; set => damageAmt = value; }
        public int HealAmt { get => healAmt; set => healAmt = value; }
        public int BlockAmt { get => blockAmt; set => blockAmt = value; }
        public int DrawCardAmt { get => drawCardAmt; set => drawCardAmt = value; }
        public int EnergyGainAmt { get => energyGainAmt; set => energyGainAmt = value; }
    }
}


