using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace AINavigation
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavmeshAgentCharacter : MonoBehaviour
    {
        private IPathfindingSystem m_PathfindingSystem;
        [SerializeField] private NavMeshAgent m_NavMeshAgent;
        [SerializeField] private MeshRenderer m_Renderer;
        [SerializeField] private List<Color> m_ColorList;
        [SerializeField] private List<Vector3> m_PointList;

        private void Awake()
        {
            m_PathfindingSystem = new NavmeshAgentSystem(this, m_NavMeshAgent);
        }

        void Update()
        {
            if (m_PathfindingSystem.IsFinishMove())
            {
                m_PointList.Clear();
                // SetDestination 4 times to test multiple handling
                for (int i = 0; i < 4; i++)
                {
                    m_PointList.Add(GetRandomPoint());
                    m_PathfindingSystem.SetDestination(m_PointList[i]);
                }
            }

            UpdateColorByDestination();
        }

        /// <summary>
        /// Return the random point on ground
        /// </summary>
        /// <returns></returns>
        public Vector3 GetRandomPoint()
        {
            return new Vector3(Random.Range(-45.0f, 45.0f), 0, Random.Range(-45.0f, 45.0f));
        }
        /// <summary>
        /// Change color of the character by the destination the character will go to
        /// </summary>
        public void UpdateColorByDestination()
        {
            int index = 0;
            for (int i = 1; i < m_PointList.Count; i++)
            {
                if (Vector3.Distance(m_PointList[i], m_PathfindingSystem.GetDestination()) < Vector3.Distance(m_PointList[index], m_PathfindingSystem.GetDestination()))
                {
                    index = i;
                }
            }
            m_Renderer.material.color = m_ColorList[index];
        }
    }
}

