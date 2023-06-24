using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject player;
    public string InteractionPrompt => _prompt;

    public void Interact(PlayerDoInteract interactor)
    {
        player.SetActive(false);
        canvas.SetActive(true);
    }
}
