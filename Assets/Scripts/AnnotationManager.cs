
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Valve.VR;

public class AnnotationManager : MonoBehaviour
{
    int lineNumber = 0;
    bool triggerClicked = false;
    bool triggerPressed = false;
    bool triggerUnclicked = false;

    public Material lMat;
    public bool playbackMode;
    private LineRenderer currLine;
    private int numClicks = 0;

    [Header("Editor Settings")]
    [SerializeField]
    protected KeyCode _startRecording = KeyCode.F5;
    [SerializeField]
    protected KeyCode _endRecording = KeyCode.F6;
    [SerializeField]
    protected string _unityAssetSavePath = "Assets/Resources/AnnotationRecording";

    public List<AnnotationRecording> AnnotationRecordings = new List<AnnotationRecording>();
    public int currentInstruction = 0;
    public AudioSource next, previous, completed;

    protected float _beginTime;
    protected AnnotationRecording _currentRecording;
    protected GameObject currentLineObject;
    protected List<float> timestamps;
    protected HeatMapLogger logger;

    public virtual void StartRecording()
    {
        _beginTime = Time.time;
        _currentRecording = ScriptableObject.CreateInstance<AnnotationRecording>();
        Debug.Log("Recording started");
    }

    public virtual AnnotationRecording EndRecording()
    {
        AnnotationRecording finishedRecording = _currentRecording;
        _currentRecording = null;
#if UNITY_EDITOR
        //Directory.CreateDirectory(_unityAssetSavePath + ".dummy");
        string path = AssetDatabase.GenerateUniqueAssetPath(_unityAssetSavePath + ".asset");
        AssetDatabase.CreateAsset(finishedRecording, path);
        AssetDatabase.SaveAssets();
#else
              throw new Exception("Cannot save unity assets outside of Unity Editor");
#endif
        Debug.Log("Annotation Recording Finished");
        return finishedRecording;
    }

    private void Start()
    {
        if (playbackMode)
            StartCoroutine(PlaybackFunction(currentInstruction));

        logger = GameObject.FindObjectOfType<HeatMapLogger>();
    }

    void Update()
    {
        if (_currentRecording != null)
        {
            if (Input.GetKeyDown(_endRecording))
            {
                EndRecording();
            }
        }
        else
        {
            if (Input.GetKeyDown(_startRecording))
            {
                StartRecording();
            }
        }

        if (playbackMode)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                if (currentInstruction == AnnotationRecordings.Count)
                    return;

                currentInstruction++;
                next.Play();
                logger.AddKeyLogs("Next");
                StartCoroutine(PlaybackFunction(currentInstruction));
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                if (currentInstruction == 0)
                    return;

                currentInstruction--;
                //Play the previous cue here
                previous.Play();
                logger.AddKeyLogs("Previous");
                StartCoroutine(PlaybackFunction(currentInstruction));
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                logger.AddKeyLogs("Repeat");
                StartCoroutine(PlaybackFunction(currentInstruction));
            }
        }
    }

    IEnumerator PlaybackFunction(int insturctionNumber)
    {
        foreach (GameObject annotation in GameObject.FindGameObjectsWithTag("Annotation"))//Remove every other instruction
        {
            Destroy(annotation);
        }

        float lastFrameTime = AnnotationRecordings[insturctionNumber].seekTime;
        foreach (AFrame aFrame in AnnotationRecordings[insturctionNumber].AFrames)
        {
            GameObject go = new GameObject("AnnotationInstruction"+insturctionNumber);
            go.tag = "Annotation";
            go.AddComponent<MeshFilter>();
            go.AddComponent<MeshRenderer>();
            currLine = go.AddComponent<LineRenderer>();
            currLine.startWidth = 0.01f;//Expose this as public laters
            currLine.endWidth = 0.01f;
            currLine.material = lMat;
            for (int i = 0; i < aFrame.positions.Count; i++)
            {
                yield return new WaitForSeconds(aFrame.frameTimes[i] - lastFrameTime);
                lastFrameTime = aFrame.frameTimes[i];
                currLine.positionCount = i + 1;
                currLine.SetPosition(i, aFrame.positions[i]);
            }
        }
        completed.Play();
    }

    public void HandleTriggerClicked(Transform transform)
    {
        GameObject go = new GameObject("Annotation");
        go.AddComponent<MeshFilter>();
        go.AddComponent<MeshRenderer>();
        currLine = go.AddComponent<LineRenderer>();
        currLine.startWidth = 0.01f;
        currLine.endWidth = 0.01f;
        currLine.SetPosition(0, transform.position);
        currLine.material = lMat;
        numClicks = 0;
        currentLineObject = go;
        timestamps = new List<float>();
        timestamps.Add(Time.time - _beginTime);
    }

    public void HandleTriggerUnclicked(Transform transform)
    {
        numClicks = 0;
        if (_currentRecording != null)
        {
            List<Vector3> pointPositions = new List<Vector3>();
            LineRenderer lr = currentLineObject.GetComponent<LineRenderer>();
            for (int i = 0; i < lr.positionCount; i++)
            {
                pointPositions.Add(lr.GetPosition(i));
            }
            _currentRecording.AFrames.Add(new AFrame(pointPositions, timestamps));
            timestamps = null;
        }
    }

    public void HandleTriggerPressed(Transform transform)
    {
        numClicks++;
        currLine.positionCount = numClicks + 1;
        currLine.SetPosition(numClicks, transform.position);
        timestamps.Add(Time.time - _beginTime);
    }

}
