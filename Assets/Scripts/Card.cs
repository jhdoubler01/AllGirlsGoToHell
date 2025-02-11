using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CardSystem
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]

    public class Card : ScriptableObject
    {

        public string cardName;
        public CardType cardType;
        public int damageAmt;
        //add more variables later for more card types

        public enum CardType
        {
            Neutral,
            Verbal,
            Physical,
            Quality,
            Gifts,
            Service
        }
    }
}
poop