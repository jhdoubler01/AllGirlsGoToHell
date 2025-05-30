using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;


namespace AGGtH.Runtime.UI
{
    public abstract class SegmentedHealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject segmentBase;

        public List<Image> FillHeartsList = new List<Image>();
        public List<Image> ArmorList = new List<Image>();

        [SerializeField] private float currentFillTotal;
        private float currentBlockAmt;

        #region Cache
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
            foreach(var armor in ArmorList)
            {
                RemoveAllBlock();
                armor.gameObject.SetActive(true);
            }
            currentFillTotal = FillHeartsList.Count;
        }

        #region Public Methods
        public virtual void AddNewSegments(int numToAdd = 1)
        {
            for (int i = 0; i <= numToAdd; i++)
            {
                Transform newSegment = Instantiate(SegmentBase, transform).transform;
                newSegment.SetAsFirstSibling();
                FillHeartsList.Insert(0, newSegment.GetChild(0).GetComponent<Image>());
                FillHeartsList[0].fillAmount = 1;
                ArmorList.Insert(1, newSegment.GetChild(1).GetComponent<Image>());
            }

        }
        public virtual void OnHealthChanged(float currentHealth, int maxHealth)
        {
            if (maxHealth > FillHeartsList.Count)
            {
                AddNewSegments(maxHealth - FillHeartsList.Count);
            }
            currentFillTotal = GetCurrentFillTotal();

            float diff = currentFillTotal - currentHealth;
            //if damage has been taken
            if (diff == 0) return;

            else if (diff > 0)
            {
                if(currentBlockAmt > 0)
                {
                    diff = RemoveBlock(diff);
                }
                if(diff <= 0) { return; }
                TakeDamage(diff);
            }
            else GainHealth(diff);
            currentFillTotal = GetCurrentFillTotal();
        }
        public virtual void AddBlock(float blockAmount)
        {
            RemoveAllBlock();
            float diff = blockAmount;
            for(int i= ArmorList.Count - 1; i>= 0; i--)
            {
                if (diff <= 0) { break; }

                ArmorList[i].fillAmount = FillHeartsList[i].fillAmount;
                diff -= ArmorList[i].fillAmount;
            }
        }
        //returns remainder if the amtToRemove is more than the currentBlockAmt so you take damage
        public virtual float RemoveBlock(float amtToRemove, bool removeAll = false)
        {
            float remainder = amtToRemove - currentBlockAmt;

            if(amtToRemove >= currentBlockAmt || removeAll) { RemoveAllBlock(); }
            else
            {
                float diff = amtToRemove;

                for (int i = ArmorList.Count - 1; i >= 0; i--)
                {

                    if (diff <= 0) { break; }
                    if (ArmorList[i].fillAmount != 0)
                    {
                        if (diff == 0.5f)
                        {
                            ArmorList[i].fillAmount -= diff;
                            diff = 0;
                        }
                        else
                        {
                            diff -= ArmorList[i].fillAmount;
                            ArmorList[i].fillAmount = 0;
                        }

                    }
                }
                currentBlockAmt -= amtToRemove;
            }

            return remainder;
        }
        public void RemoveAllBlock()
        {
            Debug.Log("remove all block");
            foreach (var armor in ArmorList)
            {
                armor.fillAmount = 0;
            }
            currentBlockAmt = 0;
        }
        #endregion

        #region Private Methods

        private float GetCurrentFillTotal()
        {
            float fill = 0;
            foreach (var heart in FillHeartsList)
            {
                fill += heart.fillAmount;
            }
            return fill;
        }
        //NOTE damage taken should only be a whole number or end in .5
        private void TakeDamage(float damageTaken)
        {
            Debug.Log("take damage");
            float diff = damageTaken;
            for (int i = FillHeartsList.Count - 1; i >= 0; i--)
            {
                if (diff <= 0) { break; }
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
        #endregion
    }
}
