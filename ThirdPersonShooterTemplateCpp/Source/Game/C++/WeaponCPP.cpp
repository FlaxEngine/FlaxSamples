#include "WeaponCPP.h"
#include "Engine/Debug/DebugLog.h"
#include "Engine/Input/Input.h"
#include "Engine/Engine/Time.h"
#include "Engine/Physics/Actors/RigidBody.h"
#include "Engine/Physics/Colliders/SphereCollider.h"
#include "Engine/Level/Actors/StaticModel.h"
#include "Engine/Level/Level.h"

WeaponCPP::WeaponCPP(const SpawnParams& params)
    : Script(params)
{
    // Enable ticking OnUpdate function
    _tickUpdate = true;
}

void WeaponCPP::OnUpdate()
{
    if (!_data)
        return;

    if (Input::GetAction(TEXT("Fire")) && Time::GetGameTime() >= _timeToFire)
    {
        _timeToFire = Time::GetGameTime() + 1.0f / _data->FireRate;

        auto bullet = New<RigidBody>();
        bullet->SetName(TEXT("Bullet"));
        bullet->SetStaticFlags(StaticFlags::None);
        bullet->SetUseCCD(true);

        auto model = New<StaticModel>();
        model->Model = _data->BulletModel;
        model->SetParent(bullet);
        model->SetStaticFlags(StaticFlags::None);

        // Ideally you would also use raycasting to determine if something was hit
        auto collider = New<SphereCollider>();
        collider->SetParent(bullet);
        collider->SetStaticFlags(StaticFlags::None);

        bullet->SetTransform(Transform(BulletSpawnPoint->GetPosition(), Quaternion::Identity, _data->BulletScale));
        Level::SpawnActor(bullet);

        bullet->SetLinearVelocity(BulletSpawnPoint->GetDirection() * _data->BulletVelocity);

        bullet->DeleteObject(_data->BulletLifetime);
    }
}

void WeaponCPP::OnStart()
{
    if(WeaponSettings == nullptr)
    {
        DebugLog::LogError(TEXT("No settings for the weapon were found"));
        return;
    }

    if (!WeaponSettings->WaitForLoaded())
    {
        _data = WeaponSettings->GetInstance<WeaponDataCPP>();
    }
}
