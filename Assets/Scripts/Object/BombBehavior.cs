using System.Collections;
using UnityEngine;
using Interfaces;

public class BombBehavior : MonoBehaviour, Iinteractive
{
    public float explosionDelay = 3.0f;
    public float explosionRadius = 5.0f;
    public int explosionDamage = 50;

    private bool isHeld = false;
    private Transform player;

    private Rigidbody rb;
    private ParticleSystem ps;
    public AudioSource m_AudioSource;

    private void Awake(){
        rb = gameObject.GetComponent<Rigidbody>();
        ps = gameObject.GetComponent<ParticleSystem>();
    }

    public void Interact()
    {
        if (!isHeld)
        {
            Pickup();
        }
        else
        {
            StartCoroutine(ThrowAndExplode());
        }
    }

    private void Pickup()
    {
        player = GameObject.FindWithTag("Player").transform;
        if (player != null)
        {
            isHeld = true;
            transform.SetParent(player);
            transform.localPosition = new Vector3(0, 0, 0.4f);
            rb.useGravity = false;
        }
    }

    private IEnumerator ThrowAndExplode()
    {
        isHeld = false;
        rb.useGravity = true;
        transform.SetParent(null);
        rb.AddForce(player.forward * 10.0f, ForceMode.Impulse);

        yield return new WaitForSeconds(explosionDelay);
        StartCoroutine(Explode()); 

    }

    private IEnumerator Explode()
    {
        if (m_AudioSource != null) m_AudioSource.Play();
        ps.Play();
        yield return new WaitForSeconds(0.1f);   
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider obj in hitObjects)
        {
            if (obj.CompareTag("Enemy"))
            {
                Enemy enemy = obj.GetComponent<Enemy>();
                enemy.Damage(explosionDamage);
            }
        }
        
        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
