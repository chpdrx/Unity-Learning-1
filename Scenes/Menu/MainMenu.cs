using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // корневой элемент UI
    public static VisualElement visuals;

    // кнопки меню
    public Button _play;
    public Button _load;
    public Button _history;
    public Button _options;
    public Button _exit;

    // Start is called before the first frame update
    void Start()
    {
        visuals = GetComponent<UIDocument>().rootVisualElement;
        _play = visuals.Q<Button>("Play");
        _play.clicked += PlayButton;
        _load = visuals.Q<Button>("Tree");
        _load.clicked += LoadButton;
        _history = visuals.Q<Button>("History");
        _history.clicked += HistoryButton;
        _options = visuals.Q<Button>("Options");
        _options.clicked += OptionsButton;
        _exit = visuals.Q<Button>("Exit");
        _exit.clicked += ExitButton;
    }

    void PlayButton()
    {
        SceneManager.LoadScene("ClassChoose");
    }

    void LoadButton()
    {
        
    }

    void HistoryButton()
    {
        
    }

    void OptionsButton()
    {
        
    }

    void ExitButton()
    {
        Application.Quit();
    }
}
