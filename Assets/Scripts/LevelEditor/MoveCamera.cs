using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float zoomMultiplier = 2f;
    
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime),Space.World);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime),Space.World);
        }

        this.GetComponent<Camera>().fieldOfView -= Input.mouseScrollDelta.y*zoomMultiplier;
    }
}
