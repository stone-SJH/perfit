/*******************************************************************************

INTEL CORPORATION PROPRIETARY INFORMATION
This software is supplied under the terms of a license agreement or nondisclosure
agreement with Intel Corporation and may not be copied or disclosed except in
accordance with the terms of that agreement
Copyright(c) 2012-2014 Intel Corporation. All Rights Reserved.

******************************************************************************/

using UnityEngine;
using System.Collections;
using RSUnityToolkit;
using System.IO;
using System;

/// <summary>
/// Point cloud viewer 
/// </summary>
public class PointCloudViewer : MonoBehaviour {
	
	#region Public Fields
	
	/// <summary>
	/// The point cloud material
	/// </summary>
	public Material PointCloudMaterial = null;	
	
	/// <summary>
	/// The max depth value.
	/// </summary>
	public float MaxDepthVal = 10f;
	public float AverageDepthVal = 0;
	
	/// <summary>
	/// When enabled UV Map will be used to show color image on the point cloud
	/// </summary>
	public bool UseUVMap = true;
	public double x1, y1, z1, x2, y2, z2, x3, y3, z3;
	
	#endregion
	
	#region Private Fields
	
	private Mesh _mesh;	
	private Vector3[] _vertices = null;		
	private Vector2[] _uv 		= null;
	private Vector4[] _tangents = null;
	private int[] _triangles 	= null;
	
	private bool _removeBackTriangles = true;
	
	private bool _lastUseUVMAP = true;
	private bool[] IsEdge ;
	
	private DrawImages _drawImagesComponent;

	private FileStream aFile;
	private StreamWriter sw;
	private string LOG_FILE_NAME = "work.txt";

	#endregion

	//游戏对象，这里是线段对象
	private GameObject LineRenderGameObject;
	
	//线段渲染器
	private LineRenderer lineRenderer;
	
	//设置线段的个数，标示一个曲线由几条线段组成
	private int lineLength = 3;
	
	//分别记录4个点，通过这4个三维世界中的点去连接一条线段
	private Vector3 v0 = new Vector3(0.0f,1.0f,0.0f);
	
	#region Private methods
	
	/// <summary>
	/// Sets the sense option according to the Stream field
	/// </summary>
	private void SetSenseOptions()
	{				
		SenseToolkitManager.Instance.SetSenseOption(SenseOption.SenseOptionID.PointCloud);	
		SenseToolkitManager.Instance.SetSenseOption (SenseOption.SenseOptionID.Hand);
		if (UseUVMap)
		{
			SenseToolkitManager.Instance.SetSenseOption(SenseOption.SenseOptionID.UVMap);	
		}
			
	}
	/// <summary>
	/// Unsets the sense option according to the Stream field
	/// </summary>	
	private void UnsetSenseOptions()
	{			
		SenseToolkitManager.Instance.UnsetSenseOption(SenseOption.SenseOptionID.PointCloud);	
		
		if (_lastUseUVMAP)
		{
			SenseToolkitManager.Instance.UnsetSenseOption(SenseOption.SenseOptionID.UVMap);		
		}
	}
	
	#endregion
	
	#region Unity's overridden methods
	
	// Use this for initialization
	void Start () 
	{	
		aFile = new FileStream(LOG_FILE_NAME, FileMode.Create);
		sw = new StreamWriter(aFile);

		var senseManager = GameObject.FindObjectOfType(typeof(SenseToolkitManager));
		if (senseManager == null)
		{
			Debug.LogWarning("Sense Manager Object not found and was added automatically");			
			senseManager = (GameObject)Instantiate(Resources.Load("SenseManager"));
			senseManager.name = "SenseManager";
		}
		
		SetSenseOptions();
		
		this.gameObject.AddComponent< MeshFilter > ();
		if (this.GetComponent<MeshRenderer>() == null)
		{
			this.gameObject.AddComponent< MeshRenderer > ();	
		}
		
		if (PointCloudMaterial!=null)
		{
			this.gameObject.GetComponent<Renderer>().material = PointCloudMaterial;
		}
		
		_drawImagesComponent = this.gameObject.AddComponent<DrawImages>();
		
		if (UseUVMap)
		{
			_drawImagesComponent.enabled = true;
		}
		else 
		{
			_drawImagesComponent.enabled = false;
		}

/*		LineRenderGameObject = GameObject.Find ("line");
		lineRenderer = (LineRenderer)LineRenderGameObject.GetComponent ("LineRenderer");
		lineRenderer.SetVertexCount(lineLength);*/
	}
	
