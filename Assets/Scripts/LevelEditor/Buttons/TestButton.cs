using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    private Button testButton;
    private Text testButtonText;
    private bool testActive=false;
    private Vector3 camInitialPos;

    [SerializeField] private Camera testCam;
    [SerializeField] private Camera editCam;

    void Start()
    {
        testButton = GetComponent<Button>();
        testButton.onClick.AddListener(TaskOnClick);
        testActive = GameManager.Instance.PlayerCanMove;
        camInitialPos = Camera.main.transform.position;
    }

  

    void TaskOnClick()
    {
        testActive = !testActive;

        if (testActive)
        {
            if (LevelEditorManager.Instance.SaveConditionsMet())
            {
                testCam.gameObject.SetActive(true);
                editCam.gameObject.SetActive(false);
                GameManager.Instance.MovementActive(true);
                GridManager.Instance.gameObject.SetActive(false);
                ActivateButtons(false);
                LevelEditorManager.Instance.SaveTemp();
                EventManager.GameStartedEvent();
                testButton.GetComponent<Image>().color = Color.red;
            }
            else
                testActive = !testActive;
                   
        }
        else
        {
            testCam.gameObject.SetActive(false);
            editCam.gameObject.SetActive(true);
            GameManager.Instance.MovementActive(false);
            GridManager.Instance.gameObject.SetActive(true);
            ActivateButtons(true);
            testButton.GetComponent<Image>().color = Color.green;

            GameObject.FindObjectOfType<Scoop>().transform.position = new Vector3(0, 0, 20);
            testCam.transform.position = camInitialPos;
            editCam.transform.position = camInitialPos;
            LevelEditorManager.Instance.DestroyLevelObjects();
            LevelEditorManager.Instance.LoadTemp();

            
        }
                                                           
    }

    void ActivateButtons(bool isActive)
    {
        Button[] buttons = GameObject.FindObjectsOfType<Button>();
        foreach (Button b in buttons)
        {
            if (b.GetComponent<TestButton>() == null)
                b.interactable = isActive;

            if(b.GetComponent<AddCollectibleButton>() != null)
            {
                b.GetComponent<AddCollectibleButton>().Deselect();
            }
        }
    }
        
    
}
