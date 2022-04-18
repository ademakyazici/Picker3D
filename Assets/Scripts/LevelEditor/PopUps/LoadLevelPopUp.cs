using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevelPopUp : MonoBehaviour
{
    [SerializeField] private InputField input;

    public void onLoadButtonClicked()
    {
        if(int.TryParse(input.text,out int level))    
            LevelEditorManager.Instance.LoadLevel(level);                  
        else        
            LevelEditorCanvasManager.Instance.ShowNotification("Input ýs not valýd. It should be numerical and not empty", Color.red);
        
        this.gameObject.SetActive(false);
    }

    public void onCancelButtonClicked()
    {
        input.text = "";
        this.gameObject.SetActive(false);
    }
}
