using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour, IInteractable
{
    [SerializeField] string scene_name;
    // метка, что это переход на следующую сцену.
    public string _prompt = "NextScene";

    // через интерфейс возвращает значение _prompt объекта туда, где вызывается интерфейс
    public string InteractionPrompt => _prompt;

    // логика того, что делать при взаимодействии с объектом
    public void Interact(PlayerDoInteract interactor)
    {
        SceneManager.LoadScene(scene_name);
    }
}
