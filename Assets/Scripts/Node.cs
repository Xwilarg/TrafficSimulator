using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace TrafficSimulator
{
    /// <summary>
    /// Represent a node, have a position and an array of next nodes
    /// </summary>
    public class Node : MonoBehaviour
    {
        public Node[] NextNodes;

        private void Start()
        {
            // If there is no next node, we go thought all the nodes in the map and set the closest as the next one
            if (NextNodes.Length == 0)
            {
                var allNodes = GameObject.FindGameObjectsWithTag("Node").Select(x => x.GetComponent<Node>());
                var closest = allNodes.Where(x => x != this).OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault();
                Assert.IsNotNull(closest);
                NextNodes = new[] { closest };
            }
        }
    }
}