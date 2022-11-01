using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableWall : MonoBehaviour
{
    //public GameObject player;
    AudioSource breakingSound;
    Renderer rend;
    ParticleSystem explosionEffect;
    // Start is called before the first frame update
    void Start()
    {
        breakingSound = this.GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        explosionEffect = GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            breakingSound.Play();
            explosionEffect.Play();
            rend.enabled = false;
            this.GetComponent<Collider>().enabled = false;
            Destroy(this.gameObject, breakingSound.clip.length);
        }
          
    }
}
