using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControlScript : MonoBehaviour {

    public static GameControlScript instance;

    public int levelCounter = 0;

    private int controlValue;

    public float startPos;
    public float lastPos;

    public bool isTouch;
    public bool nextTabLevel;
    public bool gameOver;
    public bool newReadyHex;
    public bool isNotHexNull;

    private bool restartController;

    public List<GameObject> hexagonObjectList = new List<GameObject>();

    [Header("Canvas UI")]
    public GameObject swipePanel;
    public GameObject restartButton;
    public GameObject passText;
    public GameObject animationSwipe;
    public Text levelText;
    
    private GameObject sceneObject;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("LevelCount") && levelCounter < PlayerPrefs.GetInt("LevelCount"))
            levelCounter = PlayerPrefs.GetInt("LevelCount");

        if (PlayerPrefs.GetInt("LevelCount") >= 5)
            restartController = true;

        levelText.text = "Level " + (levelCounter+1);
        passText.SetActive(false);
        animationSwipe.SetActive(true);
    }

    void Update()
    {
        if (!gameOver)
        {
            if (isNotHexNull && newReadyHex)
            {
                isNotHexNull = false;
                Destroy(sceneObject);
                newReadyHex = false;
                levelText.text = "Level " + (levelCounter + 1);
            }
            CreateSceneHexagon();
        }
        else
        {
            if(sceneObject == null)
                restartButton.SetActive(true);
        }
    }

    private void CreateSceneHexagon()
    {
        if(sceneObject == null && levelCounter < 5)
        {
            sceneObject = Instantiate(hexagonObjectList[levelCounter], Vector3.zero, Quaternion.identity);
        }
        else if(sceneObject == null && levelCounter >= 5)
        {
            int randomList = Random.Range(0, hexagonObjectList.Count);
            if (controlValue == randomList)
                return;

            controlValue = randomList;
            if(PlayerPrefs.HasKey("LevelSetReturn") && restartController)
            {
                restartController = false;
                randomList = PlayerPrefs.GetInt("LevelSetReturn");
            }
            sceneObject = Instantiate(hexagonObjectList[randomList], Vector3.zero, Quaternion.identity);
            PlayerPrefs.SetInt("LevelSetReturn", randomList);
        }
    }

    public void RestartButton()
    {
        gameOver = false;
        isNotHexNull = false;
        restartController = true;
        restartButton.SetActive(false);
    }

    public void SwipeFunction(bool _isTouch, float _startPos, float _lastPos)
    {
        isTouch = _isTouch;
        lastPos = Mathf.Round(_lastPos);
        startPos = Mathf.Round(_startPos);
    }

    public void DeleteAllKey()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetNextLevelButon()
    {
        nextTabLevel = true;
    }


}
