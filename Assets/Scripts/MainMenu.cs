using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button infoButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene");
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        infoButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("InfoMenu");
        });
    }
}
