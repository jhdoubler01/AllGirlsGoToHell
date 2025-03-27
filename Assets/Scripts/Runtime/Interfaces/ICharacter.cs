using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Characters;

namespace AGGtH.Runtime.Interfaces
{
    public interface ICharacter
    {
        public CharacterBase GetCharacterBase();
        public CharacterType GetCharacterType();
    }
}