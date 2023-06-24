using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Items : MonoBehaviour, IInteractable
{
    [SerializeField] public Sprite item_icon;
    [SerializeField] public string _description;

    // через интерфейс возвращает значение _prompt объекта туда, где вызывается интерфейс
    public string InteractionPrompt => _description;

    // логика того, что делать при взаимодействии с объектом
    public void Interact(PlayerDoInteract interactor)
    {
        StatHolder._inventory.Add(item_icon);
        Stats();
        Destroy(gameObject, 0.2f);
    }

    public abstract void Stats();
}
