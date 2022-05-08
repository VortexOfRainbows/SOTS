using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SOTS.Projectiles.Otherworld
{    
    public class ArclightBomb : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Arclight Bomb");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(48);
            aiType = 48; 
			projectile.thrown = false;
			projectile.magic = false;
			projectile.melee = false;
			projectile.ranged = true;
			projectile.width = 14;
			projectile.height = 14;
			projectile.penetrate = 1;
			projectile.timeLeft = 900;
			projectile.alpha = 0;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/ArclightBomb");
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			if (modPlayer.rainbowGlowmasks)
			{
				Color color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
				for (int k = 0; k < 3; k++)
				{
					spriteBatch.Draw(texture, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
			}
			else
			{
				Color color = Color.White;
				spriteBatch.Draw(texture, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
		}
		public override void AI()
		{
			if(projectile.timeLeft >= 800)
			{
				projectile.scale = 0.01f * Main.rand.Next(102, 141);
				projectile.timeLeft = Main.rand.Next(32, 46);
			}
		}
		public override void Kill(int timeLeft)
        {
			Vector2 position = projectile.Center;
			SoundEngine.PlaySound(3, (int)projectile.Center.X, (int)projectile.Center.Y, 53, 0.625f);
			for (int i = 0; i < 13; i++)
			{
				var num371 = Dust.NewDust(projectile.Center - new Vector2(5) - new Vector2(10, 10), 24, 24, mod.DustType("CopyDust4"), 0, 0, 100, default, 1.6f);
				Dust dust = Main.dust[num371];
				dust.velocity += projectile.velocity * 0.1f;
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(160, 200, 220, 100), new Color(120, 140, 180, 100), new Vector2(-0.5f, 0).RotatedBy(Main.rand.Next(360)).X + 0.5f);
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.alpha = projectile.alpha;
			}
			if (projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < Main.rand.Next(2) + 2; i++)
				{
					Vector2 circular = new Vector2(1.65f, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
					Projectile.NewProjectile((projectile.Center.X), projectile.Center.Y, circular.X, circular.Y, mod.ProjectileType("ArcColumn"), (int)(projectile.damage * 0.8f), 0, projectile.owner, 1 + Main.rand.Next(2));
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			if (projectile.owner == Main.myPlayer)
			{
				int npcIndex = -1;
				double distanceTB = 216;
				for (int i = 0; i < 200; i++) //find first enemy
				{
					NPC npc = Main.npc[i];
					if (!npc.friendly && npc.lifeMax > 5 && npc.active && !npc.dontTakeDamage)
					{
						if (npcIndex != i && target.whoAmI != i && npc.whoAmI != (int)projectile.ai[0])
						{
							float disX = projectile.Center.X - npc.Center.X;
							float disY = projectile.Center.Y - npc.Center.Y;
							double dis = Math.Sqrt(disX * disX + disY * disY);
							if (dis < distanceTB)
							{
								distanceTB = dis;
								npcIndex = i;
							}
						}
					}
				}
				if (npcIndex != -1)
				{
					NPC npc = Main.npc[npcIndex];
					if (!npc.friendly && npc.lifeMax > 5 && npc.active && !npc.dontTakeDamage)
					{
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("ArcLightningZap"), (int)(projectile.damage * 0.8f) + 1, target.whoAmI, projectile.owner, npc.whoAmI, 2);
					}
				}
			}
		}
	}
}
			