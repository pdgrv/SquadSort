using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    private Animation _animation;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _animation = GetComponent<Animation>();
    }

    public void ActivateCombatMode()
    {
        _animation.Play();
    }
}
