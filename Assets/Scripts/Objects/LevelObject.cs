using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    [SerializeField, ReadOnly] protected int levelObjectID;
    public int LevelObjectID => levelObjectID;
}
