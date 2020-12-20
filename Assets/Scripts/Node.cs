using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Node : MonoBehaviour
{
    public Node NextNode;

    private void Start()
    {   
        if (NextNode == null)
        {
            var allNodes = GameObject.FindGameObjectsWithTag("Node").Select(x => x.GetComponent<Node>());
            var closest = allNodes.Where(x => x != this).OrderBy(x => Vector3.Distance(x.transform.position, transform.position)).FirstOrDefault();
            Assert.IsNotNull(closest);
            NextNode = closest;
        }
    }
}
