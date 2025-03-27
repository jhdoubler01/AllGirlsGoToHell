using UnityEngine;
using System;
using AGGtH.Runtime.Data.Characters;
using AGGtH.Runtime.Interfaces;
using AGGtH.Runtime.Managers;

namespace AGGtH.Runtime.Characters
{
    public abstract class PlayerBase : CharacterBase, IPlayer
    {
        [Header("Ally Base Settings")]
        [SerializeField] private PlayerCharacterData playerCharacterData;
        public PlayerCharacterData PlayerCharacterData => playerCharacterData;

        public override void BuildCharacter()
        {
            base.BuildCharacter();
            CharacterStats = new CharacterStats(playerCharacterData.MaxHealth);

            if (!GameManager)
                throw new Exception("There is no GameManager");

            var data = GameManager.PersistentGameplayData.PlayerHealthData;

            if (data != null)
            {
                CharacterStats.CurrentHealth = data.CurrentHealth;
                CharacterStats.MaxHealth = data.MaxHealth;
            }
            else
            {
                GameManager.PersistentGameplayData.SetPlayerHealthData(PlayerCharacterData.CharacterID, CharacterStats.CurrentHealth, CharacterStats.MaxHealth);
            }

            CharacterStats.OnDeath += OnDeath;
            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);

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
        [SerializeField] private int maxHealth;
        [SerializeField] private int currentHealth;

        public int MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public int CurrentHealth
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
