using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharecterRecording : ScriptableObject
{
    public List<Vector3> headPosition = new List<Vector3>();
    public List<Quaternion> headRotation = new List<Quaternion>();
    public List<Vector3> leftHandPosition = new List<Vector3>();
    public List<Quaternion> leftHandRotation = new List<Quaternion>();
    public List<Vector3> rightHandPosition = new List<Vector3>();
    public List<Quaternion> rightHandRotation = new List<Quaternion>();
    public List<float> frameTimes = new List<float>();
}
