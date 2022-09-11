using BepInEx;
using BepInEx.Configuration;
using Receiver2;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using R2CustomSounds;

namespace FN57_plugin
{
    //if any questions arose at any point and time and space about what the hell something in this code does, send me a message (Ciarence#6364)
    [BepInPlugin("Ciarencew.FN57", "FN57 Plugin", "0.0.1")]
    public class Plugin : BaseUnityPlugin
    {
        private static ConfigEntry<bool> use_custom_sounds;

        private static Dictionary<string, string> customEvents = new()
        {
            { "fire", "custom:/fn57/fiveseven_fire" },
            { "dry_fire", "event:/guns/1911/1911_dry_fire" },
            { "safety_off", "event:/guns/deagle/safety_off" },
            { "safety_on", "event:/guns/deagle/safety_on"},
            { "slide_back", "custom:/fn57/fiveseven_slider_in_fast" },
            { "slide_released", "custom:/fn57/fiveseven_slider_out_fast" },
            { "trigger_reset", "event:/guns/1911/1911_trigger_reset" },
            { "eject_mag", "custom:/fn57/fiveseven_mag_out" },
            { "insert_mag", "custom:/fn57/fiveseven_mag_in"},
            { "start_insert_mag", "custom:/fn57/fiveseven_mag_rattle2"},
            { "press_check_start", "custom:/fn57/fiveseven_slider_out_slow" },
            { "press_check_end", "custom:/fn57/fiveseven_slider_in_slow" },
            { "eject_bullet", "event:/guns/1911/1911_eject_bullet" },
            { "trigger_forwards", "event:/guns/1911/1911_trigger_forwards" },
            { "trigger_blocked", "event:/guns/1911/1911_trigger_blocked" },
            { "slide_back_partial", "event:/guns/deagle/slide_back_partial" }
        };

        private static Dictionary<string, string> defaultEvents = new()
        {
            { "fire", "event:/guns/1911/shot" },
            { "dry_fire", "event:/guns/1911/1911_dry_fire" },
            { "safety_off", "event:/guns/deagle/safety_off" },
            { "safety_on", "event:/guns/deagle/safety_on" },
            { "slide_back", "event:/guns/deagle/slide_back" },
            { "slide_released", "event:/guns/deagle/slide_released" },
            { "trigger_reset", "event:/guns/1911/1911_trigger_reset" },
            { "eject_mag", "event:/guns/1911/eject_mag" },
            { "insert_mag", "event:/guns/1911/insert_mag" },
            { "start_insert_mag", "event:/guns/1911/start_insert_mag" },
            { "press_check_start", "event:/guns/deagle/press_check_start" },
            { "press_check_end", "event:/guns/deagle/press_check_end" },
            { "eject_bullet", "event:/guns/1911/1911_eject_bullet"},
            { "trigger_forwards", "event:/guns/1911/1911_trigger_forwards"},
            { "trigger_blocked", "event:/guns/1911/1911_trigger_blocked"},
            { "slide_back_partial", "event:/guns/deagle/slide_back_partial" }
        };

        private static void setSoundEvents(ref GunScript gun, Dictionary<string, string> events)
        {
            gun.sound_event_gunshot = events["fire"];
            gun.sound_dry_fire = events["dry_fire"];
            gun.sound_safety_off = events["safety_off"];
            gun.sound_safety_on = events["safety_on"];
            gun.sound_slide_back = events["slide_back"];
            gun.sound_slide_back_partial = events["slide_back_partial"];
            gun.sound_trigger_reset = events["trigger_reset"];
            gun.sound_eject_mag_empty = events["eject_mag"];
            gun.sound_eject_mag_loaded = events["eject_mag"];
            gun.sound_insert_mag_empty = events["insert_mag"];
            gun.sound_insert_mag_loaded = events["insert_mag"];
            gun.sound_start_insert_mag = events["start_insert_mag"];
            gun.sound_press_check_start = events["press_check_start"];
            gun.sound_press_check_end = events["press_check_end"];
            gun.sound_eject_bullet = events["eject_bullet"];
            gun.sound_trigger_forwards = events["trigger_forwards"];
            gun.sound_trigger_blocked = events["trigger_blocked"];
            gun.sound_slide_released = events["slide_released"];
        }

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo("Plugin FN57 is loaded!");

            tryFireBullet = typeof(GunScript).GetMethod("TryFireBullet", BindingFlags.NonPublic | BindingFlags.Instance);

            Harmony.CreateAndPatchAll(GetType());

            instance = this;

            ModAudioManager.LoadCustomEvents("fn57", Application.persistentDataPath + "/Guns/FN57/Sounds");

            use_custom_sounds = Config.Bind("Gun Settimgs", "Use custom sounds", true);
        }
        private static int gun_model = 1057;

