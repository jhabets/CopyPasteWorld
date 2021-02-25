# Congnito

## Adding AWS DOT NET SDK to Unity
Follow the instructions at [Special Considerations for Unity Support](https://docs.aws.amazon.com/sdk-for-net/latest/developer-guide/unity-special.html) in the `sdk-for-net` documentation.

1. Download the SDK zips and copy the dlls you need
2. The required Microsoft dll's should be in the ZIP
3. Add `link.xml` to `Assets` folder.
    ```
    <linker>
        <assembly fullname="AWSSDK.Core" preserve="all"/>
        <assembly fullname="AWSSDK.CognitoIdentity" preserve="all"/>
        <assembly fullname="AWSSDK.CognitoIdentityProvider" preserve="all"/>
        <assembly fullname="AWSSDK.Rekognition" preserve="all"/>
    </linker>
    ```


## Using Cognito Unauth Role in Unity

```
// Initialize the Amazon Cognito credentials provider
CognitoAWSCredentials credentials = new CognitoAWSCredentials (
    "us-west-2:385f5673-2391-4d6d-a78d-9c1b225304e5", // Identity pool ID
    RegionEndpoint.USWest2 // Region
);
```

## Use DetectText in Rekognition

Example of using .net is [here](https://docs.aws.amazon.com/rekognition/latest/dg/text-detecting-text-procedure.html)

```
public static void Example()
    {
        String photo = "input.jpg";
        String bucket = "bucket";

        AmazonRekognitionClient rekognitionClient = new AmazonRekognitionClient();

        DetectTextRequest detectTextRequest = new DetectTextRequest()
        {
            Image = new Image()
            {
                S3Object = new S3Object()
                {
                    Name = photo,
                    Bucket = bucket
                }
            }
        };

        try
        {
            DetectTextResponse detectTextResponse = rekognitionClient.DetectText(detectTextRequest);
            Console.WriteLine("Detected lines and words for " + photo);
            foreach (TextDetection text in detectTextResponse.TextDetections)
            {
                Console.WriteLine("Detected: " + text.DetectedText);
                Console.WriteLine("Confidence: " + text.Confidence);
                Console.WriteLine("Id : " + text.Id);
                Console.WriteLine("Parent Id: " + text.ParentId);
                Console.WriteLine("Type: " + text.Type);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
```

## Convert Image to Base64
Example from [StackOverflow](https://stackoverflow.com/questions/15089359/how-to-convert-image-png-to-base64-string-vice-a-versa-and-strore-it-to-a-sp/15089476) 
```
public string ImageToBase64(Image image, 
  System.Drawing.Imaging.ImageFormat format)
{
  using (MemoryStream ms = new MemoryStream())
  {
    // Convert Image to byte[]
    image.Save(ms, format);
    byte[] imageBytes = ms.ToArray();

    // Convert byte[] to Base64 String
    return Convert.ToBase64String(imageBytes);
  }
}
```
Another Example from [Unity Answers](https://answers.unity.com/questions/712673/how-to-encode-an-image-to-a-base64-string.html)

```
// Convert Texture2D to PNG as bytes then Convert
 Texture2D mytexture;
 byte[] bytes;
 
 bytes = mytexture.EncodeToPng();
 string enc = Convert.ToBase64String(bytes);
```

## Get Images from Cameras
https://assetstore.unity.com/packages/tools/integration/natdevice-media-device-api-162053

https://assetstore.unity.com/packages/tools/integration/natcorder-video-recording-api-102645#content
