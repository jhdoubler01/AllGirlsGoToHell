using UnityEngine;

namespace AGGtH.Runtime
{
    public class AudioManager : MonoBehaviour
    {
        public AudioManager() { }
        public static AudioManager Instance { get; private set; }
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
