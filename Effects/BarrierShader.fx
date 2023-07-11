sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
float2 uTargetPosition;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;
float size;
float pixelSize;

float dist(float2 colorvector)
{
    float ret = sqrt(colorvector.x * colorvector.x + colorvector.y * colorvector.y);
    return ret;
};

float4 BarrierShader(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float defaultSize = 256 * size;
    float2 coordsAsInt = int2((int) (coords.x * defaultSize), (int) (coords.y * defaultSize));
    float2 centerCoords = float2(0.5, 0.5);
    float4 color = tex2D(uImage0, coords);
    float4 empty = float4(0, 0, 0, 0);
    float length = dist(coordsAsInt / defaultSize - centerCoords); //Distance from center
    float Pixels = (pixelSize / defaultSize);
    if (length >= 0.5 || (length < 0.5 - Pixels))
        return empty;
    return color * sampleColor * pow((1 - 2 * abs(0.5 - Pixels / 2 - length) / Pixels), 2);
}

technique Technique1
{
    pass TPrismDyePass
    {
        PixelShader = compile ps_2_0 BarrierShader();
    }
}