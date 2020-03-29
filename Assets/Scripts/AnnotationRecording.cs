using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotationRecording : ScriptableObject
{
    public List<AFrame> AFrames = new List<AFrame>();
    public float seekTime = 0;
}

[Serializable]
public class AFrame
{
    public List<Vector3> positions;
    public List<float> frameTimes = new List<float>();

    public AFrame(List<Vector3> vector3s, List<float> timestamps)
    {
        positions = vector3s;
        frameTimes = timestamps;
    }
}