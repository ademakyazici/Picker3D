using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndGamePopUp : MonoBehaviour
{
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button restartButton;

    public void ArrangePopUp(bool successful)
    {
        string text = successful ? "GOOD JOB! \n LEVEL PASSED!" : "LEVEL FAILED. \n TRY AGAIN!";
        infoText.text = text;
        nextLevelButton.gameObject.SetActive(successful);
        restartButton.gameObject.SetActive(!successful);
    }

    public void onNextLevelButtonClicked()
    {
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
        GameManager.Instance.RestartGame();
    }

    public void onRestartButtonClicked()
    {
        GameManager.Instance.RestartGame();
    }
}
