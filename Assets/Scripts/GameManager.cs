using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void exit()
    {
        Application.Quit();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exit();
        }
    }
}
