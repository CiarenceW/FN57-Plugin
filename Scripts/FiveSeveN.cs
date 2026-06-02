using BepInEx.Configuration;
using Receiver2;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using Receiver2ModdingKit;
using BepInEx;

namespace FiveSeveN
{
    //if any questions arose at any point and time and space about what the hell something in this code does, send me a message (Ciarence#6364)
    public class FiveSeveN : ModGunScript
    {
        private readonly float[] slide_push_hammer_curve = new float[] {
            0,
            0,
            0.02f,
            1
        };

        public override void InitializeGun()
        {
            pooled_muzzle_flash = ((GunScript)ReceiverCoreScript.Instance().generic_prefabs.First(it => { return it is GunScript && ((GunScript)it).gun_model == GunModel.Model10; })).pooled_muzzle_flash;
        }

        public override void AwakeGun()
        {
            hammer.amount = 1;
        }

        public override void UpdateGun()
        {
            if (IsSafetyOn())
            { // Safety blocks the trigger from moving
                trigger.amount = Mathf.Min(trigger.amount, 0.1f);
                trigger.UpdateDisplay();
            }
            hammer.asleep = false;
            LocalAimHandler lah = LocalAimHandler.player_instance;
            float amount = hammer.amount;
            if (slide.amount > 0.2f) //makes the hammer go to its max value
            {
                hammer.amount = Mathf.Max(hammer.amount, InterpCurve(slide_push_hammer_curve, slide.amount));
            }
            if (slide.amount == 0f && trigger.amount == 0f) //makes it so you have to unpress the trigger to be able to shoot again I think actually I don't know really but it seems like what it is
            {
                _disconnector_needs_reset = false;
            }
            if (trigger.amount == 1f && hammer.amount == _hammer_cocked_val && !_disconnector_needs_reset && !IsSafetyOn() && magazine_instance_in_gun) //hammer firing logic
            {
                if (slide.amount == 0f)
                {
                    hammer.target_amount = 0f;
                    hammer.vel = -0.1f * ReceiverCoreScript.Instance().player_stats.animation_speed;
                }
                _disconnector_needs_reset = true;
            }
            float vel = hammer.vel;
            if (hammer.amount > _hammer_cocked_val) //cocking logic
            {
                _hammer_state = 2;
                hammer.target_amount = _hammer_cocked_val;
                hammer.vel = -0.1f * ReceiverCoreScript.Instance().player_stats.animation_speed;
            }
            hammer.TimeStep(Time.deltaTime);
            if (hammer.amount == 0f && _hammer_state == 2 && vel < 0) //shooting logic
            {
                TryFireBullet(1, FireBullet);
                _hammer_state = 0;
            }
            trigger.UpdateDisplay();
            safety.UpdateDisplay();
            ApplyTransform("trigger_bar", trigger.amount, transform.Find("trigger_bar"));
            ApplyTransform("barrel", slide.amount, transform.Find("barrel"));
            ApplyTransform("Cam", slide.amount, transform.Find("Cam"));
            ApplyTransform("loaded_chamber_indicator", 1, transform.Find("slide/loaded_chamber_indicator"));
            UpdateAnimatedComponents();
        }
    }
}
