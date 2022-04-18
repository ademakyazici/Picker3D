using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private int xPerRoad;
    [SerializeField] private int zPerRoad;
    [SerializeField] private float xPercentage;
    [SerializeField] private float zPercentage;

    [SerializeField,ReadOnly] private List<Node> nodes = new List<Node>();
    [SerializeField]private Canvas gridCanvas;
    [SerializeField] private GameObject nodeSprite;

    
    
    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    public void GenerateGrid(int roadCount, MeshRenderer toGrid)
    {
        ClearNodes();
        int zCount = zPerRoad * roadCount;
        Vector3 gridSize = new Vector3(toGrid.bounds.size.x * xPercentage, 0, toGrid.bounds.size.z* roadCount * zPercentage);
        float xDifference = gridSize.x / xPerRoad;
        float zDifference = gridSize.z / zCount;
        float x00Pos = (-gridSize.x+xDifference ) / 2;

        for (int x = 0; x < xPerRoad ; x++)
        {
            for (int z  = 0; z < zCount; z++)
            {
                Node node = new Node(x00Pos+xDifference*x, 0, zDifference*z);
                nodes.Add(node);
                Instantiate(nodeSprite, new Vector3(node.xPos,0.1f,node.zPos), Quaternion.Euler(new Vector3(90,0,0)), gridCanvas.transform);
                
            }
        }
    }

    public Node FindClosestNode(Vector3 position)
    {
        Node closest=null;
        float lastDistance = Mathf.Infinity;
        
        foreach(Node node in nodes)
        {
            float distance = Vector3.Distance(position, node.position);
            if (distance < lastDistance)
            {
                lastDistance = distance;
                closest = node;
            }
        }

        return closest;

    }

    public void FillNode(Node node,Transform transform)
    {
        node.FillNode(transform);
    }

    private void ClearNodes()
    {
        nodes.Clear();
        foreach (Transform child in gridCanvas.transform)
            Destroy(child.gameObject);
    }
}
