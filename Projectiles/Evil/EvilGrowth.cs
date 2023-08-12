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
			// DisplayName.SetDefault("Nightmare Arms");
		}
        public override void SetDefaults()
        {
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.timeLeft = MaxTimeLeft;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			// Projectile.ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.alpha = 0;
		}
        bool runOnce = true;
		Vector2[] trailPos = new Vector2[10];
		public void cataloguePos()
		{
			Vector2 current = Projectile.Center - new Vector2(8, -8).RotatedBy(Projectile.rotation);
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
			Player player = Main.player[Projectile.owner];
			if(runOnce2)
			{
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
				saveRotation = Projectile.rotation;
				Projectile.spriteDirection = 1;
			}
			if (Projectile.velocity.Length() <= 0.1f)
			{
				if (runOnce2)
				{
					Projectile.ai[0] = 0;
					runOnce2 = false;
					Projectile.rotation = 0;
					Projectile.scale = 0.0f;
					Projectile.netUpdate = true;
					for (int i = 0; i < 360; i += 15)
					{
						Vector2 circularLocation = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(i));
						Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>());
						dust.velocity *= 0.33f;
						dust.velocity += circularLocation;
						dust.scale *= 1.25f;
						dust.fadeIn = 0.2f;
						dust.color = new Color(ColorHelpers.EvilColor.R, ColorHelpers.EvilColor.G, ColorHelpers.EvilColor.B);
						dust.alpha = 40;
						dust.noGravity = true;
					}
					SOTSUtils.PlaySound(Terraria.ID.SoundID.Item116, (int)Projectile.Center.X, (int)Projectile.Center.Y, 2.3f, -0.5f);
				}
				if (Projectile.scale < 1)
				{
					Projectile.scale += 0.05f;
					Projectile.scale *= 1.05f;
					Projectile.rotation = saveRotation + unwind * (1 - Projectile.scale);
				}
				else
				{
					Projectile.rotation = saveRotation;
					Projectile.scale = 1;
				}
				if (stretchCounter < 120)
					stretchCounter += 5;
				else if (Projectile.ai[0] < 30)
					Projectile.ai[0] += 3;
				float bubCir = 420 * (float)Math.Sin(MathHelper.ToRadians(stretchCounter - Projectile.ai[0] + 10));
				bubbleSize = bubCir;
			}
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC target = Main.npc[i];
				if ((Projectile.Center - target.Center).Length() <= bubbleSize / 2f + 4f && !target.friendly && target.active && !target.dontTakeDamage)
				{
					if (!effected[i])
					{
						if (Main.myPlayer == Projectile.owner)
							Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<EvilStrike>(), Projectile.damage, 0, Projectile.owner, target.whoAmI, Projectile.whoAmI);
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
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 0.45f;
				dust.velocity += circularLocation;
				dust.scale *= 1.25f;
				dust.fadeIn = 0.2f;
				dust.color = new Color(ColorHelpers.EvilColor.R, ColorHelpers.EvilColor.G, ColorHelpers.EvilColor.B);
				dust.alpha = 40;
				dust.noGravity = true; 
				circularLocation = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(i));
				dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 0.45f;
				dust.velocity += circularLocation;
				dust.scale *= 1.75f;
				dust.fadeIn = 0.2f;
				dust.color = new Color(ColorHelpers.EvilColor.R, ColorHelpers.EvilColor.G, ColorHelpers.EvilColor.B);
				dust.alpha = 40;
				dust.noGravity = true;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D flower = Mod.Assets.Request<Texture2D>("Projectiles/Evil/EvilGrowth").Value;
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Gores/CircleAura").Value, Projectile.Center - Main.screenPosition, null, new Color(200, 50, 0) * 0.2f * (Projectile.timeLeft / (float)MaxTimeLeft), 0f, new Vector2(300f, 300f), bubbleSize / 600f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Gores/CircleBorder").Value, Projectile.Center - Main.screenPosition, null, new Color(150, 30, 0) * 0.5f * (Projectile.timeLeft / (float)MaxTimeLeft), 0f, new Vector2(300f, 300f), bubbleSize / 600f, SpriteEffects.None, 0f);
			Vector2 drawOrigin = new Vector2(flower.Width * 0.5f, flower.Height * 0.5f);
			Main.spriteBatch.Draw(flower, Projectile.Center - Main.screenPosition, null, new Color(ColorHelpers.EvilColor.R, ColorHelpers.EvilColor.G, ColorHelpers.EvilColor.B), Projectile.rotation, drawOrigin, Projectile.scale * 1.0f * (Projectile.timeLeft / (float)MaxTimeLeft) + 0.3f, SpriteEffects.None, 0f);
			return false;
		}
	}
}
		