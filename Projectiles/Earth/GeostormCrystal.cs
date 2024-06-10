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
using SOTS.Void;
using SOTS.Dusts;
using SOTS.Prim.Trails;

namespace SOTS.Projectiles.Earth
{    
    public class GeostormCrystal : ModProjectile
	{
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.tileCollide);
			writer.Write(Projectile.friendly);
			writer.Write(endHow);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.tileCollide = reader.ReadBoolean();
			Projectile.friendly = reader.ReadBoolean();
			endHow = reader.ReadInt32();
		}
		int count = 0;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Geostorm Crystal");
			Main.projFrames[Projectile.type] = 8;
		}
        public override void SetDefaults()
        {
			Projectile.aiStyle = 0;
			Projectile.width = 30;
			Projectile.height = 38;
			Projectile.penetrate = 3;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.timeLeft = 360;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 100;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			int width = 36;
			hitbox = new Rectangle((int)(Projectile.Center.X - width), (int)(Projectile.Center.Y - width), width * 2, width * 2);
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 24;
			height = 24;
			return true;
		}
		int endHow = 0;
		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.ai[0] == -1)
				return false;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, Projectile.height / 2);
			Color color;
			Rectangle frame = new Rectangle(0, Projectile.height * Projectile.frame, texture.Width, Projectile.height);
			for (int i = 0; i < 360; i += 60)
			{
				Vector2 circular = new Vector2(windup, 0).RotatedBy(MathHelper.ToRadians(i + windup * 3f));
				color = ColorHelpers.VibrantColorAttempt(i);
				Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, frame, color * ((255f - Projectile.alpha) / 255f) * (0.4f + 0.5f * windup / maxWindup), Projectile.rotation, origin, Projectile.scale * 1.1f, SpriteEffects.FlipVertically, 0.0f);
			}
			color = Color.White * ((255f - Projectile.alpha) / 255f);
			Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/GeostormCrystalGreen"), Projectile.Center - Main.screenPosition, frame, color, Projectile.rotation, origin, Projectile.scale * 1.2f, SpriteEffects.FlipVertically, 0.0f);
			return false;
		}
		public const float maxWindup = 22f;
		float windup = 22f;
		int spawnedNum = 0;
		int spawnDirection = -1;
		int postCounter = 0;
        public override bool PreAI()
        {
			Projectile.frame = (int)Projectile.ai[0];
            return true;
        }
        public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.3f / 255f, (255 - Projectile.alpha) * 0.1f / 255f, (255 - Projectile.alpha) * 0.9f / 255f);
			if(Projectile.ai[0] == -1)
			{
				if (Projectile.timeLeft > 30)
				{
					if (Main.rand.NextBool(2))
						spawnDirection = 1;
					Projectile.rotation = (float)Math.PI;
					DoDust(Vector2.Zero);
					Projectile.timeLeft = 30;
				}
				count++;
				if (count % 6 == 0) //will activate 5 times
				{
					float radians = MathHelper.ToRadians((spawnedNum - 2) * 21 * spawnDirection);
					Vector2 stormPos = Projectile.Center - new Vector2(0, 88).RotatedBy(radians); 
					SOTSUtils.PlaySound(SoundID.Item30, (int)stormPos.X, (int)stormPos.Y, 0.75f, -0.2f);
					if (Main.myPlayer == Projectile.owner)
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), stormPos, Vector2.Zero, Projectile.type, Projectile.damage, Projectile.knockBack, player.whoAmI, Main.rand.Next(8), radians);
					spawnedNum++;
				}
			}
			else
			{
				if (Projectile.timeLeft > 120)
                {
					if (Main.netMode != NetmodeID.Server)
						SOTS.primitives.CreateTrail(new StarTrail(Projectile, ColorHelpers.VibrantColorAttempt(Projectile.ai[1]) * 0.5f, ColorHelpers.VibrantColorAttempt(Projectile.ai[1]) * 0.5f, 12, 8));
					Projectile.timeLeft = 120;
				}
				count += 5;
				if (windup > 0)
					windup--;
				Projectile.alpha -= 11;
				if (Projectile.alpha < 0)
					Projectile.alpha = 0;
				Vector2 velocity = new Vector2(0, 16).RotatedBy(Projectile.ai[1]);
				velocity.Y += 4f;
				Projectile.rotation = velocity.ToRotation() - MathHelper.PiOver2;
				Projectile.velocity = velocity * -(float)Math.Cos(MathHelper.ToRadians(count));
				if (count > 180)
				{
					if (postCounter > 6)
						Projectile.tileCollide = true;
					postCounter++;
					Projectile.friendly = true;
					count = 180;
				}
				else
				{
					float reverseMult = 1 - count / 165f;
					reverseMult = MathHelper.Clamp(reverseMult, 0, 1);
					if(count == 170)
					{
						SOTSUtils.PlaySound(SoundID.Item72, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.75f, -0.3f);
						DoDust(velocity * -0.3f);
					}
					Projectile.velocity *= (float)Math.Sqrt(count / 180f) * reverseMult;
				}
			}
		}
		public void DoDust(Vector2 outwards)
		{
			Vector2 startingLocation;
			float degrees = MathHelper.ToDegrees(Projectile.ai[1]);
			for (int j = 0; j < 8; j++)
			{
				Vector2 offset = new Vector2(0, 8).RotatedBy(MathHelper.ToRadians(j * 90) + Projectile.rotation);
				for (int i = -4; i < 4; i++)
				{
					degrees += 360f / 40f;
					startingLocation = new Vector2(i, 8 - Math.Abs(i) * 2).RotatedBy(MathHelper.ToRadians(j * 45) + Projectile.rotation);
					Vector2 velo = offset + startingLocation;
					Dust dust = Dust.NewDustPerfect(Projectile.Center + velo * 0.4f, ModContent.DustType<CopyDust4>());
					dust.noGravity = true;
					dust.velocity *= 0.1f;
					dust.scale = 1.5f + Main.rand.NextFloat(-0.1f, 0.1f);
					dust.fadeIn = 0.1f;
					dust.alpha = 100;
					dust.color = ColorHelpers.VibrantColorAttempt(degrees);
					dust.velocity += velo * 0.15f + outwards;
				}
			}
		}
        public override void OnKill(int timeLeft)
        {
			if(Projectile.ai[0] != -1)
            {
				for(int i = 0; i < 8; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
					dust.noGravity = true;
					dust.velocity *= 0.6f;
					dust.scale = 1.8f + Main.rand.NextFloat(-0.1f, 0.1f);
					dust.fadeIn = 0.1f;
					dust.alpha = 100;
					dust.color = ColorHelpers.VibrantColorAttempt(Main.rand.NextFloat(360));
					dust.velocity += Projectile.oldVelocity * 0.3f;
				}
				for (int i = 0; i < 16; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<VibrantDust>());
					dust.noGravity = true;
					dust.velocity *= 1f;
					dust.scale = 2f + Main.rand.NextFloat(-0.2f, 0.2f);
					dust.velocity += Projectile.oldVelocity * 0.2f;
				}
			}
        }
    }
}
	