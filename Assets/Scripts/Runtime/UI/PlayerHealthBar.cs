using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace AGGtH.Runtime.UI
{
    public class PlayerHealthBar : SegmentedHealthBar
    {
        public override void AddNewSegments(int numToAdd = 1)
        {
            base.AddNewSegments(numToAdd);
            for (int i = 0; i <= numToAdd; i++)
            {
                Transform newSegment = Instantiate(SegmentBase, transform).transform;
                newSegment.SetAsFirstSibling();
                FillHeartsList.Insert(0, newSegment.GetChild(0).GetComponent<Image>());
                FillHeartsList[0].fillAmount = 1;
                ArmorList.Insert(1, newSegment.GetChild(1).GetComponent<Image>());
            }
        }
    }
}
