using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This is currently not being used!!
public class HandManager : MonoBehaviour
{
    [Header("Editor Settings")]
    [SerializeField]
    protected KeyCode _startRecording = KeyCode.F5;
    [SerializeField]
    protected KeyCode _endRecording = KeyCode.F6;
    [SerializeField]
    protected string _unityAssetSavePath = "Assets/Resources/HandRecording";

    public List<HandModelRecording> HandRecordings = new List<HandModelRecording>();
    public int currentInstruction = 0;

    protected float _beginTime;
    protected HandModelRecording _currentRecording;
    protected GameObject currentLineObject;
    protected List<float> timestamps;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
