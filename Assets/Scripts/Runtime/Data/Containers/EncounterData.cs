using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using AGGtH.Runtime.Managers;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Characters;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Data.Characters;
using AGGtH.Runtime.Extensions;

namespace AGGtH.Runtime.Data.Containers
{
    [CreateAssetMenu(fileName = "EncounterData", menuName = "Assets/Data/EncounterData")]
    public class EncounterData : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private bool encounterRandomlyAtStage;
        [SerializeField] private List<EnemyEncounterStage> enemyEncounterList;
        [SerializeField] private RewardContainerData encounterRewards;

        public bool EncounterRandomlyAtStage => encounterRandomlyAtStage;
        public List<EnemyEncounterStage> EnemyEncounterList => enemyEncounterList;
        public RewardContainerData EncounterRewards => encounterRewards;

        public EnemyEncounter GetEnemyEncounter(int stageId = 0, int encounterId = 0, bool isFinal = false)
        {
            var selectedStage = EnemyEncounterList.First(x => x.StageId == stageId);
            if (isFinal) return selectedStage.BossEncounterList.RandomItem();

            return EncounterRandomlyAtStage
                ? selectedStage.EnemyEncounterList.RandomItem()
                : selectedStage.EnemyEncounterList[encounterId] ?? selectedStage.EnemyEncounterList.RandomItem();
        }

    }


    [Serializable]
    public class EnemyEncounterStage
    {
        [SerializeField] private string name;
        [SerializeField] private int stageId;
        [SerializeField] private List<EnemyEncounter> bossEncounterList;
        [SerializeField] private List<EnemyEncounter> enemyEncounterList;
        public string Name => name;
        public int StageId => stageId;
        public List<EnemyEncounter> BossEncounterList => bossEncounterList;
        public List<EnemyEncounter> EnemyEncounterList => enemyEncounterList;
    }


    [Serializable]
    public class EnemyEncounter : EncounterBase
    {
        [SerializeField] private List<EnemyCharacterData> enemyList;
        public List<EnemyCharacterData> EnemyList => enemyList;
    }

    [Serializable]
    public abstract class EncounterBase
    {
        //[SerializeField] private BackgroundTypes targetBackgroundType;

        //public BackgroundTypes TargetBackgroundType => targetBackgroundType;
    }
}
