using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MapData : MonoBehaviour
{

    public int width = 10;
    public int height = 10;
    public bool generateRandomMap = false;

    public List<TextAsset> textAsset;
   
    // Start is called before the first frame update
    void Awake()
    {
        generateRandomMap = UIManager.getInstance().getMapOptionGenerate();      
    }

    public int[,] MakeMap()
    {

        List<string> lines = new List<string>();
        lines = GetTextFromFile();
        SetDimensions(lines); 

        int[,] map = new int[width, height];

        //read map from file
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //in case file is truncated
                if (lines[i].Length > j)
                {
                    map[j, i] = (int)Char.GetNumericValue(lines[i][j]);
                }
              
            }
        }

        if (generateRandomMap)
        {        
            map = GenerateRandomMap(19);
        } 

        return map;
    }


    public List<string> GetTextFromFile(TextAsset asset)
    {
        List<string> lines = new List<string>();


        if (asset != null)
        {
            string textData = asset.text;
            string[] delimiters = { "\r\n", "\n" };
            lines.AddRange(textData.Split(delimiters, System.StringSplitOptions.None));
            lines.Reverse();
        }
        else
        {
            Debug.LogError("bad maze file");
        }

        return lines;
    }

    public List<string> GetTextFromFile()
    {

        int randomMap = UnityEngine.Random.Range(0, textAsset.Count) ;
        TextAsset tx = textAsset[randomMap];

        return GetTextFromFile(tx);
    }

    // make sure that text file is ok formating
    public void SetDimensions(List<string> textLines)
    {
        height = textLines.Count;

        foreach(string line in textLines)
        {
            if (line.Length > width)
            {
                width = line.Length;
            }
        }
    }

    private int[,] GenerateRandomMap(int numOfObstacles)
    {
        int[,] map = new int[width, height];

       
        for (int i = 0 ; i < numOfObstacles; i++)
        {        
            int x = UnityEngine.Random.Range(0, width);
            int y = UnityEngine.Random.Range(0, height);

         
            if (UIManager.getInstance().getStartX() == x)
            {
                x = UnityEngine.Random.Range(0, width);
            }

            if (UIManager.getInstance().getGoalX() == x)
            {
                x = UnityEngine.Random.Range(0, width);
            }

            if (UIManager.getInstance().getStartY() == y)
            {
                y = UnityEngine.Random.Range(0, height);
            }

            if (UIManager.getInstance().getGoalY() == y)
            {
                y = UnityEngine.Random.Range(0, height);
            }

            map[x, y] = 1;
        }
        return map;
    }

}
