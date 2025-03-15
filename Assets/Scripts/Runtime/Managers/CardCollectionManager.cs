using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Enums;
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
        public List<CardData> PlayerHandData { get; private set; } = new List<CardData>();
        public List<CardData> DiscardPile { get; private set; } = new List<CardData>();
        public List<CardData> ExhaustPile { get; private set; } = new List<CardData>();

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
                if (GameManager.GameplayData.MaxCardOnHand <= PlayerHandData.Count) { return; }
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
                PlayerHandData.Add(randomCard);
                DrawPile.Remove(randomCard);
                currentDrawCount++;
                //UIManager.CombatCanvas.SetPileTexts();
            }

            foreach (var cardObject in HandController.hand)
                cardObject.UpdateCardText();
        }
        public void DiscardHand()
        {
            foreach (var cardBase in HandController.hand)
                cardBase.Discard();

            HandController.hand.Clear();
        }

        public void OnCardDiscarded(CardBase targetCard)
        {
            PlayerHandData.Remove(targetCard.CardData);
            DiscardPile.Add(targetCard.CardData);
            UIManager.CombatCanvas.SetPileTexts();
        }

        public void OnCardExhausted(CardBase targetCard)
        {
            PlayerHandData.Remove(targetCard.CardData);
            ExhaustPile.Add(targetCard.CardData);
            UIManager.CombatCanvas.SetPileTexts();
        }
        public void OnCardPlayed(CardBase targetCard)
        {
            if(targetCard == null || targetCard.CardData == null)
            {
                Debug.LogError("Invalid card played");
                return;
            }
            var cardData = targetCard.CardData;

            if(!GameManager.Instance.IsEnoughEnergy(cardData.EnergyCost))
            {
                Debug.LogError("Not enough energy to play card");
                return;
            }

            GameManager.Instance.spendEnergy(cardData.EnergyCost);
            
            ApplyCardAction(targetCard);

            if (targetCard.CardData.ExhaustAfterPlay)
                targetCard.Exhaust();
            else
                targetCard.Discard();

            foreach (var cardObject in HandController.hand)
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
            PlayerHandData.Clear();
            ExhaustPile.Clear();
            HandController.hand.Clear();
        }
        #endregion

        #region Private Methods
        private void ApplyCardAction(CardData cardData)
        {
            switch (cardData.CardActionType)
            {
                case CardActionType.Damage:
                    ApplyDamageToEnemy(cardData);
                    break;
                
                case CardActionType.Heal:
                    ApplyHealToPlayer(cardData);
                    break;
                
                case CardActionType.GainEnergy:
                    GameManager.Instance.GainEnergy(cardData.EnergyGainAmt);
                    break;
                
                case CardActionType.Draw:
                    DrawCards(cardData.DrawCardAmt);
                    break;
                
                default:
                    Debug.LogError($"Card action type {cardData.CardActionType} not implemented");
                    break;
            }
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
