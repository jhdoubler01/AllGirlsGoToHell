using UnityEngine;
using AGGtH.Runtime.Card;
using AGGtH.Runtime.Settings;

namespace AGGtH.Runtime.Managers
{
    public class GameManager : MonoBehaviour
    {
        public GameManager() { }
        public static GameManager Instance { get; private set; }

        [SerializeField] private CardBase cardPrefab;
        [SerializeField] private Transform cardParentTransform;
        [SerializeField] private CardCollectionManager cardCollectionManager;
        [SerializeField] private GameplayData gameplayData;

        public CardBase BuildAndGetCard(CardData targetData, Transform parent)
        {
            var clone = Instantiate(GameplayData.CardPrefab, parent);
            clone.SetCard(targetData);
            return clone;
        }

    }
}
