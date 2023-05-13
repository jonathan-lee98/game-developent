using UnityEngine;
using UnityEngine.UI;
public enum ColourType{
    Green,
    Blue,
    Other
}
public class BallManager : MonoBehaviour
{
    public GameObject imagePrefab;
    public float spawnInterval = 1f;
    public float minSpawnInterval = 0.5f;
    public float spawnIntervalDecrease = 0.1f;
    public float minSpeed = 1f;
    public float maxSpeed = 5f;
    public float speedIncrease = 0.1f;
    public Vector2 spawnOffset = new Vector2(0f, 1f);
    public GameObject canvas;
    private float spawnTimer;

    public GameObject BallParent;
    public Color c1;
    public Color c2;
    void Update()
    {
        // Decrease spawn interval and increase speed over time
        spawnInterval = Mathf.Max(spawnInterval - spawnIntervalDecrease * Time.deltaTime, minSpawnInterval);
        maxSpeed += speedIncrease * Time.deltaTime;

        // Spawn image at regular intervals
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnImage();
            spawnInterval -= 0.1f;
            if (spawnInterval < 0.1f)
            {
                spawnInterval = 0.1f;
            }
            spawnTimer = spawnInterval;
        }
    }

    void SpawnImage()
    {
        // Create image at random position and random speed
        float screenWidth = Screen.width;
        float randomX = Random.Range(0f, screenWidth);
        float randomSpeed = Random.Range(minSpeed, maxSpeed);
        Vector2 spawnPosition = new Vector2(randomX, Screen.height) + spawnOffset;

        GameObject imageObject = Instantiate(imagePrefab, spawnPosition, Quaternion.identity);
        imageObject.transform.SetParent(canvas.transform);
        imageObject.transform.SetSiblingIndex(1);
        // Set image speed and color
       BallController imageController = imageObject.GetComponent<BallController>();
        imageController.speed = randomSpeed;

        int type = UnityEngine.Random.RandomRange(0, 3);
        if(type==0)
        {
            imageController.type = ColourType.Green;
            imageObject.GetComponent<Image>().color = c1;
        }
        else if (type == 1)
        {
            imageController.type = ColourType.Blue;
            imageObject.GetComponent<Image>().color = c2;
        }
        else
        {
            imageController.type = ColourType.Other;
            imageObject.GetComponent<Image>().color =Color.yellow;
        }

        //SpriteRenderer renderer = imageObject.GetComponent<SpriteRenderer>();
        //Color randomColor = new Color(Random.value, Random.value, Random.value);
        //renderer.color = randomColor;
    }
}
