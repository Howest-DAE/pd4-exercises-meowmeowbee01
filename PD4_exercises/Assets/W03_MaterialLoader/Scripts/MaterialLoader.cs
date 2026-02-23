using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MaterialLoader : MonoBehaviour
{
	static readonly string[] _textureProperties = new[]
	{
		"_BaseMap", //Albedo
		"_OcclusionMap", //AO
		"_MetallicGlossMap", //Metallic
		"_BumpMap", //Normal
		"_ParallaxMap" //Height	
	};

	[SerializeField]
	Material _templateMaterial;

	[SerializeField]
	Material _invalidMaterial;

	[SerializeField]
	private float _spacing = 1f;

	[SerializeField]
	private string _filePath;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		int texturesPerMaterial = _textureProperties.Length;

		//TODO: read lines from file

		int numMaterials = 3; //TODO: calculate based on num textures

		Vector3 spawnPos = -Vector3.right * (numMaterials - 1) * 0.5f * _spacing;
		Vector3 spacingOffset = Vector3.right * _spacing; //add this to spawnPos after every material spawn

		//TODO: For every material:
		//	create primitive,
		//	load textures,
		//	apply textures
	}

	GameObject CreateMaterialSphere(Vector3 position)
	{
		GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		primitive.transform.SetParent(transform);
		primitive.transform.localPosition = position;

		return primitive;
	}

	void ApplyTextures(GameObject primitive, List<Texture2D> textures)
	{
		//This will throw an exception when values are not equal
		Assert.AreEqual(_textureProperties.Length, textures.Count, $"Number of textures to apply ({textures.Count}) doesn't match the number of texture properties ({_textureProperties.Length})");

		//Create a new material based on the template material
		Material material = new Material(_templateMaterial);
		primitive.GetComponent<Renderer>().sharedMaterial = material;

		//Apply each texture per texture property
		for (int idx = 0; idx < _textureProperties.Length; ++idx)
		{
			string propertyName = _textureProperties[idx];
			Texture2D texture = textures[idx];

			material.SetTexture(propertyName, texture);
		}
	}
}
