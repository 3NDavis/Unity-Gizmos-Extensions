using UnityEngine;

namespace GizmosExtensions.RuntimeGizmos
{
    public readonly struct GizmoLine
    {
        public readonly Vector3[] points;
        public readonly float progress;
        public readonly Color drawnColor;
        public readonly Color futureColor;

        public GizmoLine(params Vector3[] points)
        {
            progress = 1;
            this.points = points;
        }

        public GizmoLine(float progress, params Vector3[] points)
        {
            this.progress = Mathf.Clamp01(progress);
            this.points = points;
        }

        public GizmoLine(Color color, params Vector3[] points)
        {
            this.progress = 1;
            this.points = points;
            drawnColor = color;
        }

        public GizmoLine(Color color, Color futureColor, params Vector3[] points)
        {
            this.progress = 1;
            this.points = points;
            drawnColor = color;
            this.futureColor = futureColor;
        }

        public GizmoLine(float progress, Color color, params Vector3[] points)
        {
            this.progress = Mathf.Clamp01(progress);
            this.points = points;
            drawnColor = color;
        }

        public GizmoLine(float progress, Color color, Color futureColor, params Vector3[] points)
        {
            this.progress = Mathf.Clamp01(progress);
            this.points = points;
            drawnColor = color;
            this.futureColor = futureColor;
        }
    }
}