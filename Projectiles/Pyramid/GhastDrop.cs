using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs;
using SOTS.Void;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.Projectiles.Pyramid
{    
    public class GhastDrop : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ghast Drop");
		}
        public override void SetDefaults()
        {
			Projectile.height = 14;
			Projectile.width = 14;
			Projectile.friendly = false;
			Projectile.timeLeft = 360;
			Projectile.hostile = true;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.hide = true;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = type == -1 ? (Texture2D)Request<Texture2D>("SOTS/Projectiles/Pyramid/GhastDropFire") : type == -3 ? Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value : (Texture2D)Request<Texture2D>("SOTS/Projectiles/Pyramid/GhastDropIchor"); 
			Vector2 origin = new Vector2(texture.Width / 2, Projectile.height / 2);
			Color color = Color.Black;
			for (int i = 0; i < 360; i += 45)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(1.0f, 2.0f), 0).RotatedBy(MathHelper.ToRadians(i));
				color = type == -1 ? new Color(100, 120, 100, 0) : type == -3 ? new Color(137, 47, 137, 0) : new Color(110, 110, 100, 0);
				Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, null, color * ((255f - Projectile.alpha) / 255f), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			}
			texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			color = new Color(148, 120, 168);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color * ((255f - Projectile.alpha) / 255f), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			behindNPCs.Add(index);
        }
		int counter = 0;
		bool runOnce = true;
		int type = 0;
        public override bool PreAI()
		{
			if(runOnce)
            {
				type = (int)Projectile.ai[0];
				Projectile.ai[0] += Main.rand.Next(30);
				runOnce = false;
            }
			if (Projectile.timeLeft < 44)
			{
				Projectile.alpha += 6;
			}
			else if (Projectile.alpha > 0)
				Projectile.alpha -= 10;
			else
			{
				Projectile.alpha = 0;
			}
			if (Projectile.ai[1] >= 0 && counter < 61)
			{
				NPC ghast = Main.npc[(int)Projectile.ai[1]];
				if (ghast.active && ghast.type == NPCType<FlamingGhast>())
				{
					Projectile.ai[0]++;
					Player player = Main.player[ghast.target];
					Vector2 rotate = new Vector2(24 + counter * 0.3f, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0]));
					Vector2 rotateExtra = new Vector2(0, 18).RotatedBy(MathHelper.ToRadians(counter * 3.5f));
					Vector2 toPlayer = player.Center - ghast.Center;
					Projectile.Center = rotate + (toPlayer.SafeNormalize(Vector2.Zero) * rotateExtra.X) + ghast.Center;
					if (Projectile.timeLeft <= 300)
					{
						counter++;
					}
					if (counter >= 60)
					{
						Projectile.velocity += toPlayer.SafeNormalize(Vector2.Zero) * 3f + 0.5f *  new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
					}
				}
				else
                {
					Projectile.ai[1] = -1;
                }
			}
			else
			{
				return true;
			}
			return false;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
			if(type != -3)
			{
				VoidPlayer.VoidBurn(Mod, target, 10, 120);
				target.AddBuff(type == -1 ? BuffID.CursedInferno : BuffID.Ichor, 240);
			}
			else
			{
				VoidPlayer.VoidBurn(Mod, target, 4, 60);
			}
        }
        public override void AI()
		{
			float modifier = 0.035f * (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[0] * 6));
			Projectile.ai[0] += 0.9f;
			Projectile.rotation += Projectile.velocity.X * 0.025f;
			Projectile.velocity.X *= 0.9975f;
			Projectile.velocity.Y += 0.1f * (0.05f + modifier);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity *= 0.15f;
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}
	}
}
		