using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Characters;
using UnityEngine.UI;

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
                UIManager.CombatCanvas.SetPileTexts();
            }

            foreach (var cardObject in HandController.Hand)
                cardObject.UpdateCardText();
        }
        public void ResetVerticalLayoutGroup()
        {
            Debug.Log("reset");
            var ver = HandPileTransform.GetComponent<VerticalLayoutGroup>();
            ver.childForceExpandHeight = !ver.childForceExpandHeight;
        }
        public void DiscardHand()
        {
            int count = HandController.Hand.Count;
            for(int i=0; i< count; i++)
            {
                HandController.Hand[i].Discard();
            }
            HandController.Hand.Clear();
        }

        public void OnCardDiscarded(CardBase targetCard)
        {
            HandPile.Remove(targetCard.CardData);
            DiscardPile.Add(targetCard.CardData);
            //HandController.RemoveCardFromHand(targetCard);
            Destroy(targetCard.gameObject);
            UIManager.CombatCanvas.SetPileTexts();
        }

        public void OnCardExhausted(CardBase targetCard)
        {
            HandPile.Remove(targetCard.CardData);
            ExhaustPile.Add(targetCard.CardData);
            //HandController.RemoveCardFromHand(targetCard);

            Destroy(targetCard.gameObject);

            UIManager.CombatCanvas.SetPileTexts();
        }
        public void OnCardPlayed(CardBase targetCard)
        {
            if (targetCard.CardData.ExhaustAfterPlay)
                targetCard.Exhaust();
            else
                targetCard.Discard();

            HandController.RemoveCardFromHand(targetCard);
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
