using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed = 10f;
    public float fireRate = 0.3f;
    public float health = 10;
    public int score = 100;
    public float powerUpDropChance = 1f;

    protected bool calledShipDestroyed = false;
    protected BoundsCheck bndCheck;
    
    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    public Vector3 pos
    {
        get{ return this.transform.position;}
        set{this.transform.position = value;}
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offDown))
        {
            Destroy(gameObject);
        }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    void OnCollisionEnter(Collision coll)
    {
        if (calledShipDestroyed) return;
 
        GameObject otherGO = coll.gameObject;
        ProjectileHero p = otherGO.GetComponent<ProjectileHero>();
        if(p != null)
        {
            if (bndCheck.isOnScreen)
            {
                Destroy(p.gameObject); // Destroy projectile first to prevent further collisions
 
                health -= Main.GET_WEAPON_DEFINITION(p.type).damageOnHit;
 
                if(health <= 0)
                {
                    calledShipDestroyed = true;
                    Destroy(this.gameObject);
                    Main.SHIP_DESTROYED(this);
                }
            }
        } else {
            print("Enemy hit by non-ProjectileHero: " + otherGO.name);
        }      
    }
}
