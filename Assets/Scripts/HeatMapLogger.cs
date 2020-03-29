using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HeatMapLogger : MonoBehaviour
{
    public bool loggingEnabled = false;
    public GameObject ObjectToLog, playback;
    public CueType thisCondition;

    protected string _unityAssetSavePath = "Assets/Resources/Log/";
    protected float _beginTime;
    protected bool isLogging = false;
    protected UserLog currentLog;

    void Update()
    {
        if (loggingEnabled)
        {
            if (!isLogging)
            {
                isLogging = true;
                currentLog = ScriptableObject.CreateInstance<UserLog>();
                AddKeyLogs("StartSession");
            }
            switch (thisCondition)
            {
                case CueType.Annotation:
                    currentLog.UserLogFrames.Add(new LogFrame(ObjectToLog.transform.position, ObjectToLog.transform.rotation, (Time.time - _beginTime)));
                    break;
                case CueType.Hands:
                    currentLog.UserLogFrames.Add(new LogFrame(ObjectToLog.transform.position, ObjectToLog.transform.rotation, (Time.time - _beginTime)));
                    currentLog.PlaybackLogFrames.Add(new LogFrame(playback.transform.position, playback.transform.rotation, (Time.time - _beginTime)));
                    break;
                case CueType.Charecter:
                    currentLog.UserLogFrames.Add(new LogFrame(ObjectToLog.transform.position, ObjectToLog.transform.rotation, (Time.time - _beginTime)));
                    currentLog.PlaybackLogFrames.Add(new LogFrame(playback.transform.position, playback.transform.rotation, (Time.time - _beginTime)));
                    break;
                case CueType.Volumetric:
                    currentLog.UserLogFrames.Add(new LogFrame(ObjectToLog.transform.position, ObjectToLog.transform.rotation, (Time.time - _beginTime)));
                    break;
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (currentLog == null)
            return;

        AddKeyLogs("EndSession");
        switch (thisCondition)
        {
            case CueType.Annotation:
                _unityAssetSavePath += "Annotation";
                break;
            case CueType.Hands:
                _unityAssetSavePath += "Hands";
                break;
            case CueType.Charecter:
                _unityAssetSavePath += "Charecter";
                break;
            case CueType.Volumetric:
                _unityAssetSavePath += "Volumetric";
                break;
        }
        string path = AssetDatabase.GenerateUniqueAssetPath(_unityAssetSavePath + getTimeStamp() + ".asset");
        AssetDatabase.CreateAsset(currentLog, path);
        AssetDatabase.SaveAssets();
    }

    public string getTimeStamp()
    {
        return DateTime.Now.Hour + ":" + DateTime.Now.Minute;
    }

    public enum CueType
    {
        Annotation,
        Hands,
        Charecter,
        Volumetric
    }

    public enum KeyType//NOT USED Currently.
    {
        Next,
        Previous,
        Repeat,
        Completed
    }

    public void AddKeyLogs(string key)
    {
        if (currentLog)
            currentLog.KeyLogFrames.Add(new LogFrame(key, (Time.time - _beginTime)));
    }

}

