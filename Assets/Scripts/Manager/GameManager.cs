using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField,ReadOnly]private bool playerCanMove;
    public bool PlayerCanMove { get => playerCanMove; }

    private bool gameEnded = false;
    public bool GameEnded => gameEnded;

    [SerializeField] private float collectibleReleaseForce;
    public float CollectibleReleaseForce { get => collectibleReleaseForce; }

    [SerializeField] private float newCollectibleWaitTimeLimit = 1f;
    public float NewCollectibleWaitTimeLimit { get => newCollectibleWaitTimeLimit; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    public void MovementActive(bool active)
    {
        playerCanMove = active;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    

    private void onGameStarted()
    {
        MovementActive(true);
    }

    private void onLevelEnded(bool successful)
    {
        gameEnded = true;
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
