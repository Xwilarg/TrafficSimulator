using UnityEngine;

namespace TrafficSimulator
{
    /// <summary>
    /// Destroy any object that get triggers into the current one
    /// </summary>
    public class DestroyOnTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Destroy(other.transform.parent.gameObject);
        }
    }
}