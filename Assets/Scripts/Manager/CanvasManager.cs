using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{

    public static CanvasManager Instance;

    [SerializeField] private EndGamePopUp endGamePopUp;

    private TMP_Text tapToStartText;
    [SerializeField]private TMP_Text levelText;

    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        tapToStartText = transform.Find("TapToStartTM").GetComponent<TMP_Text>();              
            
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.Instance.GameEnded)
        {
            EventManager.GameStartedEvent();
            
        }
    }

    public void CanvasStart()
    {
        levelText.text = "Level : " + PlayerPrefs.GetInt("Level", 1);
        levelText.gameObject.SetActive(true);        
    }

    public void ActivateEndGamePop(bool active)
    {
        endGamePopUp.gameObject.SetActive(active);
    }

    private void onLevelEnded(bool successful)
    {
        ActivateEndGamePop(true);
        endGamePopUp.ArrangePopUp(successful);
    }

    private void onGameStarted()
    {              
        tapToStartText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        EventManager.GameStarted += onGameStarted;
        EventManager.LevelEnded += onLevelEnded;
    }

    private void OnDisable()
    {
        EventManager.GameStarted -= onGameStarted;
        EventManager.LevelEnded -= onLevelEnded;
    }
}
