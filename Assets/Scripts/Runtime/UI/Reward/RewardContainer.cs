using UnityEngine;
using TMPro;
using UnityEngine.UI;
using AGGtH.Runtime.Enums;
using System;

namespace AGGtH.Runtime.UI.Reward
{
    public class RewardContainer : MonoBehaviour
    {
        [SerializeField] private Button rewardButton;
        [SerializeField] private Image rewardImage;
        [SerializeField] private TextMeshProUGUI rewardTypeText;
        [SerializeField] private TextMeshProUGUI rewardDescriptionText;

        public Button RewardButton => rewardButton;
        
        public void BuildReward(Sprite rewardSprite, string rewardDescription, RewardType rewardType)
        {
            rewardImage.sprite = rewardSprite;
            rewardDescriptionText.text = rewardDescription;
            rewardTypeText.text = Enum.GetName(typeof(RewardType), rewardType);
        }
    }
}
