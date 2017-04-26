using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFRenderer.Primitives;

namespace YFRenderer.Core
{
    public static class Common
    {
        public static int CanvasWidth = 800;
        public static int CanvasHeight = 600;

        public static void Swap<T>(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }

        public static double GetFractionalPart(double num)
        {
            return (num - Math.Truncate(num));
        }

        // 限制数值范围在0和1之间 
        public static float Clamp(float value, float min = 0, float max = 1)
        {
            return Math.Max(min, Math.Min(value, max));
        }

        // 过渡插值  
        public static float Interpolate(float min, float max, float gradient)
        {
            return min + (max - min) * Clamp(gradient);
        }

        public static Mesh[] LoadJSONFileAsync(string fileName)
        {
            var meshes = new List<Mesh>();
            var materials = new Dictionary<String, Material>();
            string str = Directory.GetCurrentDirectory() + "/"+ fileName;
            StreamReader StReader = new StreamReader(str);
            string data = StReader.ReadToEnd();
            StReader.Close();

            dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(data);

            for (var materialIndex = 0; materialIndex < jsonObject.materials.Count; materialIndex++)
            {
                var material = new Material();
                material.Name = jsonObject.materials[materialIndex].name.Value;
                material.ID = jsonObject.materials[materialIndex].id.Value;
                if (jsonObject.materials[materialIndex].diffuseTexture != null)
                    material.DiffuseTextureName = jsonObject.materials[materialIndex].diffuseTexture.name.Value;

                materials.Add(material.ID, material);
            }

            for (var meshIndex = 0; meshIndex < jsonObject.meshes.Count; meshIndex++)
            {
                var verticesArray = jsonObject.meshes[meshIndex].vertices;
                // 面片  
                var indicesArray = jsonObject.meshes[meshIndex].indices;

                var uvCount = jsonObject.meshes[meshIndex].uvCount.Value;
                var verticesStep = 1;

                // 取决于纹理坐标的数量，我们动态的选择6步进、8步进以及10步进值  
                switch ((int)uvCount)
                {
                    case 0:
                        verticesStep = 6;
                        break;
                    case 1:
                        verticesStep = 8;
                        break;
                    case 2:
                        verticesStep = 10;
                        break;
                }

                // 我们感兴趣的顶点信息数量  
                var verticesCount = verticesArray.Count / verticesStep;
                // 面片的数量是索引数组长度除以3（一个面片有三个顶点索引）  
                var facesCount = indicesArray.Count / 3;
                var mesh = new Mesh(jsonObject.meshes[meshIndex].name.Value, verticesCount, facesCount);

                // 首先填充我们网格的顶点数组  
                for (var index = 0; index < verticesCount; index++)
                {
                    var x = (float)verticesArray[index * verticesStep].Value;
                    var y = (float)verticesArray[index * verticesStep + 1].Value;
                    var z = (float)verticesArray[index * verticesStep + 2].Value;

                    var nx = (float)verticesArray[index * verticesStep + 3].Value;
                    var ny = (float)verticesArray[index * verticesStep + 4].Value;
                    var nz = (float)verticesArray[index * verticesStep + 5].Value;
                    mesh.Vertices[index] = new Vertex { Coordinates = new Vector3d(x, y, z), Normal = new Vector3d(nx, ny, nz) }; ;

                    if (uvCount > 0)
                    {
                        // Loading the texture coordinates
                        float u = (float)verticesArray[index * verticesStep + 6].Value;
                        float v = (float)verticesArray[index * verticesStep + 7].Value;
                        mesh.Vertices[index].TextureCoordinates = new Vector2d(u, v);
                    }
                }

                // 然后填充面片数组  
                for (var index = 0; index < facesCount; index++)
                {
                    var a = (int)indicesArray[index * 3].Value;
                    var b = (int)indicesArray[index * 3 + 1].Value;
                    var c = (int)indicesArray[index * 3 + 2].Value;
                    mesh.Faces[index] = new Face { A = a, B = b, C = c };
                }

                // 获取在Blender中设置的位置坐标  
                var position = jsonObject.meshes[meshIndex].position;
                mesh.Position = new Vector3d((float)position[0].Value, (float)position[1].Value, (float)position[2].Value);
                if (uvCount > 0)
                {
                    // Texture
                    var meshTextureID = jsonObject.meshes[meshIndex].materialId.Value;
                    var meshTextureName = materials[meshTextureID].DiffuseTextureName;
                    if(meshTextureName != null)
                      mesh.Texture = new Texture(meshTextureName, 512, 512);
                }
                meshes.Add(mesh);
            }
            return meshes.ToArray();
        }
    }
}
