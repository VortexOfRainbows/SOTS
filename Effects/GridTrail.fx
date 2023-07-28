sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float progress;
int pointCount;
float4 ColorOne;
float4 ColorTwo;
matrix WorldViewProjection;
float4 uShaderSpecificData;

struct VertexShaderInput
{
	float2 TextureCoordinates : TEXCOORD0;
    float4 Position : POSITION0;
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
	float2 TextureCoordinates : TEXCOORD0;
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
};
texture TrailTexture;
sampler tent = sampler_state
{
    Texture = (TrailTexture);
};
VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;
    float4 pos = mul(input.Position, WorldViewProjection);
    output.Position = pos;
    
    output.Color = input.Color;

	output.TextureCoordinates = input.TextureCoordinates;

    return output;
};

float4 White(VertexShaderOutput input) : COLOR0
{
    float4 color = lerp(ColorOne, ColorTwo, sin(input.TextureCoordinates.y * 3.14159));
    float alpha = sin(input.TextureCoordinates.x * 3.14159);
    if (input.TextureCoordinates.y <= 0.08 || input.TextureCoordinates.y >= 0.92 || input.TextureCoordinates.x % (0.25 / pointCount) <= (0.06 / pointCount) || (input.TextureCoordinates.y < 0.68 && input.TextureCoordinates.y > 0.62) || (input.TextureCoordinates.y < 0.38 && input.TextureCoordinates.y > 0.32))
    {
        return color * progress * alpha;
    }
    return float4(0, 0, 0, 1) * progress * alpha;
}

technique BasicColorDrawing
{
    pass DefaultPass
	{
		VertexShader = compile vs_2_0 MainVS();
	}
    pass MainPS
    {
        PixelShader = compile ps_2_0 White();
    }
};