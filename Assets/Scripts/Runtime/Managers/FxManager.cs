using UnityEngine;
using DG.Tweening;

namespace AGGtH.Runtime.Managers
{
    public class FxManager : MonoBehaviour
    {
        public FxManager() { }
        public static FxManager Instance { get; private set; }
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
