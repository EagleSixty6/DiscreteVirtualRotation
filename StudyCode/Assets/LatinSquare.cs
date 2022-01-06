using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatinSquare : MonoBehaviour
{
    private int size;
    private System.Random rand;
    private string ls;
    // Start is called before the first frame update
    void Start()
    {
        size = 5;
        rand = new System.Random();

        Debug.Log(LatinSquareString());
        Debug.Log(LatinSquareString());
        Debug.Log(LatinSquareString());
        Debug.Log(LatinSquareString());
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private string LatinSquareString()
    {
        int[,] square = new int[size, size];
        string ls = "";

        List<List<List<int>>> list = new List<List<List<int>>>();

        bool success = true;

        for (int i = 0; i < size; i++)
        {
            List<List<int>> list1 = new List<List<int>>();
            for (int j = 0; j < size; j++)
            {
                List<int> list2 = new List<int>();
                for (int k = 0; k < size; k++)
                {
                    list2.Add(k);
                }
                list1.Add(list2);
            }
            list.Add(list1);
        }

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (list[i][j].Count == 0)
                {
                    success = false;
                    break;
                }

                int a = list[i][j][rand.Next(0, list[i][j].Count)];

                square[i, j] = a;
                RemoveFromRest(list, square[i, j], i, j);
            }
            if (!success)
            {
                break;
            }
        }
        if (success)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    ls += square[i, j] + " ";
                }
                ls += "\n";
            }
            return ls;
        }
        else { return LatinSquareString(); }
    }

    private void RemoveFromRest(List<List<List<int>>> list, int x, int row, int column)
    {
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list.Count; j++)
            {
                if (list[i][j].Contains(x) && (i == row || j == column))
                {
                    list[i][j].Remove(x);
                }
            }
        }
    }

}
