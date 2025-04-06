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
    public class CardBase : MonoBehaviour/*, IBeginDragHandler, IDragHandler, IEndDragHandler*/
    {
        [Header("Card Objects")]
        [SerializeField] private TMP_Text cardNameText;
        [SerializeField] private TMP_Text cardDescText;
        [SerializeField] private TMP_Text energyCostText;
        [SerializeField] private TMP_Text actionAmtText;

        private Transform playerHandParent;
        private bool overValidTarget;
        private Transform parentAfterDrag;

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
            //cardDescText.text = CardData.description.
        }

        public virtual void SetCard(CardData targetProfile, bool isPlayable = true)
        {
            CardData = targetProfile;
            IsPlayable = isPlayable;
            cardNameText.text = CardData.CardName;
            //cardDescText.text = CardData.MyDescription;
            energyCostText.text = CardData.EnergyCost.ToString();
            actionAmtText.text = CardData.CardActionDataList[0].ActionValue.ToString();
            //cardTypeIcon.sprite = CardData.CardSprite;

        }
        #endregion

        #region UI Event Handlers
        public void OnCardSelected()
        {
            IsSelected = !IsSelected;
            EncounterManager.SelectedCard(this, IsSelected);
        }
        //public void OnBeginDrag(PointerEventData eventData)
        //{
        //    PlayableOnBeginDrag();
        //}
        //public void OnDrag(PointerEventData eventData)
        //{
        //    transform.position = Input.mousePosition;

        //}
        //public void OnEndDrag(PointerEventData eventData)
        //{
        //    PlayableOnEndDrag();
        //}
        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    Debug.Log("trigger enter");
        //    PlayableTriggerEnter(collision);
        //}
        //private void OnTriggerExit2D(Collider2D collision)
        //{
        //    PlayableTriggerExit(collision);
        //}
        //public virtual void PlayableTriggerEnter(Collider2D collision)
        //{
        //    Debug.Log("trigger");
        //    if (collision.gameObject.CompareTag("Enemy"))
        //    {
        //        Debug.Log("enemy collision");
        //        //parentAfterDrag = collision.transform;
        //        overValidTarget = true;
        //    }
        //}
        //public virtual void PlayableTriggerExit(Collider2D collision)
        //{
        //    if (overValidTarget)
        //    {
        //        parentAfterDrag = transform.parent;
        //    }
        //}
        //public virtual void PlayableOnBeginDrag()
        //{
        //    parentAfterDrag = playerHandParent;
        //    transform.SetParent(transform.root);
        //    transform.SetAsLastSibling();
        //}

        //public virtual void PlayableOnEndDrag()
        //{
        //    Debug.Log(IsPlayable);
        //    //if (IsPlayable) { Use(); }
        //    //else transform.SetParent(parentAfterDrag);
        //}

        #endregion

        #region Card Methods
        public virtual void Use(CharacterBase self, CharacterBase target, List<EnemyBase> allEnemies, PlayerBase player)
        {
            if (!IsPlayable) { return; }
            StartCoroutine(CardUseRoutine(self, target, allEnemies, player));

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
                    targetList.Add(target);
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
        //private static List<CharacterBase> DetermineTargetsForMultipleActions(CharacterBase target, List<EnemyBase> allEnemies, PlayerBase player, CardData myCardData)
        //{
        //    List<CharacterBase> totalTargetList = new List<CharacterBase>();
        //    foreach(CardActionData playerAction in myCardData.CardActionDataList)
        //    {
        //        totalTargetList.AddRange(DetermineTargets(target, allEnemies, player, playerAction));
        //    }
        //    return totalTargetList;
        //}
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
