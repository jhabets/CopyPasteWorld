using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using UnityEngine;

public class RekognitionClient : MonoBehaviour
{
    private string COGNITO_ID = "us-west-2:385f5673-2391-4d6d-a78d-9c1b225304e5";
    private RegionEndpoint REGION = RegionEndpoint.USWest2;
    private CognitoAWSCredentials _credentials;
    private AmazonRekognitionClient _rekClient;
    
    void Start()
    {
        Debug.Log("Test 2");   
        GetCognitoClient();
    }

    
    private void GetCognitoClient()
    {
        // Initialize the Amazon Cognito credentials provider
        _credentials = new CognitoAWSCredentials (
            COGNITO_ID, // Identity pool ID
            REGION // Region
        );
        _rekClient = new AmazonRekognitionClient(_credentials, REGION);
    }

    public async void SendImage(string bs4){
  
        DetectTextRequest detectTextRequest = new DetectTextRequest()
        {
            Image = new Image()
            {
               Bytes = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(bs4))
            }
        };

        try
        {
            Debug.Log("starting detectText");
            //DetectTextResponse detectTextResponse = _rekClient.DetectText(detectTextRequest);
            DetectTextResponse detectTextResponse = await _rekClient.DetectTextAsync(detectTextRequest);
            Debug.Log("Response for Image: ");
            foreach (TextDetection text in detectTextResponse.TextDetections)
            {
                Debug.Log("Detected: " + text.DetectedText);
                Debug.Log("Confidence: " + text.Confidence);
                Debug.Log("Id : " + text.Id);
                Debug.Log("Parent Id: " + text.ParentId);
                Debug.Log("Type: " + text.Type);
            }
        }
        finally{
            Debug.Log("completed detectText");
        };
    }


}
