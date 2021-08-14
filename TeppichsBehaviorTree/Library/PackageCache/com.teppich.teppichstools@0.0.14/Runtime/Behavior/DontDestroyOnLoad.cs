using UnityEngine;

namespace TeppichsTools.Behavior
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Destroy(this);
        }
    }
}