using UnityEngine;

namespace Common
{
    public class AutoDisable : MonoBehaviour
    {
        private void Awake() => gameObject.SetActive(false);
    }
}
