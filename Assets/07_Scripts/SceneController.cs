using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    TitleScene,
    IngameScene
}

public class SceneController : MonoBehaviour
{
    static SceneController _instance;
    AsyncOperation oper;

    public static SceneController Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public AsyncOperation LoadScene(int sceneNum)
    {
        return SceneManager.LoadSceneAsync(sceneNum);
    }

    public void MoveIngameScene()
    {
        LoadScene((int)SceneType.IngameScene);
    }

    public void MoveTitleScene()
    {
        LoadScene((int)SceneType.TitleScene);
    }
}
