using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerFollowCamera : MonoBehaviour
{
    public bool isBSide;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;
    public Vector3 offset = new Vector3(10, 15, 10);

    public void OnLateUpdate()
    {
        var player = PlayerManager.Instance;
        if (player != null)
        {
            Vector3 targetPosition = MapManager.Instance.GetPlayerRelativePos() + offset + (isBSide ? 1 : 0) * MapManager.Instance.bSidePos;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            transform.position = smoothedPosition;
        }
    }
}
