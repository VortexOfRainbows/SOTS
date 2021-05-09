using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using SOTS.Buffs;
using SOTS.Dusts;

namespace SOTS.Projectiles.Celestial
{    
    public class GreaterCellBlast : ModProjectile
	{
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Texture2D texture2 = ModContent.GetTexture("SOTS/Projectiles/Celestial/CrossLaserIndicator");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 origin2 = new Vector2(texture2.Width / 2, texture2.Height / 2);
			Color color = new Color(255, 69, 0, 0);
			if (scaleIndicator > 0)
			{
				Vector2 velo = projectile.velocity.SafeNormalize(Vector2.Zero);
				float scale = scaleIndicator / 120f;
				Vector2 drawPos = projectile.Center;
				for (int j = 0; j < 300; j++)
				{
					drawPos += velo * scale * (texture2.Width);
					Main.spriteBatch.Draw(texture2, drawPos - Main.screenPosition, null, color * scale, velo.ToRotation(), origin2, scale, SpriteEffects.None, 0.0f);
					scale *= 0.99f;
					scale -= 0.01f;
					if (scale <= 0.05f)
					{
						break;
					}
				}
			}
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(3.5f, 5), 0).RotatedBy(MathHelper.ToRadians(i));
				color = new Color(255, 69, 0, 0);
				Main.spriteBatch.Draw(texture, projectile.Center + circular - Main.screenPosition, null, color * ((255f - projectile.alpha) / 255f), projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = new Color(235, 35, 50);
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, color, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Firespire");
		}
        public override void SetDefaults()
        {
			projectile.width = 56;
			projectile.height = 22;
			projectile.friendly = false;
			projectile.timeLeft = 300;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.alpha = 125;
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
		bool runOnce = true;
		Vector2 offset;
		float scaleIndicator = 120f;
		public override void AI()
		{
			if (projectile.ai[1] >= 0 & projectile.timeLeft >= 40)
			{
				NPC master = Main.npc[(int)projectile.ai[1]];
				if (master.active && (master.type == ModContent.NPCType<NPCs.Boss.SubspaceEye>() || master.type == ModContent.NPCType<NPCs.Boss.SubspaceSerpentHead>()) && master.ai[3] != -1)
				{
					if (runOnce)
					{
						offset.X = projectile.Center.X - master.Center.X;
					}
					else
					{
						projectile.Center = new Vector2(master.Center.X + offset.X, projectile.Center.Y);
					}
				}
			}
			if (projectile.ai[0] > 0)
			{
				projectile.timeLeft++;
				projectile.ai[0]--;
			}
			projectile.rotation = projectile.velocity.ToRotation();
			if (runOnce)
			{
				for (int i = 0; i < 15; i++)
				{
					int dust3 = Dust.NewDust(projectile.position - new Vector2(5), projectile.width, projectile.height, ModContent.DustType<CopyDust4>());
					Dust dust4 = Main.dust[dust3];
					dust4.velocity *= 0.45f;
					dust4.velocity += projectile.velocity.SafeNormalize(Vector2.Zero) * 2f;
					dust4.color = new Color(255, 69, 0, 0);
					dust4.noGravity = true;
					dust4.fadeIn = 0.1f;
					dust4.scale *= 2.25f;
				}
				scaleIndicator = 120;
				runOnce = false;
			}
			scaleIndicator *= 0.995f;
			scaleIndicator -= 0.1f;
			if (scaleIndicator < 1f)
			{
				if (projectile.timeLeft > 40)
					projectile.timeLeft = 40;
				scaleIndicator = 1f;
			}
			if (projectile.timeLeft < 40)
			{
				projectile.position += projectile.velocity *= 1.05f;
			}
			base.AI();
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<AbyssalInferno>(), 60, false);
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 92, 0.8f);
			if (Main.netMode != 1)
			{
				Projectile.NewProjectile(projectile.Center, projectile.velocity, ModContent.ProjectileType<BossBabyLaser>(), projectile.damage, 0, Main.myPlayer, 0);
			}
		}
	}
}
		