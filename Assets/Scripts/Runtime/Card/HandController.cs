using UnityEngine;
using System.Collections.Generic;
using AGGtH.Runtime.Characters;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Managers;

namespace AGGtH.Runtime.Card
{
    public class HandController : MonoBehaviour
    {
        [Header("References")]
        public Transform discardTransform;
        public Transform exhaustTransform;
        public Transform drawTransform;
        public LayerMask selectableLayer;
        public LayerMask targetLayer;
        public Camera cam = null;
        public Transform handParentTransform;
        public List<CardBase> Hand = new List<CardBase>(); // Cards currently in hand

        #region Cache
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected EncounterManager EncounterManager => EncounterManager.Instance;
        protected CardCollectionManager CardCollectionManager => CardCollectionManager.Instance;
        protected UIManager UIManager => UIManager.Instance;
        #endregion

        #region Public Methods
        public void AddCardToHand(CardBase card, int index = -1)
        {
            if (index < 0)
            {
                Hand.Add(card);
            }
            else
            {
                Hand.Insert(index, card);
            }
            card.transform.SetParent(handParentTransform);
        }
        #endregion

    }
}
