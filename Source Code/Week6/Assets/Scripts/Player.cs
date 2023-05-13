using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Slider slider;
    public RectTransform imageRectTransform;
    public GameObject player;

    private float imageLeftEdge;
    private float imageRightEdge;
    private float playerWidth;
    private float sliderWidth;
    private float sliderLeftEdge;
    private float sliderRightEdge;

    void Start()
    {
        // Get the left and right edges of the image and player objects
        imageLeftEdge = imageRectTransform.offsetMin.x+5f;
        imageRightEdge = imageRectTransform.offsetMax.x;
        playerWidth = player.GetComponent<RectTransform>().rect.width;

        // Get the width and left and right edges of the slider
        sliderWidth = slider.GetComponent<RectTransform>().rect.width;
        sliderLeftEdge = slider.GetComponent<RectTransform>().offsetMin.x;
        sliderRightEdge = slider.GetComponent<RectTransform>().offsetMax.x;

        // Set the player's initial position based on the slider value
        float initialPlayerX = (slider.value - 0.5f) * sliderWidth + (imageLeftEdge + imageRightEdge) / 2.0f - playerWidth / 2.0f;
        player.GetComponent<RectTransform>().anchoredPosition = new Vector2(initialPlayerX, player.GetComponent<RectTransform>().anchoredPosition.y);
    }

    void Update()
    {
        // Update the player's position based on the slider value
        float playerX = (slider.value - 0.5f) * sliderWidth + (imageLeftEdge + imageRightEdge) / 2.0f - playerWidth / 2.0f;
        player.GetComponent<RectTransform>().anchoredPosition = new Vector2(playerX+5f, player.GetComponent<RectTransform>().anchoredPosition.y);
    }
}
