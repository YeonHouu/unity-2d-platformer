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
    private Player player;

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
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ� ����
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

    private void OnEnable()
    {
        // Player�� OnGameEnd �̺�Ʈ�� ����
        Player.OnGameEnd += GameEnd;
    }

    private void OnDisable()
    {
        // ���� ���� (�޸� ���� ����)
        Player.OnGameEnd -= GameEnd;
    }

    public void GameOver()
    {
        Debug.Log("���ӿ���");
        StopCamera();
        gameOverPanel.gameObject.SetActive(true);
    }

    public void GameEnd()
    {
        Debug.Log("���ӿ���");
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
