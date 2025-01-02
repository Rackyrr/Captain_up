using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{

    public float health = 100f;
    public float maxHealth = 100f;

    public Image healthBarImage;

     public UnityEvent Death;

    // Update is called once per frame
    void Update()
    {
        healthBarImage.fillAmount = health / maxHealth;
    }

    public void TakeDammage(int dammage)
    {
        if (health < dammage)
        {
            health = 0;
        }
        else
        {
            health -= dammage;

        }
        
        if (health <= 0)
        {
            die();
        }
    }

    public void TakeHeal(int heal) 
    {
        if (health + heal > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += heal;
        }
    }

    public void die()
    {
        Death?.Invoke();
        //Faire les HUD
    }
}
