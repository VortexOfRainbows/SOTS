using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;
using static SOTS.SOTS;

namespace SOTS.Projectiles.Evil
{    
    public class EvilGrowth : ModProjectile 
    {
		public const int MaxTimeLeft = 150;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightmare Arms");
		}
        public override void SetDefaults()
        {
			projectile.width = 20;
			projectile.height = 20;
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.timeLeft = MaxTimeLeft;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.magic = true;
			projectile.ranged = false;
			projectile.alpha = 0;
		}
        bool runOnce = true;
		Vector2[] trailPos = new Vector2[10];
		public void cataloguePos()
		{
			Vector2 current = projectile.Center - new Vector2(8, -8).RotatedBy(projectile.rotation);
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
		}
		public override bool PreAI()
		{
			if (runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			cataloguePos();
			return base.PreAI();
		}
		int stretchCounter = 0;
		float unwind = 720;
		bool runOnce2 = true;
		float saveRotation = 0;
		public override void AI()
        {
			Player player = Main.player[projectile.owner];
			if(runOnce2)
			{
				projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
				saveRotation = projectile.rotation;
				projectile.spriteDirection = 1;
			}
			if (projectile.velocity.Length() <= 0.1f)
			{
				if (runOnce2)
				{
					projectile.ai[0] = 0;
					runOnce2 = false;
					projectile.rotation = 0;
					projectile.scale = 0.0f;
					projectile.netUpdate = true;
					for (int i = 0; i < 360; i += 15)
					{
						Vector2 circularLocation = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(i));
						Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>());
						dust.velocity *= 0.33f;
						dust.velocity += circularLocation;
						dust.scale *= 1.25f;
						dust.fadeIn = 0.2f;
						dust.color = new Color(VoidPlayer.EvilColor.R, VoidPlayer.EvilColor.G, VoidPlayer.EvilColor.B);
						dust.alpha = 40;
						dust.noGravity = true;
					}
					Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 116, 2.3f, -0.5f);
				}
				if (projectile.scale < 1)
				{
					projectile.scale += 0.05f;
					projectile.scale *= 1.05f;
					projectile.rotation = saveRotation + unwind * (1 - projectile.scale);
				}
				else
				{
					projectile.rotation = saveRotation;
					projectile.scale = 1;
				}
				if (stretchCounter < 120)
					stretchCounter += 5;
				else if (projectile.ai[0] < 30)
					projectile.ai[0] += 3;
				float bubCir = 420 * (float)Math.Sin(MathHelper.ToRadians(stretchCounter - projectile.ai[0] + 10));
				bubbleSize = bubCir;
			}
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC target = Main.npc[i];
				if ((projectile.Center - target.Center).Length() <= bubbleSize / 2f + 4f && !target.friendly && target.active && !target.dontTakeDamage)
				{
					if (!effected[i])
					{
						if (Main.myPlayer == projectile.owner)
							Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<EvilStrike>(), projectile.damage, 0, projectile.owner, target.whoAmI, projectile.whoAmI);
						effected[i] = true;
					}
				}
				else if (target.friendly || !target.active)
				{
					effected[i] = false;
				}
			}
		}
		public bool[] effected = new bool[200];
		float bubbleSize = 0f;
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 360; i += 10)
			{
				Vector2 circularLocation = new Vector2(9, 0).RotatedBy(MathHelper.ToRadians(i));
				Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 0.45f;
				dust.velocity += circularLocation;
				dust.scale *= 1.25f;
				dust.fadeIn = 0.2f;
				dust.color = new Color(VoidPlayer.EvilColor.R, VoidPlayer.EvilColor.G, VoidPlayer.EvilColor.B);
				dust.alpha = 40;
				dust.noGravity = true; 
				circularLocation = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(i));
				dust = Dust.NewDustDirect(projectile.Center - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 0.45f;
				dust.velocity += circularLocation;
				dust.scale *= 1.75f;
				dust.fadeIn = 0.2f;
				dust.color = new Color(VoidPlayer.EvilColor.R, VoidPlayer.EvilColor.G, VoidPlayer.EvilColor.B);
				dust.alpha = 40;
				dust.noGravity = true;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D flower = mod.GetTexture("Projectiles/Evil/EvilGrowth");
			spriteBatch.Draw(mod.GetTexture("Gores/CircleAura"), projectile.Center - Main.screenPosition, null, new Color(200, 50, 0) * 0.2f * (projectile.timeLeft / (float)MaxTimeLeft), 0f, new Vector2(300f, 300f), bubbleSize / 600f, SpriteEffects.None, 0f);
			spriteBatch.Draw(mod.GetTexture("Gores/CircleBorder"), projectile.Center - Main.screenPosition, null, new Color(150, 30, 0) * 0.5f * (projectile.timeLeft / (float)MaxTimeLeft), 0f, new Vector2(300f, 300f), bubbleSize / 600f, SpriteEffects.None, 0f);
			Vector2 drawOrigin = new Vector2(flower.Width * 0.5f, flower.Height * 0.5f);
			Main.spriteBatch.Draw(flower, projectile.Center - Main.screenPosition, null, new Color(VoidPlayer.EvilColor.R, VoidPlayer.EvilColor.G, VoidPlayer.EvilColor.B), projectile.rotation, drawOrigin, projectile.scale * 1.0f * (projectile.timeLeft / (float)MaxTimeLeft) + 0.3f, SpriteEffects.None, 0f);
			return false;
		}
	}
}
		