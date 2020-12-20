using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Road : MonoBehaviour
{
    private void Start()
    {
        var allNodes = GameObject.FindGameObjectsWithTag("Node").Select(x => x.GetComponent<Node>());
        foreach (var node in transform.GetComponentsInChildren<Node>())
        {
            if (node.NextNode == null)
            {
                var closest = allNodes.Where(x => x != node).OrderBy(x => Vector3.Distance(x.transform.position, node.transform.position)).FirstOrDefault();
                Assert.IsNotNull(closest);
                node.NextNode = closest;
            }
        }
    }
}
