using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using Terraria;
using Terraria.ModLoader;
using static SOTS.SOTS;

namespace SOTS.Projectiles.BiomeChest
{    
    public class FloweringBud : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flowering Bud");
		}
        public override void SetDefaults()
        {
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 1500;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			// Projectile.ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.alpha = 0;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.scale);
			writer.Write(Projectile.rotation);
			writer.Write(Projectile.spriteDirection);
			writer.Write(Projectile.friendly);
			writer.Write(latch);
			writer.Write(Projectile.frame);
			writer.Write(enemyIndex);
			writer.Write(diffPosX);
			writer.Write(diffPosY);
			writer.Write(bloom);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.scale = reader.ReadSingle();
			Projectile.rotation = reader.ReadSingle();
			Projectile.spriteDirection = reader.ReadInt32();
			Projectile.friendly = reader.ReadBoolean();
			latch = reader.ReadBoolean();
			Projectile.frame = reader.ReadInt32();
			enemyIndex = reader.ReadInt32();
			diffPosX = reader.ReadSingle();
			diffPosY = reader.ReadSingle();
			bloom = reader.ReadBoolean();
		}
		bool runOnce = true;
		bool latch = false;
		public int enemyIndex = -1;
		float diffPosX = 0;
		float diffPosY = 0;
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
		float unwind = 720;
		bool bloom = false;
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
			if(latch)
			{
				Projectile.netUpdate = true;
				if(enemyIndex != -1)
				{
					NPC target = Main.npc[enemyIndex];
					if (target.active && !target.friendly && !target.immortal && !target.dontTakeDamage)
					{
						Projectile.position.X = target.Center.X - Projectile.width / 2 - diffPosX;
						Projectile.position.Y = target.Center.Y - Projectile.height / 2 - diffPosY;
					}
					else
					{
						enemyIndex = -1;
						Projectile.timeLeft -= 3;
					}
				}
				else
                {
					Projectile.timeLeft -= 3;
                }
			}
			if(!Projectile.tileCollide)
			{
				Projectile.velocity *= 0.8f;
			}				
			if(!Projectile.friendly && latch || Projectile.ai[0] > 0)
			{
				if (Projectile.velocity.Length() <= 0.1f)
                {
					if(Projectile.ai[0] <= 10)
						Projectile.ai[0]++;
					if(Projectile.ai[0] > 10)
					{
						if (runOnce2)
						{
							Projectile.ai[0] = 0;
							Projectile.ai[1] = 0;
							runOnce2 = false;
							Projectile.rotation = 0;
							bloom = true;
							Projectile.scale = 0.4f;
							Projectile.netUpdate = true;
							for(int i = 0; i < 360; i += 15)
							{
								Vector2 circularLocation = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(i));
								Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>());
								dust.velocity *= 0.33f;
								dust.velocity += circularLocation;
								dust.scale *= 1.25f;
								dust.fadeIn = 0.2f;
								dust.color = new Color(120, 180, 140);
								dust.alpha = 50;
								dust.noGravity = true;
							}
							//SoundEngine.PlaySound(2, (int)Projectile.Center.X, (int)Projectile.Center.Y, 9, 0.8f);
						}
						if (Projectile.scale < 1)
                        {
							Projectile.scale += 0.03f;
							Projectile.scale *= 1.05f;
							Projectile.rotation = saveRotation + unwind * (1 - Projectile.scale);
						}
						else
                        {
							Projectile.rotation = saveRotation;
							Projectile.scale = 1;
						}
						if (Projectile.ai[1] < 120)
							Projectile.ai[1] += 4;
						else if(Projectile.ai[0] < 30)
							Projectile.ai[0] += 2;
						Vector2 bubCir = new Vector2(300, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[1] - Projectile.ai[0] + 10));
						bubbleSize = bubCir.Y;
					}
                }
			}
			if(bloom)
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC target = Main.npc[i];
					if ((Projectile.Center - target.Center).Length() <= bubbleSize / 2f + 4f && !target.friendly && target.active && !target.dontTakeDamage)
					{
						if(!effected[i])
						{
							if(Main.myPlayer == Projectile.owner)
								Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<FlowerStrike>(), Projectile.damage, 0, Projectile.owner, target.whoAmI, Projectile.whoAmI);
							effected[i] = true;
						}
					}
					else if(target.friendly || !target.active)
					{
						effected[i] = false;
					}
				}
			}
		}
		public bool[] effected = new bool[200];
		float bubbleSize = 0f;
		public override void ModifyDamageHitbox(ref Rectangle hitbox) 
		{
			hitbox = new Rectangle((int)(Projectile.Center.X - 11), (int)(Projectile.Center.Y- 8), 22, 16);
			if(Projectile.frame == 1)
			{
				hitbox = new Rectangle((int)(Projectile.position.X), (int)(Projectile.position.Y), Projectile.width, Projectile.height);
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[Projectile.owner];
			Projectile.friendly = false;
            target.immune[Projectile.owner] = 0;
			Projectile.tileCollide = false;
			latch = true;
			Projectile.velocity *= 0.1f;
			enemyIndex = target.whoAmI;
			Projectile.netUpdate = true;
			if (diffPosX == 0)
				diffPosX = target.Center.X - Projectile.Center.X;
			if (diffPosY == 0)
				diffPosY = target.Center.Y - Projectile.Center.Y;
			diffPosX *= 0.9f;
			diffPosY *= 0.9f;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) 
		{
			width = 4;
			height = 4;
			fallThrough = true;
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			enemyIndex = -1;
			latch = true;
			Projectile.friendly = false;
			Projectile.velocity *= 0.3f;
			Projectile.tileCollide = false;
			Projectile.netUpdate = true;
			return false;
		}
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
				dust.color = new Color(120, 180, 140);
				dust.alpha = 50;
				dust.noGravity = true; 
				circularLocation = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(i));
				dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 0.45f;
				dust.velocity += circularLocation;
				dust.scale *= 1.75f;
				dust.fadeIn = 0.2f;
				dust.color = new Color(140, 220, 170);
				dust.alpha = 50;
				dust.noGravity = true;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			if(bloom)
			{
				spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Gores/CircleAura").Value, Projectile.Center - Main.screenPosition, null, new Color(120, 160, 140) * (50f / 255f) * (Projectile.timeLeft / 1500f), 0f, new Vector2(300f, 300f), bubbleSize / 600f, SpriteEffects.None, 0f);
				spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Gores/CircleBorder").Value, Projectile.Center - Main.screenPosition, null, new Color(100, 140, 120) * 0.5f * (Projectile.timeLeft / 1500f), 0f, new Vector2(300f, 300f), bubbleSize / 600f, SpriteEffects.None, 0f);
				Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/BiomeChest/TangleGrowth").Value;
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
				return false;
			}
			else
			{
				Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/BiomeChest/FloweringBud").Value;
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
				if (runOnce)
					return false;
				texture = Mod.Assets.Request<Texture2D>("Projectiles/BiomeChest/FloweringBudTrail").Value;
				drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				Vector2 previousPosition = Projectile.Center - new Vector2(8, -8).RotatedBy(Projectile.rotation);
				if (previousPosition == Vector2.Zero)
				{
					return false;
				}
				for (int k = 0; k < trailPos.Length; k++)
				{
					float scale = Projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
					scale *= 0.7f;
					if (trailPos[k] == Vector2.Zero)
					{
						return false;
					}
					Vector2 drawPos = trailPos[k] - Main.screenPosition;
					Vector2 currentPos = trailPos[k];
					Vector2 betweenPositions = previousPosition - currentPos;
					Color color = new Color(80, 150, 90, 0) * ((trailPos.Length - k) / (float)trailPos.Length) * 0.25f;
					float max = betweenPositions.Length() / (4 * scale);
					for (int i = 0; i < max; i++)
					{
						drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
						for (int j = 0; j < 4; j++)
						{
							float x = Main.rand.Next(-10, 11) * 0.4f * scale;
							float y = Main.rand.Next(-10, 11) * 0.4f * scale;
							if (j <= 1)
							{
								x = 0;
								y = 0;
							}
							if (trailPos[k] != Projectile.Center)
								Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
						}
					}
					previousPosition = currentPos;
				}
				return false;
			}
		}
	}
}
		