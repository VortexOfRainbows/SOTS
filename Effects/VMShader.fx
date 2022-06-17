sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;
float4 uShaderSpecificData;

float dist(float2 colorvector)
{
    float ret = sqrt((colorvector.x * colorvector.x) + (colorvector.y * colorvector.y));
    return ret;
}
float4 VMShader(float2 coords : TEXCOORD0) : COLOR0
{
    float2 targetCoords = (uTargetPosition - uScreenPosition) / uScreenResolution;
    float4 color = tex2D(uImage0, coords);
    float4 uColor2 = float4(uColor.r, uColor.g, uColor.b, color.a);
    float yRatio = uScreenResolution.y / uScreenResolution.x;
    float matchY = coords.y * yRatio;
    float length = dist(float2(coords.x, matchY) - float2(targetCoords.x, yRatio * targetCoords.y));
    float fromCenter = length * uProgress * 20;
    float strength = (color.r + color.g + color.b) / 3.0;
    if(strength > 0.4f)
    {
        strength = strength * 1.2;
    }
    else
        strength = strength * 0.8;
    float4 color2 = lerp(float4(strength, strength, strength, color.a), uColor2, uIntensity);
    if(fromCenter > 1)
    {
        float4 color3 = lerp(float4(strength * 0.35, strength * 0.35, strength * 0.35, color.a), uColor2, uIntensity);
        return lerp(color2, color3, clamp(fromCenter * 0.2 - 1, 0, 1));
    }
    return lerp(color, color2, fromCenter * fromCenter);
}
technique Technique1
{
    pass VMShaderPass
    {
        PixelShader = compile ps_2_0 VMShader();
    }
}