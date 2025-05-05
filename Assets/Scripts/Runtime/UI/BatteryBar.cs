using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace AGGtH.Runtime.UI
{
    public class BatteryBar : MonoBehaviour
    {
        [SerializeField] protected List<Sprite> energyTicks = new List<Sprite>();
        [SerializeField] protected Image image;


        public void SetActiveTicks(int numActive)
        {
            image.sprite = energyTicks[numActive];
        }
    }
}
