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
using Terraria.ID;

namespace SOTS.Projectiles.Celestial
{    
    public class CellBlast : ModProjectile
	{
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/CrossLaserIndicator");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 origin2 = new Vector2(texture2.Width / 2, texture2.Height / 2);
			Color color = Color.Black;
			color = new Color(100, 255, 100, 0);
			if (Projectile.ai[1] > 0)
			{
				Vector2 velo = Projectile.velocity.SafeNormalize(Vector2.Zero);
				float scale = 1 - (Projectile.ai[1] / 120f);
				Vector2 drawPos = Projectile.Center;
				for (int j = 0; j < 100; j++)
				{
					drawPos += velo * scale * (texture2.Width + 0.5f);
					Main.spriteBatch.Draw(texture2, drawPos - Main.screenPosition, null, color * scale, velo.ToRotation(), origin2, scale, SpriteEffects.None, 0.0f);
					scale *= 0.94f;
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
				color = new Color(100, 255, 100, 0);
				Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, null, color * ((255f - Projectile.alpha) / 255f), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = new Color(50, 122, 50);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursespire");
		}
        public override void SetDefaults()
        {
			Projectile.width = 40;
			Projectile.height = 14;
			Projectile.friendly = false;
			Projectile.timeLeft = 75;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.alpha = 125;
			Projectile.hide = true;
		}
		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			overWiresUI.Add(index);
		}
		public override bool ShouldUpdatePosition()
        {
			return false;
        }
		bool runOnce = true;
        public override void AI()
		{
			if (runOnce)
			{
				Projectile.ai[1] = 120;
				runOnce = false;
			}
			Projectile.ai[1] *= 0.98f;
			Projectile.ai[1] -= 0.13f;
			if (Projectile.ai[1] < 0)
				Projectile.ai[1] = 0;
			Projectile.position += Projectile.velocity *= 1.033f;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		public override void Kill(int timeLeft)
		{
			SOTSUtils.PlaySound(Terraria.ID.SoundID.Item92, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.8f);
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<BabyLaser>(), Projectile.damage, 0, Main.myPlayer, Projectile.ai[0]);
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit) 
		{
			target.AddBuff(ModContent.BuffType<AbyssalInferno>(), 60, false);
		}
	}
}
		