	// Update is called once per frame
	void Update () 
	{	
	//	lineRenderer.SetPosition (1, v0);
		if (_lastUseUVMAP != UseUVMap)
		{
			UnsetSenseOptions();
			SetSenseOptions();
			if (UseUVMap)
			{
				_drawImagesComponent.enabled = true;
			}
			else 
			{
				_drawImagesComponent.enabled = false;
			}
			_lastUseUVMAP = UseUVMap;
		}
		
		if (SenseToolkitManager.Instance.PointCloud != null)
		{		
			if (_mesh == null)
			{
				// Retrieve a mesh instance
				_mesh = this.gameObject.GetComponent<MeshFilter> ().mesh;	

			}
			int _gridSize = 3;
			
			int width = SenseToolkitManager.Instance.ImageDepthOutput.info.width/_gridSize;
			int height = SenseToolkitManager.Instance.ImageDepthOutput.info.height/_gridSize;

//			sw.WriteLine("{0} {1}\n",width,height);
			
			// Build vertices and UVs
			if (_vertices == null)
			{
				_vertices = new Vector3[width * height];
			}		
							
			if (_tangents == null)
			{
				_tangents = new Vector4[width * height];
			}	
			
			if (_uv == null)
			{
				_uv = new Vector2[width * height];
			}

			PXCMHandData _handData;
			PXCMHandData.IHand _iHand;
			PXCMHandData.JointData WristData;

			if (SenseToolkitManager.Instance.Initialized && SenseToolkitManager.Instance.HandDataOutput != null)
			{
				_handData = SenseToolkitManager.Instance.HandDataOutput;
				if (_handData.QueryHandData(PXCMHandData.AccessOrderType.ACCESS_ORDER_FIXED,0,out _iHand) == pxcmStatus.PXCM_STATUS_NO_ERROR)
				{
					if (_iHand.QueryTrackedJoint ((PXCMHandData.JointType)0, out WristData) == pxcmStatus.PXCM_STATUS_NO_ERROR)	
					{
						x1 = -WristData.positionWorld.x;
						y1 = WristData.positionWorld.y;
						z1 = WristData.positionWorld.z;
	//					lineRenderer.SetPosition (0, new Vector3(-WristData.positionWorld.x,WristData.positionWorld.y,WristData.positionWorld.z)*100f);
					}
				}
			}

			int i = 0;
			for (int y=0; y <  height; y++) 
			{
				for (int x=0; x <  width - 1; x++)  
				{				
				
					int j = y * width * _gridSize * _gridSize + x * _gridSize;
					
					_vertices [i].x  = SenseToolkitManager.Instance.PointCloud[j].x / 10;
					_vertices [i].y  = SenseToolkitManager.Instance.PointCloud[j].y / 10;
					_vertices [i].z  = -SenseToolkitManager.Instance.PointCloud[j].z / 10;		


//					sw.Write ("({0},{1},{2}) ",_vertices [i].x,_vertices [i].y,_vertices [i].z);
					
					if (UseUVMap) 
					{
						_uv[i].x = SenseToolkitManager.Instance.UvMap[j].x ;
						_uv[i].y = SenseToolkitManager.Instance.UvMap[j].y ;
					}
					
					i++;
				}
			}
	

			// Assign them to the mesh
			_mesh.vertices = _vertices;
			_mesh.uv = _uv;
			
			// Build triangle indices: 3 indices into vertex array for each triangle
			if (_triangles == null)
			{
				_triangles = new int[(height - 1) * (width - 1) *6];			
			}
			
			bool backGroundTriangles = false;
			int index =0;
			for ( int y = 0; y < height - 1 ; y++ ) 
			{
				for ( int x = 0; x < width - 1; x++ ) 
				{	
					if (_removeBackTriangles) {
						backGroundTriangles = (
							( Mathf.Abs(_vertices[y * width + x].z) > MaxDepthVal ) || 
							( Mathf.Abs(_vertices[y * width + x + 1].z) > MaxDepthVal ) || 
							( Mathf.Abs(_vertices[(y + 1) * width + x].z )> MaxDepthVal ) || 
							( Mathf.Abs(_vertices[(y + 1) * width + x + 1].z) > MaxDepthVal ) 
						);

						backGroundTriangles = backGroundTriangles || (
							( Mathf.Abs(_vertices[y * width + x].z)  == 0 ) || 
							( Mathf.Abs(_vertices[y * width + x + 1].z) == 0 ) || 
							( Mathf.Abs(_vertices[(y + 1) * width + x].z ) == 0 ) || 
							( Mathf.Abs(_vertices[(y + 1) * width + x + 1].z) == 0 ) 
						);
					}
					if (!backGroundTriangles) {
						// For each grid cell output two triangles
						_triangles [index++] = (y * width) + x;
						_triangles [index++] = ((y + 1) * width) + x;
						_triangles [index++] = (y * width) + x + 1;
						
						_triangles [index++] = ((y + 1) * width) + x;
						_triangles [index++] = ((y + 1) * width) + x + 1;
						_triangles [index++] = (y * width) + x + 1;
					}
					
					
				}
			}

			int targetx = 0;

			for (int y = height - 3; y <= height - 2; y++)
				for (int x = width - 2; x >= 0; x--)
				{
					bool p = (
						( Mathf.Abs(_vertices[y * width + x].z)  == 0 ) || 
						( Mathf.Abs(_vertices[y * width + x + 1].z) == 0 ) || 
						( Mathf.Abs(_vertices[(y + 1) * width + x].z ) == 0 ) || 
					( Mathf.Abs(_vertices[(y + 1) * width + x + 1].z) == 0 ) ||
					( Mathf.Abs(_vertices[(y-1) * width + x].z ) == 0 )||
					( Mathf.Abs(_vertices[(y-1) * width + x + 1].z ) ==0 ));
					if (!p)
					{
						targetx = x;
						sw.Write ("{0} {1}      ", y,x);
/*						lineRenderer.SetPosition (2, new Vector3
					                          (-_vertices[y * width + x].x,_vertices[y * width + x].y,-_vertices[y * width + x].z));*/
						y++;
						break;
					}
				}

			bool[] pan = new bool[1111];
			for (int y = height - 30; y >= 0 ; y--)
			{
				int x = targetx - 5;
				pan[y] = (
					( Mathf.Abs(_vertices[y * width + x].z)  == 0 ) || 
					( Mathf.Abs(_vertices[y * width + x + 1].z) == 0 ) || 
					( Mathf.Abs(_vertices[(y + 1) * width + x].z ) == 0 ) || 
					( Mathf.Abs(_vertices[(y + 1) * width + x + 1].z) == 0 ) ||
					( Mathf.Abs(_vertices[(y-1) * width + x].z ) == 0 )||
					( Mathf.Abs(_vertices[(y-1) * width + x + 1].z ) ==0 ));
				if (pan[y] && pan[y+1] && pan[y+2] && pan[y+3] && pan[y+4] && pan[y+5] && pan[y+6])
				{
					y = y + 16;
					x2 = -_vertices[y * width + x].x;
					y2 = _vertices[y * width + x].y;
					z2 = -_vertices[y * width + x].z;
/*					lineRenderer.SetPosition (2, new Vector3
					                          (-_vertices[y * width + x].x,_vertices[y * width + x].y,-_vertices[y * width + x].z));*/
					break;
				}

			}

			double xx,yy,zz;
			double Max = 0, temp;
			for ( int y = 0; y < height - 1 ; y++ ) 
			{
				for ( int x = 0; x < width - 1; x++ ) 
				{	
					if (_removeBackTriangles) {
						backGroundTriangles = (
							( Mathf.Abs(_vertices[y * width + x].z) > MaxDepthVal ) || 
							( Mathf.Abs(_vertices[y * width + x + 1].z) > MaxDepthVal ) || 
							( Mathf.Abs(_vertices[(y + 1) * width + x].z )> MaxDepthVal ) || 
							( Mathf.Abs(_vertices[(y + 1) * width + x + 1].z) > MaxDepthVal ) 
							);
						
						backGroundTriangles = backGroundTriangles || (
							( Mathf.Abs(_vertices[y * width + x].z)  == 0 ) || 
							( Mathf.Abs(_vertices[y * width + x + 1].z) == 0 ) || 
							( Mathf.Abs(_vertices[(y + 1) * width + x].z ) == 0 ) || 
							( Mathf.Abs(_vertices[(y + 1) * width + x + 1].z) == 0 ) ||
							( Mathf.Abs(_vertices[(y-1) * width + x].z ) == 0 )||
							( Mathf.Abs(_vertices[(y-1) * width + x + 1].z ) ==0 )
							);
					}
					if (!backGroundTriangles) {
						xx = -_vertices[y * width + x].x;
						yy = _vertices[y * width + x].y;
						zz = -_vertices[y * width + x].z;
						if (yy > y1 || xx > x2-10 || xx < x1-5) continue;

						if ((temp = calc(xx,yy,zz,x1,y1,z1,x2,y2,z2)) > Max)
						{
							Max = temp;
							x3 = xx;
							y3 = yy;
							z3 = zz;
						}
					}
				}
			}

		/*	lineRenderer.SetPosition (1, new Vector3
			                          ((float)x3,(float)y3,(float)z3));*/
		/*	for (int y = 0; y <= 5; y++)
			{
				for (int x = 0; x <= width - 1; x++)
				{
					_triangles [index++] = (y * width) + x;
					_triangles [index++] = ((y + 1) * width) + x;
					_triangles [index++] = (y * width) + x + 1;
					
					_triangles [index++] = ((y + 1) * width) + x;
					_triangles [index++] = ((y + 1) * width) + x + 1;
					_triangles [index++] = (y * width) + x + 1;
				}
			}*/

			for ( ; index < (height - 1) * (width - 1) * 6 ; index++)
			{
				_triangles[index] = 0;
			}
			
			
			_mesh.triangles = _triangles;
			
			// Auto-calculate vertex normals from the mesh
			_mesh.Optimize();
			_mesh.RecalculateNormals ();			
		}
	}

	double calc(double x1, double y1, double z1,double x2, double y2, double z2, double x3, double y3, double z3)
	{
		double d1 = Math.Sqrt (Math.Pow (x1 - x2, 2) + Math.Pow (y1 - y2, 2) + Math.Pow (z1 - z2, 2));
		double d2 = Math.Sqrt (Math.Pow (x1 - x3, 2) + Math.Pow (y1 - y3, 2) + Math.Pow (z1 - z3, 2));
		double d3 = Math.Sqrt (Math.Pow (x3 - x2, 2) + Math.Pow (y3 - y2, 2) + Math.Pow (z3 - z2, 2));
		double p = (d1 + d2 + d3) / 2.0;
		return Math.Sqrt (p * (p - d1) * (p - d2) * (p - d3));
	}

	//On enable set sense options
	void OnEnable()
	{
		if (SenseToolkitManager.Instance == null)
		{
			return;
		}
		
		SetSenseOptions();
	}
	
	//On disable unset sense options
	void OnDisable()
	{
		UnsetSenseOptions();
	}
	
	#endregion
}