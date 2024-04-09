#pragma once

#include "Engine/Scripting/Script.h"
#include "Engine/Core/Math/Vector3.h"
#include "Engine/Physics/Colliders/CharacterController.h"
#include "Engine/Scripting/ScriptingObjectReference.h"

API_CLASS() class GAME_API CharacterCPP : public Script
{
API_AUTO_SERIALIZATION();
DECLARE_SCRIPTING_TYPE(CharacterCPP);
public:
    // Movement
    API_FIELD(Attributes="ExpandGroups, Tooltip(\"The character model\"), EditorDisplay(\"Movement\", \"Character\")")
    ScriptingObjectReference<Actor> CharacterObj = nullptr;

    API_FIELD(Attributes = "Range(0f, 300f), Tooltip(\"Movement speed factor\"), EditorDisplay(\"Movement\", \"Speed\")")
    float Speed = 250;

    API_FIELD(Attributes = "Range(0f, 300f), Tooltip(\"Movement speed factor\"), EditorDisplay(\"Movement\", \"Sprint Speed\")")
    float SprintSpeed = 300;

    API_FIELD(Attributes = "Limit(-20f, 20f), Tooltip(\"Gravity of this character\"), EditorDisplay(\"Movement\", \"Gravity\")")
    float Gravity = -9.81f;

    API_FIELD(Attributes = "Range(0f, 25f), Tooltip(\"Jump factor\"), EditorDisplay(\"Movement\", \"Jump Strength\")")
    float JumpStrength = 10;

    // Camera
    API_FIELD(Attributes = "ExpandGroups, Tooltip(\"The camera view for player\"), EditorDisplay(\"Camera\", \"Camera View\")")
    ScriptingObjectReference<Camera> CameraView = nullptr;

    API_FIELD(Attributes = "Range(0, 10f), Tooltip(\"Sensitivity of the mouse\"), EditorDisplay(\"Camera\", \"Mouse Sensitivity\")")
    float MouseSensitivity = 100;

    API_FIELD(Attributes = "Range(0f, 20f), Tooltip(\"Lag of the camera, lower = slower\"), EditorDisplay(\"Camera\", \"Camera Lag\")")
    float CameraLag = 10;

    API_FIELD(Attributes = "Range(0f, 100f), Tooltip(\"How far to zoom in, lower = closer\"), EditorDisplay(\"Camera\", \"FOV Zoom\")")
    float FOVZoom = 50;

    API_FIELD(Attributes="Tooltip(\"Determines the min and max pitch value for the camera\"), EditorDisplay(\"Camera\", \"Pitch Min Max\")")
    Float2 PitchMinMax = Float2(-45, 45);

private:
    CharacterController* _controller;
    Vector3 _velocity;

    float _yaw;
    float _pitch;
    float _defaultFov;

public:
    // [Script]
    void OnStart() override;
    void OnUpdate() override;
    void OnFixedUpdate() override;
};
