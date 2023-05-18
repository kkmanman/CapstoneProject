using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private AudioSource collectSoundEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable"))
        {
            collectSoundEffect.Play();
            ScoreSystem.AddCollectableScore();
            collision.gameObject.GetComponent<Animator>().SetTrigger("death");
            //Destroy(collision.gameObject);
        }
    }
}
