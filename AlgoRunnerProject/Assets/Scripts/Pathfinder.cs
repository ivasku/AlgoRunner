using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class Pathfinder : MonoBehaviour
{
    [HideInInspector]
    public Node m_startNode;
    [HideInInspector]
    public Node m_goalNode;

    private Graph m_graph;
    private GraphView m_graphView;

    PriorityQueue<Node> m_frontierNodes;
    //Queue<Node> m_frontierNodes;
    List<Node> m_exploredNodes;
    List<Node> m_pathNodes;

    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.cyan;
    public Color arrowColor = new Color(0.85f, 0.85f, 0.85f, 1f);
    public Color highlightColor = new Color(1f, 1f, 0.5f, 1f);

    public bool showIterations = true;
    public bool showColors = true;
    public bool showArrows = true;
    public bool exitOnGoal = true;

    public bool isComplete = false;
    private int m_interations = 0;

    [HideInInspector]
    public bool isPathBlocked = false;

    public Mode mode = Mode.BreadthFirstSearch;

    public GameObject gameErrorPanel;
    public Text gameErrorPanelText;

    public enum Mode
    {
        BreadthFirstSearch = 0,
        Dijkstra = 1,
        GreedyBestFirstSearch = 2,
        AStar = 3
    }

    public void Init(Graph graph, GraphView graphView, Node start, Node goal)
    {
        isPathBlocked = false;

        if (start == null || goal == null || graph == null)
        {
            Debug.Log("Pathfinder error, missing components");
            return;
        }

        if (start.nodeType == NodeType.Blocked || goal.nodeType == NodeType.Blocked)
        {
            Debug.Log("Pathfinder error, start or goal blocked");
            isPathBlocked = true;
            if (!gameErrorPanel.activeSelf)
            {
                gameErrorPanel.SetActive(true);
                gameErrorPanelText.text = "Start position or goal position is generated on the same spot as the obstacles. \r\n Press RUN again to generate new map.";
            }
            
            return;
        }

        m_graph = graph;
        m_graphView = graphView;
        m_startNode = start;
        m_goalNode = goal;
        ShowColors(graphView, start, goal);

        m_frontierNodes = new PriorityQueue<Node>();
        m_frontierNodes.Enqueue(start);
        m_frontierNodes.Enqueue(start);
        m_exploredNodes = new List<Node>();
        m_pathNodes = new List<Node>();

        for (int x = 0; x < m_graph.Width; x++)
        {
            for (int y = 0; y < m_graph.Height; y++)
            {
                m_graph.nodes[x, y].Reset();
            }
        }

        isComplete = false;
        m_interations = 0;
        m_startNode.distanceTraveled = 0;
    }

    private void ShowColors(GraphView graphView, Node start, Node goal)
    {

        if (graphView == null || start == null || goal == null)
        {
            return;
        }


        if (m_frontierNodes != null)
        {
            graphView.ColorNodes(m_frontierNodes.ToList(), frontierColor);
        }

        if (m_exploredNodes != null)
        {
            graphView.ColorNodes(m_exploredNodes.ToList(), exploredColor);
        }


        if (m_pathNodes != null && m_pathNodes.Count > 0)
        {
            graphView.ColorNodes(m_pathNodes, pathColor);
                
        }

        NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex];

        if (startNodeView != null)
        {
            startNodeView.ColorNode(startColor);
        }

        NodeView goalNodeView = graphView.nodeViews[goal.xIndex, goal.yIndex];

        if (goalNodeView != null)
        {
            goalNodeView.ColorNode(goalColor);
        }
    }

    private void ShowColors()
    {
        ShowColors(m_graphView, m_startNode, m_goalNode);
    }

    public IEnumerator SearchRoutine(float timeStep = 0.2f)
    {
        float timeStart = Time.time;

        yield return null;

        while (!isComplete)
        {
            if (m_frontierNodes.Count > 0 && m_frontierNodes != null)
            {
                Node currentNode = m_frontierNodes.Dequeue();
                m_interations++;

                if (!m_exploredNodes.Contains(currentNode))
                {
                    m_exploredNodes.Add(currentNode);
                }

                //switch alg
                if (mode == Mode.BreadthFirstSearch)
                {
                    //Debug.Log("Pathfinder BreadthFirstSearch " );
                    ExpandFrontierBFS(currentNode);
                }
                else if (mode == Mode.Dijkstra)
                {
                    //Debug.Log("Pathfinder Dijkstra ");
                    ExpandFrontierDijkstra(currentNode);
                }
                else if(mode == Mode.GreedyBestFirstSearch)
                {
                    //Debug.Log("Pathfinder GreedyBestFirstSearch ");
                    ExpandFrontierGBFS(currentNode);
                }
                else if (mode == Mode.AStar)
                {
                    //Debug.Log("Pathfinder AStar ");
                    ExpandFrontierAStar(currentNode);
                }



                if (m_frontierNodes.Contains(m_goalNode))
                {
                    m_pathNodes = GetPathNodes(m_goalNode); // save the path  

                    if (exitOnGoal)
                    {
                        isComplete = true;
                        //Debug.Log("Mode: " + mode.ToString() + " path length = " + m_goalNode.distanceTraveled.ToString());
                    }
                }

                if (showIterations)
                {
                    ShowPathDiagnostics();
                    yield return new WaitForSeconds(timeStep);
                }
               
            }
            else
            {
                isComplete = true;
            }
        }

        ShowPathDiagnostics();
        //Debug.Log("Pathfinder searchRoutine time: " + (Time.time - timeStart).ToString() + " seconds");
        UIManager.getInstance().setAlgStatisticsData((Time.time - timeStart).ToString(), m_goalNode.distanceTraveled.ToString(), m_exploredNodes.Count.ToString());

    }

    private void ExpandFrontierBFS(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbours.Count; i++)
            {
                if (!m_exploredNodes.Contains(node.neighbours[i]) && !m_frontierNodes.Contains(node.neighbours[i]))
                {
                    float distanceToNeigbour = m_graph.GetNodeDistance(node, node.neighbours[i]);
                    float newDistanceTraveled = distanceToNeigbour + node.distanceTraveled;

                    node.neighbours[i].distanceTraveled = newDistanceTraveled;

                    node.neighbours[i].previous = node;
                    node.neighbours[i].priority = m_exploredNodes.Count;  // priority queue fix 

                    m_frontierNodes.Enqueue(node.neighbours[i]);
                }
            }
        }
    }

    private List<Node> GetPathNodes(Node endNode)
    {
        List<Node> path = new List<Node>();

        if (endNode == null)
        {
            return path;
        }

        path.Add(endNode);

        Node currentNode = endNode.previous;

        while(currentNode != null)
        {
            path.Insert(0, currentNode); // keep the order of nodes
            currentNode = currentNode.previous;
        }

        return path;

    }

    private void ShowPathDiagnostics()
    {
        if (showColors)
        {
            ShowColors();
        }

        if (m_graphView != null && showArrows)
        {
            m_graphView.ShowNodeArrow(m_frontierNodes.ToList(), arrowColor);

            if (m_frontierNodes.Contains(m_goalNode))
            {
                m_graphView.ShowNodeArrow(m_pathNodes, highlightColor);
            }
        }

    }

    private void ExpandFrontierDijkstra(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbours.Count; i++)
            {
                if (!m_exploredNodes.Contains(node.neighbours[i]))
                {

                    float distanceToNeigbour = m_graph.GetNodeDistance(node, node.neighbours[i]);
                    float newDistanceTraveled = distanceToNeigbour + node.distanceTraveled;

                    if (float.IsPositiveInfinity(node.neighbours[i].distanceTraveled) || newDistanceTraveled < node.neighbours[i].distanceTraveled)
                    {
                        node.neighbours[i].previous = node;
                        node.neighbours[i].distanceTraveled = newDistanceTraveled;
                    }

                    if (!m_frontierNodes.Contains(node.neighbours[i]))
                    {
                         node.neighbours[i].priority = node.neighbours[i].distanceTraveled; // priority queue fix 
                        m_frontierNodes.Enqueue(node.neighbours[i]);
                    }
                    

                }
            }
        }
    }

    //Greedy Best-First Search alg
    private void ExpandFrontierGBFS(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbours.Count; i++)
            {
                if (!m_exploredNodes.Contains(node.neighbours[i]) && !m_frontierNodes.Contains(node.neighbours[i]))
                {
                    float distanceToNeigbour = m_graph.GetNodeDistance(node, node.neighbours[i]);
                    float newDistanceTraveled = distanceToNeigbour + node.distanceTraveled;

                    node.neighbours[i].distanceTraveled = newDistanceTraveled;

                    node.neighbours[i].previous = node;
                    if (m_graph != null)
                    {
                        node.neighbours[i].priority = m_graph.GetNodeDistance(node.neighbours[i], m_goalNode);
                    }
                
                    m_frontierNodes.Enqueue(node.neighbours[i]);
                }
            }
        }
    }

    private void ExpandFrontierAStar(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbours.Count; i++)
            {
                if (!m_exploredNodes.Contains(node.neighbours[i]))
                {

                    float distanceToNeigbour = m_graph.GetNodeDistance(node, node.neighbours[i]);
                    float newDistanceTraveled = distanceToNeigbour + node.distanceTraveled;

                    if (float.IsPositiveInfinity(node.neighbours[i].distanceTraveled) || newDistanceTraveled < node.neighbours[i].distanceTraveled)
                    {
                        node.neighbours[i].previous = node;
                        node.neighbours[i].distanceTraveled = newDistanceTraveled;
                    }

                    if (!m_frontierNodes.Contains(node.neighbours[i]) && m_graph != null)
                    {
                        float distanceToGoal = m_graph.GetNodeDistance(node.neighbours[i], m_goalNode);
                        node.neighbours[i].priority = node.neighbours[i].distanceTraveled + distanceToGoal; // priority queue fix 
                        m_frontierNodes.Enqueue(node.neighbours[i]);
                    }


                }
            }
        }
    }

    public List<Node> getPathNodes()
    {
        return m_pathNodes;
    }


    public void closeGameErrroPopUp()
    {
        gameErrorPanel.SetActive(false);
    }

}



