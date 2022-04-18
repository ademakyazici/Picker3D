using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneretor : LevelConstructor
{
    [Header("Prefabs")]
    [SerializeField] private GameObject scoopPrefab;
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private GameObject collectBoxPrefab;
    [SerializeField] private GameObject[] levelObjectPrefabs;

    private LevelInfo currentLevelInfo;
    private LevelInfo nextLevelInfo;

    [SerializeField] private Transform currentLevelHolder;
    [SerializeField] private Transform nextLevelHolder;
    [SerializeField] private Transform extraHolder;
    private float? firstLevelZLength=null;
    private float? secondLevelZLength = null;
    
   
    private void Start()
    {
        int currentLevel = PlayerPrefs.GetInt("Level", 1);
        LoadLevel(currentLevel);
        LoadLevel(currentLevel + 1);

        GenerateLevels();

    }

    private void GenerateLevels()
    {
        //ADD ROADS AND COLLECT BOXES
        for (int i = 0; i < currentLevelInfo.RoadCount; i++)
        {
            AddRoad(currentLevelHolder, i, currentLevelInfo);
        }

        for (int a = 0; a < nextLevelInfo.RoadCount; a++)
        {
            AddRoad(nextLevelHolder, a, nextLevelInfo);
        }

        AddRoad(extraHolder, 0);

        //ADD LEVEL OBJECTS
        for (int i = 0; i < currentLevelInfo.LevelObjectIDs.Length; i++)
        {
            Vector3 pos = currentLevelInfo.LevelObjectPositions[i];
            AddLevelObject(currentLevelHolder,currentLevelInfo.LevelObjectIDs[i], pos);
        }

        for (int i = 0; i < nextLevelInfo.LevelObjectIDs.Length; i++)
        {
            Vector3 pos = nextLevelInfo.LevelObjectPositions[i];
            AddLevelObject(nextLevelHolder, nextLevelInfo.LevelObjectIDs[i], pos);
        }

        //ARRANGE LEVEL END CONDITIONS
        CollectBox collectBox = currentLevelHolder.gameObject.GetComponentInChildren<CollectBox>();
        collectBox.SetTargetScore(currentLevelInfo.TargetCollectibleCount);

        //MOVE NEXT LEVEL
        nextLevelHolder.position += new Vector3(0, 0, (float)firstLevelZLength);
        extraHolder.position+= new Vector3(0, 0, (float)(firstLevelZLength+secondLevelZLength));

        //CREATE SCOOP
        AddScoop();

        //ACTIVATE CAMERA
        Camera.main.transform.GetComponent<CameraFollow>().enabled = true;

        //CANVAS
        CanvasManager.Instance.CanvasStart();
    }

    private void AddScoop()
    {
        Instantiate(scoopPrefab,currentLevelHolder);
    }

    public void AddRoad(Transform parent,int roadNo,LevelInfo info)
    {       
        GameObject road = Instantiate(roadPrefab, new Vector3(0, 0, roadNo* roadPrefab.transform.GetChild(0).GetComponent<Renderer>().bounds.size.z), Quaternion.identity,parent);        
        RepositionCollectBox(parent,roadNo);
        road.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(info.RoadColor[0], info.RoadColor[1], info.RoadColor[2], info.RoadColor[3]);


    }

    public void AddRoad(Transform parent, float posZ)
    {
        GameObject road = Instantiate(roadPrefab, new Vector3(0, 0, posZ), Quaternion.identity, parent);
        road.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
    }

    private void RepositionCollectBox(Transform parent,int roadNo)
    {

        CollectBox collectBox = parent.gameObject.GetComponentInChildren<CollectBox>();
        if (collectBox == null)
            Instantiate(collectBoxPrefab, new Vector3(0, 0, (roadNo+1) * roadPrefab.transform.GetChild(0).GetComponent<Renderer>().bounds.size.z), Quaternion.identity,parent);
        else
            collectBox.transform.parent.transform.position = new Vector3(0, 0, (roadNo+1) * roadPrefab.transform.GetChild(0).GetComponent<Renderer>().bounds.size.z);
            
    }

    public void AddLevelObject(Transform parent,int ix, Vector3 pos)
    {
        GameObject levelObject = Instantiate(levelObjectPrefabs[ix], 
            new Vector3(pos.x, levelObjectPrefabs[ix].GetComponent<MeshRenderer>().bounds.size.y * 0.7f, pos.z), levelObjectPrefabs[ix].transform.rotation,parent);                 
    }


    public void SetTargetCount(int target)
    {      
        CollectBox collectBox = GameObject.FindObjectOfType<CollectBox>();
        collectBox.SetTargetScore(target);
    }

    public void LoadLevel(int levelNo)
    {
        if (SaveManager.Instance.CheckIfLevelExists(levelNo))
        {           
            SaveManager.Instance.Load(levelNo);
        }
        else
        {
            int last = SaveManager.Instance.FindLastCreatedLevel();
            int random=1;
            if(last<=2)
                random = Random.Range(1,last+1);
            else
            {
                random = Random.Range(1, last+1);
                while (random ==PlayerPrefs.GetInt("Level",1))
                {
                    random = Random.Range(1, last+1);
                }
            }
            SaveManager.Instance.Load(random);
        }   
    }

  

    public override object CaptureState()
    {
        return null;
    }

    public override void RestoreState(object data, int? levelNo)
    {
        var state = (LevelInfo)data;
        
        if (firstLevelZLength == null)
        {           
            currentLevelInfo = state;
            firstLevelZLength = currentLevelInfo.LevelZLength;
        }
        else
        {
            nextLevelInfo = state;
            secondLevelZLength = nextLevelInfo.LevelZLength;
        }

        /*
      int i = 0;
      while (i < state.RoadCount)
      {
          i++;
          Color color = new Color (state.RoadColor[0], state.RoadColor[1], state.RoadColor[2], state.RoadColor[3]);
          AddRoad(color);
      }

      SetTargetCount(state.TargetCollectibleCount);

      for (int i = 0; i < state.LevelObjectIDs.Length; i++)
      {
          AddLevelObject(state.LevelObjectIDs[i], state.LevelObjectPositions[i]);
      }

      if (level != null && level != 0)
          LevelNo = level;
      */
    }
}
