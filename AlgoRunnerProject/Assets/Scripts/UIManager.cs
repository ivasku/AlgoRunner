using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    private bool isBFSEnabled = false;
    private bool isDijkstraEnabled = false;
    private bool isGBFSEnabled = false;
    private bool isAstarEnabled = false;

    private bool isMapOptionTxtEnabled = false;
    private bool isMapOptionGenerateEnabled = false;

    private int m_startXPos;
    private int m_startYPos;
    private int m_goalXPos;
    private int m_goalYPos;

    private static UIManager _instance;

    public Text algName;
    public Text EndAlgName;
    public Text EndAlgSearchTime;
    public Text EndAlgPathLength;
    public Text EndAlgExploredNodes;

    public Button showStatistics;

    public GameObject endGamePanel;

    public Text AlgSearchTime;
    public Text AlgPathLength;
    public Text AlgExplNodes;

    public static UIManager getInstance()
    {
        if (_instance == null)
        {
            GameObject gm = new GameObject("UiManager");
            gm.AddComponent<UIManager>();
        }

        return _instance;
    }

    private void OnEnable()
    {
        updateData();
    }

    private void Awake()
    {
        _instance = this;
    }

    private void updateData()
    {
        int isBFSEnabledInt = PlayerPrefs.GetInt("isBFSEnabled");
        int isDijkstraEnabledInt = PlayerPrefs.GetInt("isDijkstraEnabled");
        int isGBFSEnabledInt = PlayerPrefs.GetInt("isGBFSEnabled");
        int isAstarEnabledInt = PlayerPrefs.GetInt("isAstarEnabled");

        int isMapOptionTxtEnabledInt = PlayerPrefs.GetInt("isMapOptionTxtEnabled");
        int isMapOptionGenerateEnabledInt = PlayerPrefs.GetInt("isMapOptionGenerateEnabled");

        if (isBFSEnabledInt == 1)
        {
            isBFSEnabled = true;
        }

        if (isDijkstraEnabledInt == 1)
        {
            isDijkstraEnabled = true;
        }

        if (isGBFSEnabledInt == 1)
        {
            isGBFSEnabled = true;
        }

        if (isAstarEnabledInt == 1)
        {
            isAstarEnabled = true;
        }

        if (isMapOptionTxtEnabledInt == 1)
        {
            isMapOptionTxtEnabled = true;
        }

        if (isMapOptionGenerateEnabledInt == 1)
        {
            isMapOptionGenerateEnabled = true;
        }

        m_startXPos = PlayerPrefs.GetInt("startXPos");
        m_startYPos = PlayerPrefs.GetInt("startYPos");

        m_goalXPos = PlayerPrefs.GetInt("goalXPos");
        m_goalYPos = PlayerPrefs.GetInt("goalYPos");
    }

    public bool getBFS()
    {
        return isBFSEnabled;
    }

    public bool getDijkstra()
    {
        return isDijkstraEnabled;
    }

    public bool getGBFS()
    {
        return isGBFSEnabled;
    }

    public bool getAstar()
    {
        return isAstarEnabled;
    }

    public bool getMapOptionTxt()
    {
        return isMapOptionTxtEnabled;
    }

    public bool getMapOptionGenerate()
    {
        return isMapOptionGenerateEnabled;
    }

    public int getStartX()
    {
        return m_startXPos;
    }

    public int getStartY()
    {
        return m_startYPos;
    }

    public int getGoalX()
    {
        return m_goalXPos;
    }

    public int getGoalY()
    {
        return m_goalYPos;
    }

    public void setAlgNames(string name)
    {
        algName.text = "Alg name: " + name;
        EndAlgName.text = "Alg name: " + name;
    }

    public void openEndGamePanel()
    {
        endGamePanel.SetActive(true);
    }

    public void closeEndGamePanel()
    {
        endGamePanel.SetActive(false);
    }

    public void enableShowStatisticsButton()
    {
        showStatistics.interactable = true;
    }

    public void disableeShowStatisticsButton()
    {
        showStatistics.interactable = false;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Home");
    }

    public void setAlgStatisticsData(string searchTime, string pathLength, string exploredNodes)
    {
        AlgSearchTime.text = "Search time: " + searchTime;
        AlgPathLength.text = "Path length: " + pathLength;
        AlgExplNodes.text = "Num of explored nodes: " + exploredNodes;
    }
}
