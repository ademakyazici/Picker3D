using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public float xPos;
    public float yPos;
    public float zPos;

    public Vector3 position;

    public Transform slotGameObject;

    public Node(float xPos, float yPos, float zPos)
    {
        this.xPos = xPos;
        this.yPos = yPos;
        this.zPos = zPos;

        position = new Vector3(xPos, yPos, zPos);
    }

    public bool CheckIfEmpty()
    {
        return slotGameObject == null;
    }

    public void FillNode(Transform transform)
    {
        if (slotGameObject == null)
            slotGameObject = transform;
    }
}
