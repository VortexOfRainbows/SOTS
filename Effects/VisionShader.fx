sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float4 colorMod;
float4 lightColor;
float progress;
float2 uImageSize0;
float4 uSourceRect;

float dist(float2 colorvector)
{
    float ret = sqrt((colorvector.x * colorvector.x) + (colorvector.y * colorvector.y));
    return ret;
}
float4 White(float2 coords : TEXCOORD0) : COLOR0
{
    float frameY = (coords.y * uImageSize0.y - uSourceRect.y) / uSourceRect.w;
    float4 color = tex2D(uImage0, coords);
    float length = dist(float2(coords.x, frameY) - float2(0.5, 0.5));
    float fromCenter = clamp((progress - length), 0, 1);
    return lightColor * color * fromCenter + colorMod * (color * fromCenter).a * 3 * (1 - fromCenter);
}

technique BasicColorDrawing
{
    pass WhiteSprite
    {
        PixelShader = compile ps_2_0 White();
    }
};