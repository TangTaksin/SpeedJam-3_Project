using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform[] backgrounds; // Array of background layers to create parallax effect
    public float parallaxScale = 1.5f; // The proportion of movement to create parallax
    public float smoothing = 1f; // How smooth the parallax effect will be

    private Transform player; // Reference to the player's transform
    private Vector3 previousPlayerPosition; // The position of the player in the previous frame

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Assuming your player has a "Player" tag
        previousPlayerPosition = player.position;
    }

    void Update()
    {
        float parallax = (previousPlayerPosition.x - player.position.x) * parallaxScale;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            float backgroundTargetPosX = backgrounds[i].position.x + parallax * (i + 1);
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        previousPlayerPosition = player.position;
    }
}
