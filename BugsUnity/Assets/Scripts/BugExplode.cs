using UnityEngine;
using System.Collections;

public class BugExplode : MonoBehaviour
{

    public GameObject[] BugPieces;

    public float m_fBitLifespan;

    public Vector3 Min_Vel;
    public Vector3 Max_Vel;

    public Vector3 Min_RVel;
    public Vector3 Max_RVel;

    private Vector3 InitialVelocity;
    private Vector3 InitialRotation;

    private int m_iOnCountIndex;

    private int m_iLength;

    private Transform m_Owner;

    private Vector3 SpawnSpot;



    // Use this for initialization
    void Start()
    {

        m_Owner = gameObject.transform;
        SpawnSpot = m_Owner.position;

        m_iLength = BugPieces.Length;

        if (m_iOnCountIndex > 50)
        {
            m_iOnCountIndex = 1;
        }
        else
        {
            ++m_iOnCountIndex;
        }

        for (int i = 0; i < m_iLength; ++i)
        {
            GameObject BitSpawn = (GameObject)Instantiate(BugPieces[i], SpawnSpot, transform.rotation);
            BitSpawn.transform.parent = m_Owner.transform;
            BitSpawn.name += m_iOnCountIndex;

            InitialVelocity = new Vector3(Random.Range(Min_Vel.x, Max_Vel.x), Random.Range(Min_Vel.y, Max_Vel.y), Random.Range(Min_Vel.z, Max_Vel.z));
            InitialRotation = new Vector3(Random.Range(Min_RVel.x, Max_RVel.x), Random.Range(Min_RVel.y, Max_RVel.y), Random.Range(Min_RVel.z, Max_RVel.z));

            BitSpawn.GetComponent<Rigidbody>().velocity = InitialVelocity;
            BitSpawn.GetComponent<Rigidbody>().angularVelocity = InitialRotation;
            //BitSpawn.GetComponent<SelfDestruct>().m_fMaxTimer = m_fBitLifespan;
        }
    }
}
