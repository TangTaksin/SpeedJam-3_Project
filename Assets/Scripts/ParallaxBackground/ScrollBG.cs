using UnityEngine;

public class ScrollBG : MonoBehaviour
{
    public float parallaxFactorX = 1.0f;
    public float parallaxFactorY = 1.0f;

    [SerializeField] private float lerpTime = 0.1f;

    private void Update()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        Material material = mr.material;
        Vector2 offset = material.mainTextureOffset;

        // Calculate parallax offset based on position and local scale
        float parallaxOffsetX = transform.position.x / transform.localScale.x * parallaxFactorX;
        float parallaxOffsetY = transform.position.y / transform.localScale.y * parallaxFactorY;

        offset = Vector2.Lerp(offset, new Vector2(parallaxOffsetX, parallaxOffsetY), Time.deltaTime * lerpTime);

        // offset.x = parallaxOffsetX;
        // offset.y = parallaxOffsetY;

        material.mainTextureOffset = offset;
    }
}
