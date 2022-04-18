using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEditorManager : LevelConstructor
{
    public static LevelEditorManager Instance;

    [Header("Object Info")]
    [SerializeField, ReadOnly] private List<GameObject> roads = new List<GameObject>();
    [SerializeField, ReadOnly] private List<GameObject> levelObjects = new List<GameObject>();

    [Header("Prefabs")]
    [SerializeField] private GameObject scoopPrefab;
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private GameObject collectBoxPrefab;
    [SerializeField] private GameObject[] levelObjectPrefabs;

    [SerializeField, ReadOnly] private List<Transform> lastAdded = new List<Transform>();

    private string loadedLevel;

    [Header("Level Save Variables")]
    private int? levelNo;
    public int? LevelNo
    {
        get { return levelNo; }
        set
        {
            levelNo = value;
            LevelEditorCanvasManager.Instance.SetLevelName(value);
        }
    }

    private float levelZLength;
    private Color roadColor = Color.cyan;
    public int? targetCollectibleCount;



    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        AddRoad();
        AddScoop();
    }

    private void AddScoop()
    {
        Instantiate(scoopPrefab);
    }

    public void AddRoad()
    {
        GameObject road = Instantiate(roadPrefab, new Vector3(0, 0, roads.Count * roadPrefab.transform.GetChild(0).GetComponent<Renderer>().bounds.size.z), Quaternion.identity);
        roads.Add(road);
        lastAdded.Add(road.transform);
        RepositionCollectBox();
        GridManager.Instance.GenerateGrid(roads.Count, roadPrefab.transform.GetChild(0).GetComponent<MeshRenderer>());
        ChangeRoadColor(roadColor);

    }

    private void RepositionCollectBox()
    {
        CollectBox collectBox = GameObject.FindObjectOfType<CollectBox>();
        if (collectBox == null)
            Instantiate(collectBoxPrefab, new Vector3(0, 0, (roads.Count) * roadPrefab.transform.GetChild(0).GetComponent<Renderer>().bounds.size.z), Quaternion.identity);
        else
            collectBox.transform.parent.transform.position = new Vector3(0, 0, (roads.Count) * roadPrefab.transform.GetChild(0).GetComponent<Renderer>().bounds.size.z);
    }

    public void AddLevelObject(int ix, Node node)
    {
        GameObject levelObject = Instantiate(levelObjectPrefabs[ix], new Vector3(node.position.x, levelObjectPrefabs[ix].GetComponent<MeshRenderer>().bounds.size.y * 0.7f, node.position.z), levelObjectPrefabs[ix].transform.rotation);
        levelObjects.Add(levelObject);
        lastAdded.Add(levelObject.transform);
        GridManager.Instance.FillNode(node, levelObject.transform);
        Debug.Log(levelObject.name + " has been added to the : " + node.position);
    }

    public void AddLevelObject(int ix, Vector3 pos)
    {
        GameObject levelObject = Instantiate(levelObjectPrefabs[ix], new Vector3(pos.x, levelObjectPrefabs[ix].GetComponent<MeshRenderer>().bounds.size.y * 0.7f, pos.z), levelObjectPrefabs[ix].transform.rotation);
        levelObjects.Add(levelObject);
        lastAdded.Add(levelObject.transform);
        GridManager.Instance.FillNode(GridManager.Instance.FindClosestNode(pos), levelObject.transform);
    }

    public void ChangeRoadColor()
    {
        roadColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        for (int i = 0; i < roads.Count; i++)
        {
            roads[i].transform.GetChild(0).GetComponent<MeshRenderer>().material.color = roadColor;
        }
    }

    public void ChangeRoadColor(Color roadColor)
    {
        for (int i = 0; i < roads.Count; i++)
        {
            roads[i].transform.GetChild(0).GetComponent<MeshRenderer>().material.color = roadColor;
        }
        this.roadColor = roadColor;
    }

    public void RemoveLevelObject(GameObject objectToRemove, bool destroy)
    {
        if (roads.Contains(objectToRemove.gameObject))
        {
            if (objectToRemove.transform.GetChild(0).tag.Equals("StraightRoad"))
            {
                roads.Remove(objectToRemove.gameObject);
                RepositionCollectBox();
            }
        }
        else if (objectToRemove.GetComponent<LevelObject>() != null)
        {
            if (levelObjects.Contains(objectToRemove.gameObject))
                levelObjects.Remove(objectToRemove.gameObject);
        }
        
            lastAdded.Remove(objectToRemove.transform);


        if (destroy)
        {
            Destroy(objectToRemove);
        }

    }

    public void SetTargetCount(int target)
    {
        targetCollectibleCount = target;
        CollectBox collectBox = GameObject.FindObjectOfType<CollectBox>();
        collectBox.SetTargetScore(target);
    }

    public void RevertLast()
    {
        if (lastAdded.Count > 0)
        {
            int last = lastAdded.Count - 1;
            if (last != 0)
            {
                Transform lastGO = lastAdded[lastAdded.Count - 1];
                RemoveLevelObject(lastGO.gameObject, true);
            }

        }

    }

    public bool SaveConditionsMet()
    {
        if (levelObjects.Count <= 0)
        {
            LevelEditorCanvasManager.Instance.ShowNotification("An empty level cannot be saved or tested. Add some collectýbles before you save.", Color.white);
            return false;
        }
        else if (targetCollectibleCount == null || targetCollectibleCount == 0)
        {
            LevelEditorCanvasManager.Instance.ShowNotification("Please enter level's score target before you save or test.", Color.white);
            return false;
        }

        return true;
    }

    public void SaveLevel()
    {
        if (SaveConditionsMet())
        {
            if (levelNo != null)
            {
                int level = (int)levelNo;
                SaveManager.Instance.Save(level);
            }
            else
            {
                int level = SaveManager.Instance.FindLastCreatedLevel() + 1;
                SaveManager.Instance.Save(level);
            }
            RestartEditor();
        }

    }

    public void SaveTemp()
    {
        SaveManager.Instance.SaveTemp();
    }

    public void LoadLevel(int levelNo)
    {
        if (SaveManager.Instance.CheckIfLevelExists(levelNo))
        {
            DestroyLevelObjects();
            SaveManager.Instance.Load(levelNo);
        }
        else
            LevelEditorCanvasManager.Instance.ShowNotification("Level:" + levelNo + " doesn't exýst. ", Color.red);
    }

    public void LoadTemp()
    {
        SaveManager.Instance.LoadTemp();
    }

    public void DeleteLevel()
    {
        if (levelNo == null || levelNo == 0)
        {
            LevelEditorCanvasManager.Instance.ShowNotification("No previously saved level has been loaded. Nothing to delete", Color.white);
            return;
        }
        else
        {
            SaveManager.Instance.DeleteExistingLevel((int)levelNo);
            RestartEditor();

        }
    }

    public void RestartEditor()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DestroyLevelObjects()
    {
        while (lastAdded.Count > 1)
        {
            RemoveLevelObject(lastAdded[lastAdded.Count - 1].gameObject, true);
        }
      

        GameObject.FindObjectOfType<CollectBox>().ClearCollectibleList();
        GridManager.Instance.GenerateGrid(roads.Count, roadPrefab.transform.GetChild(0).GetComponent<MeshRenderer>());

    }

    public override object CaptureState()
    {
        levelZLength = roads.Count * roadPrefab.transform.GetChild(0).GetComponent<Renderer>().bounds.size.z + collectBoxPrefab.transform.GetChild(1).GetComponent<Renderer>().bounds.size.z;
        List<int> levelObjectIDs = new List<int>();
        List<Vector3S> levelObjectPositons = new List<Vector3S>();

        float[] roadRGB = { roadColor.r, roadColor.g, roadColor.b, roadColor.a };

        for (int i = 0; i < levelObjects.Count; i++)
        {
            levelObjectIDs.Add(levelObjects[i].GetComponent<LevelObject>().LevelObjectID);
            levelObjectPositons.Add(levelObjects[i].transform.position);
        }
        return new LevelInfo
        {
            RoadCount = roads.Count,
            LevelZLength = levelZLength,
            RoadColor = roadRGB,
            TargetCollectibleCount = (int)targetCollectibleCount,
            LevelObjectIDs = levelObjectIDs.ToArray(),
            LevelObjectPositions = levelObjectPositons.ToArray()

        };
    }

    public override void RestoreState(object data, int? level)
    {
        var state = (LevelInfo)data;
        while (roads.Count < state.RoadCount)
        {
            AddRoad();
        }
        ChangeRoadColor(new Color(state.RoadColor[0], state.RoadColor[1], state.RoadColor[2], state.RoadColor[3]));
        SetTargetCount(state.TargetCollectibleCount);
        FindObjectOfType<Scoop>().ResetScoop();
        for (int i = 0; i < state.LevelObjectIDs.Length; i++)
        {
            AddLevelObject(state.LevelObjectIDs[i], state.LevelObjectPositions[i]);
        }

        if (level != null && level != 0)
            LevelNo = level;

    }

    private void onLevelEnded(bool successful)
    {
        if (successful)
            LevelEditorCanvasManager.Instance.ShowNotification("LEVEL SUCCESSFUL. GOOD JOB!", Color.green);
        else
            LevelEditorCanvasManager.Instance.ShowNotification("LEVEL FAILED. TRY AGAIN", Color.red);
    }

    private void OnEnable()
    {
        EventManager.LevelEnded += onLevelEnded;
    }

    private void OnDisable()
    {
        EventManager.LevelEnded -= onLevelEnded;
    }





}



