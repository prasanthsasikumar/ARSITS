using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CharecterManager : MonoBehaviour
{
    public enum SaveType
    {
        None,
        UnityAsset
    }

    [Header("Record/Playback Settings")]
    [Header("Editor Settings")]
    [SerializeField]
    protected KeyCode _startRecording = KeyCode.PageUp;
    [SerializeField]
    protected KeyCode _endRecording = KeyCode.PageDown;
    [SerializeField]
    protected SaveType _saveType = SaveType.UnityAsset;
    [SerializeField]
    protected string _unityAssetSavePath = "Assets/Resources/CharecterRecording";
    public bool playbackMode = false;
    public GameObject head, leftHand, rightHand, playBackHead, playbackLeftHand, playbackRightHand;
    public GameObject ik_recording, ik_playback;
    public List<CharecterRecording> recordings = new List<CharecterRecording>();
    public int currentInstruction = 0;
    public AudioSource next, previous, completed;
    public float seekSeconds = 0f;//leave this 0 for now!

    protected float _beginTime;
    protected CharecterRecording _currentRecording;
    protected bool seeked = false;
    protected HeatMapLogger logger;


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
            _currentRecording.headPosition.Add(head.transform.position);
            _currentRecording.headRotation.Add(head.transform.rotation);
            _currentRecording.leftHandPosition.Add(leftHand.transform.position);
            _currentRecording.leftHandRotation.Add(leftHand.transform.rotation);
            _currentRecording.rightHandPosition.Add(rightHand.transform.position);
            _currentRecording.rightHandRotation.Add(rightHand.transform.rotation);
            _currentRecording.frameTimes.Add(Time.time - _beginTime);

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
            ik_playback.SetActive(true);
            if (Input.GetKeyDown(KeyCode.N))
            {
                if (currentInstruction == recordings.Count)
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
        else
            ik_recording.SetActive(true);
    }

    IEnumerator PlaybackFunction(int currentInstruction)
    {
        print("Playing Avatar/Charecter");
        float lastFrameTime = 0f;
        for (int i = 0; i < recordings[currentInstruction].frameTimes.Count; i++)
        {
            if (seekSeconds != 0f && !seeked)
            {
                i = GetSeekFrame(); lastFrameTime = recordings[currentInstruction].frameTimes[i-1];
            }

            yield return new WaitForSeconds(recordings[currentInstruction].frameTimes[i] - lastFrameTime);
            playBackHead.transform.position = recordings[currentInstruction].headPosition[i];
            playBackHead.transform.rotation = recordings[currentInstruction].headRotation[i];
            playbackLeftHand.transform.position = recordings[currentInstruction].leftHandPosition[i];
            playbackLeftHand.transform.rotation = recordings[currentInstruction].leftHandRotation[i];
            playbackRightHand.transform.position = recordings[currentInstruction].rightHandPosition[i];
            playbackRightHand.transform.rotation = recordings[currentInstruction].rightHandRotation[i];
            lastFrameTime = recordings[currentInstruction].frameTimes[i];
        }
        print("End of Avatar/Charecter Playback");
        completed.Play();
    }

    public int GetSeekFrame()
    {
        for (int i = 0; i < recordings[currentInstruction].frameTimes.Count; i++)
        {
            if (seekSeconds <= recordings[currentInstruction].frameTimes[i])
            {
                seeked = true; print(i); return i - 1;                
            }
        }
        return 0;
    }

    public virtual void StartRecording()
    {
        _beginTime = Time.time;
        _currentRecording = ScriptableObject.CreateInstance<CharecterRecording>();
        Debug.Log("Avatar/Charecter Recording started");
    }
    public virtual CharecterRecording EndRecording()
    {
        CharecterRecording finishedRecording = _currentRecording;
        _currentRecording = null;

        switch (_saveType)
        {
            case SaveType.None:
                break;
            case SaveType.UnityAsset:
#if UNITY_EDITOR
                //Directory.CreateDirectory(_unityAssetSavePath + ".dummy");
                string path = AssetDatabase.GenerateUniqueAssetPath(_unityAssetSavePath + ".asset");
                AssetDatabase.CreateAsset(finishedRecording, path);
                AssetDatabase.SaveAssets();
                break;
#else
              throw new Exception("Cannot save unity assets outside of Unity Editor");
#endif
            default:
                break;
        }
        Debug.Log("Avatar/Charecter Recording Finished");
        return finishedRecording;
    }
}
