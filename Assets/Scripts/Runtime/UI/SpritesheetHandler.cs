using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace AGGtH.Runtime
{
    public class SpritesheetHandler : MonoBehaviour
    {
        public Sprite[] SpriteList;
        public Image ImageToChange;

        public void SetNewImage(int newImageNum)
        {
            if (newImageNum-1 > SpriteList.Length) return;
            ImageToChange.sprite = SpriteList[newImageNum];
        }
    }
}
