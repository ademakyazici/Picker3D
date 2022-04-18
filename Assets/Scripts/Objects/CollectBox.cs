using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectBox : MonoBehaviour
{
    [SerializeField, ReadOnly] private List<Transform> collectibles = new List<Transform>();
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private int targetScore = 10;
    private Coroutine checkFinishCoroutine;
    

    void Start()
    {
        scoreText.text = collectibles.Count + " / " + targetScore;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTargetScore(int target)
    {
        targetScore = target;
        scoreText.text = collectibles.Count + " / " + targetScore;
    }

    public void ClearCollectibleList()
    {
        collectibles.Clear();
        checkFinishCoroutine = null;
        scoreText.text = collectibles.Count + " / " + targetScore;
    }

    public void StartEndGameCheckCoroutine()
    {
        if (checkFinishCoroutine == null)
            checkFinishCoroutine = StartCoroutine(CheckIfFinished());
    }

    private void OnTriggerEnter(Collider collision)
    {
        Collectible collectible = collision.GetComponent<Collectible>();
        if (collectible != null && !collectibles.Contains(collision.transform))
        {
            collectibles.Add(collision.transform);
            scoreText.text = collectibles.Count + " / " + targetScore;           
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Collectible collectible = collision.GetComponent<Collectible>();
        if (collectible != null && collectibles.Contains(collision.transform))
        {
            collectibles.Remove(collision.transform);
            scoreText.text = collectibles.Count + " / " + targetScore;
        }
    }

    private IEnumerator CheckIfFinished()
    {
        float time = 0;
        int collectibleGathered = 1;
        while (time<GameManager.Instance.NewCollectibleWaitTimeLimit)
        {
            if (collectibles.Count == collectibleGathered)
                time += Time.deltaTime;
            else
            {
                collectibleGathered = collectibles.Count;
                time = 0;
            }
                
            yield return new WaitForEndOfFrame();
        }

        if (collectibles.Count >= targetScore)
        {
            Debug.Log("Level Successful");
            EventManager.LevelEndedEvent(true);
        }
        else
        {
            Debug.Log("FAILED. TRY AGAIN");
            EventManager.LevelEndedEvent(false);
        }
            
    }
}
