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
using SOTS.NPCs;
using SOTS.Dusts;

namespace SOTS.Projectiles.Pyramid
{    
    public class RubySpawner : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ruby Spawner");
		}
        public override void SetDefaults()
        {
			Projectile.height = 60;
			Projectile.width = 60;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 480;
			Projectile.tileCollide = false;
			Projectile.hide = true;
		}
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			behindNPCs.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			SpriteEffects effects1 = SpriteEffects.None;
			Texture2D texture1 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Projectiles/Pyramid/RubyBackground").Value;
			Vector2 origin = new Vector2(texture1.Width/2, texture1.Height/2);
			Color alpha = Color.White;
			Color color1 = alpha * 0.8f;
			color1.A /= 2;
			Color color2 = Color.Lerp(alpha, Color.Black, 0.5f);
			color2.A = alpha.A;
			float num1 =  0.95f + (Projectile.rotation * 0.75f).ToRotationVector2().Y * 0.1f;
			Color color4 = color2 * num1;
			float scale = 0.4f + Projectile.scale * 0.8f * num1;
			Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition, null, color4, -Projectile.rotation + 0.35f, origin, scale, effects1 ^ SpriteEffects.FlipHorizontally, 0.0f);
			Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition, null, alpha, -Projectile.rotation, origin, Projectile.scale, effects1 ^ SpriteEffects.FlipHorizontally, 0.0f);
			Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition, null, alpha * 0.8f, Projectile.rotation * 0.5f, origin, Projectile.scale * 0.9f, effects1, 0.0f);
			color1.A = 0;
			for (int i = 0; i < 2; i++)
			{
				Main.spriteBatch.Draw(texture1, Projectile.Center - Main.screenPosition, null, color1, -Projectile.rotation * 0.7f * (i * 2 - 1), origin, Projectile.scale, effects1 ^ SpriteEffects.FlipHorizontally, 0.0f);
			}
			return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
		bool runOnce = true;
		public void dustRing(int direction = 1)
		{
			for (int i = -1; i <= 1; i += 2)
			{
				if(Main.rand.NextBool(2))
				{
					Vector2 circular = new Vector2(Projectile.width * 0.6f * Projectile.scale, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
					int num2 = Dust.NewDust(Projectile.Center + circular - new Vector2(4, 4), 0, 0, ModContent.DustType<Dusts.CopyDust4>());
					Dust dust = Main.dust[num2];
					dust.color = new Color(127, 80, 80, 40);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 0.15f;
					dust.scale += 1.05f * Projectile.scale;
					dust.alpha = Projectile.alpha;
					dust.velocity *= 0.2f;
					dust.velocity += circular.RotatedBy(MathHelper.ToRadians(80 * direction)).SafeNormalize(Vector2.Zero) * (1.3f + Projectile.scale * 0.7f);
					circular = new Vector2(Projectile.width * 0.2f * Projectile.scale, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
					num2 = Dust.NewDust(Projectile.Center + circular - new Vector2(4, 4), 0, 0, ModContent.DustType<Dusts.CopyDust4>());
					dust = Main.dust[num2];
					dust.color = new Color(255, 164, 164, 40);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 0.1f;
					dust.scale += 0.65f * Projectile.scale;
					dust.alpha = Projectile.alpha;
					dust.velocity *= 0.2f;
					dust.velocity += circular.RotatedBy(MathHelper.ToRadians(80 * -direction)).SafeNormalize(Vector2.Zero) * (1.0f + Projectile.scale * 0.5f);
				}
			}
		}
		public void SpawnEnemy()
        {
			int rand = Main.rand.Next(5);
			int type = ModContent.NPCType<Teratoma>();
			if(rand == 1 || rand == 2)
			{
				type = ModContent.NPCType<Ghast>();
				if(Main.hardMode && !Main.rand.NextBool(3))
                {
					if(Main.rand.NextBool(2))
					{
						type = ModContent.NPCType<BleedingGhast>();
					}
					else
						type = ModContent.NPCType<FlamingGhast>();
				}
			}
			else if(rand == 0)
            {
				type = ModContent.NPCType<Maligmor>();
			}
			NPC newNPC = Main.npc[NPC.NewNPC(Projectile.GetSource_FromThis(), (int)Projectile.Center.X, (int)Projectile.Center.Y, type)];
			newNPC.position.Y += newNPC.height * 0.5f;
			newNPC.netUpdate = true;
        }
        public override void AI()
		{
			if(runOnce)
            {
				runOnce = false;
				Projectile.scale = 0.0f;
            }
			Player player = Main.player[Projectile.owner];
			Projectile.ai[0]++;
			if(Projectile.ai[0] <= 50)
			{
				Projectile.rotation -= MathHelper.ToRadians(6);
				Projectile.scale += 0.0175f;
				dustRing(1);
			}
			else if (Projectile.ai[0] <= 90)
			{
				Projectile.rotation -= MathHelper.ToRadians(2);
				if (Main.rand.NextBool(4))
					dustRing(1);
				if (Projectile.ai[0] == 70)
				{
					for (int i = 0; i < 50; i++)
					{
						int num2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
						Dust dust = Main.dust[num2];
						dust.color = new Color(127, 80, 80, 40);
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 1.25f;
						dust.alpha = Projectile.alpha;
						dust.velocity.X *= 2.25f;
						dust.velocity.Y *= 1.55f;
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
						SpawnEnemy();
				}
			}
			else
			{
				Projectile.rotation += MathHelper.ToRadians(10);
				Projectile.scale -= 0.027f;
				if(Projectile.scale <= 0)
                {
					Projectile.Kill();
				}
				dustRing(-1);
			}
		}
		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int num2 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num2];
				dust.color = new Color(127, 80, 80, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.25f;
				dust.alpha = Projectile.alpha;
				dust.velocity.X *= 1.25f;
				dust.velocity.Y *= 0.75f;
			}
		}
	}
}
		