using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public MapData mapData;
    public Graph graph;

    public Pathfinder pathfinder;
    public int startX = 0;
    public int startY = 0;
    public int goalX = 10;
    public int goalY = 10;


    public float timeStep = 0.2f;


    private bool runOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        startRun();
    }


    private void setModeFromOptions()
    {
        if (UIManager.getInstance().getDijkstra())
        {
            pathfinder.mode = Pathfinder.Mode.Dijkstra;
        }

        if (UIManager.getInstance().getGBFS())
        {
            pathfinder.mode = Pathfinder.Mode.GreedyBestFirstSearch;
        }

        if (UIManager.getInstance().getBFS())
        {
            pathfinder.mode = Pathfinder.Mode.BreadthFirstSearch;
        }

        if (UIManager.getInstance().getAstar())
        {
            pathfinder.mode = Pathfinder.Mode.AStar;
        }

    }

    private void setlPosFromOptions()
    {
        startX = UIManager.getInstance().getStartX();
        startY = UIManager.getInstance().getStartY();

        goalX = UIManager.getInstance().getGoalX();
        goalY = UIManager.getInstance().getGoalY();
    }

    public void startRun()
    {
        UIManager.getInstance().disableeShowStatisticsButton();

        if (mapData != null && graph != null)
        {
            int[,] mapInstance = mapData.MakeMap();
            graph.Init(mapInstance);

            GraphView graphView = graph.gameObject.GetComponent<GraphView>();

            if (graphView != null)
            {
                graphView.Init(graph);
            }

            setlPosFromOptions();

            if (graph.IsWithingBounds(startX, startY) && graph.IsWithingBounds(goalX, goalY) && pathfinder != null)
            {
                Node startNode = graph.nodes[startX, startY];
                Node goalNode = graph.nodes[goalX, goalY];

                setModeFromOptions();
                pathfinder.Init(graph, graphView, startNode, goalNode);
               
                UIManager.getInstance().setAlgNames(pathfinder.mode.ToString());

                while (pathfinder.isPathBlocked)
                {
                    pathfinder.Init(graph, graphView, startNode, goalNode);
                    pathfinder.isPathBlocked = false;
                }


                StartCoroutine(pathfinder.SearchRoutine(timeStep));

                runOnce = true;
            }

        }
    }

    private void Update()
    {
        if (pathfinder.isComplete && runOnce)
        {
            UIManager.getInstance().enableShowStatisticsButton();
            runOnce = false;
        }
    }


  

}

