using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Managers;
using TMPro; //textmesh pro
using UnityEngine.EventSystems;

namespace AGGtH.Runtime.Card
{
    public class CardBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Card Objects")]
        [SerializeField] private TMP_Text cardNameText;
        [SerializeField] private TMP_Text cardDescText;
        [SerializeField] private TMP_Text energyCostText;
        [SerializeField] private Image cardTypeIcon;

        private Transform playerHandParent;
        private bool hoverOverTarget;
        private Transform parentAfterDrag;

        #region Cache
        public CardData CardData { get; private set; }
        public bool IsInactive { get; protected set; }
        public bool IsPlayable { get; protected set; } = true;
        protected EncounterManager EncounterManager => EncounterManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected UIManager UIManager => UIManager.Instance;
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
            //energyCostText.text = CardData.EnergyCost.ToString();
            //cardTypeIcon.sprite = CardData.CardSprite;

        }
        #endregion

        #region UI Event Handlers
        public void OnBeginDrag(PointerEventData eventData)
        {
            PlayableOnBeginDrag();
        }
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;

        }
        public void OnEndDrag(PointerEventData eventData)
        {
            PlayableOnEndDrag();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("trigger enter");
            PlayableTriggerEnter(collision);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            PlayableTriggerExit(collision);
        }
        public virtual void PlayableTriggerEnter(Collider2D collision)
        {
            Debug.Log("trigger");
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("enemy collision");
                //parentAfterDrag = collision.transform;
                hoverOverTarget = true;
            }
        }
        public virtual void PlayableTriggerExit(Collider2D collision)
        {
            if (hoverOverTarget)
            {
                parentAfterDrag = transform.parent;
            }
        }
        public virtual void PlayableOnBeginDrag()
        {
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
        }

        public virtual void PlayableOnEndDrag()
        {
            Debug.Log(ReadyToBePlayed());
            if (ReadyToBePlayed()) { Use(); }
            else transform.SetParent(parentAfterDrag);
        }

        #endregion

        #region Card Methods
        public virtual void Use(Transform target = null)
        {
            Debug.Log("play card");
            UIManager.SetDialogueBoxText(GetRandomDialogueOption());
            GameManager.SubtractFromCurrentEnergy(CardData.EnergyCost);

            //EncounterManager.MoveCardToDiscardPile(this);
        }
        private bool ReadyToBePlayed()
        {
            bool ready = true;

            if (!GameManager.IsEnoughEnergyToPlayCard(CardData.EnergyCost)) { Debug.Log("not enough energy"); ready = false; }
            if(!CardData.UsableWithoutTarget && !hoverOverTarget) { Debug.Log("need to target an enemy"); ready = false; }

            return ready;
        }
        public virtual void Discard()
        {

        }
        public virtual void Exhaust(bool destroy=true)
        {

        }
        protected virtual void SpendEnergy(int value)
        {

        }
        public virtual void UpdateCardText()
        {

        }
        private string GetRandomDialogueOption()
        {
            var rand = UnityEngine.Random.Range(0, CardData.DialogueOptions.Count - 1);
            return CardData.DialogueOptions[rand];
        }
        #endregion

        #region Runtime
        void Start()
        {
            playerHandParent = transform.root;
        }
        #endregion
    }
}
