using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace AGGtH.Runtime.UI
{
    public class EnemyCanvas : CharacterCanvas
    {
        [Header("Enemy Canvas Settings")]
        [SerializeField] private Image intentImage;
        //[SerializeField] private TextMeshProUGUI nextActionValueText;
        public Image IntentImage => intentImage;
        //public TextMeshProUGUI NextActionValueText => nextActionValueText;
    }
}
