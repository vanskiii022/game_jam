using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnWinTriggerEnter");
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerManager playerManager = other.transform.parent.GetComponent<PlayerManager>();

            if (playerManager != null)
            {
                GameManager.Instance.ToWinMenu();
            }
        }
    }
}
