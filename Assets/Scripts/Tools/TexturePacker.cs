using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TexturePacker : MonoBehaviour {
	
	public bool generateTriangleStrips = true;
	public bool generateLightMapUVs=true;
	public bool generateColorTextures=true;
	public MeshFilter[] excludedObjects = new MeshFilter[0];
	public Color generatedMaterialColor = Color.white;
	
	private Dictionary<Shader,List<Material>> shaderToMaterial = new Dictionary<Shader,List<Material>>();
	private Dictionary<Shader,Material> generatedMaterials = new Dictionary<Shader,Material>();
	private Dictionary<Material, Rect> generatedUVs = new Dictionary<Material, Rect>();
	private Dictionary<Material, Rect> generatedUV2s = new Dictionary<Material, Rect>();
	
	// Use this for initialization
	void Start () {
		List<Component> filters = new List<Component>();
		filters.AddRange(GetComponentsInChildren(typeof(MeshFilter)));
		
		/// First remove all excluded meshfilters from list
		foreach (MeshFilter mf in excludedObjects) {
			filters.Remove(mf);
		}
		
		Vector2[] uv,uv2;
		
		// Find all unique shaders in hierarchy.
		for (int i=0;i < filters.Count;i++) {		
			Renderer curRenderer  = filters[i].GetComponent<Renderer>();
			
			uv2 = (Vector2[])(((MeshFilter)filters[i]).mesh.uv2);
			if ((uv2==null || uv2.Length==0) && generateLightMapUVs) { 
				uv = (Vector2[])(((MeshFilter)filters[i]).mesh.uv);
				uv2 = new Vector2[uv.Length]; 
				Array.Copy(uv,uv2,uv.Length);
				((MeshFilter)filters[i]).mesh.uv2=uv2;
			}
			
			bool noTex=true;
			if (curRenderer != null && curRenderer.enabled && curRenderer.material != null)  {
				Material[] materials = curRenderer.sharedMaterials;
					
				if (materials != null) {
					foreach (Material mat in materials) {
						if (generateColorTextures) {
							if (mat.mainTexture == null) {
								
								Texture2D colorTex = new Texture2D(4,4);
								
								for (int y=0; y < colorTex.height; ++y) {
									for (int x=0; x < colorTex.width; ++x) {
										colorTex.SetPixel (x, y, mat.color);
									}
								}
								// Apply all SetPixel calls
								colorTex.Apply();
								
								mat.SetTexture("_MainTex",colorTex);
								mat.color=Color.white;
							}
							else {
								noTex=false;
							}
						}
						
						if (((mat.HasProperty("_LightMap") && (!(((MeshFilter)filters[i]).mesh.uv2.Length == 0) || generateLightMapUVs) && mat.GetTexture("_LightMap") != null) || !(mat.HasProperty("_LightMap"))) &&
							(mat.mainTextureScale==new Vector2(1.0f,1.0f) &&
							(mat.mainTextureOffset==Vector2.zero))
						) {
							if (mat.shader != null && mat.mainTexture != null) {
								if (shaderToMaterial.ContainsKey(mat.shader)) {
									shaderToMaterial[mat.shader].Add(mat);
								}
								else {
									shaderToMaterial[mat.shader]=new List<Material>();
									shaderToMaterial[mat.shader].Add(mat);
								}
							}
						}
					}
					
					if (generateColorTextures && noTex) {
						Vector2[] uvv = (Vector2[])(((MeshFilter)filters[i]).mesh.uv);
						for (int j=0; j < uvv.Length;j++) {
							uvv[j]=new Vector2(0.5f,0.5f);
						}

						((MeshFilter)filters[i]).mesh.uv = uvv;
					}
				}
			}
		}
		
		// Pack textures per shader basis and generate UV rect and material dictinaries. 
		foreach (Shader key in shaderToMaterial.Keys) {
			Texture2D packedTexture=new Texture2D(1024,1024);
			Texture2D[] texs = new Texture2D[shaderToMaterial[key].Count];
			generatedMaterials[key] = new Material(key);
			for (int i=0;i < texs.Length; i++) {
				texs[i] = shaderToMaterial[key][i].mainTexture as Texture2D;
			}
			Rect[] uvs = packedTexture.PackTextures(texs,0,2048);
			packedTexture.Apply();
			
			generatedMaterials[key].CopyPropertiesFromMaterial(shaderToMaterial[key][0]);		
			generatedMaterials[key].mainTexture=packedTexture;
			generatedMaterials[key].color = generatedMaterialColor;
			
			for (int i=0;i < texs.Length; i++) {
				if (shaderToMaterial[key][i].HasProperty("_LightMap")) {
					texs[i] = shaderToMaterial[key][i].GetTexture("_LightMap") as Texture2D;
				}	
			}
			packedTexture=new Texture2D(1024,1024);
			Rect[] uvs2 = packedTexture.PackTextures(texs,0,2048);
			packedTexture.Apply();
			if (generatedMaterials[key].HasProperty("_LightMap")) {
				generatedMaterials[key].SetTexture("_LightMap", packedTexture);
			}
			
			for (int i=0;i < texs.Length; i++) {
				generatedUVs[shaderToMaterial[key][i]] = uvs[i];
				generatedUV2s[shaderToMaterial[key][i]] = uvs2[i];
			}
		}
		
		// Calculate new UVs for all submeshes and assign generated materials.
		for (int i=0;i < filters.Count;i++) {
			
			int subMeshCount = ((MeshFilter)filters[i]).mesh.subMeshCount;
				
			Material[] mats = filters[i].gameObject.GetComponent<Renderer>().sharedMaterials;	
			uv = (Vector2[])(((MeshFilter)filters[i]).mesh.uv);
			uv2 = (Vector2[])(((MeshFilter)filters[i]).mesh.uv2);
			for (int j=0; j < subMeshCount; j++) {
				if ( generatedUVs.ContainsKey(mats[j])) {
					Rect uvs = generatedUVs[mats[j]];
					Rect uvs2 = generatedUV2s[mats[j]];
					int[] subMeshVertices = DeleteDuplicates(((MeshFilter)filters[i]).mesh.GetTriangles(j)) as int[];
					mats[j]=generatedMaterials[filters[i].gameObject.GetComponent<Renderer>().sharedMaterials[j].shader];
					foreach (int vert in subMeshVertices) {
						uv[vert]=new Vector2((uv[vert].x*uvs.width)+uvs.x, (uv[vert].y*uvs.height)+uvs.y);
						if (uv2!=null && !(uv2.Length==0)) {
							uv2[vert]=new Vector2((uv2[vert].x*uvs2.width)+uvs2.x, (uv2[vert].y*uvs2.height)+uvs2.y);
						}
					}
				}
			}
			filters[i].gameObject.GetComponent<Renderer>().sharedMaterials=mats;
			((MeshFilter)filters[i]).mesh.uv=uv;
			if (uv2!=null && !(uv2.Length==0)) {
				((MeshFilter)filters[i]).mesh.uv2=uv2;
			}
		}
		
		// Combine Meshes
		CombineMeshes();	
	}
	
	// Combine Children script to be called after Material and Texture packing.
	private void CombineMeshes() {
		//Component[] filters  = GetComponentsInChildren(typeof(MeshFilter));
		
		List<Component> filters = new List<Component>();
		filters.AddRange(GetComponentsInChildren(typeof(MeshFilter)));
		
		/// First remove all excluded meshfilters from list
		foreach (MeshFilter mf in excludedObjects) {
			filters.Remove(mf);
		}
		
		Matrix4x4 myTransform = transform.worldToLocalMatrix;
		Hashtable materialToMesh= new Hashtable();
		
		for (int i=0;i<filters.Count;i++) {
			MeshFilter filter = (MeshFilter)filters[i];
			Renderer curRenderer  = filters[i].GetComponent<Renderer>();
			MeshCombineUtility.MeshInstance instance = new MeshCombineUtility.MeshInstance ();
			instance.mesh = filter.sharedMesh;
			if (curRenderer != null && curRenderer.enabled && instance.mesh != null) {
				instance.transform = myTransform * filter.transform.localToWorldMatrix;
				
				Material[] materials = curRenderer.sharedMaterials;
				for (int m=0;m<materials.Length;m++) {
					instance.subMeshIndex = System.Math.Min(m, instance.mesh.subMeshCount - 1);
	
					ArrayList objects = (ArrayList)materialToMesh[materials[m]];
					if (objects != null) {
						objects.Add(instance);
					}
					else
					{
						objects = new ArrayList ();
						objects.Add(instance);
						materialToMesh.Add(materials[m], objects);
					}
				}
				
				curRenderer.enabled = false;
			}
		}
	
		foreach (DictionaryEntry de  in materialToMesh) {
			ArrayList elements = (ArrayList)de.Value;
			MeshCombineUtility.MeshInstance[] instances = (MeshCombineUtility.MeshInstance[])elements.ToArray(typeof(MeshCombineUtility.MeshInstance));

			// We have a maximum of one material, so just attach the mesh to our own game object
			if (materialToMesh.Count == 1)
			{
				// Make sure we have a mesh filter & renderer
				if (GetComponent(typeof(MeshFilter)) == null)
					gameObject.AddComponent(typeof(MeshFilter));
				if (!GetComponent("MeshRenderer"))
					gameObject.AddComponent<MeshRenderer>();
	
				MeshFilter filter = (MeshFilter)GetComponent(typeof(MeshFilter));
				filter.mesh = MeshCombineUtility.Combine(instances, generateTriangleStrips);
				GetComponent<Renderer>().material = (Material)de.Key;
				GetComponent<Renderer>().enabled = true;
			}
			// We have multiple materials to take care of, build one mesh / gameobject for each material
			// and parent it to this object
			else
			{
				GameObject go = new GameObject("Combined mesh");
				go.transform.parent = transform;
				go.transform.localScale = Vector3.one;
				go.transform.localRotation = Quaternion.identity;
				go.transform.localPosition = Vector3.zero;
				go.AddComponent(typeof(MeshFilter));
				go.AddComponent<MeshRenderer>();
				go.GetComponent<Renderer>().material = (Material)de.Key;
				MeshFilter filter = (MeshFilter)go.GetComponent(typeof(MeshFilter));
				filter.mesh = MeshCombineUtility.Combine(instances, generateTriangleStrips);
			}
		}	
	}
	
	public static Array DeleteDuplicates(Array arr)
	{
	   // this procedure works only with vectors
	   if (arr.Rank != 1 )
	      throw new ArgumentException("Multiple-dimension arrays are not supported");

	   // we use a hashtable to track duplicates
	   // make the hash table large enough to avoid memory re-allocations
	   Hashtable ht = new Hashtable(arr.Length * 2);
	   // we will store unique elements in this ArrayList
	   ArrayList elements = new ArrayList();

	   foreach (object Value in arr)
	   {
	      if ( !ht.Contains(Value) )
	      {
	         // we've found a non duplicate
	         elements.Add(Value);
	         // remember it for later
	         ht.Add(Value, null);
	      }
	   }
	   // return an array of same type as the original array
	   return elements.ToArray(arr.GetType().GetElementType());
	}
}
