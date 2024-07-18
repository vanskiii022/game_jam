using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // Assign in inspector
    public float followSpeed = 2f;
    public bool isFollowing = false;

    void Update()
    {
        if (isFollowing)
        {
            float randomX = Random.Range(-0.1f, 0.1f);
            float randomZ = Random.Range(-0.1f, 0.1f);

            // 将随机偏移量应用到物体的位置上
            transform.position = PlayerManager.Instance.transform.position + new Vector3(randomX, 0, 2f + randomZ);
        }
    }

    public void StartFollowing()
    {
        isFollowing = true;
    }

    public void StopFollowing()
    {
        isFollowing = false;
    }
    public void ToggleFollowing()
    {
        isFollowing = !isFollowing;
    }
}
