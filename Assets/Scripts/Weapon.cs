using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        
        collision.collider.gameObject.SendMessage("Damage", SendMessageOptions.DontRequireReceiver);
       
    }
}



