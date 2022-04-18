using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoopMovement : MonoBehaviour
{

    private Vector3 lastPos;
    private Vector3 firstPos;
    private Vector3 moveVec;
    [SerializeField] private float speedX = 2f;
    [SerializeField] private float speedZ = 0.5f;
    private Rigidbody rb;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();       

    }
    void Update()
    {

        if (GameManager.Instance.PlayerCanMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                
                firstPos = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            }
            if (Input.GetMouseButton(0))
            {
                //GET THE ASPECT RATIO OF THE SCREEN AND MULTIPLY IT WITH SPEEDX TO GET APPROXIMATELY THE SAME HORIZONTAL SPEED ON DIFFERENT SCREENS.
                float screenAspectRatio = Camera.main.aspect;
                lastPos = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
                moveVec.x = ((lastPos - firstPos)*speedX*200*screenAspectRatio).x;               
                moveVec.y = 0;
                moveVec.z = 0;               
                firstPos = lastPos;
                
            }
            if (Input.GetMouseButtonUp(0))
            {
                moveVec.x = 0;
                lastPos = Vector3.zero;
            }            
            moveVec.z = speedZ;          
            rb.velocity = Vector3.Lerp(rb.velocity, moveVec,0.3f );
           
            


        }
        else
            rb.velocity = Vector3.zero;
       
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag.Equals("WallTag"))
        {
            rb.velocity = new Vector3(0,rb.velocity.y,rb.velocity.z);
        }
    }


}
