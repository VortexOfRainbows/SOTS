using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Celestial;
using SOTS.Projectiles.Celestial;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Permissions;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SOTS.FakePlayer
{
    public class FakeModPlayer : ModPlayer
    {
        public override void SetStaticDefaults()
        {
        }
        public bool foundItem = false;
        public bool servantActive = false;
        public bool servantIsVanity = false;
        public bool hasHydroFakePlayer = false;
        public static FakeModPlayer ModPlayer(Player player)
        {
            return player.GetModPlayer<FakeModPlayer>();
        }
        public override void PostUpdate()
        {
            if(servantActive)
            {
                int type = ModContent.ProjectileType<SubspaceServant>();
                SOTSPlayer.ModPlayer(Player).runPets(ref Probe, type, 0, 0, false);
            }
            if (hasHydroFakePlayer)
            {
                int type = ModContent.ProjectileType<HydroServant>();
                SOTSPlayer.ModPlayer(Player).runPets(ref Probe2, type, 0, 0, false);
            }
        }
        public override bool CanUseItem(Item item)
        {
            if(hasHydroFakePlayer && FakePlayerProjectile.OwnerOfThisUpdateCycle == -1)
            {
                bool isHydroPlayerUsingAnItem = FakePlayer.CheckItemValidityFull(Player, item, item, 1);
                if (isHydroPlayerUsingAnItem)
                {
                    Player.SetDummyItemTime(2); //This is necessary to prevent the owner from switching items mid-use
                    Player.itemTimeMax--;
                    return false;
                }
            }
            return true;
        }
        public override bool? CanMeleeAttackCollideWithNPC(Item item, Rectangle meleeAttackHitbox, NPC target)
        {
            if (hasHydroFakePlayer && FakePlayerProjectile.OwnerOfThisUpdateCycle == -1)
            {
                bool isHydroPlayerUsingAnItem = FakePlayer.CheckItemValidityFull(Player, item, item, 1);
                if (isHydroPlayerUsingAnItem)
                {
                    return false;
                }
            }
            return null;
        }
        public int subspaceServantShader = 0;
        public override void ResetEffects()
        {
            subspaceServantShader = 0;
            servantIsVanity = false;
            servantActive = false;
            hasHydroFakePlayer = false;
            foundItem = false;
        }
        public int Probe = -1;
        public int Probe2 = -1;
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (FakePlayerProjectile.OwnerOfThisUpdateCycle != -1)
            {
                Projectile dummyProj = new Projectile();
                dummyProj.SetDefaults(item.shoot);
                if(dummyProj.aiStyle == ProjAIStyleID.Yoyo && Player.ownedProjectileCounts[item.shoot] > 1)
                {
                    return false;
                }
            }
            return true;
        }
    }
}