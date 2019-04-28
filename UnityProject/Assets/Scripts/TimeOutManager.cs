using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeOutManager : MonoBehaviour
{
    public string TimeOutScene;

    public void TimeOut()
    {
        Debug.Log("TIME OOOOUT");
        SceneManager.LoadScene(TimeOutScene);
    }
}
