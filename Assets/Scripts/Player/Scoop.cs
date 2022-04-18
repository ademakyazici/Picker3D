using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoop : MonoBehaviour
{
    [SerializeField,ReadOnly] private List<Transform> collectibles= new List<Transform>();
    private float initialScaleX;

    private void Start()
    {
        initialScaleX = transform.localScale.x;
    }

    public void ResetScoop()
    {
        transform.localScale = new Vector3(initialScaleX, transform.localScale.y, transform.localScale.z);
    }

    public void ScaleUp(float scale)
    {
        transform.localScale = new Vector3(transform.localScale.x + transform.localScale.x*scale, transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerEnter(Collider collision)
    {
        Collectible collectible = collision.GetComponent<Collectible>();
        if (collectible!=null && !collectibles.Contains(collision.transform))
        {
            collectibles.Add(collision.transform);            
        }
        else if (collision.tag.Equals("Stopper"))
        {
            GameManager.Instance.MovementActive(false);
            foreach (Transform c in collectibles)
            {
                if(c!=null)
                    c.GetComponent<Collectible>().Release();
            }
            collision.transform.parent.GetComponentInChildren<CollectBox>().StartEndGameCheckCoroutine();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Collectible collectible = collision.GetComponent<Collectible>();
        if (collectible!=null && collectibles.Contains(collision.transform))
        {
            collectibles.Remove(collision.transform);
        }
    }
    
}
