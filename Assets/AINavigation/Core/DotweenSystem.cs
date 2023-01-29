using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

namespace AINavigation
{
    public class DotweenSystem : IPathfindingSystem
    {
        private MonoBehaviour m_Owner;
        private Transform m_OwnerTransform;
        private int m_AreaMask;
        private float m_RotateSpeed;
        private float m_MovementSpeed;
        private float m_StopDistance;

        private Vector3 m_Destination;
        private NavMeshPath m_NavMeshPath;
        private Vector3[] m_WayPoints;
        private NavMeshHit m_NavMeshHit;
        private Coroutine m_Coroutine;
        private bool m_IsCalculatePath;

        private Tween m_MoveTween;
        private Tween m_RotateTween;

        public DotweenSystem(MonoBehaviour owner, int areaMask, float movementSpeed, float rotateSpeed, float stopDistance)
        {
            m_Owner = owner;
            m_OwnerTransform = owner.transform;
            m_AreaMask = areaMask;
            m_MovementSpeed = movementSpeed;
            m_RotateSpeed = rotateSpeed;
            m_StopDistance = stopDistance;

            m_Destination = m_OwnerTransform.position;
        }

        public void CalculatePath(Vector3 destination)
        {
            m_NavMeshPath = new NavMeshPath();
            NavMesh.CalculatePath(m_OwnerTransform.position, destination, m_AreaMask, m_NavMeshPath);
        }

        public Vector3 GetClosestPoint(Vector3 position, float radius)
        {
            NavMesh.SamplePosition(position, out m_NavMeshHit, radius, m_AreaMask);
            return m_NavMeshHit.position;
        }

        public Vector3 GetDestination()
        {
            return m_Destination;
        }

        public bool IsFinishMove()
        {
            return !m_IsCalculatePath && Vector3.Distance(m_OwnerTransform.position, m_Destination) < m_StopDistance;
        }

        public void MoveFollowPath()
        {
            MoveByDotween();
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
            m_WayPoints = m_NavMeshPath.corners;


            m_IsCalculatePath = false;

            MoveFollowPath();
        }
        /// <summary>
        /// Move object by dotween dopath function
        /// </summary>
        private void MoveByDotween()
        {
            m_MoveTween?.Kill();
            float duration = GetPathLength(m_WayPoints) / m_MovementSpeed;
            m_MoveTween = m_OwnerTransform.DOPath(m_WayPoints, duration).SetEase(Ease.Linear).OnWaypointChange(RotateByDotween);
        }
        /// <summary>
        /// Rotate the object when the waypoint change
        /// </summary>
        /// <param name="wayPointIndex"></param>
        private void RotateByDotween(int wayPointIndex)
        {
            m_RotateTween?.Kill();
            m_RotateTween = m_OwnerTransform.DORotateQuaternion(Quaternion.LookRotation(m_WayPoints[wayPointIndex + 1] - m_OwnerTransform.position), 1 / m_RotateSpeed);
        }
        /// <summary>
        /// Return the length of the path
        /// </summary>
        /// <param name="waypoints"></param>
        /// <returns></returns>
        private float GetPathLength(Vector3[] waypoints)
        {
            float length = 0;
            for (int i = 0; i < waypoints.Length - 1; i++)
            {
                length += Vector3.Distance(waypoints[i], waypoints[i + 1]);
            }
            return length;
        }
    }
}


