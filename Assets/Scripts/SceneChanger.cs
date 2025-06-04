using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Button startButton;

    private void Awake()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(() => ChangeScene("PlayScene"));
        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
