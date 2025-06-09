using System;
using AGGtH.Runtime.Characters;
namespace AGGtH.Runtime.Enums
{
    public enum CardActionType
    {
        Attack,
        Heal,
        Block,
        ApplyBuff,
        ApplyDebuff,
        DrawCard,
        GainEnergy,
        Exhaust,
    }
    public class CardActionModifierStats
    {
        // returns modifiers from status effects that affect each type of actiontype
        
        public static float GetCardActionModifier(CardActionType actionType, PlayerBase player)
        {
            float modifier = 0;
            switch (actionType)
            {
                case CardActionType.Attack:
                    modifier = player.CharacterStats.StatusDict[StatusType.Bold].StatusValue - player.CharacterStats.StatusDict[StatusType.Shy].StatusValue;
                    break;
                case CardActionType.Block:
                    modifier = player.CharacterStats.StatusDict[StatusType.Nonchalant].StatusValue - player.CharacterStats.StatusDict[StatusType.Flustered].StatusValue;
                    break;
                default:
                    modifier = 0;
                    break;
            }
            return modifier;
        }
    }
}