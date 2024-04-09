#include "CharacterCPP.h"
#include "Engine/Input/Input.h"
#include "Engine/Core/Math/Quaternion.h"
#include "Engine/Engine/Time.h"
#include "Engine/Engine/Screen.h"
#include "Engine/Debug/DebugLog.h"
#include "Engine/Level/Actors/Camera.h"

CharacterCPP::CharacterCPP(const SpawnParams& params)
    : Script(params)
{
    // Enable ticking OnUpdate and OnFixedUpdate functions
    _tickUpdate = true;
    _tickFixedUpdate = true;
}

void CharacterCPP::OnStart()
{
    // Get Controller, since its the script's actor we just need to cast
    _controller = (CharacterController*)GetActor();
    if (!CameraView || !CharacterObj)
    {
        DebugLog::LogError(TEXT("No Character or Camera assigned!"));
        return;
    }

    _defaultFov = CameraView->GetFieldOfView();
}

void CharacterCPP::OnUpdate()
{
    Screen::SetCursorLock(CursorLockMode::Locked);
    Screen::SetCursorVisible(false);
}

void CharacterCPP::OnFixedUpdate()
{
    // Camera Rotation
    // Get mouse axis values and clamp pitch
    _yaw += Input::GetAxis(TEXT("Mouse X")) * MouseSensitivity * Time::GetDeltaTime(); // H
    _pitch += Input::GetAxis(TEXT("Mouse Y")) * MouseSensitivity * Time::GetDeltaTime(); // V
    _pitch = Math::Clamp(_pitch, PitchMinMax.X, PitchMinMax.Y);

    // The camera's parent should be another actor, like a spring arm for instance
    CameraView->GetParent()->SetOrientation(Quaternion::Lerp(CameraView->GetParent()->GetOrientation(), Quaternion::Euler(_pitch, _yaw, 0), Time::GetDeltaTime() * CameraLag));
    CharacterObj->SetOrientation(Quaternion::Euler(0, _yaw, 0));

    // When right clicking, zoom in or out
    if (Input::GetAction(TEXT("Aim")))
    {
        CameraView->SetFieldOfView(Math::Lerp(CameraView->GetFieldOfView(), FOVZoom, Time::GetDeltaTime() * 5));
    }
    else
    {
        CameraView->SetFieldOfView(Math::Lerp(CameraView->GetFieldOfView(), _defaultFov, Time::GetDeltaTime() * 5));
    }

    // Character Movement
    // Get input axes
    auto inputH = Input::GetAxis(TEXT("Horizontal"));
    auto inputV = Input::GetAxis(TEXT("Vertical"));

    // Apply movement towards the camera direction
    auto movement = Vector3(inputH, 0.0f, inputV);
        
        
    auto movementDirection = Vector3::Transform(movement, CameraView->GetTransform().Orientation);

    // Jump if the space bar is down, jump
    if (_controller->IsGrounded() && Input::GetAction(TEXT("Jump")))
    {
        _velocity.Y = Math::Sqrt(JumpStrength * -2 * Gravity);
    }

    // Apply gravity
    _velocity.Y += Gravity * Time::GetDeltaTime();
    movementDirection += (_velocity * 0.5f);

    // Apply controller movement, evaluate whether we are sprinting or not
    auto speed = (Input::GetAction(TEXT("Sprint")) ? SprintSpeed : Speed) * Time::GetDeltaTime();
    _controller->Move(movementDirection * speed);
}
