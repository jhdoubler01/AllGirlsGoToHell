using UnityEngine;
using AGGtH.Runtime.Managers;

namespace AGGtH.Runtime.UI
{
    public class CanvasBase : MonoBehaviour
    {
        protected EncounterManager EncounterManager => EncounterManager.Instance;
        protected CardCollectionManager CardCollectionManager => CardCollectionManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected UIManager UIManager => UIManager.Instance;
        public virtual void OpenCanvas()
        {
            gameObject.SetActive(true);
        }


        public virtual void CloseCanvas()
        {
            gameObject.SetActive(false);
        }

        public virtual void ResetCanvas()
        {

        }

    }
}


