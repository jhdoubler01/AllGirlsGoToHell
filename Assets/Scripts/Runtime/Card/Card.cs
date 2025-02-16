using UnityEngine;
using AGGtH.Runtime.Enums;

namespace AGGtH.Runtime.Card
{

    public class Card : MonoBehaviour
    {

        public string cardName;
        public CardLoveLanguageType cardLoveLanguageType;
        public CardActionType cardActionType;
        public ActionTargetType actionTargetType;

        public BuffType buffType;
        public DebuffType debuffType;

        public int damageAmt;
        public int blockAmt;

        //add more variables later for more card types

    }
}
