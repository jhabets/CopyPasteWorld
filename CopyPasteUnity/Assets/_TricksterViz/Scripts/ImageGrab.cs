using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageGrab : MonoBehaviour
{

    public ARCameraManager cameraManager;

    private Texture2D _texture;
    private bool _isBusy = false;

    void OnEnable()
    {

        cameraManager.frameReceived += OnCameraFrameReceived;
    }

    void OnDisable()
    {
        cameraManager.frameReceived -= OnCameraFrameReceived;
    }

    void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        //try to get the image... creates a local scope image var
        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            if(!_isBusy){
                Debug.Log("Wait a minute baby. image needs time to be ready.");
                StartCoroutine(ProcessImage(image));
            }
            image.Dispose();
        }
    }

    IEnumerator ProcessImage(XRCpuImage image)
    {
        _isBusy = true;
    
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
            Debug.Log("getting image broke");
            request.Dispose();
            _isBusy = false;
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

        string base64str = System.Convert.ToBase64String(_texture.EncodeToPNG());
        request.Dispose();
        Debug.Log($"base64 string: {base64str}");
        _isBusy = false;
    }
}
