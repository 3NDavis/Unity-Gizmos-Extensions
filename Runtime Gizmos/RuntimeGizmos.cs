using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace GizmosExtensions.RuntimeGizmos
{
    public class RuntimeGizmos : MonoBehaviour
    {
#region  Variables
        private GameObject emptyHolder;

        private static GameObject linesEmpty;
        public LineRenderer linePrefab;
        private static LineRenderer s_LinePrefab;
        private static GameObject shapesEmpty;
        public LineRenderer shapePrefab;
        private static LineRenderer s_ShapePrefab;
        //if you add objects here make sure to:
        //  add a corresponding enum in pooltype
        //  add it to the SetParentObject switch statement

        private static Dictionary<LineRenderer, ObjectPool<LineRenderer>> objectPools;
        private static List<LineRenderer> pooledObjects;
        private static Dictionary<LineRenderer, LineRenderer> cloneToPrefabMap;

        public enum PoolType
        {
            Lines,
            Shapes,
        }
        public static PoolType PoolingType;
#endregion

#if USE_RUNTIME_GIZMOS
#region  Initialization
        private void Awake()
        {
            objectPools = new();
            cloneToPrefabMap = new();
            pooledObjects = new();

            s_LinePrefab = linePrefab;
            s_ShapePrefab = shapePrefab;

            SetupEmpties();
        }

        private void SetupEmpties()
        {
            emptyHolder = new GameObject("Runtime Gizmos");

            linesEmpty = new GameObject("Lines");
            linesEmpty.transform.SetParent(emptyHolder.transform);

            shapesEmpty = new GameObject("Shapes");
            shapesEmpty.transform.SetParent(emptyHolder.transform);
        }
#endregion

#region  Pool
        private static void CreatePool(LineRenderer prefab, PoolType poolType = PoolType.Lines, params Vector3[] points)
        {
            ObjectPool<LineRenderer> pool = new ObjectPool<LineRenderer>(
                createFunc: () => CreateObject(prefab, poolType, points),
                actionOnGet: OnGetObject,
                actionOnRelease: OnReleaseObject,
                actionOnDestroy: OnDestroyObject
                );

            objectPools.Add(prefab, pool);
        }

        private static LineRenderer CreateObject(LineRenderer prefab, PoolType poolType = PoolType.Lines, params Vector3[] points)
        {
            //set the prefab to false so Awake, OnEnable and Start arent called unnessesarily
            prefab.gameObject.SetActive(false);

            LineRenderer instance = Instantiate(prefab);
            instance.SetPositions(points);

            prefab.gameObject.SetActive(true);

            GameObject parentObject = SetParentObject(poolType);
            instance.transform.SetParent(parentObject.transform);

            return instance;
        }

        private static GameObject SetParentObject(PoolType poolType)
        {
            switch (poolType)
            {
                case PoolType.Lines:
                    return linesEmpty;
                case PoolType.Shapes:
                    return shapesEmpty;

                default:
                    Debug.LogError("The Setparent object switch case did not have a valid return object for the given enum");
                    return null;
            }
        }

        private static void OnGetObject(LineRenderer obj)
        {
            // pooledObjects.Add(obj);
        }

        private static void OnReleaseObject(LineRenderer obj)
        {
            obj.gameObject.SetActive(false);
        }

        private static void OnDestroyObject(LineRenderer obj)
        {
            pooledObjects.Remove(obj);
            if (cloneToPrefabMap.ContainsKey(obj))
            {
                cloneToPrefabMap.Remove(obj);
            }
        }
#endregion
#endif

        public static LineRenderer DrawLine(GizmoLine gizmoLine)
        {
            var line = SpawnGizmo(s_LinePrefab, PoolType.Lines, gizmoLine.points);

            // Create a new MaterialPropertyBlock
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

            propertyBlock.SetFloat("_Progress", gizmoLine.progress);

            propertyBlock.SetColor("_Color", gizmoLine.drawnColor);
            propertyBlock.SetColor("_FutureColor", gizmoLine.futureColor);

            // Apply the MaterialPropertyBlock to the GameObject
            line.SetPropertyBlock(propertyBlock);

            return line;
        }


        public static LineRenderer DrawShape(params Vector3[] points)
        {
            return SpawnGizmo(s_ShapePrefab, PoolType.Shapes, points);
        }
        private static LineRenderer SpawnGizmo(LineRenderer prefab, PoolType poolType, Vector3 center, params Vector3[] points)
        {
#if USE_RUNTIME_GIZMOS
            if (!objectPools.ContainsKey(prefab))
            {
                CreatePool(prefab, poolType, points);
            }

            LineRenderer instance = objectPools[prefab].Get();

            if(instance != null)
            {
                if (!cloneToPrefabMap.ContainsKey(instance))
                {
                    cloneToPrefabMap.Add(instance, prefab);
                }

                instance.transform.position = center;
                instance.useWorldSpace = false;

                instance.positionCount = points.Length;
                instance.SetPositions(points);

                pooledObjects.Add(instance);

                instance.gameObject.SetActive(true);

                return instance;
            }
#elif UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"A runtime gizmos was attempted to be drawn, add USE_RUNTIME_GIZMOS to the player setting's scripting defines to start using them");
#endif
            return null;
        }

        private static LineRenderer SpawnGizmo(LineRenderer prefab, PoolType poolType, params Vector3[] points)
        {
           return SpawnGizmo(prefab, poolType, Vector3.zero, points); 
        }

        public static void ClearGizmos()
        {
#if USE_RUNTIME_GIZMOS
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                ReturnLineToPool(pooledObjects[i]);
            }
            pooledObjects.Clear();
#endif
        }

        private static void ReturnLineToPool(LineRenderer instance)
        {
#if USE_RUNTIME_GIZMOS
            if(cloneToPrefabMap.TryGetValue(instance, out LineRenderer prefab))
            {
                if(objectPools.TryGetValue(prefab, out ObjectPool<LineRenderer> pool))
                {
                    instance.useWorldSpace = true;
                    pool.Release(instance);
                }
            }
            else
            {
    #if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("Trying to return an object that is not pooled: " + instance.name);
    #endif
            }
#endif
        }
    }

}   
