using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Managers;
using TMPro; //textmesh pro
using UnityEngine.EventSystems;
using AGGtH.Runtime.Extensions;
using AGGtH.Runtime.Characters;

namespace AGGtH.Runtime.Card
{
    public class CardBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
    {
        [Header("Card Objects")]
        [SerializeField] protected TMP_Text cardNameText;
        [SerializeField] protected TMP_Text cardDescText;
        [SerializeField] protected TMP_Text energyCostText;
        [SerializeField] protected TMP_Text actionAmtText;
        [SerializeField] protected Image cardImage;
        [SerializeField] protected Image cardTooltipImage;

        private Transform playerHandParent;
        //private bool overValidTarget;
        //private Transform parentAfterDrag;
        //private EnemyBase enemyTarget;

        private int enemyTargetLayer;

        #region Cache
        public CardData CardData { get; private set; }
        public bool IsInactive { get; protected set; }
        protected Transform CachedTransform { get; set; }
        protected WaitForEndOfFrame CachedWaitFrame { get; set; }
        public bool IsPlayable { get; protected set; } = true;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected FxManager FxManager => FxManager.Instance;
        protected EncounterManager EncounterManager => EncounterManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected UIManager UIManager => UIManager.Instance;
        protected CardCollectionManager CardCollectionManager => CardCollectionManager.Instance;
        public bool IsExhausted { get; private set;}
        public bool IsSelected = false;

        public event Action<CardBase> OnCardSelected, OnCardDeselected, OnHover;

        #endregion

        #region Setup Methods
        protected virtual void Awake()
        {
            CachedTransform = transform;
            CachedWaitFrame = new WaitForEndOfFrame();
            playerHandParent = CardCollectionManager.HandPileTransform;
            enemyTargetLayer = LayerMask.NameToLayer("Enemies");
        }
        private void SetCardNameText()
        {
            cardNameText.text = CardData.CardName;
        }
        private void SetCardDescriptionText()
        {
            cardDescText.text = CardData.MyDescription;
        }

        public virtual void SetCard(CardData targetProfile, bool isPlayable = true)
        {
            CardData = targetProfile;
            IsPlayable = isPlayable;
            cardNameText.text = CardData.CardName;
            cardDescText.text = CardData.MyDescription;
            energyCostText.text = CardData.EnergyCost.ToString();
            actionAmtText.text = (CardData.CardActionDataList[0].ActionValue * 2).ToString();
            //cardTypeIcon.sprite = CardData.CardSprite;

        }
        #endregion

