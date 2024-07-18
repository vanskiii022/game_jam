using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter");
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerManager playerManager = other.transform.parent.GetComponent<PlayerManager>();

            if (playerManager != null)
            {
                playerManager.Respawn();
            }
        }
    }
}
