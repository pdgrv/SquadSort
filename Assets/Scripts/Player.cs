using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SortingContainer _currentContainer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, 300f))
            {
                if (hitInfo.collider.TryGetComponent(out SortingContainer targetContainer))
                {
                    if (_currentContainer == null && targetContainer.IsFree)
                    {
                        return;
                    }

                    if (_currentContainer == null)
                    {
                        _currentContainer = targetContainer;
                        Debug.Log("Первый выбор " + _currentContainer.name + " контейнера");
                    }
                    else
                    {
                        targetContainer.Interact(ref _currentContainer);
                    }
                }
            }
        }
    }
}
