using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableEntity : MonoBehaviour
{
    [SerializeField] private string id;
    public string ID => id;

    [ContextMenu("Generate ID")]
    private void GenerateID() => id = Guid.NewGuid().ToString();

    public object CaptureState()
    {
        var state = new Dictionary<string, object>();

        foreach (var saveable in GetComponents<ISaveable>())
        {
            state[saveable.GetType().BaseType.ToString()] = saveable.CaptureState();
        }

        return state;
    }

    public void RestoreState(object state,int? levelNo)
    {
        var stateDictionary = (Dictionary<string, object>)state;

        foreach (var saveable in GetComponents<ISaveable>())
        {
            string typeName = saveable.GetType().BaseType.ToString();
            
            if (stateDictionary.TryGetValue(typeName, out object value))
            {
                saveable.RestoreState(value, levelNo);
            }
            else
                Debug.LogError("Load type mismatch");
        }
    }

}
