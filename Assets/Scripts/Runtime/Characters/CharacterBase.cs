using UnityEngine;
using AGGtH.Runtime.Managers;
using AGGtH.Runtime.Enums;

namespace AGGtH.Runtime.Characters

{
    public class CharacterBase : ScriptableObject
    {
        [Header("Base")]
        [SerializeField] private CharacterType characterType;

        #region Cache
        public CharacterStats CharacterStats { get; protected set; }
        public CharacterType CharacterType => characterType;
        //protected FxManager FxManager => FxManager.Instance;
        //protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected EncounterManager EncounterManager => EncounterManager.Instance;
        protected CardCollectionManager CardCollectionManager => CardCollectionManager.Instance;
        protected UIManager UIManager => UIManager.Instance;

        #endregion

        //[SerializeField] protected string characterID;
        //[SerializeField] protected string characterName;
        //[SerializeField][TextArea] protected string characterDescription;
        ////[SerializeField] protected int maxHealth;

        //public string CharacterID => characterID;

        //public string CharacterName => characterName;

        //public string CharacterDescription => characterDescription;

        //public int MaxHealth => maxHealth;
        public virtual void Awake()
        {

        }
        public virtual void BuildCharacter()
        {

        }
        protected virtual void OnDeath()
        {

        }
        public CharacterBase GetCharacterBase()
        {
            return this;
        }
        public CharacterType GetCharacterType()
        {
            return CharacterType;
        }
    }
}
