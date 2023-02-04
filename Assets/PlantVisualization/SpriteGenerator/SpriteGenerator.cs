using System.IO;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Utility;

namespace PlantVisualization.SpriteGenerator
{
    public static class SpriteGenerator
    {
        [MenuItem("GGJ_23/GenerateStemTextures")]
        public static void GenerateStemTextures()
        {
            const int size = 512;
            const int radius = 96;

            const int sizeTotal = size + radius * 2 + 4;

            var texture = new Texture2D(sizeTotal, sizeTotal, TextureFormat.RGBA32, false);

            CreateTexture(3, 1);
            CreateTexture(3, 0);
            CreateTexture(3, 5);
            CreateTexture(3, null);

            Object.DestroyImmediate(texture);

            void CreateTexture(int sideA, int? sideB)
            {
                using NativeArray<byte> textureData = GenerateStemTexture(size, sideA, sideB, radius);
                texture.LoadRawTextureData(textureData);

                File.WriteAllBytes(Application.dataPath + $"/PlantVisualization/Sprites/stem_{sideA}-{sideB}.png", texture.EncodeToPNG());
            }
        }

        private static NativeArray<byte> GenerateStemTexture(int hexagonSize, int sideA, int? sideB, int stemRadius)
        {
            int resolutionWithPadding = hexagonSize + stemRadius * 2 + 4;
            var imageSize = new float2(resolutionWithPadding);

            float2 center = imageSize * 0.5f;
            float2 offset = new float2(0.0f, hexagonSize * 0.5f);
            float2 positionA = center + offset.Rotate(-60 * sideA);
            float2 positionB = sideB.HasValue ? center + offset.Rotate(-60 * sideB.Value) : center;

            using NativeArray<float2> stemPositions = GenerateStemPositions(center, positionA, positionB);

            var textureData = new NativeArray<byte>(resolutionWithPadding * resolutionWithPadding * 4, Allocator.TempJob);
            var job = new GenerateStemTextureJob(textureData, resolutionWithPadding, stemRadius, stemPositions);
            job.Schedule().Complete();

            return textureData;
        }

        private static NativeArray<float2> GenerateStemPositions(float2 center, float2 positionA, float2 positionB)
        {
            var positions = new NativeArray<float2>(256, Allocator.TempJob);

            for (var i = 0; i < 128; i++)
                positions[i] = math.lerp(positionB, center, i / 128.0f);

            for (var i = 0; i < 128; i++)
                positions[i + 128] = math.lerp(center, positionA, i / 128.0f);

            return positions;
        }
    }
}