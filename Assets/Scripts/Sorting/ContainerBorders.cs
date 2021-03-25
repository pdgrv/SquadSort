using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ContainerBorders : MonoBehaviour
{
    [SerializeField] private NavMeshObstacle _leftBorder;
    [SerializeField] private NavMeshObstacle _rightBorder;
    [SerializeField] private NavMeshObstacle _bottomBorder;
    [SerializeField] private float _sizePerUnit = 0.2f;

    public void UpdateBorders(int unitsCount)
    {
        if (unitsCount == 0)
        {
            _leftBorder.enabled = false;
            _rightBorder.enabled = false;
            _bottomBorder.enabled = false;
        }
        else
        {
            _leftBorder.enabled = true;
            _rightBorder.enabled = true;
            _bottomBorder.enabled = true;

            UpdateBorderSize(_leftBorder, unitsCount);
            UpdateBorderSize(_rightBorder, unitsCount);
        }
    }

    private void UpdateBorderSize(NavMeshObstacle targetBorder, int count)
    {
        count--;
        targetBorder.size = new Vector3(targetBorder.size.x, targetBorder.size.y, _sizePerUnit * count);
        targetBorder.center = new Vector3(0, 0, -0.4f + _sizePerUnit / 2 * count);
    }
}
