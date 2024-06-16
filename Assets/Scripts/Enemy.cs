using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject deathFX;
    [SerializeField] Transform parent;
    [SerializeField] int scorePerHit = 15;
    [SerializeField] int hitPoints = 1;


    ScoreBoard scoreBoard;

    void Start()
    {
        // go look trough the entire object and the first scoreboar you find thats the one we reffering to.
        // it is resource intensive, so we dont want to use it often. We should not at all use it in Update();
        scoreBoard = FindObjectOfType<ScoreBoard>(); 

    }


    void OnParticleCollision(GameObject other)
    {

        GameObject vfx = Instantiate(deathFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parent;  
        Destroy(gameObject);
    }

}