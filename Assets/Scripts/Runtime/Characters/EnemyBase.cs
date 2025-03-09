using UnityEngine;
using Systems.Collections.Generic;
using AGGtH.Runtime.Enums;

namespace AGGtH.Runtime.Characters.Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;
        private int currentHealth;
        private List<DebuffType> debuffList = new List<DebuffType>();

        private void Start()
        {
            if(enemyData == null)
            {
                Debug.LogError("EnemyData is not assigned to asset.");
                return;
            }

            currentHealth = enemyData.HealthAmt;
            Debug.Log($"{enemyData.EnemyName} has spawned with {currentHealth} health.");
        }
    }
}
