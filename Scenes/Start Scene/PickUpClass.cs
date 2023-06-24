using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PickUpClass : MonoBehaviour
{
    [SerializeField] GameObject warrior;
    [SerializeField] GameObject archer;

    private void Start()
    {
        SetCharacter(DataSceneHolder.ClassChoose);
    }

    public void SetCharacter(string character)
    {
        if (character == "Warrior")
        {
            warrior.SetActive(true);
        }
        if (character == "Archer")
        {
            warrior.SetActive(true);
        }
    }
}
