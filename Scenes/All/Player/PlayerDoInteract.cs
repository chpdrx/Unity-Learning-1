using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;


public class PlayerDoInteract : MonoBehaviour
{
    // �������, �� ������� ��� �������������� (��������� � ���������)
    [SerializeField] private Transform _interactionPoint; 
    // ������ ���� ��������
    [SerializeField] private float _interactionPointRadius = 0.5f;
    // ���� ��������, � �������� ��������������� ����� ����
    [SerializeField] private LayerMask _interactableMask;
    // ������� � UI ��� ��������� � UI ��� ���������������
    [SerializeField] private InteractionPromptUI _interactionPromptUI;
    // ������ ����������� ����������� ����� � ������� ��������������
    private readonly Collider[] _colliders = new Collider[3];
    // ���������� ��������� ����������� ����������� ����� � ������� ��������������
    [SerializeField] private int _interactFound;
    //������� �� ��������������
    [SerializeField] private float interactCooldown = 2.0f;

    // ��������� ��� ��������������
    private IInteractable _interactable;

    // ����, �� Input System
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
        // � ���������� ������������ ���������� ����������� ���������� ���������� ����� � ��������, � �������� �������� ��������������
        _interactFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

        // ����� �������, ������� ��������� ������� �������������� � ������ ��� �� ��� ������������� ���.
        ObjectsInteract();
    }

    // ������������ ������ ����������� ����� ��������������, ������ ��� ����������� ��������.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }

    // �������������� �� ������ E � ��������� ���� Interactable
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
