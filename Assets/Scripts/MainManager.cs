using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{

   

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    public int m_Points;
    
    private bool m_GameOver = false;
    public Text ScoreText1;

    public int highScore;
    public bool recordBroken = false;


   

    // Start is called before the first frame update
    void Start()
    {
        highScore = GameHandler.Instance.highestScore;
      

        NameReference();



        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {

        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {

            UpdateHighScore();
            GameHandler.Instance.UpdateHighScore();
            NameReference();

            GameHandler.Instance.SaveRecordBreaker();
            GameHandler.Instance.SaveHighestScore();
           


            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

   public void NameReference()
    {
      ScoreText1.text = GameHandler.Instance.myText + ": Highscore: " + GameHandler.Instance.highestScore;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
        GameHandler.Instance.rootCanvas2.SetActive(true);
    }

   
    public void UpdateHighScore()
    {
        
        if(m_Points < highScore)
        {
            recordBroken = false;
            return;

        } else if(m_Points > highScore)
        {
            highScore = m_Points;
            recordBroken = true;

            GameHandler.Instance.highestScore = highScore;
        }

         

    }
    
}
