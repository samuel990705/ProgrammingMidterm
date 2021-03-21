using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scoreboard : MonoBehaviour
{
    public Font font;
    void Start()
    {
        //read textfile
        string path = Application.dataPath + "/scoreboard.txt";
        string[] lines = File.ReadAllText(path).Split('\n');//each line
        Data[] data = new Data[11];//there can at most be 11 (10 top score +1 data point from previous game that was appended to textfile)
        string[] s;
        for(int i = 0; i < lines.Length; i++)//for each line
        {
            s = lines[i].Split(',');
            data[i].name = s[0];
            data[i].score = int.Parse(s[1]);
        }

        //sort 
        quickSort(data, 0, data.Length - 1);
        System.Array.Reverse(data);

        //write top 10 scores back into text file
        string temp = "";
        for (int i = 0; i < 9; i++)
        {
            temp += data[i].name + "," + data[i].score + "\n";
        }
        temp += data[9].name + "," + data[9].score;
        File.WriteAllText(path, temp);

        /********************************
         *                              *
         *     Construct scoreboard     *
         *                              *
         ********************************/
        GameObject G = new GameObject();
        G.transform.SetParent(this.transform);
        G.AddComponent<Text>();
        G.GetComponent<Text>().text = "Name" + "\t\t\t\t\t\t" + "Score";
        G.GetComponent<Text>().font = font;
        G.GetComponent<Text>().fontSize = 25;
        G.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        G.GetComponent<RectTransform>().localPosition = new Vector3(0, 100, 0);
        G.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 75);
        G.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        for (int i = 0; i < 10; i++)//
        {
            GameObject GO = new GameObject();
            GO.transform.SetParent(this.transform);
            GO.AddComponent<Text>();
            GO.GetComponent<Text>().text = data[i].name+"\t\t\t"+data[i].score;
            GO.GetComponent<Text>().font = font;
            GO.GetComponent<Text>().fontSize = 25;
            GO.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            GO.GetComponent<RectTransform>().localPosition = new Vector3(0, 50- i * 25, 0);
            GO.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 75);
            GO.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }



    }

    void Update()
    {
        //play again
        if (Input.GetKey("space"))
        {
            SceneManager.LoadScene("Game");
        }
    }

    //quicksort implementation
    void quickSort(Data[] array, int left, int right)
    {
        int pivotIndex = partition(array, left, right);

        if (pivotIndex - left > 1)
            quickSort(array, left, pivotIndex - 1);

        if (right - pivotIndex > 1)
            quickSort(array, pivotIndex + 1, right);
    }

    int partition(Data[] array, int left, int right)
    {

        int leftOrg = left;
        int rightOrg = right;


        // middle element chosen as pivot
        int pivotIndex = (left + right) / 2;

        //move pivot to the right
        swap(array, right, pivotIndex);
        pivotIndex = right;
        right = right - 1;//exclude pivot

        while (left <= right)
        {
            // find first element from the left larger than pivot
            while (array[left].score-array[pivotIndex].score < 0)
                left++;

            // find first element from the right smaller than pivot
            while (right >= left && array[right].score-array[pivotIndex].score >= 0)
                right--;

            //swap elements
            if (right > left)
            {
                swap(array, left, right);
            }
        }
        //swap pivot back in
        swap(array, left, pivotIndex);

        return left;
    }

    //swaps objects at 2 indices
    void swap(Data[] array, int ind1, int ind2)
    {
        Data tmp = array[ind1];
        array[ind1] = array[ind2];
        array[ind2] = tmp;
    }
}

//used to store data read from textfile
public struct Data
{
    public string name;//name of player
    public int score;//player's score
}
