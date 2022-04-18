using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveLevelPopUp : MonoBehaviour
{
    [SerializeField] private Text levelText;
    [SerializeField] private Text infoText;

    private void OnEnable()
    {        
        if (LevelEditorManager.Instance.LevelNo != null)
        {
            infoText.text = "This level will be overwritten: ";
            levelText.text = LevelEditorManager.Instance.LevelNo.ToString();
        }
        else
        {
            infoText.text = "This level will be saved as Level:";
            levelText.text = (SaveManager.Instance.FindLastCreatedLevel() + 1).ToString();
        }
        
    }
    public void onSaveButtonClicked()
    {
        LevelEditorManager.Instance.SaveLevel();
        this.gameObject.SetActive(false);
        
    }

    public void onCancelButtonClicked()
    {       
        this.gameObject.SetActive(false);
    }
}
