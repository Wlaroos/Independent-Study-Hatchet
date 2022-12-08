using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    float _length;
    float _startPos;
    Camera _camRef;

    [SerializeField] float _parallaxAmount;


    private void Awake()
    {
        _camRef = Camera.main;
        _startPos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = (_camRef.transform.position.x * (1 - _parallaxAmount));
        float dist = (_camRef.transform.position.x * _parallaxAmount);

        transform.position = new Vector3(_startPos + dist, transform.position.y, transform.position.z);

        if(temp > _startPos + _length)
        {
            _startPos += _length;
        }
        else if (temp < _startPos - _length)
        {
            _startPos -= _length;
        }
    }
}
