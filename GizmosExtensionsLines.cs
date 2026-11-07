using UnityEngine;

namespace GizmosExtensions
{
    public partial class Debug : UnityEngine.Debug
    {
        public static void DrawLine(Color color, params Vector3[] points)
        {
            for (int i = 0; i < points.Length - 1; i++)
            {
                DrawLine(points[i], points[i+1], color);
            }
        }

        public static void DrawArrow(Vector3 startPosition, Vector3 endPosition, Color color)
        {
            DrawLine(startPosition, endPosition, color);

            var tenPercent = (startPosition - endPosition) * 0.1f;

            DrawLine(endPosition, endPosition + Quaternion.Euler(0, 0, 30) * tenPercent, color);
            DrawLine(endPosition, endPosition + Quaternion.Euler(0, 0, -30) * tenPercent, color);
        }

        public static void DrawArrow(Vector3 startPosition, Quaternion rotation, Color color)
        {
            DrawArrow(startPosition, startPosition + (rotation * Vector2.up), color);
        }

        public static void DrawLine(Vector3 startPosition, Quaternion rotation, Color color)
        {
            DrawLine(startPosition, startPosition + (rotation * Vector2.up), color);
        }

    }
}