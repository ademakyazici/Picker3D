using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AddCollectibleButton : SelectibleButton
{
        
    [SerializeField] private int collectibleIx;
       
    private void Update()
    {
        if (isSelected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit) && hit.collider.tag.Equals("StraightRoad"))
                {
                    Node closest = GridManager.Instance.FindClosestNode(hit.point);
                    if (closest.CheckIfEmpty())
                    {
                        LevelEditorManager.Instance.AddLevelObject(collectibleIx, closest);                        
                    }
                    else
                    {
                        Debug.Log("That node is not empty. Delete before you add or select another.");
                    }
                        
                }
            }
           

        }
    }

}