        private static MethodInfo tryFireBullet;
        private static Plugin instance;

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
        [HarmonyPatch(typeof(CartridgeSpec), "SetFromPreset")]
        [HarmonyPrefix]
        private static void PatchSetFromPreset(ref CartridgeSpec __instance, CartridgeSpec.Preset preset)
        {
            if ((int)preset == gun_model)
            {
                __instance.extra_mass = 7.5f;
                __instance.mass = 2f;
                __instance.speed = 716f;
                __instance.diameter = 0.0054f;
            }
        }
        [HarmonyPatch(typeof(ReceiverCoreScript), "Awake")]
        [HarmonyPostfix]
        private static void patchCoreAwake(ref ReceiverCoreScript __instance, ref GameObject[] ___gun_prefabs_all, ref List<MagazineScript> ___magazine_prefabs_all)
        {
            GameObject FN57 = null;
            MagazineScript FN57_mag_std = null;

            try
            {
                FN57 = ___gun_prefabs_all.First(go => (int)go.GetComponent<GunScript>().gun_model == gun_model);

                FN57_mag_std = ___magazine_prefabs_all.First(ms => (int)ms.gun_model == gun_model);
            }
            catch (Exception e)
            {
                Debug.LogError("Couldn't load gun \"FN-57\"");
                Debug.Log(e.StackTrace);
                return;
            }
            Debug.Log(FN57 == null);

            FN57.GetComponent<GunScript>().pooled_muzzle_flash = ___gun_prefabs_all.First(go => go.GetComponent<GunScript>().gun_model == GunModel.Model10).GetComponent<GunScript>().pooled_muzzle_flash;

            Debug.Log("FN57 loaded");

            __instance.generic_prefabs = new List<InventoryItem>(__instance.generic_prefabs) {
                FN57.GetComponent<GunScript>(),
                FN57_mag_std,
                FN57_mag_std.round_prefab.GetComponent<ShellCasingScript>()
            }.ToArray();

            LocaleTactics lt = new LocaleTactics();
            __instance.PlayerData.unlocked_gun_names.Add("Ciarencew.FN57");
            lt.title = "FN Five-seveN";
            lt.gun_internal_name = "CiarencewFN57";
            lt.text = "A modded pistol that goes pew pew got damnnn\n" + 
                "The Five-seveN is a pistol manufactured by FN Herstal Belgium, firing a special 5.7x28mm cartridge, originally intended to replace the 9x19mm Parabellum cartridge, thanks to its superior penetration of body armor. To safely holster the Five-seveN, switch on the safety, or remove the magazine.";

            Locale.active_locale_tactics.Add("Ciarencew.FN57", lt);
        }
        [HarmonyPatch(typeof(GunScript), "Awake")]
        [HarmonyPostfix]
        public static void PatchGunAwake(ref GunScript __instance, ref float ___hammer_cocked_val)
        {
            if ((int)__instance.gun_model != gun_model) return;

            ___hammer_cocked_val = 0.9f;

            foreach (var spring in __instance.update_springs)
            {
                spring.spring.orig_center = (spring.spring.transform.InverseTransformPoint(spring.spring.new_top.position) + spring.spring.transform.InverseTransformPoint(spring.spring.new_bottom.position)) / 2;
                spring.spring.orig_dist = Vector3.Distance(spring.spring.new_top.position, spring.spring.new_bottom.position);
            }

        }
        [HarmonyPatch(typeof(GunScript), "Update")]
        [HarmonyPostfix]
        private static void PatchGunUpdate(ref GunScript __instance, ref int ___hammer_state, ref bool ___disconnector_needs_reset, ref float ___hammer_cocked_val)
        {
            if ((int)__instance.gun_model != gun_model || Time.timeScale == 0 || !__instance.enabled || __instance.GetHoldingPlayer() == null || LocalAimHandler.player_instance.hands[1].state != LocalAimHandler.Hand.State.HoldingGun) return;

            setSoundEvents(ref __instance, use_custom_sounds.Value ? customEvents : defaultEvents);
            if (__instance.IsSafetyOn())
            { // Safety blocks the trigger from moving
                __instance.trigger.amount = Mathf.Min(__instance.trigger.amount, 0.1f);
                __instance.trigger.UpdateDisplay();
            }
                __instance.hammer.asleep = false;
            LocalAimHandler lah = LocalAimHandler.player_instance;
            float amount = __instance.hammer.amount;
	        if (__instance.slide.amount > 0.2f) //makes the hammer go to its max value
	        {
		        __instance.hammer.amount = 1f;
	        }
            if (__instance.slide.amount == 0f && __instance.trigger.amount == 0f) //makes it so you have to unpress the trigger to be able to shoot again I think actually I don't know really but it seems like what it is
            {
                ___disconnector_needs_reset = false;
            }
            if (__instance.trigger.amount == 1f && __instance.hammer.amount == ___hammer_cocked_val && !___disconnector_needs_reset && !__instance.IsSafetyOn() && __instance.magazine_instance_in_gun) //hammer firing logic
            {
                if (__instance.slide.amount == 0f)
                {
                    __instance.hammer.target_amount = 0f;
                    __instance.hammer.vel = -0.1f * ReceiverCoreScript.Instance().player_stats.animation_speed;
                }
                ___disconnector_needs_reset = true;
            }
            float vel = __instance.hammer.vel;
            if (__instance.hammer.amount > ___hammer_cocked_val) //cocking logic
            {
                ___hammer_state = 2;
                __instance.hammer.target_amount = ___hammer_cocked_val;
                __instance.hammer.vel = -0.1f * ReceiverCoreScript.Instance().player_stats.animation_speed;
            }
        __instance.hammer.TimeStep(Time.deltaTime);
            if (__instance.hammer.amount == 0f && ___hammer_state == 2 && vel < 0) //shooting logic
            {
                tryFireBullet.Invoke(__instance, new object[] { 0.5f });
                ___hammer_state = 0;
            }
        __instance.trigger.UpdateDisplay();
        __instance.safety.UpdateDisplay();
        __instance.ApplyTransform("trigger_bar", __instance.trigger.amount, __instance.transform.Find("trigger_bar"));
        __instance.ApplyTransform("barrel", __instance.slide.amount, __instance.transform.Find("barrel"));
        __instance.ApplyTransform("Cam", __instance.slide.amount, __instance.transform.Find("Cam"));
        }
    }
}
