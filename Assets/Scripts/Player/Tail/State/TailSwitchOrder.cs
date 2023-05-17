using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TailSwitchOrder : MonoBehaviour
{
    public List<int> tailOrder = new() { 3, 2, 3 };
    public int levelOrder;
    public int previousLevelOrder;

    private void Start()
    {
        levelOrder = 1;
        previousLevelOrder = 1;
    }
    public List<int> GetTailOrder()
    {
        if (GetActiveSceneID() == 2)
        {
            if (levelOrder == 1)
            {
                tailOrder = new List<int> { 3, 1, 2 };
            }
            if (levelOrder == 2)
            {
                tailOrder = new List<int> { 1, 1, 1 };
            }
            if (levelOrder == 3)
            {
                tailOrder = new List<int> { 3, 3, 3 };
            }
            if (levelOrder == 4)
            {
                tailOrder = new List<int> { 2, 2, 2 };
            }
            if(levelOrder == 5)
            {
                tailOrder = new List<int> { 2, 3, 2, 3};
            }
            if(levelOrder == 6)
            {
                tailOrder = new List<int> { 2, 2, 2, 2};
            }
            if(levelOrder == 7)
            {
                tailOrder = new List<int> { 1, 1, 1, 1 };
            }
            if (levelOrder == 8)
            {
                tailOrder = new List<int> { 3, 2, 3, 2 };
            }
        }

        return tailOrder;
    }

    private int GetActiveSceneID()
    {
        int sceneID = SceneManager.GetActiveScene().buildIndex;
        return sceneID;
    }
}