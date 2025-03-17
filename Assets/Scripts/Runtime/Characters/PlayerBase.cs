using UnityEngine;
using AGGtH.Runtime.Data.Characters;


namespace AGGtH.Runtime.Characters
{
    public class PlayerBase : CharacterBase
    {
        [Header("Player Base Settings")]
        [SerializeField] private PlayerCharacterData playerCharacterData;
        public PlayerCharacterData PlayerCharacterData => playerCharacterData;

        public override void BuildCharacter()
        {
            base.BuildCharacter();
            CharacterStats = new CharacterStats(playerCharacterData.MaxHealth);
        }
    }
}
