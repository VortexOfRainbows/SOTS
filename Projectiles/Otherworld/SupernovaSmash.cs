using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Terraria.World.Generation;
using Terraria.Enums;
using System.Diagnostics.Contracts;

namespace SOTS.Projectiles.Otherworld
{
	public class SupernovaSmash : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Supernova Hammer");
		}
		public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 200;
            projectile.alpha = (int)byte.MaxValue;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 120;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.melee = true;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if ((double)projectile.ai[0] >= 42.0)
                projectile.localAI[1] = 1f;
            if(crit)
            {
                Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 14, 0.6f);
                if (projectile.owner == Main.myPlayer)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
                        Projectile.NewProjectile(target.Center.X, target.Center.Y, circular.X, circular.Y, mod.ProjectileType("Seeker"), projectile.damage, projectile.knockBack, Main.myPlayer, Main.rand.Next(360), target.whoAmI);
                    }
                }
            }
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0)
            {
                Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 14, 0.6f);
                if (projectile.owner == Main.myPlayer)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
                        Projectile.NewProjectile(target.Center.X, target.Center.Y, circular.X, circular.Y, mod.ProjectileType("Seeker"), (int)(0.7f * projectile.damage) + 1, projectile.knockBack, Main.myPlayer, Main.rand.Next(360), target.whoAmI);
                    }
                }
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }
        public override void AI()
        {
            var tileCoordinates1 = projectile.TopLeft.ToTileCoordinates();
            var tileCoordinates2 = projectile.BottomRight.ToTileCoordinates();
            var num1 = tileCoordinates1.X / 2;
            var num2 = tileCoordinates2.X / 2;
            var width = projectile.width;
            ++projectile.ai[0];
            if ((double)projectile.ai[0] > 20.0)
            {
                projectile.Kill();
            }
            else
            {
                if ((double)projectile.ai[0] != 1.0)
                    return;
                var flag = false;

                var bottom = projectile.Bottom;
                var spinningpoint = new Vector2(7f, 0.0f);
                var vector2_1 = new Vector2(1f, 0.7f);
                var color = new Color(0, 200, 220, 100);
                for (var num4 = 0.0f; num4 < 25.0; ++num4)
                {
                    var vector2_2 =
                        spinningpoint.RotatedBy(num4 * 6.28318548202515 / 25.0, new Vector2()) * vector2_1;
                    var dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Electric, 0.0f, 0.0f, 0,
                        new Color(), 1f);
                    dust.alpha = 0;
                    if (!flag)
                        dust.alpha = 50;
                    dust.color = color;
                    dust.position = bottom + vector2_2;
                    dust.velocity.Y -= 5f;
                    dust.velocity.X *= 0.5f;
                    dust.fadeIn = 0.3f;
                    dust.scale *= 1.6f;
                }

                if (flag)
                    return;
                for (var num4 = 0.0f; num4 < 25.0; ++num4)
                {
                    var vector2_2 =
                        spinningpoint.RotatedBy(num4 * 6.28318548202515 / 25.0, new Vector2()) * vector2_1;
                    var dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Electric, 0.0f, 0.0f, 0,
                        new Color(), 1f);
                    dust.alpha = 100;
                    dust.color = color;
                    dust.position = bottom + vector2_2;
                    dust.velocity.Y -= 7f;
                    dust.velocity.X *= 0.8f;
                    dust.fadeIn = 0.3f;
                    dust.scale *= 1.6f;
                }
            }
        }
    }
}

