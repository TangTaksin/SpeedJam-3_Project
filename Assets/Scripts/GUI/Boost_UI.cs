using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boost_UI : MonoBehaviour
{
    [Header("Values")]
    float fillValue;
    [SerializeField] float followSpeed = .1f;
    [SerializeField] Vector2 offset;

    [Header("References")]
    [SerializeField] Image boostFill;
    [SerializeField]PlayerController player;
    Camera cam;
    RectTransform rectSelf;

    private void Start()
    {
        if (player != null)
        {
            player.onBoostChanged += UpdateGuage;
        }

        cam = Camera.main;
        rectSelf = GetComponent<RectTransform>();
    }

    private void OnGUI()
    {
        var tarPos = cam.WorldToScreenPoint(player.transform.position) + (Vector3)offset;
        rectSelf.position = Vector3.Lerp(transform.position, tarPos, Time.deltaTime * followSpeed);
    }

    private void UpdateGuage(float max, float curr)
    {
        fillValue = curr/max;
        boostFill.fillAmount = fillValue;
    }
}
