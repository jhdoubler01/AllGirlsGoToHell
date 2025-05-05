using UnityEngine;
using TMPro;
using AGGtH.Runtime.Extensions;
using AGGtH.Runtime.UI;

namespace AGGtH.Runtime.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text dialogueBox;
        [SerializeField] private TMP_Text energyBox;
        [SerializeField] private SegmentedHealthBar playerHealthBar;

        public SegmentedHealthBar PlayerHealthBar => playerHealthBar;
        public UIManager() { }
        public static UIManager Instance { get; private set; }

        [Header("Canvases")]
        [SerializeField] private CombatCanvas combatCanvas;
        [SerializeField] private RewardCanvas rewardCanvas;

        #region Cache
        public CombatCanvas CombatCanvas => combatCanvas;
        public RewardCanvas RewardCanvas => rewardCanvas;
        #endregion

        public void SetDialogueBoxText(string dialogue, Color? color = null)
        {
            dialogueBox.text = dialogue;
            dialogueBox.color = color ?? Color.white;
        }
        public void SetEnergyBoxText(int energy)
        {
            energyBox.text = energy.ToString();
        }
        private void Awake()
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
    }
}
