using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using System.Collections;
using AGGtH.Runtime.Enums;

namespace AGGtH.Runtime.Card
{

    public class CardData : ScriptableObject
    {
        [Header("Card Profile")]
        [SerializeField] private string id;
        [SerializeField] private string cardName;
        [SerializeField] private int energyCost;
        [SerializeField] private Sprite cardSprite;
        [SerializeField] private CardLoveLanguageType cardLoveLanguageType;
        //[SerializeField] private RarityType rarity;

        [Header("Action Settings")]
        [SerializeField] private bool usableWithoutTarget;
        [SerializeField] private bool exhaustAfterPlay;
        [SerializeField] private List<CardActionData> cardActionDataList;

        #region Cache
        public string Id => id;
        public string CardName => cardName;
        public int EnergyCost => energyCost;
        public Sprite CardSprite => cardSprite;
        public bool UsableWithoutTarget => usableWithoutTarget;
        public bool ExhaustAfterPlay => exhaustAfterPlay;
        public List<CardActionData> CardActionDataList => cardActionDataList;
        #endregion

        #region Editor Methods
        public void EditCardName(string newName) => cardName = newName;
        public void EditId(string newId) => id = newId;
        public void EditEnergyCost(int newEnergyCost) => energyCost = newEnergyCost;
        public void EditCardSprite(Sprite newSprite) => cardSprite = newSprite;
        public void EditUsableWithoutTarget(bool newStat) => usableWithoutTarget = newStat;
        public void EditExhaustAfterPlay(bool newStat) => exhaustAfterPlay = newStat;
        public void EditCardActionDataList(List<CardActionData> newList) => cardActionDataList = newList;
        #endregion
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

        public ActionTargetType ActionTargetType => actionTargetType;
        public CardActionType CardActionType => cardActionType;
        public BuffType BuffType => buffType;
        public DebuffType DebuffType => debuffType;
        public int DamageAmt => damageAmt;
        public int HealAmt => healAmt;
        public int BlockAmt => blockAmt;
        public int DrawCardAmt => drawCardAmt;
        public int EnergyGainAmt => energyGainAmt;
    }
}
