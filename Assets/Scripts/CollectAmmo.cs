using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectAmmo : MonoBehaviour
{
    public AudioSource SoundToPlay;
  

    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if (controller.CogAmmount <= 5)
            {
                controller.CogAmmount += 1;

            

                Destroy(gameObject);

                SoundToPlay.Play();
            }
        }

    }
}