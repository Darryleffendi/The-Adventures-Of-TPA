using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> requestQueue = new Queue<PathRequest>();
    PathRequest currentRequest;

    Pathfinding pathfinding;
    static PathRequestManager instance;

    bool isProcessing = false;

    void Start()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    public static void RequestPath(Vector3 startPos, Vector3 targetPos, Action<Vector3[], bool> callback, bool findCrystal = false)
    {
        PathRequest newRequest = new PathRequest(startPos, targetPos, callback, findCrystal);

        instance.requestQueue.Enqueue(newRequest);
        instance.ProcessPath();
    }

    private void ProcessPath()
    {
        if (isProcessing || requestQueue.Count <= 0) return;

        currentRequest = requestQueue.Dequeue();
        isProcessing = true;
        pathfinding.StartPathfinding(currentRequest.startPos, currentRequest.targetPos, currentRequest.findCrystal);
    }

    public void FinishPathProcessing(Vector3[] path, bool success)
    {
        currentRequest.callback(path, success);
        isProcessing = false;
        ProcessPath();
    }
}
