using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text;
using System.Collections;
using AGGtH.Runtime.Enums;

namespace AGGtH.Runtime.Characters.Enemy
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Characters/EnemyData")]

    public class EnemyData : ScriptableObject
    {
        [Header("Enemy Profile")]
        [SerializeField] private string id;
        [SerializeField] private string enemyName;
        [SerializeField] private int healthAmt;
        [SerializeField] private Sprite enemySprite;
        [SerializeField] private EnemyLoveLanguageType enemyLoveLanguageType;

/*         [Header("Action Settings")]
        [SerializeField] private bool usableWithoutTarget;
        [SerializeField] private bool exhaustAfterPlay;
        [SerializeField] private List<CardActionData> cardActionDataList = new List<CardActionData>(); */

        #region Cache
        public string Id { get => id; set => id = value; }
        public string EnemyName { get => enemyName; set => enemyName = value; }
        public int HealthAmt { get => healthAmt; set => healthAmt = value; }
        public Sprite EnemySprite { get => enemySprite; set => enemySprite = value; }
        public EnemyLoveLanguageType EnemyLoveLanguageType { get => enemyLoveLanguageType; set => enemyLoveLanguageType = value; }

        #endregion
    }
}
/*     [Serializable]
    public class CardActionData
    {
        [SerializeField] private ActionTargetType actionTargetType;
        [SerializeField] private CardActionType cardActionType;
        [SerializeField] private BuffType buffType;
        [SerializeField] private DebuffType debuffType;
        [SerializeField] private int damageAmt;
        [SerializeField] private int healAmt;
        [SerializeField] private int blockAmt;
        [SerializeField] private int drawCardAmt;
        [SerializeField] private int energyGainAmt;

        public ActionTargetType ActionTargetType { get => actionTargetType; set => actionTargetType = value; }
        public CardActionType CardActionType { get => cardActionType; set => cardActionType = value; }
        public BuffType BuffType { get => buffType; set => buffType = value; }
        public DebuffType DebuffType { get => debuffType; set => debuffType = value; }
        public int DamageAmt { get => damageAmt; set => damageAmt = value; }
        public int HealAmt { get => healAmt; set => healAmt = value; }
        public int BlockAmt { get => blockAmt; set => blockAmt = value; }
        public int DrawCardAmt { get => drawCardAmt; set => drawCardAmt = value; }
        public int EnergyGainAmt { get => energyGainAmt; set => energyGainAmt = value; }
    }
}
 */

