using UnityEngine;
using UnityEngine.UI;


namespace AGGtH.Runtime.UI
{
    public class SegmentedHealthBar : MonoBehaviour
    {
        [SerializeField] private int numSegments;
        [SerializeField] private GameObject segmentBase;

        [SerializeField] private Image[] fillHearts;
        [SerializeField] private int currentFillTotal;

        public void UpdateHeartFill()
        {

        }

    }
}
