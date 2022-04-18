using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelEditorCanvasManager : MonoBehaviour
{
    public static LevelEditorCanvasManager Instance;

    [SerializeField] private GameObject savePopUp;
    [SerializeField] private GameObject loadPopUp;
    [SerializeField] private GameObject deletePopUp;
    [SerializeField] private GameObject setTargetPopUp;

    [SerializeField] private TMP_Text levelText;

    [SerializeField] private TMP_Text notificationText;

    void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
    }   

    public void SetLevelName(int? level)
    {
        if (level != null)
            levelText.text = "Level : " + level;
        else
            levelText.text = "New Level";
    }

    public void onSaveButtonClicked()
    {
        savePopUp.SetActive(true);
    }

    public void onLoadButtonClicked()
    {
        loadPopUp.SetActive(true);
    }

    public void onDeleteButtonClicked()
    {
        deletePopUp.SetActive(true);
    }

    public void onSetTargetButtonClicked()
    {
        setTargetPopUp.SetActive(true);
    }

    public void onNewLevelButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowNotification(string text, Color color)
    {
        StartCoroutine(ShowNotificationCoroutine(text, color));
    }

    public void DeselectButtons()
    {
        foreach(SelectibleButton button in FindObjectsOfType<SelectibleButton>())
        {
            button.Deselect();
        }

        
    }

    private IEnumerator ShowNotificationCoroutine(string text, Color color)
    {       
        float duration = text.ToCharArray().Length * 0.06f;
        notificationText.text = text;
        notificationText.color = color;
        notificationText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        notificationText.gameObject.SetActive(false);
    }
}
