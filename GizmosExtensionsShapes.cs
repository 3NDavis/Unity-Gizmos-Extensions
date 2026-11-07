using System;
using UnityEngine;

namespace GizmosExtensions
{
    // Shapes
    public partial class Debug : UnityEngine.Debug
    {
        public static Vector3[] GetRectanglePoints(Vector3 position, Quaternion rotation, Vector4 sideLengths)
        {
            Vector3[] points = new Vector3[4];

            sideLengths = new Vector4(Mathf.Abs(sideLengths.x), Mathf.Abs(sideLengths.y), Mathf.Abs(sideLengths.z), Mathf.Abs(sideLengths.w));

            points[0] = position + (rotation * new Vector2(-sideLengths.x / 2, +sideLengths.z / 2));
            points[1] = position + (rotation * new Vector2(+sideLengths.x / 2, +sideLengths.w / 2));
            points[2] = position + (rotation * new Vector2(-sideLengths.y / 2, -sideLengths.z / 2));
            points[3] = position + (rotation * new Vector2(+sideLengths.y / 2, -sideLengths.w / 2));

            return points;
        }

        public static Vector3[] GetRectanglePoints(Vector3 position, Quaternion rotation, float top, float bottom, float left, float right)
        {
            return GetRectanglePoints(position, rotation, new Vector4(top, bottom, left, right));
        }

        public static Vector3[] GetCirclePoints(Vector3 position, Quaternion rotation, float radius, int segments = 32)
        {
            return GetArcPoints(position, rotation, radius, segments, 360);
        }

        public static Vector3[] GetHexagonPoints(Vector3 position, Quaternion rotation, float radius)
        {
            return GetArcPoints(position, rotation, radius, 6, 360);
        }
        public static Vector3[] GetPentagonPoints(Vector3 position, Quaternion rotation, float radius)
        {
            return GetArcPoints(position, rotation, radius, 5, 360);
        }

        public static Vector3[] GetArcPoints(Vector3 position, Quaternion rotation, float radius, int segments, float arcLenghtDegrees = 360)
        {
            // If either radius or number of segments are less or equal to 0, skip drawing
            if (radius <= 0.0f || segments <= 0)
            {
                return null;
            }

            // Single segment of the circle covers (360 / number of segments) degrees
            float angleStep = (arcLenghtDegrees / segments);

            // Result is multiplied by Mathf.Deg2Rad constant which transforms degrees to radians
            // which are required by Unity's Mathf class trigonometry methods

            angleStep *= Mathf.Deg2Rad;

            // lineStart and lineEnd variables are declared outside of the following for loop
            Vector2 lineStart = Vector3.zero;
            Vector2 lineEnd = Vector3.zero;

            Vector3[] points = new Vector3[segments + 1];

            for (int i = 0; i < segments + 1; i++)
            {
                // Line start is defined as starting angle of the current segment (i)
                lineStart.x = Mathf.Cos(angleStep * i);
                lineStart.y = Mathf.Sin(angleStep * i);

                // Line end is defined by the angle of the next segment (i+1)
                lineEnd.x = Mathf.Cos(angleStep * (i + 1));
                lineEnd.y = Mathf.Sin(angleStep * (i + 1));

                // Results are multiplied so they match the desired radius
                lineStart *= radius;
                lineEnd *= radius;

                points[i] = rotation * lineStart + position;
                
            }

            return points;
        }

        public static void DrawShape(Color color, bool close, Vector3 center, params Vector3[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if(i == points.Length - 1 & !close)
                    break;

                DrawLine(center + points[i], center + points[(i + 1) % points.Length], color);
            }
        }

        public static void DrawShape(Color color, bool close, params Vector3[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if(i == points.Length - 1 & !close)
                    break;

                DrawLine(points[i], points[(i + 1) % points.Length], color);
            }
        }

        public static void DrawRectangle(Vector3 position, Quaternion rotation, float top, float bottom, float left, float right, Color color)
        {
            DrawShape(color, true, GetRectanglePoints(position, rotation, top, bottom, left, right));
        }
        public static void DrawRectangle(Vector3 position, Quaternion rotation, Vector4 sideLengths, Color color)
        {
            DrawShape(color, true, GetRectanglePoints(position, rotation, sideLengths));
        }
        public static void DrawArc(Vector3 position, Quaternion rotation, float radius, int segments, Color color, float arcLenghtDegrees)
        {
            DrawShape(color, false, GetArcPoints(position, rotation, radius, segments, arcLenghtDegrees));
        }
        public static void DrawCircle(Vector3 position, Quaternion rotation, float radius, Color color, int segments = 32)
        {
            DrawShape(color, false, GetArcPoints(position, rotation, radius, segments, 360));
        }
        public static void DrawHexagon(Vector3 position, Quaternion rotation, float radius, Color color)
        {
            DrawCircle(position, rotation, radius, color, 6);
        }

        public static void DrawPentagon(Vector3 position, Quaternion rotation, float radius, Color color)
        {
            DrawCircle(position, rotation, radius, color, 5);
        }
    }
}