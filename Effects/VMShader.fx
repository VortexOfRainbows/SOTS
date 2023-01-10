sampler uImage0 : register(s0);
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
float4 uShaderSpecificData; //All the variables defined above are inputted into this class from elsewhere: they're parameters!

//this is simply a distance function that is used to calculate distance from center of the screen
float dist(float2 colorvector)
{
    float ret = sqrt((colorvector.x * colorvector.x) + (colorvector.y * colorvector.y));
    return ret;
}
float4 VMShader(float2 coords : TEXCOORD0) : COLOR0
{
    float2 targetCoords = (uTargetPosition - uScreenPosition) / uScreenResolution; //(float, float) vector at the center of the screen
    float4 color = tex2D(uImage0, coords); //What is RGB(float, float, float, float) of the pixel at these coordinates?
    float4 uColor2 = float4(uColor.r, uColor.g, uColor.b, color.a); //uColor2 is the color to transition to, AKA Black and White
    float yRatio = uScreenResolution.y / uScreenResolution.x; //Find ration of Y/X of screen dimensions
    float matchY = coords.y * yRatio; //Increase the y-Ratio to mimic the screen Y dimension being identical to the X dimension
    float length = dist(float2(coords.x, matchY) - float2(targetCoords.x, yRatio * targetCoords.y)); //Distance from center
    float fromCenter = length * uProgress * 20; //uProgress is a timer variable inserting into this class when the effect triggers
    float strength = (color.r + color.g + color.b) / 3.0; //Finds the average luminosity of the colors
    if(strength > 0.4f) 
    {
        strength = strength * 1.2; //Make the colors brighter to add some contrast, if they are already bright
    }
    else
        strength = strength * 0.8; //Make the colors darker to add some contrast, if they are already dark
    float4 color2 = lerp(float4(strength, strength, strength, color.a), uColor2, uIntensity);
    if(fromCenter > 1) //this adds a limit on how strong the shader is at distances very far from the center
    { //without artificially increasing Terraria's zoom-out capabilities, this doesn't really do anything
        float4 color3 = lerp(float4(strength * 0.35, strength * 0.35, strength * 0.35, color.a), uColor2, uIntensity);
        return lerp(color2, color3, clamp(fromCenter * 0.2 - 1, 0, 1));
    }
    return lerp(color, color2, fromCenter * fromCenter); //interpolate to the new color based on the square of the distance
    //creating a small, circular zone where the color gradients in the center of the screen
}
technique Technique1
{
    pass VMShaderPass
    {
        PixelShader = compile ps_2_0 VMShader();
    }
}