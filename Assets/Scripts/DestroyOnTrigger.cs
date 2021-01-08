using UnityEngine;

namespace TrafficSimulator
{
    public class DestroyOnTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Destroy(other.gameObject);
        }
    }
}