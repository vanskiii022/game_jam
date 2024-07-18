using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private ThirdPersonController _player;
    private StarterAssetsInputs _input;
    private CharacterController _characterController;

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
}
