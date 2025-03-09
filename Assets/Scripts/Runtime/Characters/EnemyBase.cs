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

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            Debug.Log($"{enemyData.EnemyName} took {damage} damage. Remaining health: {currentHealth}");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void ApplyDebuff(DebuffType debuff)
        {
            if (!debuffList.Contains(debuff))
            {
                debuffList.Add(debuff);
                Debug.Log($"{enemyData.EnemyName} is affected by {debuff}");
            }
        }

        private void Die()
        {
            Debug.Log($"{enemyData.EnemyName} has been defeated.");
            Destroy(gameObject);
        }

        /* public void AttackPlayer()
        {
            // Implement attack logic here
            Debug.Log($"{enemyData.EnemyName} attacks the player!");
        } */
    }
}
