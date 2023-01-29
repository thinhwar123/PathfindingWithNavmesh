using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AINavigation
{
    /// <summary>
    /// The interface defines a set of methods for pathfinding in a 3D environment. 
    /// </summary>
    public interface IPathfindingSystem 
    {
        /// <summary>
        /// Sets the destination for the pathfinding algorithm
        /// </summary>
        /// <param name="destination"></param>
        public void SetDestination(Vector3 destination);
        /// <summary>
        /// Retrieves the current destination
        /// </summary>
        /// <returns></returns>
        public Vector3 GetDestination();
        /// <summary>
        /// Returns the closest point can move within a given radius to a given position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public Vector3 GetClosestPoint(Vector3 position, float radius);
        /// <summary>
        /// Calculates the path from the current position to the destination
        /// </summary>
        /// <param name="destination"></param>
        public void CalculatePath(Vector3 destination);
        /// <summary>
        /// Moves the object along the calculated path
        /// </summary>
        public void MoveFollowPath();
        /// <summary>
        /// Defines whether object is finish move
        /// </summary>
        /// <returns></returns>
        public bool IsFinishMove();
    }
}

