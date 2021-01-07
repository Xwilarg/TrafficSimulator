using System;
using System.Collections.Generic;
using UnityEngine;

namespace Debug
{
    public class DebugManager : MonoBehaviour
    {
        public static DebugManager S { set; get; }

        private List<RaycastInfo> _raycasts = new List<RaycastInfo>();
        private List<(HitInfo, DateTime)> _hits = new List<(HitInfo, DateTime)>();

        private void Awake()
        {
            S = this;
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
        public bool RaycastWithDebug(Vector3 origin, Vector3 direction, float size, Color color, out RaycastHit hit)
        {
            var isHit = Physics.Raycast(origin, direction, out hit, size);
            _raycasts.Add(new RaycastInfo(origin, isHit ? hit.point : origin + (direction.normalized * size), color));
            if (isHit)
                _hits.Add((new HitInfo(hit.point, Color.red), DateTime.Now.AddMilliseconds(500)));
            return isHit;
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
                Gizmos.color = r.Color;
                Gizmos.DrawLine(r.Origin, r.Destination);
            }
            foreach (var h in _hits)
            {
                Gizmos.color = h.Item1.Color;
                Gizmos.DrawSphere(h.Item1.Position, .5f);
            }
            _raycasts.Clear();
            _hits.RemoveAll(x => DateTime.Now > x.Item2);
        }
    }
}
