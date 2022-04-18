using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeleteGOButton : SelectibleButton
{
  

    private void Update()
    {
        if (isSelected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;              
                if (Physics.Raycast(ray, out hit) && hit.collider.tag.Equals("StraightRoad") )
                {
                    Node closest = GridManager.Instance.FindClosestNode(hit.point);                    
                    if (!closest.CheckIfEmpty())
                    {                        
                        LevelEditorManager.Instance.RemoveLevelObject(closest.slotGameObject.gameObject, true);
                    }
                }
                else
                {
                    Debug.Log("Raycast empty");
                }               
            }
        }       
    }

    
}
