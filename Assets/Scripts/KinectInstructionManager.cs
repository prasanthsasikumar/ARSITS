using com.rfilkov.kinect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinectInstructionManager : MonoBehaviour
{
    public GameObject kinect1, kinect2, kinect3;
    public bool playbackMode = false;
    public int currentInstruction = 0;
    [Header("Press N(next)/P(previous)/C(completed)")]

    public AudioSource next; public AudioSource previous, completed;
    public List<string> instructionLocations = new List<string>();
    protected HeatMapLogger logger;


    void Start()
    {
        kinect1.SetActive(true); kinect2.SetActive(true); kinect3.SetActive(true);
        if (playbackMode)
        {
            kinect1.GetComponent<Kinect4AzureInterface>().deviceStreamingMode = KinectInterop.DeviceStreamingMode.PlayRecording;
            kinect2.GetComponent<Kinect4AzureInterface>().deviceStreamingMode = KinectInterop.DeviceStreamingMode.PlayRecording;
            kinect3.GetComponent<Kinect4AzureInterface>().deviceStreamingMode = KinectInterop.DeviceStreamingMode.PlayRecording;
            UpdateAndPlay();
        }
        else
        {
            kinect1.GetComponent<Kinect4AzureInterface>().deviceStreamingMode = KinectInterop.DeviceStreamingMode.ConnectedSensor;
            kinect2.GetComponent<Kinect4AzureInterface>().deviceStreamingMode = KinectInterop.DeviceStreamingMode.ConnectedSensor;
            kinect3.GetComponent<Kinect4AzureInterface>().deviceStreamingMode = KinectInterop.DeviceStreamingMode.ConnectedSensor;
        }
        logger = GameObject.FindObjectOfType<HeatMapLogger>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (currentInstruction == instructionLocations.Count)
                return;

            currentInstruction++;
            next.Play();
            logger.AddKeyLogs("Next");
            UpdateAndPlay();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (currentInstruction == 0)
                return;

            currentInstruction--;
            previous.Play();
            logger.AddKeyLogs("Previous");
            UpdateAndPlay();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            logger.AddKeyLogs("Repeat");
            UpdateAndPlay();
        }
        else if (Input.GetKeyDown(KeyCode.C))
            completed.Play();        
    }

    public void UpdateAndPlay()
    {
        kinect1.GetComponent<Kinect4AzureInterface>().recordingFile = instructionLocations[currentInstruction]+ "/output-1.mkv";
        kinect2.GetComponent<Kinect4AzureInterface>().recordingFile = instructionLocations[currentInstruction] + "/output-2.mkv";
        kinect3.GetComponent<Kinect4AzureInterface>().recordingFile = instructionLocations[currentInstruction] + "/output-3.mkv";
        this.GetComponent<KinectManager>().restartPlayback = true;
    }
}
