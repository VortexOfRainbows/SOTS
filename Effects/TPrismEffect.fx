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

float4 TPrismEffect(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	//uSourceRect.x;
	//uSourceRect.y;
	float1 pixW = 2.0 / uImageSize0.x;
	float1 pixH = 2.0 / uImageSize0.y;
	float4 color = tex2D(uImage0, coords);
    //tex2D(uImage0, coords + float2(0, pixH)).w != 0
    if (!(coords.y + pixH > 1 || coords.y - pixH < 0 || coords.x - pixW < 0 || coords.x + pixW > 1) && tex2D(uImage0, coords - float2(0, pixH)).w != 0 && tex2D(uImage0, coords - float2(pixW, 0)).w != 0 && tex2D(uImage0, coords + float2(pixW, 0)).w != 0)
    {
        float luminosity = (color.r + color.g + color.b) / 15; 
        color.rgb = float3(luminosity, luminosity, luminosity);
    }
    else if (color.w != 0)
    {
        float time = uTime * 2.1;
        float red = 0.15 + 0.4 * (sin(time + coords.x * 1.5) + 1) / 2;
        float grn = 0.15 + 0.4 * (sin(time + 1.047) + 1) / 2;
        float blu = 0.4 + 0.5 * (sin(time + 2.09) + 1) / 2;
        color.rgb *= 0.4;
        color.rgba += float4(red, grn, blu, 0.4);
    }
	return color * sampleColor;
}

technique Technique1
{
    pass TPrismDyePass
    {
        PixelShader = compile ps_2_0 TPrismEffect();
    }
}