sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float progress;
float4 ColorOne;
float4 ColorTwo;
matrix WorldViewProjection;

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
    float2 coords = float2(input.TextureCoordinates.x * 3, 0.25 + input.TextureCoordinates.y * 0.5);
    float3 color = tex2D(tent, coords + float2(progress, 0)).xyz;
    float3 color2 = tex2D(tent, coords + float2(-progress, 0)).xyz * 0.5;

    return float4((color + color2) * ColorOne * (1.0 + color.x * 2.0), color.x * ColorOne.w);
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