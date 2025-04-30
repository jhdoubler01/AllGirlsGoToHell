using UnityEngine;
using AGGtH.Runtime.Managers;
using AGGtH.Runtime.Enums;
using AGGtH.Runtime.Extensions;
using TMPro;

namespace AGGtH.Runtime.UI
{
    public class CombatCanvas : CanvasBase
    {
        [Header("Info Bars")]
        [SerializeField] private SpritesheetHandler playerEnergyBar;
        [SerializeField] private SegmentedHealthBar playerHealthBar;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI drawPileTextField;
        [SerializeField] private TextMeshProUGUI discardPileTextField;
        [SerializeField] private TextMeshProUGUI exhaustPileTextField;
        [SerializeField] private TextMeshProUGUI energyTextTextField;

        [Header("Panels")]
        [SerializeField] private GameObject combatWinPanel;
        [SerializeField] private GameObject combatLosePanel;

        public SegmentedHealthBar PlayerHealthBar => playerHealthBar;
        public TextMeshProUGUI DrawPileTextField => drawPileTextField;
        public TextMeshProUGUI DiscardPileTextField => discardPileTextField;
        public TextMeshProUGUI ManaTextTextField => energyTextTextField;
        public GameObject CombatWinPanel => combatWinPanel;
        public GameObject CombatLosePanel => combatLosePanel;

        public TextMeshProUGUI ExhaustPileTextField => exhaustPileTextField;

        private void Awake()
        {

        }
        #region Public Methods
        public void SetPileTexts()
        {
            //DrawPileTextField.text = $"{CollectionManager.DrawPile.Count.ToString()}";
            //DiscardPileTextField.text = $"{CollectionManager.DiscardPile.Count.ToString()}";
            //ExhaustPileTextField.text = $"{CollectionManager.ExhaustPile.Count.ToString()}";
            //ManaTextTextField.text = $"{GameManager.PersistentGameplayData.CurrentMana.ToString()}/{GameManager.PersistentGameplayData.MaxMana}";
            //playerEnergyBar.SetNewImage(GameManager.PersistentGameplayData.CurrentEnergy);
        }
        #endregion
    }
}
