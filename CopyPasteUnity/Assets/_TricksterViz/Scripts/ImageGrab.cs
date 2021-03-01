using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[System.Serializable]
public class ImageGrabbedEvent : UnityEvent<string>
{
}

public class ImageGrab : MonoBehaviour
{

    public ARCameraManager cameraManager;
    public ImageGrabbedEvent ImageGrabbed;

    private Texture2D _texture;
    private string _base64 = "";
    private bool _isBusy = false;

    void Update(){

        if(Input.touchCount > 0 && !_isBusy){
            if(Input.touches[0].phase == TouchPhase.Began){
                Grab();
            }
        }
    }

    void Grab(){
        //XRCpuImage is a pointer into device memory, handle carefully
        if( cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image) ){
            StartCoroutine(Process(image));//pass off a reference to image
            image.Dispose(); //clean up this reference to image
        }
    }


    IEnumerator Process(XRCpuImage image)
    {
        _isBusy = true; //set flag to prevent multiple calls to Process.
        var request = image.ConvertAsync(new XRCpuImage.ConversionParams{
            inputRect = new RectInt( 0, 0, image.width, image.height),
            //downsample in half
            outputDimensions = new Vector2Int(image.width/2, image.height/2),
            outputFormat = TextureFormat.RGB24,
            transformation = XRCpuImage.Transformation.None
        });

        //wait for it to finish
        while(!request.status.IsDone()){
            yield return null;      
        }

        if(request.status != XRCpuImage.AsyncConversionStatus.Ready){
            Debug.Log($"image is done, but not ready.");
            request.Dispose();
            _isBusy = false;  //set flag to allow new calls to Process
            yield break;
        }

        var rawData = request.GetData<byte>();
        if(_texture == null){
            _texture =new Texture2D(
                request.conversionParams.outputDimensions.x,
                request.conversionParams.outputDimensions.y,
                request.conversionParams.outputFormat,
                false);
        }

        //To texture, to PNG, to Base64
        _texture.LoadRawTextureData(rawData);
        _texture.Apply();

        string _base64 = System.Convert.ToBase64String(_texture.EncodeToPNG());
        request.Dispose();
        
        PublishBase64(_base64);
        _isBusy = false; //set flag to allow new calls to Process
    }

    public void PublishBase64(string b64){
        //cache the b64 image string 
        _base64 = b64;

        //Emit Event with image string as payload.
        ImageGrabbed.Invoke(b64);
    }

    //make the _base64 image string available via component access
    public string GetBase64Image(){
        return _base64;
    }
}
