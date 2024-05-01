using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public Vector3 direction;
    public Vector2 headsetPosition2D;
    public bool isWindowOpening;
    public float windowLength;
    public Vector2 windowPosition2D;

}
