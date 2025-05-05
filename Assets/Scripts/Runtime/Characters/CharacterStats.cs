using System;
using System.Collections.Generic;
using System.Linq;
using AGGtH.Runtime.Enums;
using UnityEngine;

namespace AGGtH.Runtime.Characters
{
    public class StatusStats
    {
        public StatusType StatusType { get; set; }
        public float StatusValue { get; set; }
        public bool DecreaseOverTurn { get; set; } // If true, decrease on turn end
        public bool IsPermanent { get; set; } // If true, status can not be cleared during combat
        public bool IsActive { get; set; }
        public bool CanNegativeStack { get; set; }
        public bool ClearAtNextTurn { get; set; }

        public Action OnTriggerAction;
        public StatusStats(StatusType statusType, float statusValue, bool decreaseOverTurn = false, bool isPermanent = false, bool isActive = false, bool canNegativeStack = false, bool clearAtNextTurn = false)
        {
            StatusType = statusType;
            StatusValue = statusValue;
            DecreaseOverTurn = decreaseOverTurn;
            IsPermanent = isPermanent;
            IsActive = isActive;
            CanNegativeStack = canNegativeStack;
            ClearAtNextTurn = clearAtNextTurn;
        }
    }
    public class CharacterStats
    {
        public int MaxHealth { get; set; }
        public float CurrentHealth { get; set; }
        public bool IsStunned { get; set; }
        public bool IsDeath { get; private set; }

        public CardLoveLanguageType LoveLanguageType { get; private set; }

        public Action OnDeath;
        public Action<float, bool> OnBlockChanged;
        public Action<float, int> OnHealthChanged;
        private readonly Action<StatusType, float> OnStatusChanged;
        private readonly Action<StatusType, float> OnStatusApplied;
        private readonly Action<StatusType> OnStatusCleared;




        public Action OnHealAction;
        public Action OnTakeDamageAction;

        public readonly Dictionary<StatusType, StatusStats> StatusDict = new Dictionary<StatusType, StatusStats>();

        #region Setup
        public CharacterStats(int maxHealth, CardLoveLanguageType loveLanguage)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            LoveLanguageType = loveLanguage;
            SetAllStatus();

            //OnHealthChanged += characterCanvas.UpdateHealthText;
            //OnStatusChanged += characterCanvas.UpdateStatusText;
            //OnStatusApplied += characterCanvas.ApplyStatus;
            //OnStatusCleared += characterCanvas.ClearStatus;
        }

        private void SetAllStatus()
        {
            for (int i = 0; i < Enum.GetNames(typeof(StatusType)).Length; i++)
                StatusDict.Add((StatusType)i, new StatusStats((StatusType)i, 0));

            StatusDict[StatusType.Tipsy].DecreaseOverTurn = true;
            StatusDict[StatusType.Tipsy].OnTriggerAction += DamagePoison;

            StatusDict[StatusType.Block].ClearAtNextTurn = true;

            StatusDict[StatusType.Shy].CanNegativeStack = true;
            StatusDict[StatusType.Flustered].CanNegativeStack = true;

            StatusDict[StatusType.Stun].DecreaseOverTurn = true;
            StatusDict[StatusType.Stun].OnTriggerAction += CheckStunStatus;

        }
        #endregion

        #region Public Methods
        public void ApplyStatus(StatusType targetStatus, float value)
        {
            if (StatusDict[targetStatus].IsActive)
            {
                StatusDict[targetStatus].StatusValue += value;
                OnStatusChanged?.Invoke(targetStatus, StatusDict[targetStatus].StatusValue);
                

            }
            else
            {
                StatusDict[targetStatus].StatusValue = value;
                StatusDict[targetStatus].IsActive = true;
                OnStatusApplied?.Invoke(targetStatus, StatusDict[targetStatus].StatusValue);
            }
        }
        public void TriggerAllStatus()
        {
            for (int i = 0; i < Enum.GetNames(typeof(StatusType)).Length; i++)
                TriggerStatus((StatusType)i);
        }

        public void SetCurrentHealth(float targetCurrentHealth)
        {
            CurrentHealth = targetCurrentHealth <= 0 ? 1 : targetCurrentHealth;
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        public void Heal(int value)
        {
            CurrentHealth += value;
            if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        public void Damage(float value, bool canPierceArmor = false)
        {
            if (IsDeath) return;
            OnTakeDamageAction?.Invoke();
            float remainingDamage = value;

            if (!canPierceArmor)
            {
                if (StatusDict[StatusType.Block].IsActive)
                {
                    ApplyStatus(StatusType.Block, -value);

                    remainingDamage = 0;
                    if (StatusDict[StatusType.Block].StatusValue <= 0)
                    {
                        remainingDamage = StatusDict[StatusType.Block].StatusValue * -1;
                        ClearStatus(StatusType.Block);
                    }
                }
            }

            CurrentHealth -= remainingDamage;
            Debug.Log("took " + remainingDamage + " damage. health: " + CurrentHealth + "/" + MaxHealth);
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                OnDeath?.Invoke();
                IsDeath = true;
            }
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        public void IncreaseMaxHealth(int value)
        {
            MaxHealth += value;
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        public void ClearAllStatus()
        {
            foreach (var status in StatusDict)
                ClearStatus(status.Key);
        }

        public void ClearStatus(StatusType targetStatus)
        {
            StatusDict[targetStatus].IsActive = false;
            StatusDict[targetStatus].StatusValue = 0;
            OnStatusCleared?.Invoke(targetStatus);
        }

        #endregion

        #region Private Methods
        private void TriggerStatus(StatusType targetStatus)
        {
            StatusDict[targetStatus].OnTriggerAction?.Invoke();

            //One turn only statuses
            if (StatusDict[targetStatus].ClearAtNextTurn)
            {
                ClearStatus(targetStatus);
                OnStatusChanged?.Invoke(targetStatus, StatusDict[targetStatus].StatusValue);
                return;
            }

            //Check status
            if (StatusDict[targetStatus].StatusValue <= 0)
            {
                if (!StatusDict[targetStatus].CanNegativeStack)
                {
                    if (StatusDict[targetStatus].StatusValue == 0 && !StatusDict[targetStatus].IsPermanent)
                        ClearStatus(targetStatus);
                }
                else
                {
                    if (!StatusDict[targetStatus].IsPermanent)
                        ClearStatus(targetStatus);
                }
            }

            if (StatusDict[targetStatus].DecreaseOverTurn)
                StatusDict[targetStatus].StatusValue--;

            if (StatusDict[targetStatus].StatusValue == 0)
                if (!StatusDict[targetStatus].IsPermanent)
                    ClearStatus(targetStatus);

            OnStatusChanged?.Invoke(targetStatus, StatusDict[targetStatus].StatusValue);
        }

        public void AddBlock()
        {

        }
        private void RemoveBlock()
        {

        }
        private void DamagePoison()
        {
            if (StatusDict[StatusType.Tipsy].StatusValue <= 0) return;
            Damage(StatusDict[StatusType.Tipsy].StatusValue, true);
        }

        public void CheckStunStatus()
        {
            if (StatusDict[StatusType.Stun].StatusValue <= 0)
            {
                IsStunned = false;
                return;
            }

            IsStunned = true;
        }

        #endregion
    }
}