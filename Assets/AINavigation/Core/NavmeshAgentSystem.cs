using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace AINavigation
{
    public class NavmeshAgentSystem : IPathfindingSystem
    {
        private MonoBehaviour m_Owner;
        private NavMeshAgent m_NavMeshAgent;

        private Vector3 m_Destination;
        private NavMeshPath m_NavMeshPath;
        private NavMeshHit m_NavMeshHit;
        private Coroutine m_Coroutine;
        private bool m_IsCalculatePath;
        public NavmeshAgentSystem(MonoBehaviour owner, NavMeshAgent navMeshAgent)
        {
            m_Owner = owner;
            m_NavMeshAgent = navMeshAgent;
        }
        public void CalculatePath(Vector3 destination)
        {
            m_NavMeshPath = new NavMeshPath();
            m_NavMeshAgent.CalculatePath(destination, m_NavMeshPath);
        }
        public Vector3 GetClosestPoint(Vector3 position, float radius)
        {
            NavMesh.SamplePosition(position, out m_NavMeshHit, radius, m_NavMeshAgent.areaMask);
            return m_NavMeshHit.position;
        }
        public Vector3 GetDestination()
        {
            return m_Destination;
        }

        public bool IsFinishMove()
        {
            if (!m_IsCalculatePath)
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

        public void MoveFollowPath()
        {
            m_NavMeshAgent.SetPath(m_NavMeshPath);
        }
        public void SetDestination(Vector3 destination)
        {
            if (m_Coroutine != null)
            {
                m_Owner.StopCoroutine(m_Coroutine);
            }
            m_Coroutine = m_Owner.StartCoroutine(CoSetDestination(destination));
        }
        /// <summary>
        /// Coroutine controls the path computation and can prevents multiple path computations from overlapping
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        IEnumerator CoSetDestination(Vector3 destination)
        {
            m_Destination = GetClosestPoint(destination, 10);

            m_IsCalculatePath = true;
            CalculatePath(m_Destination);
            yield return new WaitUntil(() => m_NavMeshPath.status == NavMeshPathStatus.PathComplete && m_NavMeshPath.corners.Length != 0);
            m_IsCalculatePath = false;

            MoveFollowPath();
        }
    }
}

