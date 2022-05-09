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
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/CrossLaserIndicator");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 origin2 = new Vector2(texture2.Width / 2, texture2.Height / 2);
			Color color = new Color(255, 69, 0, 0);
			if (scaleIndicator > 0)
			{
				Vector2 velo = Projectile.velocity.SafeNormalize(Vector2.Zero);
				float scale = scaleIndicator / 120f;
				Vector2 drawPos = Projectile.Center;
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
				Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, null, color * ((255f - Projectile.alpha) / 255f), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = new Color(235, 35, 50);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Firespire");
		}
        public override void SetDefaults()
        {
			Projectile.width = 56;
			Projectile.height = 22;
			Projectile.friendly = false;
			Projectile.timeLeft = 300;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.alpha = 125;
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
			if (Projectile.ai[1] >= 0 & Projectile.timeLeft >= 40)
			{
				NPC master = Main.npc[(int)Projectile.ai[1]];
				if (master.active && (master.type == ModContent.NPCType<NPCs.Boss.SubspaceEye>() || master.type == ModContent.NPCType<NPCs.Boss.SubspaceSerpentHead>()) && master.ai[3] != -1)
				{
					if (runOnce)
					{
						offset.X = Projectile.Center.X - master.Center.X;
					}
					else
					{
						Projectile.Center = new Vector2(master.Center.X + offset.X, Projectile.Center.Y);
					}
				}
			}
			if (Projectile.ai[0] > 0)
			{
				Projectile.timeLeft++;
				Projectile.ai[0]--;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (runOnce)
			{
				for (int i = 0; i < 15; i++)
				{
					int dust3 = Dust.NewDust(Projectile.position - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
					Dust dust4 = Main.dust[dust3];
					dust4.velocity *= 0.45f;
					dust4.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * 2f;
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
				if (Projectile.timeLeft > 40)
					Projectile.timeLeft = 40;
				scaleIndicator = 1f;
			}
			if (Projectile.timeLeft < 40)
			{
				Projectile.position += Projectile.velocity *= 1.05f;
			}
			base.AI();
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<AbyssalInferno>(), 60, false);
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(2, (int)Projectile.Center.X, (int)Projectile.Center.Y, 92, 0.8f);
			if (Main.netMode != 1)
			{
				Projectile.NewProjectile(Projectile.Center, Projectile.velocity, ModContent.ProjectileType<BossBabyLaser>(), Projectile.damage, 0, Main.myPlayer, 0);
			}
		}
	}
}
		