using UnityEngine;

public class confettigoboom : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent <ParticleSystem>().Play ();
        ParticleSystem.EmissionModule em = GetComponent<ParticleSystem>().emission;
        em.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
