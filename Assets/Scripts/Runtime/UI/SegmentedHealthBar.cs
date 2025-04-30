using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;


namespace AGGtH.Runtime.UI
{
    public abstract class SegmentedHealthBar : MonoBehaviour
    {
        [SerializeField] private int numSegments;
        [SerializeField] private GameObject segmentBase;

        public List<Image> FillHeartsList = new List<Image>();
        public List<Image> ArmorList = new List<Image>();

        [SerializeField] private float currentFillTotal;

        #region Cache
        public int NumSegments => numSegments;
        public GameObject SegmentBase => segmentBase;


        public float CurrentFillTotal => currentFillTotal;
        #endregion

        void Start()
        {
            Setup();
        }

        //makes sure all the hearts are at 100% fill
        void Setup()
        {
            foreach(var heart in FillHeartsList)
            {
                heart.fillAmount = 1;
            }
            numSegments = FillHeartsList.Count;
            currentFillTotal = numSegments;
        }
        public virtual void AddNewSegments(int numToAdd = 1)
        {
            for(int i=0; i < numToAdd; i++)
            {
                var newSegment = Instantiate(segmentBase, transform, false);
                newSegment.transform.SetAsFirstSibling();
                FillHeartsList.Insert(0, newSegment.GetComponent<Image>());
                ArmorList.Insert(0, newSegment.GetComponentInChildren<Image>());
            }
        }
        public void OnHealthChanged(float currentHealth, int maxHealth)
        {
            if(maxHealth > numSegments)
            {
                AddNewSegments(maxHealth - numSegments);
            }

            float diff = currentFillTotal - currentHealth;
            //if damage has been taken
            if (diff == 0) return;
            else if (diff > 0)
            {
                TakeDamage(diff);
            }
            else GainHealth(diff);
        }
        //NOTE damage taken should only be a whole number or end in .5
        private void TakeDamage(float damageTaken)
        {
            float diff = damageTaken;
            for (int i = FillHeartsList.Count - 1; i >= 0; i--)
            {
                if (diff == 0) { break; }
                if (FillHeartsList[i].fillAmount != 0)
                {
                    if(diff == 0.5f)
                    {
                        FillHeartsList[i].fillAmount -= diff;
                        diff = 0;
                    }
                    else
                    {
                        diff -= FillHeartsList[i].fillAmount;
                        FillHeartsList[i].fillAmount = 0;
                    }

                }
            }
        }
        private void GainHealth(float healthGained)
        {
            float diff = healthGained;
            for (int i = 0; i < FillHeartsList.Count; i++)
            {
                if (diff == 0) { break; }
                if (FillHeartsList[i].fillAmount < 1)
                {
                    if (diff == 0.5f)
                    {
                        FillHeartsList[i].fillAmount += diff;
                        diff = 0;
                    }
                    else
                    {
                        diff -= 1 - FillHeartsList[i].fillAmount;
                        FillHeartsList[i].fillAmount = 1;
                    }

                }
            }
        }
    }
}
