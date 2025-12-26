#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class MenuUIHandler : MonoBehaviour
{
    public TMP_Text bestScoreText;
    public TMP_InputField playerName;
    public new string name = "";

    public string bestPlayer;
    public int bestScore;

    public static MenuUIHandler Instance;

    private void Awake() 
    {
        if(Instance != null) 
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadGameInfo();
    }

    private void Start() 
    {
        if(bestPlayer != "") 
        {
            bestScoreText.text = "Best Score : " + bestPlayer + " : " + bestScore;
        }
    }

    public void SetBestScore(int score)
    {
        if (score>bestScore)
        {
            bestScore =score;
            bestPlayer = name;
            SaveGameInfo();
            MainManager.Instance.BestScoreText.text = "Best Score: " + bestPlayer + " : " + bestScore;
        }
        Debug.Log( "Player: " + name + " Score: " + score);
    }
    public void StarNew()
    {
        if (playerName.text != "")
        {
            name = playerName.text;
            SceneManager.LoadScene(1);
        }
        else 
        Debug.LogWarning("Please enter name!");
    }

    public void Exit()
    {
    #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
    #else
        Application.Quit();
    #endif
    }

    [System.Serializable]
    class SaveData
    {
        public string name;
        public int bestScore;
    }

    public void SaveGameInfo()
    {
        SaveData data = new SaveData();
        data.name = bestPlayer;
        data.bestScore = bestScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadGameInfo()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayer = data.name;
            bestScore = data.bestScore;
        }
    }
        
}
