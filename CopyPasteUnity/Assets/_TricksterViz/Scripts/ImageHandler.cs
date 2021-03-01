using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageHandler : MonoBehaviour
{
    public ImageGrab grabber;
    public RekognitionClient rekognizer;

    void OnEnable(){
        grabber?.ImageGrabbed.AddListener(HandleImage);
    }

    void OnDisable(){
        grabber?.ImageGrabbed.RemoveListener(HandleImage);
    }

    public void HandleImage(string b64){
        Debug.Log($"Heard the image grab: {b64}");
        rekognizer.SendImage(b64);
    }
}
