using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserLog : ScriptableObject
{
    public List<LogFrame> UserLogFrames = new List<LogFrame>();
    public List<LogFrame> PlaybackLogFrames = new List<LogFrame>();
    public List<LogFrame> KeyLogFrames = new List<LogFrame>();
}


[Serializable]
public class LogFrame
{
    public Vector3 position;
    public Quaternion rotation;
    public float frameTime;
    public string keyPressed;

    public LogFrame(Vector3 vector3s, Quaternion rotations, float timestamp)
    {
        position = vector3s;
        frameTime = timestamp;
        this.rotation = rotations;
    }
    public LogFrame(string key, float timestamp)
    {
        keyPressed = key;
        frameTime = timestamp;
    }
}
