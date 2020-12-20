using UnityEngine;

public class DebugManager : MonoBehaviour
{
    private void OnDrawGizmos()
    {
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
    }
}
