sampler2D r_ImplicitInput : register(S0);

float r_DesaturationFactor : register(C0);

float4 main(float2 rpCoordinate : TEXCOORD) : COLOR
{
    float4 rColor = tex2D(r_ImplicitInput, rpCoordinate);
    float rGray = dot(rColor.rgb, float3(0.299, 0.587, 0.114));

    float4 rResult;
    rResult.r = (rColor.r - rGray) * r_DesaturationFactor + rGray;
    rResult.g = (rColor.g - rGray) * r_DesaturationFactor + rGray;
    rResult.b = (rColor.b - rGray) * r_DesaturationFactor + rGray;
    rResult.a = rColor.a;

    return rResult;
}
