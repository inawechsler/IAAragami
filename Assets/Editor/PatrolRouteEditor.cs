#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(PatrolRoute))]
public class PatrolRouteEditor : Editor
{
    private PatrolRoute patrolRoute;

    private void OnEnable()
    {
        patrolRoute = (PatrolRoute)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Herramientas de Edición", EditorStyles.boldLabel);

        // Campo para agregar un punto manualmente
        PatrolPoint pointToAdd = (PatrolPoint)EditorGUILayout.ObjectField("Agregar Punto", null, typeof(PatrolPoint), true);
        if (pointToAdd != null)
        {
            patrolRoute.AddPoint(pointToAdd);
            EditorUtility.SetDirty(patrolRoute);
        }

        // Botón para encontrar puntos en la escena
        if (GUILayout.Button("Buscar todos los PatrolPoints en la escena"))
        {
            PatrolPoint[] allPoints = FindObjectsOfType<PatrolPoint>();
            foreach (var point in allPoints)
            {
                patrolRoute.AddPoint(point);
            }
            EditorUtility.SetDirty(patrolRoute);
        }

        if (GUILayout.Button("Limpiar puntos"))
        {
            patrolRoute.ClearPoints();
            EditorUtility.SetDirty(patrolRoute);
        }
    }

    private void OnSceneGUI()
    {
        // Dibujar las líneas en la escena
        for (int i = 0; i < patrolRoute.PointCount; i++)
        {
            PatrolPoint point = patrolRoute.GetPointAt(i);
            if (point == null) continue;

            // Dibujar líneas entre puntos
            if (i < patrolRoute.PointCount - 1)
            {
                PatrolPoint nextPoint = patrolRoute.GetPointAt(i + 1);
                if (nextPoint != null)
                {
                    Handles.color = Color.green;
                    Handles.DrawLine(point.transform.position, nextPoint.transform.position);

                    // Opcional: dibujar una flecha o indicador de dirección
                    Vector3 direction = nextPoint.transform.position - point.transform.position;
                    Vector3 midPoint = point.transform.position + direction * 0.5f;
                    float arrowSize = 0.5f;
                    Handles.ConeHandleCap(0, midPoint, Quaternion.LookRotation(direction), arrowSize, EventType.Repaint);
                }
            }
            else if (patrolRoute.IsLooping && patrolRoute.PointCount > 1)
            {
                PatrolPoint firstPoint = patrolRoute.GetPointAt(0);
                if (firstPoint != null)
                {
                    Handles.color = Color.yellow; // Color diferente para el ciclo
                    Handles.DrawLine(point.transform.position, firstPoint.transform.position);

                    Vector3 direction = firstPoint.transform.position - point.transform.position;
                    Vector3 midPoint = point.transform.position + direction * 0.5f;
                    float arrowSize = 0.5f;
                    Handles.ConeHandleCap(0, midPoint, Quaternion.LookRotation(direction), arrowSize, EventType.Repaint);
                }
            }

            // Dibujar etiqueta con el índice del punto
            Handles.Label(point.transform.position + Vector3.up * 0.5f, $"Punto {i}: {point.name}");
        }
    }
}
#endif