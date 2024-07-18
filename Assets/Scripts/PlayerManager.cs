using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private ThirdPersonController _player;
    private StarterAssetsInputs _input;
    private CharacterController _characterController;
    public float respawnFxTime = 0f;

    public ThirdPersonController player
    {
        get
        {
            if (_player == null)
            {
                _player = GetComponent<ThirdPersonController>();
            }
            return _player;
        }
    }

    public StarterAssetsInputs input
    {
        get
        {
            if (_input == null)
            {
                _input = GetComponent<StarterAssetsInputs>();
            }
            return _input;
        }
    }

    public CharacterController characterController
    {
        get
        {
            if (_characterController == null)
            {
                _characterController = GetComponent<CharacterController>();
            }
            return _characterController;
        }
    }

    public void Respawn()
    {
        Vector3 spawnPos;
        Quaternion spawnRot;
        MapManager.Instance.GetSpawnPoint(out spawnPos, out spawnRot);
        var inputs = GetComponent<StarterAssetsInputs>();
        inputs.jump = false;
        input.switchSide = false;
        player.Teleport(spawnPos, spawnRot);
        player.IsDead = false;
    }

    public IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnFxTime);
        Respawn();
    }
}
