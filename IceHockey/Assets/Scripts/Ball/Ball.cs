using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material startingMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material blueMaterial;

    private Renderer renderer;

    private void Awake()
    {
        renderer = gameObject.GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BluePlayer"))
        {
            renderer.material = blueMaterial;
            // Calculate new direction
        }

        if (other.CompareTag("RedPlayer"))
        {
            renderer.material = redMaterial;
            // Calculate new direction
        }

        if (other.CompareTag("Hole"))
        {
            // Score for Color
            // Gray Color
            // Start with non-score
        }
    }
}
