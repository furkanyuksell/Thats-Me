using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] Animator _doorAnimator;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _doorAnimator.SetBool("character_nearby", true);
        }
    }
}
