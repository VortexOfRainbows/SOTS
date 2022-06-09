using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;
using SOTS.Common.GlobalNPCs;
using System.IO;

namespace SOTS.Projectiles.BiomeChest
{    
    public class Sawflake : ModProjectile 
    {
		float rotation = 0;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.tileCollide);
			writer.Write(Projectile.velocity.X);
			writer.Write(Projectile.velocity.Y);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.tileCollide = reader.ReadBoolean();
			Projectile.velocity.X = reader.ReadSingle();
			Projectile.velocity.Y = reader.ReadSingle();
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sawflake");
		}
        public override void SetDefaults()
        {
			Projectile.height = 50;
			Projectile.width = 50;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 144;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			Projectile.timeLeft = 89;
			Projectile.netUpdate = true;
			Projectile.tileCollide = false;
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 32;
			height = 32;
            return true;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			hitDirection = initialDirection;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
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
			velo += Projectile.velocity;
			if(Projectile.velocity.Length() == 0)
            {
				velo *= 0;
				current = Projectile.Center;
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
				if (previousPosition == Projectile.Center)
				{
					iterator++;
				}
			}
			if (iterator >= trailPos.Length)
				Projectile.Kill();
		}
		Vector2 initialVelo;
		Vector2 initialCenter;
		int initialDirection = 0;
        public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			if (runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
				initialVelo = Projectile.velocity;
				if(Projectile.velocity.X > 0)
					initialDirection = 1;
				else
					initialDirection = -1;
				initialCenter = player.Center;
				Projectile.ai[0] = -180 * initialDirection;
			}
			Projectile.ai[0] += 6f * initialDirection;
			if (Projectile.timeLeft >= 90)
			{
				Vector2 initialCenter = this.initialCenter;
				int length = 270;
				double rad = MathHelper.ToRadians(Projectile.ai[0]);
				Vector2 ovalArea = new Vector2(length, 0).RotatedBy(initialVelo.ToRotation());
				Vector2 ovalArea2 = new Vector2(length, 0).RotatedBy((float)rad);
				ovalArea2.Y *= 0.33f;
				ovalArea2 = ovalArea2.RotatedBy(initialVelo.ToRotation());
				ovalArea.X += ovalArea2.X;
				ovalArea.Y += ovalArea2.Y;
				Vector2 goTo = initialCenter + ovalArea;
				float dist = (Projectile.Center - goTo).Length();
				Vector2 circular = new Vector2(-(dist > 18 ? 18 : dist), 0).RotatedBy((Projectile.Center - goTo).ToRotation());
				Projectile.velocity = circular;
				if (Main.myPlayer == Projectile.owner && Projectile.timeLeft % 5 == 0)
				{
					Projectile.netUpdate = true;
				}
			}
			else if(Projectile.timeLeft > 30)
            {
				float dist = (Projectile.Center - player.Center).Length();
				Vector2 circular = new Vector2(-(dist > 24 ? 24 : dist), 0).RotatedBy((Projectile.Center - player.Center).ToRotation());
				Projectile.velocity = circular;
				Projectile.tileCollide = false;
				if((Projectile.Center - player.Center).Length() <= 12)
                {
					Projectile.timeLeft = 30;
                }
			}
            return base.PreAI();
        }
        public override void AI()
		{
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.6f / 255f, (255 - Projectile.alpha) * 0.6f / 255f, (255 - Projectile.alpha) * 1.8f / 255f);
			if(Projectile.timeLeft <= 30)
            {
				checkPos();
				Projectile.velocity *= 0f;
				Projectile.alpha = 255;
            }
			Player player = Main.player[Projectile.owner];
			Vector2 circularLocation = new Vector2(Projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation));
			cataloguePos(circularLocation + Projectile.Center, trailPos, MathHelper.ToRadians(rotation));

			circularLocation = new Vector2(Projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation + 90));
			cataloguePos(circularLocation + Projectile.Center, trailPos2, MathHelper.ToRadians(rotation + 90));

			circularLocation = new Vector2(Projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation + 180));
			cataloguePos(circularLocation + Projectile.Center, trailPos3, MathHelper.ToRadians(rotation + 180));

			circularLocation = new Vector2(Projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation + 270));
			cataloguePos(circularLocation + Projectile.Center, trailPos4, MathHelper.ToRadians(rotation + 270));

			rotation += initialDirection * 11.5f;
			Projectile.rotation = rotation;
			Projectile.spriteDirection = 1;
			if(Projectile.timeLeft <= 30)
            {
				Projectile.friendly = false;
            }
		}
        public override void Kill(int timeLeft)
		{
			base.Kill(timeLeft);
        }
        public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return true;
			Draw(trailPos);
			Draw(trailPos2);
			Draw(trailPos3);
			Draw(trailPos4);
			Texture2D texture2 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(texture2.Width / 2, texture2.Height / 2), Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Projectile.alpha >= 150)
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
					float scale = Projectile.scale * (trailArray.Length - i) / (float)trailArray.Length;
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
		public void Draw(Vector2[] trailArray)
		{
			if (trailArray[0] == Vector2.Zero)
			{
				return;
			}
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Projectiles/BiomeChest/SawflakeTrail").Value;
			Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			Color color = new Color(140, 140, 205, 0);
			for (int k = 0; k < trailArray.Length; k++)
			{
				float scale = Projectile.scale * (trailArray.Length - k) / (float)trailArray.Length;
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
						Main.spriteBatch.Draw(texture2, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin2, scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
					}
				}
				previousPosition = currentPos;
			}
		}
	}
}
		