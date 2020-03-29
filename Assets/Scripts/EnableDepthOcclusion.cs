using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vive.Plugin.SR;

public class EnableDepthOcclusion : MonoBehaviour
{

    public bool DepthOcclusionEnabled;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DepthOcclusionEnabled)
        {
            ViveSR_DualCameraImageCapture.EnableDepthProcess(true);
            ViveSR_DualCameraRig.Instance.TrackedCameraLeft.DepthImageOccluder.gameObject.SetActive(true);
            ViveSR_DualCameraRig.Instance.TrackedCameraRight.DepthImageOccluder.gameObject.SetActive(true);
            ViveSR_DualCameraImageRenderer.UpdateDepthMaterial = true;
        }
        else
        {
            ViveSR_DualCameraImageCapture.EnableDepthProcess(false);
            ViveSR_DualCameraRig.Instance.TrackedCameraLeft.DepthImageOccluder.gameObject.SetActive(false);
            ViveSR_DualCameraRig.Instance.TrackedCameraRight.DepthImageOccluder.gameObject.SetActive(false);
            ViveSR_DualCameraImageRenderer.UpdateDepthMaterial = false;
        }
    }
}
