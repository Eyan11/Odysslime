using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isStart;
    public bool isQuit;
    public string sceneName;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseUp()
    {
        if (isStart)
        {
            changeScene();
        }
        if (isQuit)
        {
            Application.Quit();
        }
    }

    public void changeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
