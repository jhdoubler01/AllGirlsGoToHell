using UnityEngine;
using TMPro;
using AGGtH.Runtime.UI;

namespace AGGtH.Runtime.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text dialogueBox;
        [SerializeField] private TMP_Text energyBox;
        public UIManager() { }
        public static UIManager Instance { get; private set; }

        [Header("Canvases")]
        [SerializeField] private CombatCanvas combatCanvas;

        #region Cache
        public CombatCanvas CombatCanvas => combatCanvas;
        #endregion

        public void SetDialogueBoxText(string dialogue)
        {
            dialogueBox.text = dialogue;
        }
        public void SetEnergyBoxText(int energy)
        {
            energyBox.text = energy.ToString();
        }

        public void UpdateEnergyDisplay(int startEnergy, int targetEnergy, float duration){
            StopAllCoroutines("AnimateEnergyDisplay");
            StartCoroutine(AnimateEnergyDisplay(startEnergy, targetEnergy, duration));
        }
        private IEnumerator AnimateEnergyDisplay(int startEnergy, int targetEnergy, float duration){
            float elapsed = 0f;

            while(elapsed < duration){
                elapsed += Time.deltaTime;
                int newEnergy = (int)Mathf.Lerp(startEnergy, targetEnergy, elapsed / duration);
                energyBox.text = newEnergy.ToString();
                yield return null;
            }
            energyBox.text = targetEnergy.ToString();
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
