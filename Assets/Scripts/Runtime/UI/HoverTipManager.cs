using UnityEngine;
using System;
using TMPro;
using System.Collections;
using System.Collections.Generic;


namespace AGGtH.Runtime
{
    public class HoverTipManager : MonoBehaviour
    {
        public HoverTipManager() { }
        public static HoverTipManager Instance { get; private set; }

        public TextMeshProUGUI tipText;
        public RectTransform tipWindow;

        public static Action<string, Vector2> OnMouseHover;
        public static Action OnMouseLoseFocus;
        private void OnEnable()
        {
            OnMouseHover += ShowTip;
            OnMouseLoseFocus += HideTip;
        }
        private void OnDisable()
        {
            OnMouseHover -= ShowTip;
            OnMouseLoseFocus -= HideTip;
        }
        void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }
        }
        void Start()
        {
            HideTip();
        }
        private void ShowTip(string tip, Vector2 mousePos)
        {
            tipText.text = tip;
            tipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 200 ? 200 : tipText.preferredWidth, tipText.preferredHeight);

            tipWindow.gameObject.SetActive(true);

        }
        private void HideTip()
        {
            tipText.text = default;
            tipWindow.gameObject.SetActive(false);
        }
    }
}
