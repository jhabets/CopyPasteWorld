using System.Collections;
using System.Collections.Generic;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.Rekognition;
using UnityEngine;

public class RekognitionClient : MonoBehaviour
{
    public string hi = "hello";
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Test 2");   
        GetCognitoAuth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void GetCognitoAuth()
    {
        // Initialize the Amazon Cognito credentials provider
        CognitoAWSCredentials credentials = new CognitoAWSCredentials (
            "us-west-2:385f5673-2391-4d6d-a78d-9c1b225304e5", // Identity pool ID
            RegionEndpoint.USWest2 // Region
        );
        Debug.Log("got the credentials client");
        
        AmazonRekognitionClient rekClient = new AmazonRekognitionClient(credentials, RegionEndpoint.USWest2);
        Debug.Log("got the rekognition client");
    }
}
