using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickable : LevelObject
{
      
    protected virtual void onPick()
    {
        this.gameObject.SetActive(false);
    }
}
