using UnityEngine;
using System;
using System.Collections.Generic;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Characters;
using AGGtH.Runtime.Data.Containers;
using AGGtH.Runtime.Extensions;
using Random = UnityEngine.Random;

namespace AGGtH.Runtime.Data.Characters
{
    [CreateAssetMenu(fileName = "EnemyCharacterData", menuName = "Assets/Data/Characters/Enemies")]
    public class EnemyCharacterData : CharacterDataBase
    {
        [Header("Enemy Defaults")]
        [SerializeField] private EnemyBase enemyPrefab;
        [SerializeField] private bool followAbilityPattern;
        [SerializeField] private List<EnemyAbilityData> enemyAbilityList;
        public List<EnemyAbilityData> EnemyAbilityList => enemyAbilityList;

        public EnemyBase EnemyPrefab => enemyPrefab;

        public EnemyAbilityData GetAbility()
        {
            return EnemyAbilityList.RandomItem();
        }

        public EnemyAbilityData GetAbility(int usedAbilityCount)
        {
            if (followAbilityPattern)
            {
                var index = usedAbilityCount % EnemyAbilityList.Count;
                return EnemyAbilityList[index];
            }

            return GetAbility();
        }
    }
    [Serializable]
    public class EnemyAbilityData
    {
        [Header("Settings")]
        [SerializeField] private string name;
        [SerializeField] private EnemyIntentionData intention;
        [SerializeField] private bool hideActionValue;
        [SerializeField] private List<string> dialogueList;
        [SerializeField] private List<EnemyActionData> actionList;

        public string Name => name;
        public EnemyIntentionData Intention => intention;
        public List<EnemyActionData> ActionList => actionList;
        public bool HideActionValue => hideActionValue;
        public string Dialogue => dialogueList.RandomItem();
    }

    [Serializable]
    public class EnemyActionData
    {
        [SerializeField] private EnemyActionType actionType;
        [SerializeField] private float minActionValue;
        [SerializeField] private float maxActionValue;
        public EnemyActionType ActionType => actionType;
        public StatusType StatusType;
        public float ActionValue => Random.Range(minActionValue, maxActionValue);

    }
}
