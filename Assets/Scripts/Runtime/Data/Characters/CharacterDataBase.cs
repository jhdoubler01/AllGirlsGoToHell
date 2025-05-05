using UnityEngine;
using AGGtH.Runtime.Enums;

namespace AGGtH.Runtime.Data.Characters
{
    [CreateAssetMenu(fileName = "CharacterDataBase", menuName = "Scriptable Objects/CharacterDataBase")]
    public class CharacterDataBase : ScriptableObject
    {
        [Header("Base")]
        [SerializeField] protected string characterID;
        [SerializeField] protected string characterName;
        [SerializeField][TextArea] protected string characterDescription;
        [SerializeField] protected CardLoveLanguageType loveLanguageType;
        [SerializeField] protected int maxHealth;
        [SerializeField] private Color dialogueColor;

        public string CharacterID => characterID;

        public string CharacterName => characterName;

        public string CharacterDescription => characterDescription;

        public CardLoveLanguageType LoveLanguageType => loveLanguageType;

        public int MaxHealth => maxHealth;

        public Color DialogueColor => dialogueColor;

    }
}
