using UnityEngine;

namespace GizmosExtensions.RuntimeGizmos.Tests
{
    public class GizmoTest : MonoBehaviour
    {
        [Header("Shape")]
        [SerializeField] private Gizmo[] shapes;

        [Header("Lines")]
        [SerializeField] private Line[] lines;
        [System.Serializable]
        private class Gizmo
        {
            public Vector3[] points;        
        }
        [System.Serializable]
        private class Line : Gizmo
        {
            [Range(0,1)]
            public float drawProgress;
            public Color color = Color.green;
        }

        [SerializeField] private int number;


        void LateUpdate()
        {
            foreach(var shape in shapes)
            {
                RuntimeGizmos.DrawShape(shape.points);
            }

            for (int i = 0; i < lines.Length; i++)
            {
                RuntimeGizmos.DrawLine(new GizmoLine(lines[i].drawProgress, lines[i].color, lines[i].points));
            }
            RuntimeGizmos.DrawLine(new GizmoLine(Debug.GetHexagonPoints(transform.position, transform.rotation, 4)));
            RuntimeGizmos.DrawLine(new GizmoLine(Debug.GetPentagonPoints(transform.position, transform.rotation, 2)));
            RuntimeGizmos.DrawLine(new GizmoLine(Debug.GetArcPoints(transform.position, transform.rotation, 1, 30, 120)));
        }

        void OnDrawGizmos()
        {
            foreach(var shape in shapes)
            {
                Debug.DrawShape(Color.black, true, shape.points);
            }
            for (int i = 0; i < lines.Length; i++)
            {
                Debug.DrawLine(Color.black, lines[i].points);
            }

            if(Application.isPlaying)
                return;

            Debug.DrawHexagon(transform.position, transform.rotation, 4, Color.red);
            Debug.DrawPentagon(transform.position, transform.rotation, 2, Color.blue);
            Debug.DrawArc(transform.position, transform.rotation, 1, 120, 32, Color.magenta);
            Debug.DrawNumber(transform.position + (Vector3.up * 3), number, Color.green);

        }
    }
}
