using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteLevelPopUp : MonoBehaviour
{

    public void onDeleteButtonClicked()
    {      
        LevelEditorManager.Instance.DeleteLevel();       
    }

    public void onCancelButtonClicked()
    {       
        this.gameObject.SetActive(false);
    }
}
