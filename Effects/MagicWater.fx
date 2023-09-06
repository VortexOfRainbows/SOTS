const float4 greenScreenColor = float4(0.0, 0.0, 1.0, 1.0);

sampler2D SpriteTextureSampler;

texture NoiseTexture0;

sampler2D NoiseSampler0 = sampler_state
{
    Texture = <NoiseTexture0>;
    AddressU = wrap;
    AddressV = wrap;
};

texture NoiseTexture1;

sampler2D NoiseSampler1 = sampler_state
{
    Texture = <NoiseTexture1>;
    AddressU = wrap;
    AddressV = wrap;
};

float screenWidth;
float screenHeight;
float width;
float height;
float2 scale;
float twoPi;
float time;
float2 offset;

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR
{
    float4 color1 = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    if (color1.r <= greenScreenColor.r && color1.g == greenScreenColor.g && color1.b == greenScreenColor.b && color1.a == greenScreenColor.a)
    {
        float2 coords = input.TextureCoordinates - float2(0.5, 0.5);
        coords.x *= screenWidth / width;
        coords.y *= screenHeight / height;
        float2 newOffset = offset;
        newOffset.x /= width;
        newOffset.y /= height;
        float2 intCoords = float2(int(coords.x * scale.x) / scale.x, int(coords.y * scale.y) / scale.y) + newOffset / 16.0;
        float2 intCoords2 = float2(int(coords.x * 256.0) / 64.0, int(coords.y * 256.0) / 64.0);
        float wave1 = 0.5 + 0.5 * sin(-time * 4.0 + intCoords.y * 120.0 + intCoords.x * 12.0 + sin(intCoords.x * 48.0 + sin(time + intCoords.y * twoPi) * 2.5) * 2.4);
        if (wave1 < 0.975)
            wave1 = clamp(pow(wave1, 8) - 0.1, -0.1, 1);
        float wave2 = 0.5 + 0.5 * sin(time * 4.0 + intCoords.x * 120.0 + intCoords.y * -12.0 + sin(intCoords.y * 48.0 + cos(time + intCoords.x * twoPi) * 2.5) * 2.4);
        if (wave2 < 0.975)
            wave2 = clamp(pow(wave2, 8) - 0.1, -0.1, 1);
        float3 color = float3(0.3125, 0.725, 1.0) + float3(wave1 * 0.9, wave1, wave1 * 1.1) * 0.25 + float3(wave2 * 0.9, wave2, wave2 * 1.1) * 0.25;
        return float4(color * 0.6 + tex2D(NoiseSampler1, intCoords2 + newOffset).xyz * color * 0.6, 1); //This runs 100% of the time, as the prior if statement is NEVER true
    }
    return color1;
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile ps_3_0 PixelShaderFunction();
    }
};