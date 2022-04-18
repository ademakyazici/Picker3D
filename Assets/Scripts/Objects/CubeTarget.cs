using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTarget : Collectible
{
    public override void Start()
    {
        base.Start();
        levelObjectID = 0;
    }

    public override void Release()
    {
        base.Release();
        
    }


}
