using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class ImageShakeResponse : MonoBehaviour
{
    public Image placeholderImage; // Assign the placeholder Image in the Inspector
    public GameObject uiImage1;    // Assign the UI image GameObject that prompts the user to shake
    public Sprite newBoba;
    private ShakeDetector shakeDetector;
    public Image deliverButton;
    public Sprite deliveredTexture;
    public Sprite greyTexture;

    void Start()
    {
        // Get the ShakeDetector component from the gameObject
        shakeDetector = gameObject.GetComponent<ShakeDetector>();

        // Initially hide the shake prompt UI
        uiImage1.SetActive(false);

        // Check the current sprite name
        // CheckSpriteAndUpdateUI();
    }

    void Update()
    {


        if (placeholderImage.sprite.name == "boba-sized")
        {
            // Show the UI that prompts the user to shake the phone
            Debug.Log("Shaked!");
            uiImage1.SetActive(true);
            deliverButton.sprite = greyTexture;
            if (shakeDetector != null && shakeDetector.IsShakeDetected)
            {
                // After a shake is detected
                ShakeResponse();
            }
            }
            else{
                Debug.Log("not detected!");
            }
    }

    // private void CheckSpriteAndUpdateUI()
    // {
    //     Check if the placeholder's sprite is the specific one that requires shake
    //     if (placeholderImage.sprite.name == "boba-sized")
    //     {
    //         // Show the UI that prompts the user to shake the phone
    //         Debug.Log("Shaked!");
    //         uiImage1.SetActive(true);
    //     }
    //     else{
    //         Debug.Log("not detected!");
    //     }
    // }

    private void ShakeResponse()
    {
        // Change the sprite to the next one
        placeholderImage.sprite = newBoba;

        // Hide the UI image
        uiImage1.SetActive(false);
        Handheld.Vibrate();
        deliverButton.sprite = deliveredTexture;

        // Reset shake detection (optional, depends on how the ShakeDetector is implemented)
        shakeDetector.ResetShakeDetection();
    }
}