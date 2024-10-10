using UnityEngine;
using System.Collections.Generic;

public class WaterCube : MonoBehaviour
{
	[Header("Mesh")]
	[SerializeField, Range(2, 100)]
	int m_Row;

	[SerializeField, Range(2, 100)]
	int m_Column;

	Mesh m_Mesh;
	MeshContructor m_MeshConstructor = new MeshContructor();

	void Start()
	{
		CreateMesh(GetComponent<MeshFilter>());
	}

	MeshFilter CreateMesh(MeshFilter mf)
	{
		AddBase();
		AddAround();
		AddUp();

		m_Mesh = new Mesh();
		m_Mesh.vertices = m_MeshConstructor.GetVertices();
		m_Mesh.triangles = m_MeshConstructor.GetTriangles();
		m_Mesh.RecalculateBounds();
		m_Mesh.RecalculateNormals();
		mf.sharedMesh = m_Mesh; 
		return mf;
	}

    private void AddBase()
	{
		m_MeshConstructor.AddSimpleFace(new Vector3(180, 0, 0), new Vector3(0.0f, -0.5f, 0.0f));
		m_MeshConstructor.AddSimpleFace(new Vector3(90, 0, 0), new Vector3(0.0f, -0.25f, 0.5f), new Vector3(1, 0.5f, 1));
		m_MeshConstructor.AddSimpleFace(new Vector3(-90, 0, 0), new Vector3(0.0f, -0.25f, -0.5f), new Vector3(1, 0.5f, 1));
		m_MeshConstructor.AddSimpleFace(new Vector3(0, 0, 90), new Vector3(-0.5f, -0.25f, 0.0f), new Vector3(1, 0.5f, 1));
		m_MeshConstructor.AddSimpleFace(new Vector3(0, 0, -90), new Vector3(0.5f, -0.25f, 0.0f), new Vector3(1, 0.5f, 1));
	}

	private void AddAround()
	{
		for (int j = 0; j < m_Column - 1; j++)
		{
			float offset = ((float)j + 1 - (m_Column) / 2f) / (m_Column - 1);
			m_MeshConstructor.AddSimpleFace(new Vector3(90, 0, 0),
				new Vector3(offset, 0.25f, 0.5f),
				new Vector3((float)1 / (m_Column - 1), 0.5f, 1));
		}
		for (int j = 0; j < m_Column - 1; j++)
		{
			float offset = ((float)j + 1 - (m_Column) / 2f) / (m_Column - 1);
			m_MeshConstructor.AddSimpleFace(new Vector3(-90, 0, 0),
				new Vector3(offset, 0.25f, -0.5f),
				new Vector3((float)1 / (m_Column - 1), 0.5f, 1));
		}
		for (int i = 0; i < m_Row - 1; i++)
		{
			float offset = ((float)i + 1 - (m_Row) / 2f) / (m_Row - 1);
			m_MeshConstructor.AddSimpleFace(new Vector3(0, 0, 90),
				new Vector3(-0.5f, 0.25f, offset),
				new Vector3(1, 0.5f, (float)1 / (m_Row - 1)));
		}
		for (int i = 0; i < m_Row - 1; i++)
		{
			float offset = ((float)i + 1 - (m_Row) / 2f) / (m_Row - 1);
			m_MeshConstructor.AddSimpleFace(new Vector3(0, 0, -90),
				new Vector3(0.5f, 0.25f, offset),
				new Vector3(1, 0.5f, (float)1 / (m_Row - 1)));
		}
	}

	private void AddUp()
	{
		for (int i = 0; i < m_Row - 1; i++)
		{
			for (int j = 0; j < m_Column - 1; j++)
			{
				float offsetX = ((float)j + 1 - (m_Column) / 2f) / (m_Column - 1);
				float offsetZ = ((float)i + 1 - (m_Row) / 2f) / (m_Row - 1);
				m_MeshConstructor.AddSimpleFace(new Vector3(0, 0, 0),
					new Vector3(offsetX, 0.5f, offsetZ),
					new Vector3((float)1 / (m_Column - 1), 1, (float)1 / (m_Row - 1)));
			}
		}
	}
}


public class MeshContructor
{
	public List<Vector3> Vertices;
	public List<int> Triangles;

	public MeshContructor()
	{
		Vertices = new List<Vector3>();
		Triangles = new List<int>();
	}

	public void AddSimpleFace()
	{
		AddSimpleFace(Vector3.zero, Vector3.zero, Vector3.one);
	}
	public void AddSimpleFace(Vector3 _rotation)
	{
		AddSimpleFace(_rotation, Vector3.zero, Vector3.one);
	}
	public void AddSimpleFace(Vector3 _rotation, Vector3 _position)
    {
		AddSimpleFace(_rotation, _position, Vector3.one);
	}
	public void AddSimpleFace(Vector3 _rotation, Vector3 _position, Vector3 _scale)
    {
		int verticeCount = Vertices.Count;
		Quaternion rotation = Quaternion.Euler(_rotation);
		Vertices.Add(Vector3.Scale(rotation * new Vector3( 0.5f, 0,  0.5f), _scale) + _position);
		Vertices.Add(Vector3.Scale(rotation * new Vector3( 0.5f, 0, -0.5f), _scale) + _position);
		Vertices.Add(Vector3.Scale(rotation * new Vector3(-0.5f, 0, -0.5f), _scale) + _position);

		Triangles.Add(verticeCount + 0);
		Triangles.Add(verticeCount + 1);
		Triangles.Add(verticeCount + 2);


		Vertices.Add(Vector3.Scale(rotation * new Vector3(0.5f, 0, 0.5f), _scale) + _position);
		Vertices.Add(Vector3.Scale(rotation * new Vector3(-0.5f, 0, -0.5f), _scale) + _position);
		Vertices.Add(Vector3.Scale(rotation * new Vector3(-0.5f, 0, 0.5f), _scale) + _position);

		Triangles.Add(verticeCount + 3);
		Triangles.Add(verticeCount + 4);
		Triangles.Add(verticeCount + 5);
	}

	public Vector3[] GetVertices()
	{
		Vector3[] vertices = new Vector3[Vertices.Count];
		for (int i = 0; i < Vertices.Count; i++) vertices[i] = Vertices[i];
		return vertices;
	}

	public int[] GetTriangles()
	{
		int[] triangles = new int[Triangles.Count];
		for (int i = 0; i < Triangles.Count; i++) triangles[i] = Triangles[i];
		return triangles;
	}
}