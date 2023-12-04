using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScrollBG : MonoBehaviour
{
    public float scrollSpeedX = 1.0f;
    public float scrollSpeedY = 1.0f;

    [SerializeField] private float lerpTime = 0.1f;

    private void Update()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        Material material = mr.material;
        Vector2 offset = material.mainTextureOffset;

        // Calculate parallax offset based on time and scroll speeds
        float parallaxOffsetX = Time.time * scrollSpeedX;
        float parallaxOffsetY = Time.time * scrollSpeedY;

        offset = Vector2.Lerp(offset, new Vector2(parallaxOffsetX, parallaxOffsetY), Time.deltaTime * lerpTime);

        material.mainTextureOffset = offset;
    }
}
