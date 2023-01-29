using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace AINavigation
{
    public class CharacterSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject m_CharacterPrefab;
        [SerializeField] private int m_MaxCharacterCount;
        [SerializeField] private int m_CurrentCharacterCount;
        [SerializeField] private Text m_TextCurrentCharacterCount;
        private void Start()
        {
            StartCoroutine(SpawnCharacter());
        }
        IEnumerator SpawnCharacter()
        {
            m_CurrentCharacterCount = 0;
            for (int i = 0; i < m_MaxCharacterCount; i++)
            {
                Instantiate(m_CharacterPrefab, GetRandomPointOnNavmesh(30), Quaternion.identity, transform);
                m_CurrentCharacterCount++;
                m_TextCurrentCharacterCount.text = $"Character Count: {m_CurrentCharacterCount}";
                yield return new WaitForSeconds(0.1f);
            }
        }

        public Vector3 GetRandomPointOnNavmesh(float radius)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
            {
                finalPosition = hit.position;
            }
            return finalPosition;
        }

    }
}

