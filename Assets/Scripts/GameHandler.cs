using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif




public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance;
    public InputField mainInputField;
    public string myText;


    public int highestScore;
    public Text highScores;



    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

   


    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        SaveHighestScore();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif

    }

    public void ResetPassword()
    {
        highestScore = 0;
    }

    private void Start()
    {
        
        LoadHighestScore();
    }

    private void LateUpdate()
    {
        UpdateName();
        UpdateHighScore();
        
    }

    public void UpdateName()
    {
        myText = mainInputField.text;

    }

    public void UpdateHighScore()
    {
        highScores.text = "HighScore: " + highestScore;
    }

    [System.Serializable]
    class SaveData
    {
        public int highestScore;
    }

    public void SaveHighestScore()
    {
        SaveData data = new SaveData();
        data.highestScore = highestScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighestScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);
            highestScore = data.highestScore;
        }
    }
}
    