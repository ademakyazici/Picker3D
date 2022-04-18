using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FallerDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        Collectible collectible = collision.GetComponent<Collectible>();
        if (collectible != null )
        {
            Destroy(collectible.gameObject);
        }
      
    }
}
