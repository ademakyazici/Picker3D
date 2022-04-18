using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {

    }

    private string LevelsPath => Application.dataPath + "/Levels/";
    private string TempPath => Application.dataPath+"/";
    private string fileExtension = ".txt";
    private string SavePath = "";


    public void Save(int levelNo)
    {
        string path = LevelsPath + levelNo + fileExtension;
        var state = LoadFile(levelNo, path);
        CaptureState(state);
        SaveFile(state, levelNo, path);
    }

    public void SaveTemp()
    {
        string path = TempPath + "0" + fileExtension;
        var state = LoadFile(0, path);
        CaptureState(state);
        SaveFile(state, 0, path);
    }

    public void Load(int levelNo)
    {
        string path = LevelsPath + levelNo + fileExtension;
        var state = LoadFile(levelNo, path);
        RestoreState(state,levelNo);
    }

    public void LoadTemp()
    {
        string path = TempPath + "0" + fileExtension;
        var state = LoadFile(0, path);
        RestoreState(state,null);
    }

    private void SaveFile(object state, int levelNo, string SavePath)
    {
        using (var stream = File.Open(SavePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
            Debug.Log("Level has been saved as Level " + levelNo);
        }
    }

    private Dictionary<string, object> LoadFile(int levelNo, string SavePath)
    {
        if (!File.Exists(SavePath))
        {
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(SavePath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    private void CaptureState(Dictionary<string, object> state)
    {
        foreach (var saveable in GameObject.FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.ID] = saveable.CaptureState();
        }
    }

    private void RestoreState(Dictionary<string, object> state,int? levelNo)
    {
        foreach (var saveable in GameObject.FindObjectsOfType<SaveableEntity>())
        {
            if (state.TryGetValue(saveable.ID, out object value))
            {
                saveable.RestoreState(value,levelNo);               
            }
        }
    }

    public bool CheckIfLevelExists(int levelNo)
    {
        SavePath = LevelsPath + levelNo + fileExtension;
        if (File.Exists(SavePath))
            return true;
        else
            return false;
    }

    public void DeleteExistingLevel(int levelNo)
    {
        SavePath = LevelsPath + levelNo + fileExtension;
        try
        {
            bool deleted = false;
            if (File.Exists(SavePath))
            {
                // no error
                File.Delete(SavePath);

                Debug.Log("Level: " + levelNo + "has been deleted. Higher level name's have been reorganized.");
                deleted = true;
            }
            else
            {
                Debug.Log("Level " + levelNo + " cannot be found, thus cannot be deleted.");
            }
            // but still exists
            if (File.Exists(SavePath))
            {
                throw new IOException(string.Format("Failed to delete file: '{0}'.", SavePath));
            }

            if (deleted)
                OrganizeLevels();
        }
        catch (Exception ex)
        {
            throw ex;
        }


    }

    public int FindLastCreatedLevel()
    {
        DirectoryInfo d = new DirectoryInfo(LevelsPath);
        int biggest = 0;
        foreach (var file in d.GetFiles("*" + fileExtension))
        {
            string name = file.Name;
            if (name.Contains(fileExtension))
                name = name.Replace(fileExtension, "");
            else
                Debug.LogWarning("Wrong file type in Levels Folder!! All levels should be saved in .txt format");

            bool isNumeric = int.TryParse(name, out int levelNo);
            if (isNumeric)
            {
                if (levelNo > biggest)
                    biggest = levelNo;
            }
            else
            {
                Debug.Log("There is a non numeric level file in Levels Folder. All Level files should be named numeratically.");
            }

        }
        return biggest;
    }

    private int? GetLevelNoAsInt(FileInfo file)
    {
        string name = file.Name;
        if (name.Contains(fileExtension))
            name = name.Replace(fileExtension, "");
        else
            Debug.LogWarning("Wrong file type in Levels Folder!! All levels should be saved in .txt format");

        bool isNumeric = int.TryParse(name, out int levelNo);

        if (isNumeric)
            return levelNo;
        else
            return null;
    }

    //IF A LEVEL IS DELETED, HIGHER LEVEL NUMBERS SHOULD BE REORGANIZED.
    private void OrganizeLevels()
    {
        DirectoryInfo d = new DirectoryInfo(LevelsPath);
        List<int> levelNos = new List<int>();
        FileInfo[] levelInfos = d.GetFiles("*" + fileExtension);
        foreach (var file in levelInfos)
        {
            int? levelNo = GetLevelNoAsInt((FileInfo)file);
            if (levelNo != null)
            {
                levelNos.Add((int)levelNo);
            }
            else
            {
                Debug.Log("There is a non numeric level file in Levels Folder. All Level files should be named numeratically.");
            }
        }
        levelNos.Sort();
        int? breakLevelIx = null;
        for (int i = 1; i < levelNos.Count; i++)
        {
            if (levelNos[i] != levelNos[i - 1] + 1)
            {
                breakLevelIx = i;
                break;
            }

        }

        if (breakLevelIx != null)
        {
            int lastCorrect = levelNos[(int)(breakLevelIx - 1)];
            int a = 1;
            for (int i = (int)breakLevelIx; i < levelNos.Count; i++)
            {
                string oldPath = LevelsPath + levelNos[i] + fileExtension;
                string newPath = LevelsPath + (lastCorrect + a) + fileExtension;
                if (File.Exists(oldPath))
                    File.Move(oldPath, newPath);
                a++;
            }
        }
        else
        {
            Debug.Log("All levels are already organized");
        }


    }
}
