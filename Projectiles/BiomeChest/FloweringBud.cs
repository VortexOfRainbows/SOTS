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
			projectile.width = 20;
			projectile.height = 20;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 1500;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.magic = true;
			projectile.ranged = false;
			projectile.alpha = 0;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.scale);
			writer.Write(projectile.rotation);
			writer.Write(projectile.spriteDirection);
			writer.Write(projectile.friendly);
			writer.Write(latch);
			writer.Write(projectile.frame);
			writer.Write(enemyIndex);
			writer.Write(diffPosX);
			writer.Write(diffPosY);
			writer.Write(bloom);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.scale = reader.ReadSingle();
			projectile.rotation = reader.ReadSingle();
			projectile.spriteDirection = reader.ReadInt32();
			projectile.friendly = reader.ReadBoolean();
			latch = reader.ReadBoolean();
			projectile.frame = reader.ReadInt32();
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
		float unwind = 720;
		bool bloom = false;
		bool runOnce2 = true;
		float saveRotation = 0;
		float scaleCounter = 0;
		public override void AI()
        {
			Player player = Main.player[projectile.owner];
			if(runOnce2)
			{
				projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45);
				saveRotation = projectile.rotation;
				projectile.spriteDirection = 1;
			}
			if(latch)
			{
				projectile.netUpdate = true;
				if(enemyIndex != -1)
				{
					NPC target = Main.npc[enemyIndex];
					if (target.active && !target.friendly && !target.immortal && !target.dontTakeDamage)
					{
						projectile.position.X = target.Center.X - projectile.width / 2 - diffPosX;
						projectile.position.Y = target.Center.Y - projectile.height / 2 - diffPosY;
					}
					else
					{
						enemyIndex = -1;
						projectile.timeLeft -= 3;
					}
				}
				else
                {
					projectile.timeLeft -= 3;
                }
			}
			if(!projectile.tileCollide)
			{
				projectile.velocity *= 0.8f;
			}				
			if(!projectile.friendly && latch || projectile.ai[0] > 0)
			{
				if (projectile.velocity.Length() <= 0.1f)
                {
					if(projectile.ai[0] <= 10)
						projectile.ai[0]++;
					if(projectile.ai[0] > 10)
					{
						if (runOnce2)
						{
							projectile.ai[0] = 0;
							projectile.ai[1] = 0;
							runOnce2 = false;
							projectile.rotation = 0;
							bloom = true;
							projectile.scale = 0.4f;
							projectile.netUpdate = true;
							for(int i = 0; i < 360; i += 15)
							{
								Vector2 circularLocation = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(i));
								Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>());
								dust.velocity *= 0.33f;
								dust.velocity += circularLocation;
								dust.scale *= 1.25f;
								dust.fadeIn = 0.2f;
								dust.color = new Color(120, 180, 140);
								dust.alpha = 50;
								dust.noGravity = true;
							}
							//Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 9, 0.8f);
						}
						if (projectile.scale < 1)
                        {
							projectile.scale += 0.03f;
							projectile.scale *= 1.05f;
							projectile.rotation = saveRotation + unwind * (1 - projectile.scale);
						}
						else
                        {
							projectile.rotation = saveRotation;
							projectile.scale = 1;
						}
						if (projectile.ai[1] < 120)
							projectile.ai[1] += 4;
						else if(projectile.ai[0] < 30)
							projectile.ai[0] += 2;
						Vector2 bubCir = new Vector2(300, 0).RotatedBy(MathHelper.ToRadians(projectile.ai[1] - projectile.ai[0] + 10));
						bubbleSize = bubCir.Y;
					}
                }
			}
			if(bloom)
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC target = Main.npc[i];
					if ((projectile.Center - target.Center).Length() <= bubbleSize / 2f + 4f && !target.friendly && target.active && !target.dontTakeDamage)
					{
						if(!effected[i])
						{
							if(Main.myPlayer == projectile.owner)
								Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<FlowerStrike>(), projectile.damage, 0, projectile.owner, target.whoAmI, projectile.whoAmI);
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
			hitbox = new Rectangle((int)(projectile.Center.X - 11), (int)(projectile.Center.Y- 8), 22, 16);
			if(projectile.frame == 1)
			{
				hitbox = new Rectangle((int)(projectile.position.X), (int)(projectile.position.Y), projectile.width, projectile.height);
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[projectile.owner];
			projectile.friendly = false;
            target.immune[projectile.owner] = 0;
			projectile.tileCollide = false;
			latch = true;
			projectile.velocity *= 0.1f;
			enemyIndex = target.whoAmI;
			projectile.netUpdate = true;
			if (diffPosX == 0)
				diffPosX = target.Center.X - projectile.Center.X;
			if (diffPosY == 0)
				diffPosY = target.Center.Y - projectile.Center.Y;
			diffPosX *= 0.9f;
			diffPosY *= 0.9f;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough) 
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
			projectile.friendly = false;
			projectile.velocity *= 0.3f;
			projectile.tileCollide = false;
			projectile.netUpdate = true;
			return false;
		}
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
				dust.color = new Color(120, 180, 140);
				dust.alpha = 50;
				dust.noGravity = true; 
				circularLocation = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(i));
				dust = Dust.NewDustDirect(projectile.Center - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 0.45f;
				dust.velocity += circularLocation;
				dust.scale *= 1.75f;
				dust.fadeIn = 0.2f;
				dust.color = new Color(140, 220, 170);
				dust.alpha = 50;
				dust.noGravity = true;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if(bloom)
			{
				spriteBatch.Draw(mod.GetTexture("Gores/CircleAura"), projectile.Center - Main.screenPosition, null, new Color(120, 160, 140) * (50f / 255f) * (projectile.timeLeft / 1500f), 0f, new Vector2(300f, 300f), bubbleSize / 600f, SpriteEffects.None, 0f);
				spriteBatch.Draw(mod.GetTexture("Gores/CircleBorder"), projectile.Center - Main.screenPosition, null, new Color(100, 140, 120) * 0.5f * (projectile.timeLeft / 1500f), 0f, new Vector2(300f, 300f), bubbleSize / 600f, SpriteEffects.None, 0f);
				Texture2D texture = mod.GetTexture("Projectiles/BiomeChest/TangleGrowth");
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
				return false;
			}
			else
			{
				Texture2D texture = mod.GetTexture("Projectiles/BiomeChest/FloweringBud");
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
				if (runOnce)
					return false;
				texture = mod.GetTexture("Projectiles/BiomeChest/FloweringBudTrail");
				drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				Vector2 previousPosition = projectile.Center - new Vector2(8, -8).RotatedBy(projectile.rotation);
				if (previousPosition == Vector2.Zero)
				{
					return false;
				}
				for (int k = 0; k < trailPos.Length; k++)
				{
					float scale = projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
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
							if (trailPos[k] != projectile.Center)
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
		