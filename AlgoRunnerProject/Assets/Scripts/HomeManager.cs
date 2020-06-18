using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{

    public GameObject panel;
    public GameObject infoPanel;
    public Text infoPanelText;

    public Toggle BFS;
    public Toggle Dijkstra;
    public Toggle GBFS;
    public Toggle Astar;

    public Toggle MapOptionTxt;
    public Toggle MapOptionGenerate;

    public InputField StartXPos;
    public InputField StartYPos;

    public InputField GoalXPos;
    public InputField GoalYPos;

    private bool isBFSEnabled = true;
    private bool isDijkstraEnabled = false;
    private bool isGBFSEnabled = false;
    private bool isAstarEnabled = false;

    private bool isMapOptionTxtEnabled = true;
    private bool isMapOptionGenerateEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        BFS.onValueChanged.AddListener(delegate
        {
            BFSToggleValueChanged(BFS);
        });

        Dijkstra.onValueChanged.AddListener(delegate
        {
            DijkstraToggleValueChanged(Dijkstra);
        });

        GBFS.onValueChanged.AddListener(delegate
        {
            GBFSToggleValueChanged(GBFS);
        });

        Astar.onValueChanged.AddListener(delegate
        {
            AstarToggleValueChanged(Astar);
        });

        MapOptionTxt.onValueChanged.AddListener(delegate
        {
            MapOptionTxtToggleValueChanged(MapOptionTxt);
        });

        MapOptionGenerate.onValueChanged.AddListener(delegate
        {
            MapOptionGenerateToggleValueChanged(MapOptionGenerate);
        });

        StartXPos.text = "0";
        StartYPos.text = "4";
        GoalXPos.text = "9";
        GoalYPos.text = "4";
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void startGameScene()
    {

        if (!PlayerPrefs.HasKey("startXPos"))
        {

            Debug.LogError("Default Values!!!");
           
            checkForEmptyInputFields();

            int startXPos = 0;
            int startYPos = 4;
            int goalXPos = 9;
            int goalYPos = 4;

            checkInputDate(startXPos, startYPos, goalXPos, goalYPos);

            //save data from options
            PlayerPrefs.SetInt("startXPos", startXPos);
            PlayerPrefs.SetInt("startYPos", startYPos);
            PlayerPrefs.SetInt("goalXPos", goalXPos);
            PlayerPrefs.SetInt("goalYPos", goalYPos);

            PlayerPrefs.SetInt("isBFSEnabled", (isBFSEnabled ? 1 : 0));
            PlayerPrefs.SetInt("isMapOptionTxtEnabled", (isMapOptionTxtEnabled ? 1 : 0));

            //close panel
            panel.SetActive(false);
            
        }

        SceneManager.LoadScene("Game");
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void openOptions()
    {
        panel.SetActive(true);
        infoPanel.SetActive(false);
        PlayerPrefs.DeleteAll();
    }

    public void closeOptions()
    {
        
        checkForEmptyInputFields();

        int startXPos = int.Parse(StartXPos.text);
        int startYPos = int.Parse(StartYPos.text);
        int goalXPos = int.Parse(GoalXPos.text);
        int goalYPos = int.Parse(GoalYPos.text);

        checkInputDate(startXPos, startYPos, goalXPos, goalYPos);

        if (panel.activeSelf && !infoPanel.activeSelf)
        {    
            //save data from options
            PlayerPrefs.SetInt("startXPos", startXPos);
            PlayerPrefs.SetInt("startYPos", startYPos);
            PlayerPrefs.SetInt("goalXPos", goalXPos);
            PlayerPrefs.SetInt("goalYPos", goalYPos);

            writeSelectedAlg();
            writeSelectedMapGenerator();

            //close panel
            panel.SetActive(false);
        }

    }


    private void BFSToggleValueChanged(Toggle change)
    {    
        isBFSEnabled = change.isOn;   
    }

    private void DijkstraToggleValueChanged(Toggle change)
    {      
        isDijkstraEnabled = change.isOn;     
    }

    private void GBFSToggleValueChanged(Toggle change)
    {
        isGBFSEnabled = change.isOn;   
    }

    private void AstarToggleValueChanged(Toggle change)
    {      
        isAstarEnabled = change.isOn;  
    }

    private void MapOptionTxtToggleValueChanged(Toggle change)
    {      
        isMapOptionTxtEnabled = change.isOn;
    }

    private void MapOptionGenerateToggleValueChanged(Toggle change)
    {      
        isMapOptionGenerateEnabled = change.isOn;
    }

    public void closeInfoPanel()
    {
        infoPanel.SetActive(false);
    }


    private void checkInputDate(int startXPos, int startYPos, int goalXPos, int goalYPos)
    {
        if (startXPos > 9 || startXPos < 0)
        {
            // open mini message with text must enter valid position
            infoPanel.SetActive(true);
            infoPanelText.text = "Please enter values from 0 to 10 in all fields !!!";
        }

        if (startYPos > 9 || startYPos < 0)
        {
            // open mini message with text must enter valid position
            showInfoMessage("Please enter values from 0 to 9 in all fields !!!");
        }

        if (goalXPos > 9 || goalXPos < 0)
        {
            // open mini message with text must enter valid position
            showInfoMessage("Please enter values from 0 to 9 in all fields !!!");
        }

        if (goalYPos > 9 || goalYPos < 0)
        {
            // open mini message with text must enter valid position
            showInfoMessage("Please enter values from 0 to 9 in all fields !!!");
        }
    }

    private void checkForEmptyInputFields()
    {
        Debug.Log("StartXPos.text.Length " + StartXPos.text.Length);
        if (StartXPos.text.Length == 0)
        {
            StartXPos.text = "0";
            
        }

        if (StartYPos.text.Length == 0)
        {
            StartYPos.text = "4";
            
        }

        if (GoalXPos.text.Length == 0)
        {
            GoalXPos.text = "9";
            
        }

        if (GoalYPos.text.Length == 0)
        {
            GoalYPos.text = "4";
            
        }
    }

    private void showInfoMessage(string Text) {
        infoPanel.SetActive(true);
        infoPanelText.text = Text;
    }

    private void writeSelectedAlg()
    {
        if (isBFSEnabled)
        {        
            PlayerPrefs.SetInt("isBFSEnabled", (isBFSEnabled ? 1 : 0));
        }

        if (isDijkstraEnabled)
        {        
            PlayerPrefs.SetInt("isDijkstraEnabled", (isDijkstraEnabled ? 1 : 0));
        }

        if (isGBFSEnabled)
        {
            PlayerPrefs.SetInt("isGBFSEnabled", (isGBFSEnabled ? 1 : 0));
        }

        if (isAstarEnabled)
        {
            PlayerPrefs.SetInt("isAstarEnabled", (isAstarEnabled ? 1 : 0));
        }

    }

    private void writeSelectedMapGenerator()
    {
        if (isMapOptionTxtEnabled)
        {         
            PlayerPrefs.SetInt("isMapOptionTxtEnabled", (isMapOptionTxtEnabled ? 1 : 0));
        }
        else
        {        
            PlayerPrefs.SetInt("isMapOptionGenerateEnabled", (isMapOptionGenerateEnabled ? 1 : 0));
        }
    }

  

}
