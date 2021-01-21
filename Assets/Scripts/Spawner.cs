using UnityEngine;

namespace TrafficSimulator
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        private float _minTime, _maxTime;

        [SerializeField]
        private GameObject[] _prefabs;

        private float _currTime;

        private void Start()
        {
            ResetTimer();
        }

        private void Update()
        {
            _currTime -= Time.deltaTime;
            if (_currTime < 0f)
            {
                var randomGO = _prefabs[Random.Range(0, _prefabs.Length)];
                var go = Instantiate(randomGO, transform);
                go.transform.position = transform.position;
                ResetTimer();
            }
        }

        private void ResetTimer()
        {
            _currTime = Random.Range(_minTime, _maxTime);
        }
    }
}