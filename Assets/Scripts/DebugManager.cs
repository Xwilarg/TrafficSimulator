using UnityEngine;

public class DebugManager : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (var node in GameObject.FindGameObjectsWithTag("Node"))
        {
            var nodeC = node.GetComponent<Node>();
            Debug.Log(nodeC.name);
            if (nodeC.NextNode != null)
            {
                Gizmos.DrawLine(nodeC.transform.position, nodeC.NextNode.transform.position);
            }
        }
    }
}
