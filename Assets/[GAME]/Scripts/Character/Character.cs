﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CharacterControllerType { Player, AI }

public class Character : MonoBehaviour
{
    public CharacterControllerType CharacterControllerType = CharacterControllerType.Player;

    private Rigidbody rigidbody;
    public Rigidbody Rigidbody { get { return (rigidbody == null) ? rigidbody = GetComponent<Rigidbody>() : rigidbody; } }

    private Collider collider;
    public Collider Collider { get { return (collider == null) ? collider = GetComponent<Collider>() : collider; } }

    private bool isControlable;
    public bool IsControlable { get { return isControlable; } set { isControlable = value; } }



    private void OnEnable()
    {
        if (Managers.Instance == null)
            return;

        CharacterManager.Instance.AddCharacter(this);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null)
            return;

        CharacterManager.Instance.RemoveCharacter(this);

    }

    private void OnTriggerEnter(Collider other)
    {
        DoorBase door = other.GetComponent<DoorBase>();
        if (door != null)
        {
            GetComponent<CharacterController>().Stay();
            IsControlable = true;
            if (CharacterControllerType == CharacterControllerType.Player)
                door.PlayerInputSelection();
            else
            {
                Debug.Log(other.name + " AI Collide");
                StartCoroutine(door.AIInputSelection(GetComponent<AIController>()));
            }
            //Animation Trigger
        }
    }

}