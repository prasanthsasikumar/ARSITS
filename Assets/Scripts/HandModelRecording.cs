using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViveHandTracking;

public class HandModelRecording : ScriptableObject
{
    public List<GestureResult> GRFrames;
    public List<HandFrame> Handframes = new List<HandFrame>();
    public List<float> frameTimes = new List<float>();
    public int seekToFrame = 0;
    public int finishAtSeconds = 0;

    public List<GestureResult> GetGestureResults()
    {
        foreach(HandFrame Hframe in Handframes)
        {
            GRFrames.Add(new GestureResult(Hframe.isLeft, Hframe.points, Hframe.gesture, Hframe.position, Hframe.rotation));
        }
        return GRFrames;
    }
}

[Serializable]
public class HandFrame : IEquatable<HandFrame>
{
    public bool isLeft;
    public Vector3[] points;
    public GestureType gesture;
    public Vector3 position;
    public Quaternion rotation;

    public HandFrame(bool isleft, Vector3[] points, GestureType gesture, Vector3 position, Quaternion rotation)
    {
        this.isLeft = isleft;
        this.points = points;
        this.gesture = gesture;
        this.position = position;
        this.rotation = rotation;
    }

    public bool Equals(HandFrame other)
    {
        throw new NotImplementedException();
    }
}

