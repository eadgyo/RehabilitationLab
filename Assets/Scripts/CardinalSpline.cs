using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class CardinalSpline : MonoBehaviour
{
    [SerializeField]
    private int m_segments;

    [SerializeField]
    private float m_tension;

    [SerializeField]
    private float m_bias;

    [SerializeField]
    private List<Vector3> m_points;

    private List<Vector3> m_interpolated_points;

    [SerializeField]
    private List<float> m_delays;

    [SerializeField]
    private LineRenderer m_lineRenderer;

    private float m_last_time;
    private float m_check_point;

    private int m_index = 0;
    private int m_segment_index = 0;

    void Start()
    {
        m_last_time = Time.time;
        m_check_point = Time.time;

        m_interpolated_points = new List<Vector3>();
    }

    void Update()
    {
        Render();
    }

    public void DrawCurve(int start, int end)
    {
        for (int i = start; i < end; ++i)
        {
            Vector3 prev;
            Vector3 next;

            if (i == 0)
            {
                prev = m_points[0];
            }
            else
            {
                prev = m_points[i - 1];
            }

            if (i == m_points.Count - 2)
            {
                next = m_points[m_points.Count - 1];
            }
            else
            {
                next = m_points[i + 2];
            }

            Vector3 t1 = (m_points[i] - prev) * (1 + m_bias) * (1 - m_tension) / 2 + (m_points[i + 1] - m_points[i]) * (1 - m_bias) * (1 - m_tension) / 2;
            Vector3 t2 = (m_points[i + 1] - m_points[i]) * (1 + m_bias) * (1 - m_tension) / 2 + (next - m_points[i + 1]) * (1 - m_bias) * (1 - m_tension) / 2;

            for (int j = 0; j < m_segments; ++j)
            {
                float s = j / (float) m_segments;

                float c1 = 2 * Mathf.Pow(s, 3) - 3 * Mathf.Pow(s, 2) + 1;
                float c2 = -2 * Mathf.Pow(s, 3) + 3 * Mathf.Pow(s, 2);
                float c3 = Mathf.Pow(s, 3) - 2 * Mathf.Pow(s, 2) + s;
                float c4 = Mathf.Pow(s, 3) - Mathf.Pow(s, 2);

                Vector3 v = new Vector3();

                v.x = c1 * m_points[i].x + c2 * m_points[i + 1].x + c3 * t1.x + c4 * t2.x;
                v.y = c1 * m_points[i].y + c2 * m_points[i + 1].y + c3 * t1.y + c4 * t2.y;
                v.z = c1 * m_points[i].z + c2 * m_points[i + 1].z + c3 * t1.z + c4 * t2.z;

                m_interpolated_points.Add(v);

                //m_lineRenderer.numPositions = (i * m_segments) + j + 1;
                //m_lineRenderer.SetPosition((i * m_segments) + j, v);
            }
        }

        //m_interpolated_points.Add(m_points[m_points.Count - 1]);
    }

    public void DrawCurve(int start)
    {
        DrawCurve(start, m_points.Count - 1);
    }

    public void DrawCurve()
    {
        DrawCurve(0);
    }

    public void Render()
    {
        if (m_index < m_points.Count)
        {
            m_last_time += Time.deltaTime;

            int section = m_index * m_segments + (int) ((m_last_time - m_check_point) / m_delays[m_index] * m_segments);

            for (int i = m_segment_index; i < section; ++i)
            {
                if (section <= m_interpolated_points.Count)
                {
                    m_lineRenderer.numPositions = section;
                    m_lineRenderer.SetPosition(i, m_interpolated_points[i]);
                    m_segment_index = section;
                }
            }

            if (m_last_time - m_check_point > m_delays[m_index])
            {
                m_check_point += m_delays[m_index];
                m_index++;
            }
        }
    }

    public void AddPoint(Vector3 point, float delay)
    {
        m_points.Add(point);
        m_delays.Add(delay);

        DrawCurve(m_points.Count - 2, m_points.Count - 1);
    }

    public void AddPoint(GameObject obj, float delay)
    {
        AddPoint(obj.transform.position, delay);
    }

    public void SetPoint(int index, Vector3 point)
    {
        m_points[index] = point;

        ReDraw();
    }

    public void SetPoint(int index, GameObject obj)
    {
        SetPoint(index, obj.transform.position);
    }

    public void ReDraw()
    {
        m_interpolated_points.Clear();

        DrawCurve();

    }

    public void Clear()
    {
        m_points.Clear();
        m_interpolated_points.Clear();
        m_lineRenderer.numPositions = 0;

        m_index = 0;
        m_segment_index = 0;

        m_last_time = Time.time;
        m_check_point = Time.time;
    }
}
