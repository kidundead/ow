//! @file SpriteMesh.cs


using UnityEngine;
using System.Collections;


//! @class SpriteMesh
//! @brief 精灵网格
public class SpriteMesh : MonoBehaviour
{
	//! 使用的层
	private int m_Layer = 0;

	//! MeshFilter
	private MeshFilter m_MeshFilter = null;

	//! MeshRenderer
	private MeshRenderer m_MeshRenderer = null;

	//! 精灵数组
	private ArrayList m_Sprites = null;

	//! 最大精灵层数
	private int m_MaxSpriteLayer = 0;

	//! 每层精灵分组(按照使用的材质分组)
	private Hashtable [] m_SpritesGroup = null;


	//! 初始化
	public void Initialize(int layer, int max_sprite_layer)
	{
		//
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;

		//
		m_Layer = layer;
		gameObject.layer = m_Layer;

		//
		m_MeshFilter = (MeshFilter)gameObject.AddComponent(typeof(MeshFilter));

		//
		m_MeshRenderer = (MeshRenderer)gameObject.AddComponent(typeof(MeshRenderer));
		m_MeshRenderer.castShadows = false;
		m_MeshRenderer.receiveShadows = false;

		//
		m_Sprites = new ArrayList();

		m_MaxSpriteLayer = max_sprite_layer;
		m_SpritesGroup = new Hashtable [m_MaxSpriteLayer];
		for (int i = 0; i < m_MaxSpriteLayer; ++i)
		{
			m_SpritesGroup[i] = new Hashtable();
		}
	}

	public void LateUpdate()
	{
		//
		m_MeshFilter.mesh.Clear();

		//
		if (m_Sprites.Count <= 0)
		{
			return;
		}

		// 按照层号材质分组(同时过滤掉材质无效,层号无效的精灵)
		int total_sprite_count = 0;

		for (int i = 0; i < m_MaxSpriteLayer; ++i)
		{
			m_SpritesGroup[i].Clear();
		}

		for (int i = 0; i < m_Sprites.Count; ++i)
		{
			Sprite sprite = (Sprite)m_Sprites[i];

			int layer = sprite.Layer;
			if ((layer < 0) || (layer >= m_MaxSpriteLayer))
			{
				continue;
			}

			Material material = sprite.Material;
			if (material == null)
			{
				continue;
			}

			total_sprite_count++;

			if (m_SpritesGroup[layer].Contains(material))
			{
				ArrayList sprites = (ArrayList)m_SpritesGroup[layer][material];
				sprites.Add(sprite);
			}
			else
			{
				ArrayList sprites = new ArrayList();
				sprites.Add(sprite);

				m_SpritesGroup[layer].Add(material, sprites);
			}
		}

		// 计算子网格数量
		int sub_mesh_count = 0;
		for (int i = 0; i < m_MaxSpriteLayer; ++i)
		{
			sub_mesh_count += m_SpritesGroup[i].Count;
		}

		// 合并子网格
		Vector3 [] vertices = new Vector3[total_sprite_count * 4];
		Vector2 [] uv = new Vector2[total_sprite_count * 4];
		Color [] colors = new Color[total_sprite_count * 4];
		int vertex_index = 0;

		int [][] triangles = new int[sub_mesh_count][];
		Material [] materials = new Material [sub_mesh_count];
		int sub_mesh_index = 0;


		// 处理每个层
		for (int layer = 0; layer < m_MaxSpriteLayer; ++layer)
		{
			//
			if (m_SpritesGroup[layer].Count <= 0)
			{
				continue;
			}

			//
			Material [] keys = new Material [m_SpritesGroup[layer].Count];
			m_SpritesGroup[layer].Keys.CopyTo(keys, 0);

			// 处理每个材质
			for (int i = 0; i < keys.Length; ++i)
			{
				Material material = keys[i];
				ArrayList sprites = (ArrayList)m_SpritesGroup[layer][material];

				//
				triangles[sub_mesh_index] = new int[sprites.Count * 6];

				//
				for (int j = 0; j < sprites.Count; ++j)
				{
					Sprite sprite = (Sprite)sprites[j];

					vertices[vertex_index + 0] = sprite.Vertices[0];
					vertices[vertex_index + 1] = sprite.Vertices[1];
					vertices[vertex_index + 2] = sprite.Vertices[2];
					vertices[vertex_index + 3] = sprite.Vertices[3];
	
					uv[vertex_index + 0] = sprite.UV[0];
					uv[vertex_index + 1] = sprite.UV[1];
					uv[vertex_index + 2] = sprite.UV[2];
					uv[vertex_index + 3] = sprite.UV[3];

					colors[vertex_index + 0] = sprite.Color;
					colors[vertex_index + 1] = sprite.Color;
					colors[vertex_index + 2] = sprite.Color;
					colors[vertex_index + 3] = sprite.Color;

					int index = j * 6;

					triangles[sub_mesh_index][index + 0] = vertex_index + Sprite.Triangles[0];
					triangles[sub_mesh_index][index + 1] = vertex_index + Sprite.Triangles[1];
					triangles[sub_mesh_index][index + 2] = vertex_index + Sprite.Triangles[2];
					triangles[sub_mesh_index][index + 3] = vertex_index + Sprite.Triangles[3];
					triangles[sub_mesh_index][index + 4] = vertex_index + Sprite.Triangles[4];
					triangles[sub_mesh_index][index + 5] = vertex_index + Sprite.Triangles[5];

					vertex_index += 4;
				}

				//
				materials[sub_mesh_index] = material;

				sub_mesh_index++;
			}
		}

		// 设置网格
		m_MeshFilter.mesh.subMeshCount = sub_mesh_count;
		m_MeshFilter.mesh.vertices = vertices;
		m_MeshFilter.mesh.uv = uv;
		m_MeshFilter.mesh.colors = colors;
		for (int i = 0; i < sub_mesh_count; i++)
		{
			m_MeshFilter.mesh.SetTriangles(triangles[i], i);
		}

		// 设置材质
		m_MeshRenderer.materials = materials;
	}

	//! 添加精灵
	public void Add(Sprite sprite)
	{
		m_Sprites.Add(sprite);
	}

	//! 移除精灵
	public void Remove(Sprite sprite)
	{
		m_Sprites.Remove(sprite);
	}

	//! 移除全部精灵
	public void RemoveAll()
	{
		m_Sprites.Clear();
	}
}

