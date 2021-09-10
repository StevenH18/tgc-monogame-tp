﻿#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Custom Effects - https://docs.monogame.net/articles/content/custom_effects.html
// High-level shader language (HLSL) - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl
// Programming guide for HLSL - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-pguide
// Reference for HLSL - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-reference
// HLSL Semantics - https://docs.microsoft.com/en-us/windows/win32/direct3dhlsl/dx-graphics-hlsl-semantics

float4x4 World;
float4x4 View;
float4x4 Projection;

float Time = 0;

struct VertexShaderInput
{
	float4 Position : POSITION0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float3 MyPosition : TEXCOORD1;
};

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    // Clear the output
	VertexShaderOutput output = (VertexShaderOutput)0;
	
	// Hago algunas olas 
	// TODO hacer que las olas sean parametrizadas (supongo que la simulacion tiene que ser igual del lado de los barcos para saber su velocidad)
    input.Position = float4(input.Position.x, sin(input.Position.x / 100 + Time) * 10 + sin(input.Position.z / 50 + Time * 2) * 2, input.Position.zw);
	
    // Model space to World space
    float4 worldPosition = mul(input.Position, World);
    // World space to View space
    float4 viewPosition = mul(worldPosition, View);		
	// View space to Projection space
    output.Position = mul(viewPosition, Projection);
	
	// Guardo en otra variable la posicion en el mundo para que sea accesible en el Pixel Shader
    output.MyPosition = worldPosition;

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float height = max(input.MyPosition.y / 10, 0.7);
	
    float red = 0.18 * height;
    float green = 0.5 * height;
    float blue = 0.69 * height;
	
    return float4(red, green, blue, 1.0);
}

technique BasicColorDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
