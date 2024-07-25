using System.Collections;
using System;
using UnityEngine;

public class PathRequest
{
    public Vector3 startPos;
    public Vector3 targetPos;
    public Action<Vector3[], bool> callback;
    public bool findCrystal;
    
    public PathRequest(Vector3 startPos, Vector3 targetPos, Action<Vector3[], bool> callback, bool findCrystal)
    {
        this.startPos = startPos;
        this.targetPos = targetPos;
        this.callback = callback;
        this.findCrystal = findCrystal;
    }
}
