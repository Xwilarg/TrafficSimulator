using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace TrafficSimulator
{
    public class Node : MonoBehaviour
    {
        public Node[] NextNodes;

        private void Start()
        {
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