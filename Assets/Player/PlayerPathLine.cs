using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerPathLine : MonoBehaviour
{

    LineRenderer lineRenderer;
    NavMeshAgent navAgent;
    public float vertexCount = 5f;
    
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void MakePathLine()
    {
        FindPathToDestination();
    }

    public void ClearPathLine()
    {
        lineRenderer.positionCount = 0;
    }

    void FindPathToDestination()
    {
        if (navAgent.destination == null)
            return;

        //NavMeshPath navPath = new NavMeshPath();
        //NavMesh.CalculatePath(transform.position, navAgent.destination, NavMesh.AllAreas, navPath);
        //CreatePathLine(navPath.corners);
        Vector3[] corners = navAgent.path.corners;
        CreatePathLine(corners);
    }

    void CreatePathLine(Vector3[] corners)
    {
        #region Curving Path
        //public int intermediateVertexCount = 12;
        //List<Vector3> smooth_corners = new List<Vector3>();
        //for (int i = 0; i < corners.Length - 2; i++)
        //{
        //    for (int j = 0; j <= 1; j += 1 / intermediateVertexCount)
        //    {
        //        Vector3 tangent_line_vertex_a = Vector3.Lerp(corners[i], corners[i + 1], j);
        //        Vector3 tangent_line_vertex_b = Vector3.Lerp(corners[i + 1], corners[i + 2], j);
        //        Vector3 curved_point = Vector3.Lerp(tangent_line_vertex_a, tangent_line_vertex_b, j);
        //        smooth_corners.Add(curved_point);
        //    }

        //}

        //line.positionCount = smooth_corners.Count;
        //line.SetPositions(smooth_corners.ToArray());
        #endregion

        lineRenderer.positionCount = corners.Length;
        for(int i=0; i< corners.Length;i++)
            lineRenderer.SetPosition(i, corners[i]);
        //Can use this after making path to stop updating each frame

        //Smoothy();
    }


    void Smoothy()
    {
        //print("Points" + lineRenderer.positionCount);
        for (int i = 0; i < lineRenderer.positionCount - 2; i++)
        {
            var dir1 = Vector3.Normalize(lineRenderer.GetPosition(i + 1) - lineRenderer.GetPosition(i));
            var dir2 = Vector3.Normalize(lineRenderer.GetPosition(i + 2) - lineRenderer.GetPosition(i + 1));
            if (Vector3.Angle(dir1, dir2) > 10)
            {
            //print("Angles: " + Vector3.Angle(dir1, dir2));
                var lis = SmoothLine(lineRenderer.GetPosition(i), lineRenderer.GetPosition(i + 1), lineRenderer.GetPosition(i + 2));
                var f = (int)vertexCount - 2;
                lineRenderer.positionCount += f;
                for (int j = lineRenderer.positionCount-1; j >= i + f; j--)
                {
                    //print("J: " + j);
                    lineRenderer.SetPosition(j, lineRenderer.GetPosition(j - f));
                }
                for (int j = i, k = 0; j < i + vertexCount; j++, k++)
                {
                    print("Index:J " + j);
                    print("Index K: " + k);
                    lineRenderer.SetPosition(j, lis[k]);
                }
                i += (int)vertexCount - 2;
            }
        }

    }


    List<Vector3> SmoothLine(Vector3 a, Vector3 b, Vector3 c)
    {
        List<Vector3> smoothedPoints = new List<Vector3>();
        for (float ratio = 0; ratio <= 1; ratio += 1f / vertexCount)
        {
            var tangent1 = Vector3.Lerp(a, b, ratio);
            var tangent2 = Vector3.Lerp(b, c, ratio);
            var bezierPoint = Vector3.Lerp(tangent1, tangent2, ratio);
            smoothedPoints.Add(bezierPoint);
        }
        return smoothedPoints;
    }
}