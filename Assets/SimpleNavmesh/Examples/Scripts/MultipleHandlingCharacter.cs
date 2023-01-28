using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace SimpleNavmesh
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MultipleHandlingCharacter : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent m_NavMeshAgent;
        [SerializeField] private MeshRenderer m_Renderer;
        [SerializeField] private List<Color> m_ColorList;
        [SerializeField] private List<Vector3> m_PointList;

        void Update()
        {
            if (IsFinishMoveOnNavemesh())
            {
                m_PointList.Clear();
                for (int i = 0; i < 4; i++)
                {
                    m_PointList.Add(GetRandomPoint());
                    m_NavMeshAgent.SetDestination(m_PointList[i]);
                }
            }

            UpdateColorByDestination();
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
        public void UpdateColorByDestination()
        {
            int index = 0;
            for (int i = 1; i < m_PointList.Count; i++)
            {
                if (Vector3.Distance(m_PointList[i], m_NavMeshAgent.destination) < Vector3.Distance(m_PointList[index], m_NavMeshAgent.destination))
                {
                    index = i;
                }
            }
            m_Renderer.material.color = m_ColorList[index];
        }
    }
}

