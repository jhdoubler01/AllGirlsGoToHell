using UnityEngine;
using System;
using AGGtH.Runtime.Data.Characters;
using AGGtH.Runtime.Interfaces;
using AGGtH.Runtime.Extensions;
using AGGtH.Runtime.Managers;

namespace AGGtH.Runtime.Characters
{
    public abstract class PlayerBase : CharacterBase, IPlayer
    {
        [Header("UI")]
        [SerializeField] private ClassicProgressBar energyBar;

        [Header("Player Base Settings")]
        [SerializeField] private PlayerCharacterData playerCharacterData;
        public PlayerCharacterData PlayerCharacterData => playerCharacterData;

        public override void BuildCharacter()
        {
            base.BuildCharacter();
            //SetHealthBar(UIManager.PlayerHealthBar);
            CharacterStats = new CharacterStats(PlayerCharacterData.MaxHealth, PlayerCharacterData.LoveLanguageType);
            Debug.Log("Player character Data max health: " + playerCharacterData.MaxHealth);


            if (!GameManager)
                throw new Exception("There is no GameManager");

            var data = GameManager.PersistentGameplayData.PlayerHealthData;

            if (data != null)
            {
                CharacterStats.CurrentHealth = data.CurrentHealth;
                Debug.Log("data current health: " + data.CurrentHealth);
                CharacterStats.MaxHealth = data.MaxHealth;
                Debug.Log("data max health: " + data.MaxHealth);

            }
            else
            {
                Debug.Log("Data is null!!");
                GameManager.PersistentGameplayData.SetPlayerHealthData(PlayerCharacterData.CharacterID, CharacterStats.CurrentHealth, CharacterStats.MaxHealth);
            }



            CharacterStats.OnDeath += OnDeath;

            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);

            CharacterStats.OnHealthChanged += ChangeHealthBarFill;




            if (EncounterManager != null)
                EncounterManager.OnPlayerTurnStarted += CharacterStats.TriggerAllStatus;
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            if (EncounterManager != null)
            {
                EncounterManager.OnPlayerTurnStarted -= CharacterStats.TriggerAllStatus;
                EncounterManager.OnPlayerDeath(this);
            }

            Destroy(gameObject);
        }
    }
    [Serializable]
    public class PlayerHealthData
    {
        [SerializeField] private string characterId;
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;

        public float MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public float CurrentHealth
        {
            get => currentHealth;
            set => currentHealth = value;
        }

        public string CharacterId
        {
            get => characterId;
            set => characterId = value;
        }
    }
}
