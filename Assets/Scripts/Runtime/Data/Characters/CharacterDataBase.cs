using UnityEngine;

namespace AGGtH.Runtime.Data.Characters
{
    [CreateAssetMenu(fileName = "CharacterDataBase", menuName = "Scriptable Objects/CharacterDataBase")]
    public class CharacterDataBase : ScriptableObject
    {
        [Header("Base")]
        [SerializeField] protected string characterID;
        [SerializeField] protected string characterName;
        [SerializeField][TextArea] protected string characterDescription;
        [SerializeField] protected int maxHealth;

        public string CharacterID => characterID;

        public string CharacterName => characterName;

        public string CharacterDescription => characterDescription;

        public int MaxHealth => maxHealth;

    }
}
