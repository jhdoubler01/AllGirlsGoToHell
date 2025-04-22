using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Extensions;
using AGGtH.Runtime.Characters;


namespace AGGtH.Runtime.Managers
{
    public class CardCollectionManager : MonoBehaviour
    {
        [Header("Controllers")]
        [SerializeField] private HandController handController;

        #region Cache
        private CardCollectionManager() { }
        public static CardCollectionManager Instance { get; private set; }
        public List<CardData> DrawPile { get; private set; } = new List<CardData>();
        public List<CardData> HandPile { get; private set; } = new List<CardData>();
        public List<CardData> DiscardPile { get; private set; } = new List<CardData>();
        public List<CardData> ExhaustPile { get; private set; } = new List<CardData>();

        public Transform HandPileTransform;
        public Transform DrawPileTransform;
        public Transform DiscardPileTransform;

        public HandController HandController => handController;
        protected GameManager GameManager => GameManager.Instance;
        protected EncounterManager EncounterManager => EncounterManager.Instance;
        protected UIManager UIManager => UIManager.Instance;
        #endregion


        #region Setup
        void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }
        }
        #endregion

        #region Public Methods

        public void DrawCards(int targetDrawCount)
        {
            var currentDrawCount = 0;

            for(int i = 0; i < targetDrawCount; i++)
            {
                if (GameManager.GameplayData.MaxCardOnHand <= HandPile.Count) { return; }
                if(DrawPile.Count <= 0)
                {
                    var nDrawCount = targetDrawCount - currentDrawCount;

                    if (nDrawCount >= DiscardPile.Count)
                        nDrawCount = DiscardPile.Count;

                    ReshuffleDiscardPile();
                    DrawCards(nDrawCount);
                    break;
                }

                var randomCard = DrawPile[UnityEngine.Random.Range(0, DrawPile.Count)];
                var clone = GameManager.BuildAndGetCard(randomCard, HandController.drawTransform);
                HandController.AddCardToHand(clone);
                HandPile.Add(randomCard);
                DrawPile.Remove(randomCard);
                currentDrawCount++;
                //UIManager.CombatCanvas.SetPileTexts();
            }

            foreach (var cardObject in HandController.Hand)
                cardObject.UpdateCardText();
        }
        public void DiscardHand()
        {
            foreach (var cardBase in HandController.Hand)
                cardBase.Discard();

            HandController.Hand.Clear();
        }

        public void OnCardDiscarded(CardBase targetCard)
        {
            HandPile.Remove(targetCard.CardData);
            DiscardPile.Add(targetCard.CardData);
            //Destroy(targetCard.gameObject);
            //UIManager.CombatCanvas.SetPileTexts();
        }

        public void OnCardExhausted(CardBase targetCard)
        {
            HandPile.Remove(targetCard.CardData);
            ExhaustPile.Add(targetCard.CardData);

            //Destroy(targetCard.gameObject);

            //UIManager.CombatCanvas.SetPileTexts();
        }
        public void OnCardPlayed(CardBase targetCard)
        {
            if(targetCard == null || targetCard.CardData == null)
            {
                Debug.LogError("Invalid card played");
                return;
            }
            var cardData = targetCard.CardData;

            if(!GameManager.Instance.IsEnoughEnergyToPlayCard(cardData.EnergyCost))
            {
                Debug.LogError("Not enough energy to play card");
                return;
            }

            
            ApplyCardAction(targetCard.CardData);

            if (targetCard.CardData.ExhaustAfterPlay)
                targetCard.Exhaust();
            else
                targetCard.Discard();

            foreach (var cardObject in HandController.Hand)
                cardObject.UpdateCardText();
        }
        public void SetGameDeck()
        {
            foreach (var i in GameManager.PersistentGameplayData.CurrentCardsList)
                DrawPile.Add(i);
        }

        public void ClearPiles()
        {
            DiscardPile.Clear();
            DrawPile.Clear();
            HandPile.Clear();
            ExhaustPile.Clear();
            HandController.Hand.Clear();
        }
        #endregion

        #region Private Methods
        private void ApplyCardAction(CardData cardData)
        {
            //this needs to be fixed,, it uses enum flags so idk how to do rn will report back later lol
            //also need to change some of the logic but i have to finish making characterbase and playerbase first
            //foreach(var action in cardData.CardActionDataList)
            //{
                switch (action.CardActionType)
                {
                case CardActionType.Attack:
                    ApplyDamageToEnemy(action);
                    break;
                
                case CardActionType.Heal:
                    ApplyHealToPlayer(action);
                    break;
                
                case CardActionType.Block:
                    GameManager.Instance.Player.Block += action.BlockAmt;
                    break;
                
                case CardActionType.Buff:
                    GameManager.Instance.Player.BuffList.Add(new Buff(action.BuffType));
                    break;
                
                case CardActionType.Debuff:
                    GameManager.Instance.Player.DebuffList.Add(new Debuff(action.DebuffType));
                    break;
                
                case CardActionType.GainEnergy:
                    GameManager.Instance.GainEnergy(action.EnergyGainAmt);
                    break;
                
                case CardActionType.Draw:
                    DrawCards(action.DrawCardAmt);
                    break;
                
                default:
                    Debug.LogError($"Card action type {cardData.CardActionType} not implemented");
                    break;
                }
            //}
        }

        private void ApplyDamageToEnemy(CardData cardData)
        {
            var targetEnemy = EncounterManager.GetCurrentEnemy();
            if (targetEnemy == null)
            {
                Debug.LogError("No enemy to apply damage to");
                return;
            }

            targetEnemy.Health -= cardData.DamageAmt;
            if (targetEnemy.Health <= 0)
                targetEnemy.Die();
        }

        private void ApplyHealToPlayer(CardData cardData)
        {
            GameManager.Instance.Player.Health += cardData.HealAmt;
            if (GameManager.Instance.Player.Health > GameManager.Instance.Player.MaxHealth)
                GameManager.Instance.Player.Health = GameManager.Instance.Player.MaxHealth;
        }


        private void ReshuffleDiscardPile()
        {
            foreach (var i in DiscardPile)
                DrawPile.Add(i);

            DiscardPile.Clear();
        }
        private void ReshuffleDrawPile()
        {
            foreach (var i in DrawPile)
                DiscardPile.Add(i);

            DrawPile.Clear();
        }
        #endregion
    }
}
