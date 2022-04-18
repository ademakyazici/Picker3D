using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetCountPopUp : MonoBehaviour
{
    [SerializeField] private InputField input;
    private int? targetCount;

    private void OnEnable()
    {
        targetCount = LevelEditorManager.Instance.targetCollectibleCount;
        input.text = (targetCount!=null ? (int)targetCount:0).ToString();
    }

    public void onSetTargetButtonClicked()
    {
        if (!string.IsNullOrEmpty(input.text))
        {
            if (int.TryParse(input.text, out int result) && result>0)
            {
                targetCount = result;
                LevelEditorManager.Instance.SetTargetCount((int)targetCount);               
            }
            else
            {
                LevelEditorCanvasManager.Instance.ShowNotification("Please enter a number bigger than 0", Color.white);
            }                                
        }
        this.gameObject.SetActive(false);

    }
    public void onCancelButtonClicked()
    {
        input.text = targetCount.ToString();
        this.gameObject.SetActive(false);
    }
}
