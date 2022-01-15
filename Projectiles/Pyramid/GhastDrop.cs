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
			DisplayName.SetDefault("Ghast Drop");
		}
        public override void SetDefaults()
        {
			projectile.height = 14;
			projectile.width = 14;
			projectile.friendly = false;
			projectile.timeLeft = 360;
			projectile.hostile = true;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.netImportant = true;
			projectile.hide = true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = type == -1 ? GetTexture("SOTS/Projectiles/Pyramid/GhastDropFire") : type == -3 ? Main.projectileTexture[projectile.type] :  GetTexture("SOTS/Projectiles/Pyramid/GhastDropIchor"); 
			Vector2 origin = new Vector2(texture.Width / 2, projectile.height / 2);
			Color color = Color.Black;
			for (int i = 0; i < 360; i += 45)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(1.0f, 2.0f), 0).RotatedBy(MathHelper.ToRadians(i));
				color = type == -1 ? new Color(100, 120, 100, 0) : type == -3 ? new Color(137, 47, 137, 0) : new Color(110, 110, 100, 0);
				Main.spriteBatch.Draw(texture, projectile.Center + circular - Main.screenPosition, null, color * ((255f - projectile.alpha) / 255f), projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			}
			texture = Main.projectileTexture[projectile.type];
			color = new Color(148, 120, 168);
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, color * ((255f - projectile.alpha) / 255f), projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
			drawCacheProjsBehindNPCs.Add(index);
        }
		int counter = 0;
		bool runOnce = true;
		int type = 0;
        public override bool PreAI()
		{
			if(runOnce)
            {
				type = (int)projectile.ai[0];
				projectile.ai[0] += Main.rand.Next(30);
				runOnce = false;
            }
			if (projectile.timeLeft < 44)
			{
				projectile.alpha += 6;
			}
			else if (projectile.alpha > 0)
				projectile.alpha -= 10;
			else
			{
				projectile.alpha = 0;
			}
			if (projectile.ai[1] >= 0 && counter < 61)
			{
				NPC ghast = Main.npc[(int)projectile.ai[1]];
				if (ghast.active && ghast.type == NPCType<FlamingGhast>())
				{
					projectile.ai[0]++;
					Player player = Main.player[ghast.target];
					Vector2 rotate = new Vector2(24 + counter * 0.3f, 0).RotatedBy(MathHelper.ToRadians(projectile.ai[0]));
					Vector2 rotateExtra = new Vector2(0, 18).RotatedBy(MathHelper.ToRadians(counter * 3.5f));
					Vector2 toPlayer = player.Center - ghast.Center;
					projectile.Center = rotate + (toPlayer.SafeNormalize(Vector2.Zero) * rotateExtra.X) + ghast.Center;
					if (projectile.timeLeft <= 300)
					{
						counter++;
					}
					if (counter >= 60)
					{
						projectile.velocity += toPlayer.SafeNormalize(Vector2.Zero) * 3f + 0.5f *  new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1));
					}
				}
				else
                {
					projectile.ai[1] = -1;
                }
			}
			else
			{
				return true;
			}
			return false;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
			if(type != -3)
			{
				VoidPlayer.VoidBurn(mod, target, 10, 120);
				target.AddBuff(type == -1 ? BuffID.CursedInferno : BuffID.Ichor, 240);
			}
			else
			{
				VoidPlayer.VoidBurn(mod, target, 4, 60);
			}
        }
        public override void AI()
		{
			float modifier = 0.035f * (float)Math.Sin(MathHelper.ToRadians(projectile.ai[0] * 6));
			projectile.ai[0] += 0.9f;
			projectile.rotation += projectile.velocity.X * 0.025f;
			projectile.velocity.X *= 0.9975f;
			projectile.velocity.Y += 0.1f * (0.05f + modifier);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.velocity *= 0.15f;
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}
	}
}
		