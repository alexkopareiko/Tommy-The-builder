using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int maxMana = 100;
    public int currentMana;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;
        Debug.Log("currentHealth " + currentHealth);
        // Play hurt animation

        if(currentHealth <= 0) {
            Die();
        }
    }
    public void SpendMana(int mana){
        currentMana -= mana;
        Debug.Log("currentMana " + currentMana);
    }

    void Die() {
        Debug.Log("Player died");
        // Die animation

        // Disable the enemy
    }


}
