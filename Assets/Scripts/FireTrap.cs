using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    private float triggerDelay = 2f;
    [SerializeField] private float activationDelay = 2f;
    [SerializeField] private float activationTime = 2f;
    [SerializeField] private AudioSource attackSoundEffect;

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private bool isTriggered = false;
    private bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        triggerDelay = Random.Range(4f, 6f);
    }

    private void Update()
    {
        if (triggerDelay > 0 && !isActive && !isTriggered)
        {
            triggerDelay -= Time.deltaTime;            
        } 
        else
        {            
            if (!isTriggered) 
            {
                isTriggered = true;
                spriteRenderer.color = Color.red;
                StartCoroutine(ActivateFireTrap());
            }                
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isActive && collision.gameObject.name == "Player")
        {
            collision.GetComponent<PlayerLife>().isDead = true;
            collision.GetComponent<PlayerLife>().Dead();
        }
    }

    private IEnumerator ActivateFireTrap()
    {        
        yield return new WaitForSeconds(activationDelay);
        //attackSoundEffect.Play();        
        isActive = true;
        spriteRenderer.color = Color.white;
        anim.SetBool("active", true);

        yield return new WaitForSeconds(activationTime);
        isActive = false;
        isTriggered = false;
        anim.SetBool("active", false);

        triggerDelay = Random.Range(5f, 10f);        
    }
}
