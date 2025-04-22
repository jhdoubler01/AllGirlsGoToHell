using UnityEngine;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Managers;
using AGGtH.Runtime.Interfaces;
using AGGtH.Runtime.Extensions;
using System;
using System.Collections.Generic;

namespace AGGtH.Runtime.Characters

{
    public abstract class CharacterBase : MonoBehaviour, ICharacter
    {
        [Header("Base settings")]
        [SerializeField] private CharacterType characterType;
        [SerializeField] private ClassicProgressBar healthBar;
        [SerializeField] private Transform statusIconContainer;

        #region Cache
        public CharacterStats CharacterStats { get; protected set; }
        public CharacterType CharacterType => characterType;
        public ClassicProgressBar HealthBar => healthBar;
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
        public virtual void SetHealthBarMaxHealth(int maxHealth)
        {
            //each segment is worth 2hp so divide max health by two and make sure its an even number when setting number of segments
            float num = maxHealth / 2;
            if(num % 2 != 0) { num += 0.5f; }
            HealthBar.SetNewMaxHealth((int)num);
        }
        public virtual void ChangeHealthBarFill(int currentHealth, int maxHealth)
        {
            if(maxHealth <= 0)
            {
                Debug.Log("Error! Max health cannot be " + maxHealth);
                return;
            }
            float fillAmt = currentHealth / maxHealth;
            HealthBar.SetNewFillAmount(fillAmt);
        }
        public CharacterBase GetCharacterBase()
        {
            return this;
        }

        public CharacterType GetCharacterType()
        {
            return CharacterType;
        }

        public void SetHealthBar(ClassicProgressBar newHealthBar)
        {
            healthBar = newHealthBar;
        }

    }
}
