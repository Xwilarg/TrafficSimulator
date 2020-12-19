using UnityEngine;

public class Node : MonoBehaviour
{
    public Node NextNode;

    private int _id = Id++;

    public static int Id = 0;
}
