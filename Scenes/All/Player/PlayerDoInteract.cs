using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;


public class PlayerDoInteract : MonoBehaviour
{
    // объекты, от которых идёт взаимодействие (привязаны к персонажу)
    [SerializeField] private Transform _interactionPoint; 
    // радиус этих объектов
    [SerializeField] private float _interactionPointRadius = 0.5f;
    // слои объектов, с которыми взаимодействуют точки выше
    [SerializeField] private LayerMask _interactableMask;
    // скрипты с UI для изменения в UI при взаимодействиях
    [SerializeField] private InteractionPromptUI _interactionPromptUI;
    // список пересечений коллайдеров точки и объекта взаимодействия
    private readonly Collider[] _colliders = new Collider[3];
    // количество найденных пересечений коллайдеров точки и объекта взаимодействия
    [SerializeField] private int _interactFound;
    //таймаут на взаимодействие
    [SerializeField] private float interactCooldown = 2.0f;

    // интерфейс для взаимодействий
    private IInteractable _interactable;

    // ввод, из Input System
    private StarterAssetsInputs _input;

    private bool canInteract = true;

    private void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
    }

    private void OnEnable()
    {
        canInteract = true;
    }

    private void Update()
    {
        // в переменные записывается количество пересечений созданного коллайдера точки и объектов, с которыми возможно взаимодействие
        _interactFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

        // Вызов функций, которые проверяют наличие взаимодействий и делают что то при возникновании его.
        ObjectsInteract();
    }

    // отрисовывает радиус коллайдеров точек взаимодействия, просто для визуального удобства.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }

    // взаимодействие по кнопке E с объектами слоя Interactable
    private void ObjectsInteract()
    {
        if (_interactFound > 0)
        {
            var _interactable = _colliders[0].GetComponent<IInteractable>();

            if (_interactable != null)
            {
                if (_interactable.InteractionPrompt == "NextScene")
                {
                    _interactable.Interact(this);
                }
                else
                {
                    if (!_interactionPromptUI.IsDisplayed) _interactionPromptUI.SetUp(_interactable.InteractionPrompt);

                    if (_input.interact && canInteract)
                    {
                        StartCoroutine(Cooldown());
                        _interactable.Interact(this);
                    }
                }
            }
        }
        else
        {
            _input.interact = false;
            if (_interactable != null) _interactable = null;
            if (_interactionPromptUI.IsDisplayed) _interactionPromptUI.Close();
        }
    }

    IEnumerator Cooldown()
    {
        _input.interact = false;
        canInteract = false;
        yield return new WaitForSeconds(interactCooldown);
        canInteract = true;
    }
}
