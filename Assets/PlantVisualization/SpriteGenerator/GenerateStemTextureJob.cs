using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace PlantVisualization.SpriteGenerator
{
    [BurstCompile(OptimizeFor = OptimizeFor.Performance)]
    public struct GenerateStemTextureJob : IJob
    {
        private NativeArray<byte> _textureData;
        private int _textureWidth;
        private int _stemRadius;

        [ReadOnly]
        private NativeArray<float2> _stemPositions;

        public GenerateStemTextureJob(NativeArray<byte> textureData, int textureWidth, int stemRadius, NativeArray<float2> stemPositions)
        {
            _textureData = textureData;
            _textureWidth = textureWidth;
            _stemRadius = stemRadius;
            _stemPositions = stemPositions;
        }

        public void Execute()
        {
            for (int y = 0; y < _textureWidth; y++)
            for (int x = 0; x < _textureWidth; x++)
            {

                int byteIndex = y * _textureWidth * 4 + x * 4;
                // _textureData[byteIndex + 0] = (byte) i; // R
                _textureData[byteIndex + 1] = 255; // G
                // _textureData[byteIndex + 2] = (byte) i; // B
                // _textureData[byteIndex + 3] = (byte) 255; // A
            }

            for (var i = 0; i < _stemPositions.Length; i++)
            {
                var stemExtents = new float2(_stemRadius);
                var min = (int2) math.floor(_stemPositions[i] - stemExtents);
                var max = (int2) math.ceil(_stemPositions[i] + stemExtents);

                for (int y = min.y; y <= max.y; y++)
                for (int x = min.x; x <= max.x; x++)
                {
                    var position = new int2(x, y);
                    float distance = math.distance(_stemPositions[i], position);

                    if (distance > _stemRadius)
                        continue;

                    int byteIndex = y * _textureWidth * 4 + x * 4;
                    _textureData[byteIndex + 0] = (byte) (i); // R

                    byte minDistance = (byte) distance <= _textureData[byteIndex + 1]
                        ? (byte) distance
                        : _textureData[byteIndex + 1];
                    _textureData[byteIndex + 1] = minDistance; // G
                    // _textureData[byteIndex + 2] = (byte) i; // B
                    _textureData[byteIndex + 3] = (byte) 255; // A
                }
            }
        }
    }
}