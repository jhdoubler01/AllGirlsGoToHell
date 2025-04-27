using UnityEngine;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Managers;
using AGGtH.Runtime.Interfaces;
using AGGtH.Runtime.Extensions;
using System;
using System.Collections.Generic;
using AGGtH.Runtime.UI;

namespace AGGtH.Runtime.Characters
{
    public abstract class CharacterBase : MonoBehaviour, ICharacter
    {
        [Header("Base settings")]
        [SerializeField] private CharacterType characterType;

        #region Cache
        public CharacterStats CharacterStats { get; protected set; }
        public CharacterType CharacterType => characterType;
        //public SegmentedHealthBar HealthBar => healthBar;
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected EncounterManager EncounterManager => EncounterManager.Instance;
        protected CardCollectionManager CardCollectionManager => CardCollectionManager.Instance;
        protected UIManager UIManager => UIManager.Instance;

        #endregion


        public virtual void Awake()
        {
        }

        public virtual void BuildCharacter()
        {
        }
        protected virtual void OnDeath()
        {

        }
        public virtual void ChangeHealthBarFill(float currentHealth, float maxHealth)
        {

        }
        //public virtual void SetHealthBar(SegmentedHealthBar newHealthBar)
        //{
        //    healthBar = newHealthBar;
        //}
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
