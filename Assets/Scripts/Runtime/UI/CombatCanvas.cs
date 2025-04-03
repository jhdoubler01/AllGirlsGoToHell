using UnityEngine;
using AGGtH.Runtime.Managers;
using AGGtH.Runtime.Enums;
using TMPro;

namespace AGGtH.Runtime.UI
{
    public class CombatCanvas : CanvasBase
    {
        [SerializeField] private SpritesheetHandler playerEnergyBar;

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
            playerEnergyBar.SetNewImage(GameManager.PersistentGameplayData.CurrentEnergy);
        }
        #endregion
    }
}
