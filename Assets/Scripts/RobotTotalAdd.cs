using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotTotalAdd : MonoBehaviour
{
    public int TotalRobotsFixed = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        EnemyController e = other.collider.GetComponent<EnemyController>();
        HardRobots f = other.collider.GetComponent<HardRobots>();

        if (e != null)
        {
            e.Fix();

        }
        if (f != null)
        {
            f.Fix();
        }

        Destroy(gameObject);
    }

}
