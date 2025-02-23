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
         

        #region Cache
        public CardData CardData { get; private set; }
        public bool IsInactive { get; protected set; }
        public bool IsPlayable { get; protected set; } = true;
        protected EncounterManager EncounterManager => EncounterManager.Instance;
        public bool IsExhausted { get; private set;}
        #endregion
    }
}
