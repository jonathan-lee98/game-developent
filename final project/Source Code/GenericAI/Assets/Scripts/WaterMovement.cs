using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    public float speed = 0.5f;
    public Vector2 direction = Vector2.right;
    public string textureName = "_MainTex";

    private Renderer rend;
    private Vector2 offset;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        offset += direction * speed * Time.deltaTime;
        rend.material.SetTextureOffset(textureName, offset);
    }
}
