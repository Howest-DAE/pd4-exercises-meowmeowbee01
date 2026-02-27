using Assets.AsyncExercises;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.W03_MaterialLoader.Scripts
{
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

		async void Start()
		{
			int texturesPerMaterial = _textureProperties.Length;

			var lines = await File.ReadAllLinesAsync(_filePath);

			int materialCount = lines.Length / texturesPerMaterial;

			Vector3 spawnPosition = (materialCount - 1) * _spacing * 0.5f * -Vector3.right;
			Vector3 spacingOffset = Vector3.right * _spacing;

			//Enumerable.Range(0, materialCount).ToList().ForEach(async i => ApplyTextures(CreateMaterialSphere(spawnPosition += spacingOffset), (await Task.WhenAll(lines.Skip(i * texturesPerMaterial).Take(texturesPerMaterial).Select(s => TextureLoader.LoadTextureAsync(s)))).ToList()));

			for (int i = 0; i < materialCount; i++)
			{
				//ApplyTextures(CreateMaterialSphere(spawnPosition += spacingOffset), (await Task.WhenAll(lines.Skip(i * texturesPerMaterial).Take(texturesPerMaterial).Select(s => TextureLoader.LoadTextureAsync(s)))).ToList());

				var sphere = CreateMaterialSphere(spawnPosition);
				spawnPosition += spacingOffset;
				try
				{
					var textureTasks = lines
						.Skip(i * texturesPerMaterial)
						.Take(texturesPerMaterial)
						.Select(s => TextureLoader.LoadTextureAsync(s));
					var textures = (await Task.WhenAll(textureTasks)).ToList();

					ApplyTextures(sphere, textures);
				}
				catch (System.InvalidOperationException)
				{
					sphere.GetComponent<MeshRenderer>().material = _invalidMaterial;
				}
			}
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
}