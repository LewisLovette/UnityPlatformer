using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    public int health;
    public GameObject deathAnim;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Instantiate(deathAnim, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void DamageTaken(int damage)
    {
        health -= damage;
        Debug.Log("Enemy has taken " + damage + " damage");
    }
}
