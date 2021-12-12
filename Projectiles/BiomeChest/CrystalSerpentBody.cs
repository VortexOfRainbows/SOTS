using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using System.Collections.Generic;
using SOTS.Buffs;

namespace SOTS.Projectiles.BiomeChest
{
	public class CrystalSerpentBody : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Serpent Counter");
			Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
		}
		public sealed override void SetDefaults()
        {
            projectile.width = 50;
			projectile.height = 50;
			projectile.penetrate = -1;
            projectile.timeLeft = 120;
			projectile.minion = true;
			projectile.friendly = false;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.alpha = 255;
            projectile.hide = true;
            projectile.netImportant = true;
            projectile.minionSlots = 1f;
		}
        public bool runOnce = true;
        public sealed override void AI()
        {
            Player player = Main.player[projectile.owner];
            int ownedCounter = 0;
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI)
                {
                    if (proj.type == ModContent.ProjectileType<CrystalSerpentHead>() && proj.whoAmI != projectile.whoAmI)
                        ownedCounter++;
                }
            }
            if (ownedCounter != 1)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    Projectile.NewProjectile(projectile.Center, projectile.velocity, ModContent.ProjectileType<CrystalSerpentHead>(), projectile.damage, projectile.knockBack, Main.myPlayer);
                }
            }
            projectile.Center = player.Center;
            if (!player.active && ownedCounter != 1)
            {
                projectile.active = false;
            }
            if (player.HasBuff(ModContent.BuffType<StarlightSerpent>()))
            {
                projectile.timeLeft = 2;
            }
            else if(Main.myPlayer == projectile.owner)
                projectile.Kill();
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}