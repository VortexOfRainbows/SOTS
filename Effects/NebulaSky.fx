const float4 greenScreenColor = float4(0.0, 1.0, 0.0, 1.0);

sampler2D SpriteTextureSampler;

texture GalaxyTexture0;

sampler2D GalaxySampler0 = sampler_state
{
    Texture = <GalaxyTexture0>;
    AddressU = wrap;
    AddressV = wrap;
};

texture GalaxyTexture1;

sampler2D GalaxySampler1 = sampler_state
{
    Texture = <GalaxyTexture1>;
    AddressU = wrap;
    AddressV = wrap;
};

texture GalaxyTexture2;

sampler2D GalaxySampler2 = sampler_state
{
    Texture = <GalaxyTexture2>;
    AddressU = wrap;
    AddressV = wrap;
};


float screenWidth;
float screenHeight;
float width;
float height;
float2 offset;
float time;

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);

    float2 coords = input.TextureCoordinates;
    coords.x *= screenWidth / width;
    coords.y *= screenHeight / height;

    float2 newOffset = offset;
    newOffset.x /= width;
    float2 other = float2(time * 0.0025f, (1 - screenHeight / height) / 2 + newOffset.y / height);
    newOffset.y = 0;
    float4 parallax1 = tex2D(GalaxySampler1, coords);
    if (parallax1.a != 0) //front layer
        return parallax1;
    return tex2D(GalaxySampler0, coords);
}

technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile ps_3_0 MainPS();
    }
};