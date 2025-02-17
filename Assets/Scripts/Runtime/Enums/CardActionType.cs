using System;
namespace AGGtH.Runtime.Enums
{
    [Flags]
    public enum CardActionType
    {
        None = 0,
        Attack = 1 << 0,
        Heal = 1 << 1,
        Block = 1 << 2,
        Buff = 1 << 3, //positive effect (boosts stats)
        Debuff = 1 << 4, //negative effect (lowers stats)
        DrawCard = 1 << 5,
        GainEnergy = 1 << 6,
        Exhaust = 1 << 7,
    }
}