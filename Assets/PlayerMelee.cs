using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    public float attackDelayTime;    //oof to variable names here
    private float attackTimer = 0;

    public Transform attackPos;
    public LayerMask whatIsEnemy;
    public float attackRange;
    public float slowTimeTo;
    private float slowTimeTimer;
    public float onHitSlowTimer;

    //For box
    //public float attackRangeX;
    //public float attackRangeY;

    public int damage;

    // Update is called once per frame
    void Update()
    {
        if (attackTimer <= 0)
        {

            if (Input.GetButtonDown("Melee"))   //looks for enemies within the circle, if in the circle do damage
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);

                //For Box
                //Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0, whatIsEnemy);

                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<Damage>().DamageTaken(damage);   //find said enemy and pass damage through.

                    if (enemiesToDamage[i].GetComponent<Damage>().health > 0)   //Only slow time if enemy hasn't died from this hit.
                    {
                        slowTimeTimer = onHitSlowTimer;
                    }
                }

                attackTimer = attackDelayTime;
            }
        }
        else
        {  
            attackTimer -= Time.deltaTime;
            //Debug.Log("Time left: " + attackTimer);

            //slowing time on melee hit
            //if slow time to 0.1 then time runs at 1/10th speed and so 0.6 = 6 sec.
            //to keep in this 'else statement' needs to add up and be lower than 'attackDelayTime' - this gives efficiency.
            if (slowTimeTimer <= 0)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = slowTimeTo;
                slowTimeTimer -= Time.deltaTime;
            }

        }

        
    }

    void OnDrawGizmosSelected() //drawing red circle to indicate position
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);

        //For Box
        //Gizmos.DrawWireSphere(attackPos.position, new Vector3(attackRangeX, attackRangeY, 1));
        
    }
}
