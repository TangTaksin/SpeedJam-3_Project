using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boost_UI : MonoBehaviour
{
    [Header("Values")]
    float _fillValue;
    [SerializeField] float _followSpeed = .1f;
    [SerializeField] Vector2 _offset;

    [Header("References")]
    [SerializeField] Image _boostFill;
    [SerializeField] PlayerController _player;
    Camera _cam;
    RectTransform _rectSelf;

    private void Start()
    {
        if (_player != null)
        {
            _player.onBoostChanged += UpdateGuage;
        }

        _cam = Camera.main;
        _rectSelf = GetComponent<RectTransform>();

        if (_boostFill == null)
        {
            Debug.LogError("BoostFill not assigned to Boost_UI script.");
        }
    }

    private void OnGUI()
    {
        var tarPos = _cam.WorldToScreenPoint(_player.transform.position) + (Vector3)_offset;
        _rectSelf.position = Vector3.Lerp(transform.position, tarPos, Time.deltaTime * _followSpeed);
    }

    private void UpdateGuage(float max, float curr)
    {
        _fillValue = curr / max;
        _boostFill.fillAmount = _fillValue;
    }
}
