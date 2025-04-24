using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


namespace AGGtH.Runtime.UI
{
    public class SegmentedHealthBar : MonoBehaviour
    {
        [SerializeField] private int numSegments;
        [SerializeField] private GameObject segmentBase;

        [SerializeField] private Image[] fillHearts;

        [SerializeField] private float currentFillTotal;

        void Start()
        {
            Setup();
        }

        //makes sure all the hearts are at 100% fill
        void Setup()
        {
            foreach(var heart in fillHearts)
            {
                heart.fillAmount = 1;
            }
        }
        //NOTE damage taken should only be a whole number or end in .5
        public void TakeDamage(float damageTaken)
        {
            float diff = damageTaken;
            for (int i = fillHearts.Length - 1; i >= 0; i--)
            {
                if (diff == 0) { break; }
                if (fillHearts[i].fillAmount != 0)
                {
                    if(diff == 0.5f)
                    {
                        fillHearts[i].fillAmount -= diff;
                        diff = 0;
                    }
                    else
                    {
                        diff -= fillHearts[i].fillAmount;
                        fillHearts[i].fillAmount = 0;
                    }

                }
            }
        }
        public void GainHealth(float healthGained)
        {
            float diff = healthGained;
            for (int i = 0; i < fillHearts.Length; i++)
            {
                if (diff == 0) { break; }
                if (fillHearts[i].fillAmount < 1)
                {
                    if (diff == 0.5f)
                    {
                        fillHearts[i].fillAmount += diff;
                        diff = 0;
                    }
                    else
                    {
                        diff -= 1 - fillHearts[i].fillAmount;
                        fillHearts[i].fillAmount = 1;
                    }

                }
            }
        }
    }
}
