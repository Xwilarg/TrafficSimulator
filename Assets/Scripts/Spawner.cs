using UnityEngine;

namespace TrafficSimulator
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Time intervalle between 2 cars spawn")]
        private float _minTime, _maxTime;

        [SerializeField]
        [Tooltip("Prefab to spawn")]
        private GameObject[] _prefabs;

        [SerializeField]
        [Tooltip("Does spawn a car on start")]
        private bool _spawnOnStart;

        private float _currTime;

        private void Start()
        {
            if (_spawnOnStart)
                _currTime = 0f;
            else
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