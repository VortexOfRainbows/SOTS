using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;
using SOTS.NPCs.ArtificialDebuffs;
using System.IO;

namespace SOTS.Projectiles.BiomeChest
{    
    public class Sawflake : ModProjectile 
    {
		float rotation = 0;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.tileCollide);
			writer.Write(projectile.velocity.X);
			writer.Write(projectile.velocity.Y);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.tileCollide = reader.ReadBoolean();
			projectile.velocity.X = reader.ReadSingle();
			projectile.velocity.Y = reader.ReadSingle();
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sawflake");
		}
        public override void SetDefaults()
        {
			projectile.height = 50;
			projectile.width = 50;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.timeLeft = 144;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.alpha = 0;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 30;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Main.PlaySound(SoundID.Dig, projectile.Center);
			projectile.timeLeft = 89;
			projectile.netUpdate = true;
			projectile.tileCollide = false;
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 32;
			height = 32;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			hitDirection = initialDirection;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
			if(Main.rand.NextBool(2))
            {
				target.AddBuff(BuffID.Frostburn, 360);
            }
			else
			{
				target.AddBuff(BuffID.Frostburn, 720);
			}
		}
		bool runOnce = true;
		Vector2[] trailPos = new Vector2[14];
		Vector2[] trailPos2 = new Vector2[14];
		Vector2[] trailPos3 = new Vector2[14];
		Vector2[] trailPos4 = new Vector2[14];
		public void cataloguePos(Vector2 catalogue, Vector2[] trialArray, float rotation)
		{
			Vector2 current = catalogue;
			Vector2 velo = new Vector2(7.5f, 0);
			velo = velo.RotatedBy(rotation);
			velo += projectile.velocity;
			if(projectile.velocity.Length() == 0)
            {
				velo *= 0;
				current = projectile.Center;
			}
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trialArray[i];
				trialArray[i] = current;
				current = previousPosition;
				if (trialArray[i] != Vector2.Zero)
					trialArray[i] += velo * (trialArray.Length - i) / (float)trialArray.Length;
			}
		}
		public void checkPos()
		{
			float iterator = 0f;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				if (previousPosition == projectile.Center)
				{
					iterator++;
				}
			}
			if (iterator >= trailPos.Length)
				projectile.Kill();
		}
		Vector2 initialVelo;
		Vector2 initialCenter;
		int initialDirection = 0;
        public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			if (runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
				initialVelo = projectile.velocity;
				if(projectile.velocity.X > 0)
					initialDirection = 1;
				else
					initialDirection = -1;
				initialCenter = player.Center;
				projectile.ai[0] = -180 * initialDirection;
			}
			projectile.ai[0] += 6f * initialDirection;
			if (projectile.timeLeft >= 90)
			{
				Vector2 initialCenter = this.initialCenter;
				int length = 270;
				double rad = MathHelper.ToRadians(projectile.ai[0]);
				Vector2 ovalArea = new Vector2(length, 0).RotatedBy(initialVelo.ToRotation());
				Vector2 ovalArea2 = new Vector2(length, 0).RotatedBy((float)rad);
				ovalArea2.Y *= 0.33f;
				ovalArea2 = ovalArea2.RotatedBy(initialVelo.ToRotation());
				ovalArea.X += ovalArea2.X;
				ovalArea.Y += ovalArea2.Y;
				Vector2 goTo = initialCenter + ovalArea;
				float dist = (projectile.Center - goTo).Length();
				Vector2 circular = new Vector2(-(dist > 18 ? 18 : dist), 0).RotatedBy((projectile.Center - goTo).ToRotation());
				projectile.velocity = circular;
				if (Main.myPlayer == projectile.owner && projectile.timeLeft % 5 == 0)
				{
					projectile.netUpdate = true;
				}
			}
			else if(projectile.timeLeft > 30)
            {
				float dist = (projectile.Center - player.Center).Length();
				Vector2 circular = new Vector2(-(dist > 24 ? 24 : dist), 0).RotatedBy((projectile.Center - player.Center).ToRotation());
				projectile.velocity = circular;
				projectile.tileCollide = false;
				if((projectile.Center - player.Center).Length() <= 12)
                {
					projectile.timeLeft = 30;
                }
			}
            return base.PreAI();
        }
        public override void AI()
		{
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.6f / 255f, (255 - projectile.alpha) * 0.6f / 255f, (255 - projectile.alpha) * 1.8f / 255f);
			if(projectile.timeLeft <= 30)
            {
				checkPos();
				projectile.velocity *= 0f;
				projectile.alpha = 255;
            }
			Player player = Main.player[projectile.owner];
			Vector2 circularLocation = new Vector2(projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation));
			cataloguePos(circularLocation + projectile.Center, trailPos, MathHelper.ToRadians(rotation));

			circularLocation = new Vector2(projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation + 90));
			cataloguePos(circularLocation + projectile.Center, trailPos2, MathHelper.ToRadians(rotation + 90));

			circularLocation = new Vector2(projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation + 180));
			cataloguePos(circularLocation + projectile.Center, trailPos3, MathHelper.ToRadians(rotation + 180));

			circularLocation = new Vector2(projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation + 270));
			cataloguePos(circularLocation + projectile.Center, trailPos4, MathHelper.ToRadians(rotation + 270));

			rotation += initialDirection * 11.5f;
			projectile.rotation = rotation;
			projectile.spriteDirection = 1;
			if(projectile.timeLeft <= 30)
            {
				projectile.friendly = false;
            }
		}
        public override void Kill(int timeLeft)
		{
			base.Kill(timeLeft);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return true;
			Draw(spriteBatch, trailPos);
			Draw(spriteBatch, trailPos2);
			Draw(spriteBatch, trailPos3);
			Draw(spriteBatch, trailPos4);
			Texture2D texture2 = Main.projectileTexture[projectile.type];
			Main.spriteBatch.Draw(texture2, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(Color.White), projectile.rotation, new Vector2(texture2.Width / 2, texture2.Height / 2), projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (projectile.alpha >= 150)
			{
				return false;
			}
			Vector2[] trailArray = trailPos;
			for(int k = 0; k < 4; k++)
			{
				if(k == 0)
					trailArray = trailPos;
				if (k == 1)
					trailArray = trailPos2;
				if (k == 2)
					trailArray = trailPos3;
				if (k == 3)
					trailArray = trailPos4;
				for (int i = 0; i < trailArray.Length - 2; i++)
				{
					float scale = projectile.scale * (trailArray.Length - i) / (float)trailArray.Length;
					scale *= 1f;
					float width = 20 * scale;
					float height = 20 * scale;
					Vector2 pos = trailArray[i];
					projHitbox = new Rectangle((int)pos.X - (int)width / 2, (int)pos.Y - (int)height / 2, (int)width, (int)height);
					if (projHitbox.Intersects(targetHitbox))
					{
						return true;
					}
				}
			}
			return (bool?)null;
		}
		public void Draw(SpriteBatch spriteBatch, Vector2[] trailArray)
		{
			if (trailArray[0] == Vector2.Zero)
			{
				return;
			}
			Texture2D texture2 = mod.GetTexture("Projectiles/BiomeChest/SawflakeTrail");
			Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			Vector2 previousPosition = projectile.Center;
			Color color = new Color(140, 140, 205, 0);
			for (int k = 0; k < trailArray.Length; k++)
			{
				float scale = projectile.scale * (trailArray.Length - k) / (float)trailArray.Length;
				scale *= 1f;
				if (trailArray[k] == Vector2.Zero)
				{
					return;
				}
				Vector2 drawPos = trailArray[k] - Main.screenPosition;
				Vector2 currentPos = trailArray[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color *= 0.95f;
				float max = betweenPositions.Length() / (texture2.Width  * 0.6f * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 3; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.1f * scale;
						float y = Main.rand.Next(-10, 11) * 0.1f * scale;
						if (j <= 1)
						{
							x = 0;
							y = 0;
						}
						Main.spriteBatch.Draw(texture2, drawPos + new Vector2(x, y), null, color, projectile.rotation, drawOrigin2, scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
					}
				}
				previousPosition = currentPos;
			}
		}
	}
}
		