        #region Card Methods
        public virtual void Use(CharacterBase self, CharacterBase target, List<EnemyBase> allEnemies, PlayerBase player)
        {
            if(GameManager.PersistentGameplayData.CurrentEnergy < CardData.EnergyCost) { Debug.Log("not enough energy!"); return; }
            if (!IsPlayable || !GameManager.PersistentGameplayData.CanUseCards) { return; }
            StartCoroutine(CardUseRoutine(self, target, allEnemies, player));
            UIManager.SetDialogueBoxText(CardData.DialogueOptions.RandomItem());

        }
        private IEnumerator CardUseRoutine(CharacterBase self, CharacterBase target, List<EnemyBase> allEnemies, PlayerBase player)
        {
            SpendEnergy(CardData.EnergyCost);
            Debug.Log("Player energy: " + GameManager.PersistentGameplayData.CurrentEnergy);
            foreach(var playerAction in CardData.CardActionDataList)
            {
                yield return new WaitForSeconds(playerAction.ActionDelay);
                var targetList = DetermineTargets(target, allEnemies, player, playerAction);
                foreach(var tar in targetList)
                {
                    CardActionProcessor.GetAction(playerAction.CardActionType)
                        .DoAction(new CardActionParameters(playerAction.ActionValue,
                            target, self, CardData, this));
                }
                CardCollectionManager.OnCardPlayed(this);

            }
        }
        private static List<CharacterBase> DetermineTargets(CharacterBase target, List<EnemyBase> allEnemies, PlayerBase player, CardActionData playerAction)
        {
            List<CharacterBase> targetList = new List<CharacterBase>();
            switch (playerAction.ActionTargetType)
            {
                case ActionTargetType.Enemy:
                    targetList.Add(target);
                    break;
                case ActionTargetType.Player:
                    targetList.Add(player);
                    break;
                case ActionTargetType.AllEnemies:
                    foreach (var enemyBase in allEnemies)
                        targetList.Add(enemyBase);
                    break;
                case ActionTargetType.RandomEnemy:
                    if (allEnemies.Count > 0)
                        targetList.Add(allEnemies.RandomItem());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return targetList;
        }
        public bool CheckIfValidTarget(CharacterBase target)
        {
            if(target.CharacterType == CharacterType.Player)
            {
                foreach(var playerAction in CardData.CardActionDataList)
                {
                    if(playerAction.ActionTargetType == ActionTargetType.Player) { return true; }
                }
            }
            else
            {
                foreach (var playerAction in CardData.CardActionDataList)
                {
                    if (playerAction.ActionTargetType == ActionTargetType.Enemy) { return true; }
                    if (playerAction.ActionTargetType == ActionTargetType.AllEnemies) { return true; }
                    if (playerAction.ActionTargetType == ActionTargetType.RandomEnemy) { return true; }
                }
            }

            return false;
        }
        public virtual void Discard()
        {
            if (IsExhausted) return;
            if (!IsPlayable) return;
            CardCollectionManager.OnCardDiscarded(this);
            //Destroy(gameObject);
        }
        public virtual void Exhaust(bool destroy=true)
        {
            if (IsExhausted) return;
            if (!IsPlayable) return;
            CardCollectionManager.OnCardExhausted(this);
            Destroy(gameObject);
        }
        protected virtual void SpendEnergy(int energyCost)
        {
            if (!IsPlayable) { return; }

            GameManager.PersistentGameplayData.CurrentEnergy -= energyCost;
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
        #region Pointer Events
        public void OnPointerEnter(BaseEventData eventData)
        {
            PointerEventData pointerData = (PointerEventData)eventData;
            OnHover?.Invoke(this);
        }
        public void OnSelect(BaseEventData eventData)
        {
            if (!GameManager.PersistentGameplayData.CanSelectCards) { return; }

            PointerEventData pointerData = (PointerEventData)eventData;
            EncounterManager.OnCardSelected(this);
            if (UnityEngine.EventSystems.EventSystem.current.alreadySelecting == false) { 
                eventData.selectedObject = gameObject;
            }
            OnCardSelected?.Invoke(this);

        }
        public void OnDeselect(BaseEventData eventData)
        {
            PointerEventData pointerData = (PointerEventData)eventData;
            //UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

            //EncounterManager.OnCardDeselected();

            OnCardDeselected?.Invoke(this);
        }
        public virtual void OnPointerExit(PointerEventData eventData)
        {

            //HideTooltipInfo(TooltipManager.Instance);
        }
        public void ResetVerticalLayout()
        {
            CardCollectionManager.ResetVerticalLayoutGroup();

        }
        public virtual void OnPointerClick(PointerEventData eventData)
        {
        }
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            //HideTooltipInfo(TooltipManager.Instance);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            //ShowTooltipInfo();
        }
        #endregion
        #region Tooltip
        //protected virtual void ShowTooltipInfo()
        //{
        //    if (!descriptionRoot) return;
        //    if (CardData.KeywordsList.Count <= 0) return;

        //    var tooltipManager = TooltipManager.Instance;
        //    foreach (var cardDataSpecialKeyword in CardData.KeywordsList)
        //    {
        //        var specialKeyword = tooltipManager.SpecialKeywordData.SpecialKeywordBaseList.Find(x => x.SpecialKeyword == cardDataSpecialKeyword);
        //        if (specialKeyword != null)
        //            ShowTooltipInfo(tooltipManager, specialKeyword.GetContent(), specialKeyword.GetHeader(), descriptionRoot, CursorType.Default, CollectionManager ? CollectionManager.HandController.cam : Camera.main);
        //    }
        //}
        //public virtual void ShowTooltipInfo(TooltipManager tooltipManager, string content, string header = "", Transform tooltipStaticTransform = null, CursorType targetCursor = CursorType.Default, Camera cam = null, float delayShow = 0)
        //{
        //    tooltipManager.ShowTooltip(content, header, tooltipStaticTransform, targetCursor, cam, delayShow);
        //}

        //public virtual void HideTooltipInfo(TooltipManager tooltipManager)
        //{
        //    tooltipManager.HideTooltip();
        //}
        #endregion
        #region Routines

        #endregion
        #region Runtime
        void Start()
        {
            playerHandParent = transform.root;
        }
        #endregion
    }
}
