#include <noise_functions.fx>

sampler baseSampler : register(s0) = sampler_state
{
	AddressU = Wrap;
	AddressV = Wrap;
};

float2 aspectRatio;
float2 offset;
float noiseFrequency;
float noiseGain;
float noiseLacunarity;
float multiplier;
float4 noiseLowColor;
float4 noiseHighColor;
bool fbmPerlinBasis;
bool fbmCellBasis;
bool fbmInvCellBasis;
int fbmIterations;

int opaqueBlend = 0;
int overlayBlend = 1;
int additiveBlend = 2;
int blendType;
int worleyFeature;	// 0 = F1, 1 = F2, 2 = F2-F1 -- defined in noise_functions.fx
bool invert;
float4x4 matrixTransform;

// scaleTexCoords
float2 scaleTexCoords(float2 texCoords)
{
	float2 fixedRatio = renderSize / float2(512, 512);
	return (offset / renderSize) - (texCoords / noiseScale) * fixedRatio;
}

// blend
float4 blend(float noiseValue, float2 texCoords)
{
	float4 base = lerp(noiseLowColor, noiseHighColor, noiseValue);
	
	if (blendType == overlayBlend)
	{
		base *= tex2D(baseSampler, texCoords);
	}
	else if (blendType == additiveBlend)
	{
		base += tex2D(baseSampler, texCoords);
	}

	return base;
}

// getPerlin
float getPerlin(float2 texCoords)
{
	// Base values
	float4 base = tex2D(baseSampler, texCoords);
	float baseValue = (base.r + base.g + base.b) / 3;
	
	// Calculate noise
	float2 coords = scaleTexCoords(texCoords);
	float value = fbmPerlin(coords, fbmIterations, noiseFrequency, noiseGain, noiseLacunarity);
	return invert ? 1 - value : value;
}

// getWorley
float getWorley(float2 texCoords, int feature)
{
	// Base values
	float4 base = tex2D(baseSampler, texCoords);
	float baseValue = (base.r + base.g + base.b) / 3;
	
	// Calculate noise
	float2 coords = scaleTexCoords(texCoords);
	float value = fbmWorley(coords, feature, fbmIterations, noiseFrequency, noiseGain, noiseLacunarity);
	return invert ? 1 - value : value;
}

// Vertex shader
void VSBase(inout float4 color:COLOR0, inout float2 texCoord:TEXCOORD0, inout float4 position:SV_Position) 
{ 
	position = mul(position, matrixTransform); 
}

// Perlin pixel shaders
float4 PSPerlin(float2 texCoords:TEXCOORD0) : COLOR0
{
	float value = getPerlin(texCoords) * multiplier;
	float4 final = blend(value, texCoords);

	return final;
}

// Worley pixel shaders
float4 PSWorley(float2 texCoords:TEXCOORD0) : COLOR0
{
	float value = getWorley(texCoords, worleyFeature) * multiplier;
	float4 final = blend(value, texCoords);

	return final;
}

// Techniques
technique perlin_noise
{ 
	pass main 
	{ 
		VertexShader = compile vs_3_0 VSBase();
		PixelShader = compile ps_3_0 PSPerlin(); 
	}
}

// Worley techniques
technique worley_noise
{ 
	pass main
	{
		VertexShader = compile vs_3_0 VSBase();
		PixelShader = compile ps_3_0 PSWorley();
	}
}