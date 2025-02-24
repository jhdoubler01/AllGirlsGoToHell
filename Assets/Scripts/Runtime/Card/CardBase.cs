using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Managers;
using TMPro;

namespace AGGtH.Runtime.Card
{
    public class CardBase : MonoBehaviour
    {
        [Header("Card Objects")]
        [SerializeField] private TMP_Text cardNameText;
        [SerializeField] private TMP_Text cardDescText;
        [SerializeField] private TMP_Text energyCostText;
        [SerializeField] private Image cardTypeIcon;

        #region Cache
        public CardData CardData { get; private set; }
        public bool IsInactive { get; protected set; }
        public bool IsPlayable { get; protected set; } = true;
        protected EncounterManager EncounterManager => EncounterManager.Instance;
        public bool IsExhausted { get; private set;}
        #endregion

        #region Setup Methods
        private void SetCardNameText()
        {
            cardNameText.text = CardData.CardName;
        }
        private void SetCardDescriptionText()
        {
            //cardDescText.text = CardData.description.
        }

        public virtual void SetCard(CardData targetProfile, bool isPlayable = true)
        {
            CardData = targetProfile;
            IsPlayable = isPlayable;
            cardNameText.text = CardData.CardName;
            //cardDescText.text = CardData.MyDescription;
            energyCostText.text = CardData.EnergyCost.ToString();
            //cardTypeIcon.sprite = CardData.CardSprite;

        }
        #endregion


    }
}
