using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private SquadsContainer _currentContainer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, 300f))
            {
                if (hitInfo.collider.TryGetComponent(out SquadsContainer targetContainer))
                {
                    if (_currentContainer == null)
                    {
                        if (targetContainer.IsFree)
                        {
                            targetContainer.FocusBad();
                        }
                        else
                        {
                            SelectCurrentContainer(targetContainer);
                        }
                    }
                    else
                    {
                        if (_currentContainer.Equals(targetContainer))
                        {
                            UnselectCurrentContainer();
                        }
                        else
                        {
                            if (targetContainer.TryMoveSquad(_currentContainer))
                                UnselectCurrentContainer();
                            else
                                SelectCurrentContainer(targetContainer);
                        }
                    }
                }
            }
        }
    }

    private void SelectCurrentContainer(SquadsContainer targetContainer)
    {
        if (_currentContainer != null)
            UnselectCurrentContainer();

        _currentContainer = targetContainer;
        targetContainer.FocusOn();
    }

    private void UnselectCurrentContainer()
    {
        _currentContainer.FocusOff();
        _currentContainer = null;
    }
}
