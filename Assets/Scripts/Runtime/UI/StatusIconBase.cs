using UnityEngine;
using AGGtH.Runtime.Data.Containers;
using TMPro;
using UnityEngine.UI;

namespace AGGtH.Runtime.UI
{
    public class StatusIconBase : MonoBehaviour
    {
        [SerializeField] private Image statusImage;
        [SerializeField] private TextMeshProUGUI statusValueText;

        public StatusIconData MyStatusIconData { get; private set; } = null;
        public Image StatusImage => statusImage;
        public TextMeshProUGUI StatusValueText => statusValueText;

        public void SetStatus(StatusIconData statusIconData)
        {
            MyStatusIconData = statusIconData;
            StatusImage.sprite = statusIconData.IconSprite;
        }
        public void SetStatusValue(float statusValue)
        {
            StatusValueText.text = statusValue.ToString();
        }
    }
}
