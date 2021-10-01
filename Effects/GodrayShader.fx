sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float4 colorMod;
float distance;
float rotation;
float opacity2;

texture noise;
sampler tent = sampler_state
{
    Texture = (noise);
};
float2 rotate(float2 coords, float delta)
{
    float2 ret;
    ret.x = (coords.x * cos(delta)) - (coords.y * sin(delta));
    ret.y = (coords.x * sin(delta)) + (coords.y * cos(delta));
    return ret;
}
float lengthSquared(float2 colorvector)
{
    float ret = (colorvector.x * colorvector.x) + (colorvector.y * colorvector.y);
    return ret;
}
float4 White(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float2 noiseCoords = float2((distance / 10), 0);
    float angle = atan2(coords.x - 0.5f, coords.y - 0.5f) + rotation;
    noiseCoords = rotate(noiseCoords, angle);
    float4 noiseColor = tex2D(tent, noiseCoords + float2(0.5f,0.5f));
    float2 colorvector = float2(coords.x - 0.5f, coords.y - 0.5f);
    float colordist = opacity2 / sqrt(sqrt((colorvector.x * colorvector.x) + (colorvector.y * colorvector.y)));
    color = colorMod * color.r * noiseColor.r * noiseColor.r * noiseColor.r * colordist;
    return color;
}

technique BasicColorDrawing
{
    pass WhiteSprite
    {
        PixelShader = compile ps_2_0 White();
    }
};