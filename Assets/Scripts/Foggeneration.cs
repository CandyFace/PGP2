using System;
using System.Collections.Generic;
using UnityEngine;

namespace N.Shaders.Volume {

	/// Generate a series of parallel planes relative to the camera
	[AddComponentMenu("N/Shaders/Simple Volume Renderer")]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class Foggeneration : MonoBehaviour {

		[Tooltip("The camera to make this volume viewable from")]
		public new UnityEngine.Camera camera;

		[Tooltip("The size of the frame to generate for this object")]
		public float size = 1f;

		[Tooltip("The number of slices to mesh")]
		public int slices = 10;
		private int slices_;

		[Tooltip("The slice interval")]
		public float sliceGap = 0.1f;

		/// Internal vertex buffer
		private Vector3[] points;

		/// Internal center id name
		private int centerId;
		private Renderer renders;

		public void Start() {
			Build();
			centerId = Shader.PropertyToID("_center");
			renders = GetComponent<Renderer> ();
			renders.material.SetFloat("Seed", UnityEngine.Random.Range(0.0f, 5.0f));
		}

		public void Update() {
			if (slices_ != slices) {
				Build();
			}
			Rebuild();
			renders.material.SetVector(centerId, this.transform.position);
		}

		/// Generate a quad facing the camera at the given offset
		private void BuildQuad(Vector3 origin, Vector3 up, Vector3 right, int offset) {
			points[offset * 4 + 0] = origin + right - up;
			points[offset * 4 + 1] = origin + right + up;
			points[offset * 4 + 2] = origin - right + up;
			points[offset * 4 + 3] = origin - right - up;
		}

		/// Generate new points for each quad
		public Vector3[] MeshPoints() {
			if ((points == null) || (points.Length != 4 * slices)) {
				points = new Vector3[4 * slices];
			}
			var offset = size / 2f;
			var normal = (-1f * camera.transform.forward).normalized;
			var up = camera.transform.up.normalized * offset;
			var right = Vector3.Cross(normal, up).normalized * offset;
			for (var i = 0; i < slices; ++i) {
				var o = i - slices / 2;
				var src = - sliceGap * o * normal;
				BuildQuad(src, up, right, i);
			}
			return points;
		}

		/// Rebuild vertex points only
		public void Rebuild() {
			MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
			meshFilter.mesh.vertices = MeshPoints();
		}

		/// Build all details of the mesh
		public void Build() {
			if (camera == null) {
				return;
			}

			MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();

			// Generate a set of meshes
			var mesh = new Mesh();
			mesh.Clear();

			var verts = MeshPoints();
			mesh.vertices = verts;

			// Full quads for the shader to use
			var uvs = new Vector2[4 * slices];
			for (var i = 0; i < slices; ++i) {
				uvs[i * 4 + 0]  = new Vector2(0f, 0f);
				uvs[i * 4 + 1]  = new Vector2(1f, 0f);
				uvs[i * 4 + 2]  = new Vector2(1f, 1f);
				uvs[i * 4 + 3]  = new Vector2(0f, 1f);
			}
			mesh.uv = uvs;

			// Always aim at camera
			var normal = (-1f * camera.transform.forward).normalized;
			var normals = new Vector3[4 * slices];
			for (var i = 0; i < slices; ++i) {
				normals[i * 4 + 0] = normal;
				normals[i * 4 + 1] = normal;
				normals[i * 4 + 2] = normal;
				normals[i * 4 + 3] = normal;
			}
			//mesh.normals = normals;

			// A series of quads
			var triangles = new int[6 * slices];
			for (var i = 0; i < slices; ++i) {
				triangles[i * 6 + 0] = i * 4 + 2;
				triangles[i * 6 + 1] = i * 4 + 1;
				triangles[i * 6 + 2] = i * 4 + 0;
				triangles[i * 6 + 3] = i * 4 + 0;
				triangles[i * 6 + 4] = i * 4 + 3;
				triangles[i * 6 + 5] = i * 4 + 2;
			}
			mesh.triangles = triangles;

			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
			mesh.Optimize();
			meshFilter.mesh = mesh;
			slices_ = slices;
		}

		public void OnDrawGizmos() {
			Gizmos.color = new Color(0.5f, 0.5f, 1.0f, 0.75f);
			Gizmos.DrawCube(transform.position, transform.lossyScale);
		}
	}
}