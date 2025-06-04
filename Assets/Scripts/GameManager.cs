using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private PlayerHealth playerHealth;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [Header("UI")]
    public GameObject gameOverPanel;
    public GameObject gameEndPanel;

    [Header("gameOverPanel")]
    public Button gameRetryButton_over;
    public Button startSceneButton_over;
    public Button gameExitButton_over;

    [Header("gameEndPanel")]
    public Button startSceneButton_end;
    public Button gameExitButton_end;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
        }
        else
        {
            Destroy(gameObject); // 중복 방지
            return;
        }

        playerHealth = GetComponent<PlayerHealth>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();


        gameExitButton_over.onClick.AddListener(() => GameExit());
        gameRetryButton_over.onClick.AddListener(() => LoadScene("PlayScene"));
        startSceneButton_over.onClick.AddListener(() => LoadScene("StartScene"));

        gameExitButton_end.onClick.AddListener(() => GameExit());
        startSceneButton_end.onClick.AddListener(() => LoadScene("StartScene"));
    }

    public void GameOver()
    {
        Debug.Log("게임오버");
        StopCamera();
        gameOverPanel.gameObject.SetActive(true);
    }
    public void GameEnd()
    {
        Debug.Log("게임엔딩");
        StopCamera();
        gameEndPanel.gameObject.SetActive(true);
    }

    public void StopCamera()
    {
        var transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_XDamping = 5f;
        transposer.m_YDamping = 5f;
        virtualCamera.Follow = null;
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
