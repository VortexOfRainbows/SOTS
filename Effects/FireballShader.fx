sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float4 colorMod;
float opacity2;

float counter;
texture noise;
sampler tent = sampler_state
{
    Texture = (noise);
};

float4 Fireball(float2 coords : TEXCOORD0, float4 origcolor : COLOR0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float noiseR = pow(tex2D(tent, float2(counter % 1, coords.y)), sqrt(coords.y));
    
    float2 colorvector = float2(coords.x - 0.85f, coords.y - 0.5f);
    float colorDist = opacity2 / (colorvector.x * colorvector.x) + (colorvector.y * colorvector.y);
    color = lerp(float4(0, 0, 0, 0), colorMod, color.r * noiseR * colorDist);
    color = lerp(color, float4(1, 1, 1, 1), pow(clamp(sqrt(sqrt(color.a)) / 2, 0, 1), 2));
    color /= 2;
    return floor(origcolor * color * 4) / 4;
}

technique BasicColorDrawing
{
    pass Fireball
    {
        PixelShader = compile ps_2_0 Fireball();
    }
};