using UnityEngine;
using AGGtH.Runtime.Enums;

namespace AGGtH.Runtime.Data.Containers
{
    [CreateAssetMenu(fileName = "EnemyIntentionData", menuName = "Assets/Data/EnemyIntentionData")]
    public class EnemyIntentionData : ScriptableObject
    {
        [SerializeField] private EnemyIntentionType enemyIntentionType;
        [SerializeField] private Sprite intentionSprite;

        public EnemyIntentionType EnemyIntentionType => enemyIntentionType;

        public Sprite IntentionSprite => intentionSprite;
    }
}
