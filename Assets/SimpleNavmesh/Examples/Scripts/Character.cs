using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace SimpleNavmesh
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Character : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent m_NavMeshAgent;

        void Update()
        {
            if (IsFinishMoveOnNavemesh())
            {
                m_NavMeshAgent.SetDestination(GetRandomPoint());
            }
        }

        public bool IsFinishMoveOnNavemesh()
        {
            if (!m_NavMeshAgent.pathPending)
            {
                if (m_NavMeshAgent.remainingDistance <= m_NavMeshAgent.stoppingDistance)
                {
                    if (!m_NavMeshAgent.hasPath || m_NavMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Vector3 GetRandomPoint()
        {
            return new Vector3(Random.Range(-45.0f, 45.0f), 0, Random.Range(-45.0f, 45.0f));
        }
    }
}

