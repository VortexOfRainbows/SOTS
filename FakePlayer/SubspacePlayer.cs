using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Celestial;
using SOTS.Projectiles.Celestial;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SOTS.FakePlayer
{
    public class SubspacePlayer : ModPlayer
    {
        public override void SetStaticDefaults()
        {
        }
        public bool foundItem = false;
        public bool servantActive = false;
        public bool servantIsVanity = false;
        public static SubspacePlayer ModPlayer(Player player)
        {
            return player.GetModPlayer<SubspacePlayer>();
        }
        public override void PostUpdate()
        {
            if(servantActive)
            {
                Summon();
            }
        }
        public int subspaceServantShader = 0;
        public override void ResetEffects()
        {
            subspaceServantShader = 0;
            servantIsVanity = false;
            servantActive = false;
            foundItem = false;
        }
        public int Probe = -1;
        public void Summon()
        {
            int type = ModContent.ProjectileType<SubspaceServant>();
            SOTSPlayer.ModPlayer(Player).runPets(ref Probe, type, 0, 0, false);
        }
    }
}