using UnityEngine;
using TMPro;

namespace AGGtH.Runtime.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text dialogueBox;
        [SerializeField] private TMP_Text energyBox;
        public UIManager() { }
        public static UIManager Instance { get; private set; }

        public void SetDialogueBoxText(string dialogue)
        {
            dialogueBox.text = dialogue;
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
