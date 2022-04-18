using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SelectibleButton : MonoBehaviour
{

    protected Button button;
    protected bool isSelected;

    private void Awake()
    {
        button = GetComponent<Button>();
        isSelected = false;
    }

    private void Start()
    {
        button.onClick.AddListener(Select);
    }

    public virtual void Select()
    {
        isSelected = true;
        button.image.color = Color.green;

        foreach (SelectibleButton button in GameObject.FindObjectsOfType<SelectibleButton>())
        {
            if (button != this)
                button.Deselect();
        }


        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Deselect);
    }
    public virtual void Deselect()
    {
        isSelected = false;
        button.image.color = Color.white;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(Select);
    }
}
