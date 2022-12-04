using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableWall : MonoBehaviour
{
    public BulldozerAbility bda;
    Collider collie;

    AudioSource breakingSound;
    Renderer rend;
    ParticleSystem explosionEffect;
    // Start is called before the first frame update
    void Start()
    {
        bda.ReportBulldozing += toggleTrigger;

        breakingSound = this.GetComponent<AudioSource>();
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        explosionEffect = GetComponent<ParticleSystem>();
        collie = GetComponent<Collider>();
        collie.isTrigger = false;

    }

    void toggleTrigger(bool yesno)
    {
        collie.isTrigger = yesno;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Player")) {
            AbObserver2 observer = other.gameObject.GetComponent<AbObserver2>();
            if (observer.ability.GetAbilityName() == "Bulldozer")
            {
                breakingSound.Play();
                explosionEffect.Play();
                rend.enabled = false;
                this.GetComponent<Collider>().enabled = false;
                Destroy(this.gameObject, breakingSound.clip.length);
            }
        }
          
    }
}
