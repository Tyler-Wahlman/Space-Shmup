using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
/// <summary>
/// Enemy_5 – "Zigzagger"
/// 
/// A fast enemy that alternates sharp horizontal dashes as it descends.
/// It accelerates each time it reaches a dash waypoint, making it
/// increasingly dangerous the longer it survives.
///
/// HOW TO SET UP IN UNITY:
/// 1. Duplicate any Enemy prefab and replace the Enemy_X script with Enemy_5.
/// 2. Set speed to ~14, score to 250, health to ~8.
/// 3. Adjust dashInterval and dashWidth in the Inspector.
/// 4. Add this prefab to Main's prefabEnemies array.
/// </summary>
public class Enemy_5 : Enemy
{
    [Header("Enemy_5 Inscribed Fields")]
    [Tooltip("How many seconds between each horizontal dash")]
    public float dashInterval = 0.8f;
    [Tooltip("How far left/right the enemy dashes (world units)")]
    public float dashWidth = 4f;
    [Tooltip("Speed of the horizontal dash lerp")]
    public float dashSpeed = 12f;
    [Tooltip("Speed increase applied each time the enemy dashes")]
    public float speedRampPerDash = 0.5f;
 
    private float nextDashTime;
    private float targetX;
    private float birthTime;
    private int   dashCount = 0;
 
    void Start()
    {
        birthTime   = Time.time;
        nextDashTime = Time.time + dashInterval;
        targetX      = pos.x;
    }
 
    public override void Move()
    {
        // ── Descend (inherited downward speed) ────────────────────────────
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
 
        // ── Horizontal Dash ───────────────────────────────────────────────
        if (Time.time >= nextDashTime)
        {
            dashCount++;
            speed += speedRampPerDash;          // ramp up vertical speed too
            nextDashTime = Time.time + dashInterval;
 
            // Alternate direction; clamp within screen
            float camW = bndCheck != null ? bndCheck.camWidth : 8f;
            float half  = dashWidth * 0.5f;
            float newX  = (dashCount % 2 == 0)
                          ? Mathf.Clamp(tempPos.x + dashWidth, -camW + 1f,  camW - 1f)
                          : Mathf.Clamp(tempPos.x - dashWidth, -camW + 1f,  camW - 1f);
            targetX = newX;
        }
 
        // Smoothly interpolate toward targetX
        tempPos.x = Mathf.MoveTowards(tempPos.x, targetX, dashSpeed * Time.deltaTime);
 
        // Roll the ship in the dash direction for a nice visual lean
        float xDiff = targetX - tempPos.x;
        float rollAngle = Mathf.Clamp(xDiff * -8f, -45f, 45f);
        this.transform.rotation = Quaternion.Euler(0, rollAngle, 0);
 
        pos = tempPos;
    }
}
