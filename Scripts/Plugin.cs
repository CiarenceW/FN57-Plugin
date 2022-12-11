using BepInEx.Configuration;
using Receiver2;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using R2CustomSounds;
using Receiver2ModdingKit;
using BepInEx;

namespace FN57_plugin
{
    //if any questions arose at any point and time and space about what the hell something in this code does, send me a message (Ciarence#6364)
    public class FN57Script : ModGunScript
    {
        private ModHelpEntry help_entry;
        public Sprite help_entry_sprite;
        private readonly float[] slide_push_hammer_curve = new float[] {
            0,
            0,
            0.02f,
            1
        };
        /*[HarmonyPatch(typeof(ReceiverCoreScript), "Awake")]
        [HarmonyPostfix]
        private static void PatchCoreAwake(ref ReceiverCoreScript __instance, ref GameObject[] ___gun_prefabs_all)
        {
            GunScript CiarencewFN57 = null;

            CiarencewFN57 = ___gun_prefabs_all.Single(gameObject => {
                return ((int)gameObject.GetComponent<GunScript>().gun_model == gun_model);
            }).GetComponent<GunScript>();

            __instance.generic_prefabs = new List<InventoryItem>(__instance.generic_prefabs) { CiarencewFN57 }.ToArray();
        }*/

        public override ModHelpEntry GetGunHelpEntry()
        {
            return help_entry = new ModHelpEntry("FN57")
            {
                info_sprite = help_entry_sprite,
                title = "FN Five-seveN",
                description = "FN Herstal Five-seveN MKII FDE\n"
                            + "Capacity: 20 + 1, 5.7x29mm SS190\n"
                            + "\n"
                            + "Made in conjunction with the 5.7x29mm cartridge, originally intended to replace the 9x19mm Parabellum cartridge, thanks to its superior penetration of body armor. The Five-seveN was made to fit one of two types of weapons firing this cartridge, as requested by NATO. Developed as a companion to the P90, the Five-seveN shares many design similarities, being mainly made out of polymer, with a large capacity magazine for its size, ambidextrous controls, and low recoil.\n"
                            + "\n"
                            + "After being exclusive to military and law enforcement customers, FN Herstal started selling it in 2004 to civilians, for personal protection and target shooting, which contributed even more to its iconic status. In 2013, FN Herstal introduced as the new standard version of the Five-seveN the MK2 model, offered in both black and FDE, it features a new one-piece metal slide underneath the cover, and a new combat adjustable rear-sight."
            };
        }
        public override LocaleTactics GetGunTactics()
        {
            return new LocaleTactics()
            {
                title = "FN Five-seveN",
                gun_internal_name = "CiarencewFN57",
                text = "A modded pistol that goes pew pew got damnnn\n" +
                "The Five-seveN is a pistol manufactured by FN Herstal Belgium, firing a special 5.7x28mm cartridge. To safely holster the Five-seveN, switch on the safety, or remove the magazine."
            };
        }
        public override CartridgeSpec GetCustomCartridgeSpec()
            {
            return new CartridgeSpec()
            {
                extra_mass = 7.5f,
                mass = 2f,
                speed = 716f,
                diameter = 0.0057f
            };
        }
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
            if (this.magazine_instance_in_gun != null)
            {
                this.magazine_instance_in_gun.spring.orig_dist = 0.098f;
            }
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
            ApplyTransform("loaded_chamber_indicator", loaded_chamber_indicator.amount, transform.Find("slide/loaded_chamber_indicator"));

        }
    }
}
