﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
  private Rigidbody playerRb;
  private float speed = 5.0f;
  private GameObject focalPoint;

  public bool hasPowerup;
  public GameObject powerupIndicator;
  public ParticleSystem smokeParticle;
  public int powerUpDuration = 5;
  public float speedBoostFactor = 10.0f;
  public float distanceBehind = 1.0f;

  private float normalStrength = 10; // how hard to hit enemy without powerup
  private float powerupStrength = 25; // how hard to hit enemy with powerup

  void Start()
  {
    playerRb = GetComponent<Rigidbody>();
    focalPoint = GameObject.Find("Focal Point");
  }

  void Update()
  {
    // Add force to player in direction of the focal point (and camera)
    float verticalInput = Input.GetAxis("Vertical");
    playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed);

    // Set powerup indicator position to beneath player
    powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

    // Player gets a speed boost whenever space is pressed
    if (Input.GetKeyDown(KeyCode.Space))
    {
      playerRb.AddForce(focalPoint.transform.forward * speedBoostFactor, ForceMode.Impulse);
      smokeParticle.Play();
    }

    ManageSmokeParticleEffect();
  }

  // Used to sort of keep the particle emitter from rolling with the player
  private void ManageSmokeParticleEffect()
  {
    // Determine the backward direction of the sphere
    Vector3 backwardDirection = -playerRb.transform.forward;

    // Offset position for the particle effect to make it appear behind the sphere
    Vector3 offset = backwardDirection * distanceBehind;

    // Update the particle effect's position
    smokeParticle.transform.position = playerRb.transform.position + offset;
  }

  // If Player collides with powerup, activate powerup
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.CompareTag("Powerup"))
    {
      Destroy(other.gameObject);
      hasPowerup = true;
      powerupIndicator.SetActive(true);
      StartCoroutine(PowerupCooldown());
    }
  }

  // Coroutine to count down powerup duration
  IEnumerator PowerupCooldown()
  {
    yield return new WaitForSeconds(powerUpDuration);
    hasPowerup = false;
    powerupIndicator.gameObject.SetActive(false);
  }

  // If Player collides with enemy
  private void OnCollisionEnter(Collision other)
  {
    if (other.gameObject.CompareTag("Enemy"))
    {
      Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
      // should be enemy pos - player pos
      Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;

      if (hasPowerup) // if have powerup hit enemy with powerup force
      {
        enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
      }
      else // if no powerup, hit enemy with normal strength 
      {
        enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
      }


    }
  }



}
