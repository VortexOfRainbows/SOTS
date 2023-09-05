const float4 black = float4(0.0, 0.0, 0.0, 0.0);
const float4 white = float4(0.0, 0.0, 1.0, 1.0);
const float4 codedColor = float4(0.0, 0.0, 1.0, 1.0);
const float edge = 0.98f;

sampler2D SpriteTextureSampler;


struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
    if (color.b > edge && color.r < 0.01 && color.g < 0.01)
    {
        return codedColor;
    }
    return color;
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile ps_2_0 MainPS();
    }
};