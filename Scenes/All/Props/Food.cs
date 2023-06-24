using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, IInteractable
{
    // текстовое поле, с которого можно что то забирать у объекта
    [SerializeField] private string _prompt;

    // через интерфейс возвращает значение _prompt объекта туда, где вызывается интерфейс
    public string InteractionPrompt => _prompt;

    // логика того, что делать при взаимодействии с объектом
    public void Interact(PlayerDoInteract interactor)
    {
        Debug.Log("Eat food!");
    }
}
