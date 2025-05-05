using UnityEngine;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Managers;
using AGGtH.Runtime.Interfaces;
using AGGtH.Runtime.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using AGGtH.Runtime.UI;

namespace AGGtH.Runtime.Characters
{
    public abstract class CharacterBase : MonoBehaviour, ICharacter, IPointerClickHandler
    {
        [Header("Base settings")]
        [SerializeField] private CharacterType characterType;
        [SerializeField] private SegmentedHealthBar healthBar;
        //[SerializeField] private GameObject spriteRoot;

        #region Cache
        public CharacterStats CharacterStats { get; protected set; }
        public CharacterType CharacterType => characterType;
        public SegmentedHealthBar HealthBar => healthBar;
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
            CharacterStats.OnBlockChanged += OnBlockChanged;
        }
        protected virtual void OnDeath()
        {

        }
        public virtual void ChangeHealthBarFill(float currentHealth, int maxHealth)
        {
            healthBar.OnHealthChanged(currentHealth, maxHealth);
        }
        public virtual void OnBlockChanged(float block, bool clearAll)
        {
            if (clearAll) { healthBar.RemoveBlock(0, true); }


        }

        public virtual void SetHealthBar(SegmentedHealthBar newHealthBar)
        {
            healthBar = newHealthBar;
        }
        public virtual void OnPointerClick(PointerEventData pointerEventData)
        {
            if(EncounterManager.SelectedCard == null) { Debug.Log("selected card null"); return; }

            EncounterManager.PlayCard(this);
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
