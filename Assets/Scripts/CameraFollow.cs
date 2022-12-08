using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] Transform _target;
    [SerializeField] float _smoothTime = 0.15f;
    [SerializeField] float posY = 2;
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;

    private Vector3 _velocity = Vector3.zero;

    private void LateUpdate()
    {
        Vector3 _targetPosition = _target.TransformPoint(new Vector3(0, posY, -10));
        Vector3 _desiredPosition = Vector3.SmoothDamp(transform.position, _targetPosition, ref _velocity, _smoothTime);
        transform.position = new Vector3(Mathf.Clamp(_desiredPosition.x, minX, maxX), Mathf.Clamp(_desiredPosition.y, minY, maxY), _desiredPosition.z);
    }
}
