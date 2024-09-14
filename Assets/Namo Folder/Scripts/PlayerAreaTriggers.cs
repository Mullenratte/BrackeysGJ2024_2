using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAreaTrigers : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Area1"))
        {
            Debug.Log("you are in area 1");
            FindObjectOfType<SoundManager>().PlaySound("sound1");
        }

        else if (other.CompareTag("Area2"))
        {
            Debug.Log("you are in area 2");
            FindObjectOfType<SoundManager>().PlaySound("sound2");
        }

        else if (other.CompareTag("Area3"))
        {
            Debug.Log("you are in area 3");
            FindObjectOfType<SoundManager>().PlaySound("sound3");
        }
    }

}
