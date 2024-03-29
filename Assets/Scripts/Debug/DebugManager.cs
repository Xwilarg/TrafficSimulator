using System;
using System.Collections.Generic;
using System.Linq;
using TrafficSimulator.Vehicle;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TrafficSimulator.Debug
{
    public class DebugManager : MonoBehaviour
    {
        public static DebugManager S { set; get; }

        private Dictionary<string, RaycastInfo> _raycasts = new Dictionary<string, RaycastInfo>();
        private List<HitInfo> _hits = new();

        [SerializeField]
        [Tooltip("Reference to child text to display")]
        private Text _debugText;
        private GameObject _debugPanel;

        [SerializeField]
        [Tooltip("Prefab to display on top of vehicle we are targeting")]
        private GameObject _pointerPrefab;
        private GameObject _pointerInstance = null;

        [SerializeField]
        [Tooltip("Current object we are targetting, can be set in the inspector to target an object on game start")]
        private VehicleController _currentDebug;

        private void Awake()
        {
            S = this;
        }

        private void Start()
        {
            _debugPanel = _debugText.transform.parent.gameObject;
            if (_currentDebug != null) // Value was set in the inspector
            {
                SetDebugInfo(_currentDebug);
            }
        }

        /// <summary>
        /// Do a raycast and display it using Gizmos
        /// </summary>
        /// <param name="origin">Where the raycast start</param>
        /// <param name="direction">Direction of the raycast from its origin</param>
        /// <param name="size">Size of the raycast</param>
        /// <param name="color">Color in which the raycast will be displayed</param>
        /// <param name="hit">Information returned about the raycast</param>
        /// <returns>Boolean telling if the raycast hit something or not</returns>
        public bool RaycastWithDebug(string id, Vector3 origin, Vector3 direction, float size, Color color, out RaycastHit hit, int layer)
        {
            var isHit = Physics.Raycast(origin, direction, out hit, size, layer);
            var raycast = new RaycastInfo(origin, isHit ? hit.point : origin + (direction.normalized * size), color);
            if (_raycasts.ContainsKey(id))
                _raycasts[id] = raycast;
            else
                _raycasts.Add(id, raycast);
            if (isHit)
                _hits.Add(new HitInfo(hit.point, Color.red));
            return isHit;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) // We did a left click, but not on an UI element
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    if (GetComponentInParents<VehicleController>(hit.collider.gameObject) != null) // We clicked on a vehicle
                    {
                        _currentDebug = GetComponentInParents<VehicleController>(hit.collider.gameObject);
                        SetDebugInfo(_currentDebug);
                    }
                    else
                    {
                        Destroy(_pointerInstance);
                        _debugPanel.SetActive(false);
                    }
                }
                else
                {
                    Destroy(_pointerInstance);
                    _debugPanel.SetActive(false);
                }
            }

            // If we are following a car progression
            if (_currentDebug != null)
                _debugText.text = _currentDebug.GetDebugInformation();
        }

        private void SetDebugInfo(VehicleController vehicle)
        {
            _debugText.text = vehicle.GetDebugInformation();
            _debugPanel.SetActive(true);
            if (_pointerInstance == null)
                _pointerInstance = Instantiate(_pointerPrefab);
            _pointerInstance.transform.position = vehicle.gameObject.transform.position + (Vector3.up * 1.5f);
            _pointerInstance.transform.parent = vehicle.gameObject.transform;
        }

        /// <summary>
        /// Search a component by going throught the current object and all parents
        /// </summary>
        /// <typeparam name="T">Type of the component to search for</typeparam>
        /// <param name="go">Starting object</param>
        /// <returns>Component T or null if not found</returns>
        public T GetComponentInParents<T>(GameObject go) where T : Component
        {
            do
            {
                var c = go.GetComponent<T>();
                if (c != null)
                    return c;
                go = go.transform.parent.gameObject;
            } while (go.transform.parent != null);
            return null;
        }

        private void OnDrawGizmos()
        {
            // Printing node and links between them
            Gizmos.color = Color.blue;
            foreach (var node in GameObject.FindGameObjectsWithTag("Node"))
            {
                var nodeC = node.GetComponent<Node>();
                foreach (var iNode in nodeC.NextNodes)
                {
                    if (iNode != null)
                        Gizmos.DrawLine(nodeC.transform.position, iNode.transform.position);
                }
            }

            // Printing all raycast
            foreach (var r in _raycasts)
            {
                Gizmos.color = r.Value.Color;
                Gizmos.DrawLine(r.Value.Origin, r.Value.Destination);
            }
            foreach (var h in _hits)
            {
                Gizmos.color = h.Color;
                Gizmos.DrawSphere(h.Position, .5f);
            }
            _hits.RemoveAll(x => DateTime.Now > x.ExpireTime);
            for (int i = _raycasts.Keys.Count - 1; i >= 0; i--) // TODO: Refactor this
            {
                var currKey = _raycasts.Keys.ToArray()[i];
                if (DateTime.Now > _raycasts[currKey].ExpireTime)
                    _raycasts.Remove(currKey);
            }
        }

        public void BreakCurrentCar()
        {
            _currentDebug.Break();
        }
    }
}
