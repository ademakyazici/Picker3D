using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeUpPickable : Pickable
{
    [SerializeField] private float scaleUpRatio;

    private void Start()
    {
        levelObjectID = 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Scoop>(out Scoop scoop))
        {
            scoop.ScaleUp(scaleUpRatio);
            onPick();
        }
    }

    protected override void onPick()
    {
        base.onPick();
    }

    
}
