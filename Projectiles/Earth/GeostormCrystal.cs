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
			writer.Write(projectile.tileCollide);
			writer.Write(projectile.friendly);
			writer.Write(endHow);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.tileCollide = reader.ReadBoolean();
			projectile.friendly = reader.ReadBoolean();
			endHow = reader.ReadInt32();
		}
		int count = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geostorm Crystal");
			Main.projFrames[projectile.type] = 8;
		}
        public override void SetDefaults()
        {
			projectile.aiStyle = 0;
			projectile.width = 30;
			projectile.height = 38;
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.timeLeft = 360;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.alpha = 255;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 100;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			int width = 36;
			hitbox = new Rectangle((int)(projectile.Center.X - width), (int)(projectile.Center.Y - width), width * 2, width * 2);
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 24;
			height = 24;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough);
		}
		int endHow = 0;
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (projectile.ai[0] == -1)
				return false;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width / 2, projectile.height / 2);
			Color color;
			Rectangle frame = new Rectangle(0, projectile.height * projectile.frame, texture.Width, projectile.height);
			for (int i = 0; i < 360; i += 60)
			{
				Vector2 circular = new Vector2(windup, 0).RotatedBy(MathHelper.ToRadians(i + windup * 3f));
				color = VoidPlayer.VibrantColorAttempt(i);
				Main.spriteBatch.Draw(texture, projectile.Center + circular - Main.screenPosition, frame, color * ((255f - projectile.alpha) / 255f) * (0.4f + 0.5f * windup / maxWindup), projectile.rotation, origin, projectile.scale * 1.1f, SpriteEffects.FlipVertically, 0.0f);
			}
			color = Color.White * ((255f - projectile.alpha) / 255f);
			Main.spriteBatch.Draw(ModContent.GetTexture("SOTS/Projectiles/Earth/GeostormCrystalGreen"), projectile.Center - Main.screenPosition, frame, color, projectile.rotation, origin, projectile.scale * 1.2f, SpriteEffects.FlipVertically, 0.0f);
			return false;
		}
		public const float maxWindup = 22f;
		float windup = 22f;
		int spawnedNum = 0;
		int spawnDirection = -1;
		int postCounter = 0;
        public override bool PreAI()
        {
			projectile.frame = (int)projectile.ai[0];
            return true;
        }
        public override void AI()
		{
			Player player = Main.player[projectile.owner];
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.3f / 255f, (255 - projectile.alpha) * 0.1f / 255f, (255 - projectile.alpha) * 0.9f / 255f);
			if(projectile.ai[0] == -1)
			{
				if (projectile.timeLeft > 30)
				{
					if (Main.rand.NextBool(2))
						spawnDirection = 1;
					projectile.rotation = (float)Math.PI;
					DoDust(Vector2.Zero);
					projectile.timeLeft = 30;
				}
				count++;
				if (count % 6 == 0) //will activate 5 times
				{
					float radians = MathHelper.ToRadians((spawnedNum - 2) * 21 * spawnDirection);
					Vector2 stormPos = projectile.Center - new Vector2(0, 88).RotatedBy(radians); 
					Main.PlaySound(SoundID.Item, (int)stormPos.X, (int)stormPos.Y, 30, 0.75f, -0.2f);
					if (Main.myPlayer == projectile.owner)
						Projectile.NewProjectile(stormPos, Vector2.Zero, projectile.type, projectile.damage, projectile.knockBack, player.whoAmI, Main.rand.Next(8), radians);
					spawnedNum++;
				}
			}
			else
			{
				if (projectile.timeLeft > 120)
                {
					if (Main.netMode != NetmodeID.Server)
						SOTS.primitives.CreateTrail(new StarTrail(projectile, VoidPlayer.VibrantColorAttempt(projectile.ai[1]) * 0.5f, VoidPlayer.VibrantColorAttempt(projectile.ai[1]) * 0.5f, 12, 8));
					projectile.timeLeft = 120;
				}
				count += 5;
				if (windup > 0)
					windup--;
				projectile.alpha -= 11;
				if (projectile.alpha < 0)
					projectile.alpha = 0;
				Vector2 velocity = new Vector2(0, 16).RotatedBy(projectile.ai[1]);
				velocity.Y += 4f;
				projectile.rotation = velocity.ToRotation() - MathHelper.PiOver2;
				projectile.velocity = velocity * -(float)Math.Cos(MathHelper.ToRadians(count));
				if (count > 180)
				{
					if (postCounter > 6)
						projectile.tileCollide = true;
					postCounter++;
					projectile.friendly = true;
					count = 180;
				}
				else
				{
					float reverseMult = 1 - count / 165f;
					reverseMult = MathHelper.Clamp(reverseMult, 0, 1);
					if(count == 170)
					{
						Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 72, 0.75f, -0.3f);
						DoDust(velocity * -0.3f);
					}
					projectile.velocity *= (float)Math.Sqrt(count / 180f) * reverseMult;
				}
			}
		}
		public void DoDust(Vector2 outwards)
		{
			Vector2 startingLocation;
			float degrees = MathHelper.ToDegrees(projectile.ai[1]);
			for (int j = 0; j < 8; j++)
			{
				Vector2 offset = new Vector2(0, 8).RotatedBy(MathHelper.ToRadians(j * 90) + projectile.rotation);
				for (int i = -4; i < 4; i++)
				{
					degrees += 360f / 40f;
					startingLocation = new Vector2(i, 8 - Math.Abs(i) * 2).RotatedBy(MathHelper.ToRadians(j * 45) + projectile.rotation);
					Vector2 velo = offset + startingLocation;
					Dust dust = Dust.NewDustPerfect(projectile.Center + velo * 0.4f, ModContent.DustType<CopyDust4>());
					dust.noGravity = true;
					dust.velocity *= 0.1f;
					dust.scale = 1.5f + Main.rand.NextFloat(-0.1f, 0.1f);
					dust.fadeIn = 0.1f;
					dust.alpha = 100;
					dust.color = VoidPlayer.VibrantColorAttempt(degrees);
					dust.velocity += velo * 0.15f + outwards;
				}
			}
		}
        public override void Kill(int timeLeft)
        {
			if(projectile.ai[0] != -1)
            {
				for(int i = 0; i < 8; i++)
				{
					Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<CopyDust4>());
					dust.noGravity = true;
					dust.velocity *= 0.6f;
					dust.scale = 1.8f + Main.rand.NextFloat(-0.1f, 0.1f);
					dust.fadeIn = 0.1f;
					dust.alpha = 100;
					dust.color = VoidPlayer.VibrantColorAttempt(Main.rand.NextFloat(360));
					dust.velocity += projectile.oldVelocity * 0.3f;
				}
				for (int i = 0; i < 16; i++)
				{
					Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<VibrantDust>());
					dust.noGravity = true;
					dust.velocity *= 1f;
					dust.scale = 2f + Main.rand.NextFloat(-0.2f, 0.2f);
					dust.velocity += projectile.oldVelocity * 0.2f;
				}
			}
        }
    }
}
	