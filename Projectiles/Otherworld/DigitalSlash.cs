using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;

namespace SOTS.Projectiles.Otherworld
{    
    public class DigitalSlash : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Digital Slash");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;    
		}        
		public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 32; 
            projectile.timeLeft = 100;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.melee = true; 
			projectile.alpha = 0;
			projectile.hide = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 5;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Player player = Main.player[projectile.owner];
			Vector2 center = player.Center;
			float point = 0f;
			Vector2 previousPosition = projectile.Center;
			float scale = projectile.scale * 1f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), previousPosition, center, 24f * scale, ref point))
			{
				return true;
			}
			return false;
		}
		public override bool? CanHitNPC(NPC target)
		{
			Player player = Main.player[projectile.owner];
			Vector2 center = player.Center;
			bool hitThroughWall = Collision.CanHitLine(center - new Vector2(5, 5), 10, 10, target.Hitbox.TopLeft(), target.Hitbox.Width, target.Hitbox.Height) && !target.friendly;
			return hitThroughWall || target.behindTiles;
		}
		public override bool ShouldUpdatePosition()
        {
            return false;
        }
		public Vector2 relativePoint(Vector2 toArea, float length = 24)
        {
			Vector2 velo = Vector2.Zero;
			float num1 = length * projectile.scale;
			float num2 = toArea.X;
			float num3 = toArea.Y;
			float num5 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
			float num6 = num1 / num5;
			float num7 = num2 * num6;
			float num8 = num3 * num6;
			velo.X = num7;
			velo.Y = num8;
			return velo;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			Texture2D texture = Main.projectileTexture[projectile.type];
			Texture2D texture2 = ModContent.GetTexture("SOTS/Projectiles/Otherworld/DigitalSlashBlade");
			Texture2D texture3 = ModContent.GetTexture("SOTS/Projectiles/Otherworld/DigitalSlashBlade2");
			Vector2 toProjectile = projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
			int length = (int)toProjectile.Length() / 2 - 8;
			Vector2 rotateToPosition = relativePoint(toProjectile);
			Vector2 drawPos = player.Center + rotateToPosition - Main.screenPosition;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);

			int direction = 1;
			if (toCursor.X < 0)
			{
				direction = -1;
				direction *= -(int)projectile.ai[0];
			}
			else
				direction *= (int)projectile.ai[0];
			float rotation = toProjectile.ToRotation() + MathHelper.ToRadians(direction == -1 ? -215 : 45);
			for(int i = 0; i < length + 1; i++)
			{
				Color color1 = Color.Lerp(new Color(0, 110, 170), new Color(122, 243, 255), 1f - (float)i / length);
				color1 = color1.MultiplyRGBA(new Color(70, 70, 80, 0));
				Vector2 toProj2 = rotateToPosition + rotateToPosition.SafeNormalize(Vector2.Zero) * (i * 2);
				for(int l = 0; l < 3; l++)
					spriteBatch.Draw(texture3, player.Center + toProj2 - Main.screenPosition + new Vector2(0, Main.rand.NextFloat(0.25f, 1f) * l * 0.5f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360))), null, color1, rotation, origin + new Vector2(0, 3), projectile.scale, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				if(i < length && i % 4 != 3)
					spriteBatch.Draw(texture2, player.Center + toProj2 - Main.screenPosition, null, Color.Black, rotation, origin + new Vector2(0, 1), 1.05f, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			
			spriteBatch.Draw(texture, drawPos, null, Color.White, rotation, origin, projectile.scale * 1.2f, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}
		float counter = 225;
		float spinSpeed = 0;
		int storeData = -1;
		int storeData2 = -1;
		bool starttrail = false;
        public override void PostAI()
		{
			Player player = Main.player[projectile.owner];
			if (starttrail)
			{
				if (storeData == -1 && projectile.owner == Main.myPlayer)
				{
					storeData = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("DigitalTrail"), (int)(projectile.damage * 1f) + 1, projectile.knockBack * 0.5f, projectile.owner, 1, projectile.identity);
					projectile.localAI[1] = storeData;
					projectile.netUpdate = true;
				}
				if (storeData2 == -1 && projectile.owner == Main.myPlayer)
				{
					storeData2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("DigitalTrail"), (int)(projectile.damage * 1f) + 1, projectile.knockBack * 0.5f, projectile.owner, -1, projectile.identity);
					projectile.localAI[0] = storeData2;
					projectile.netUpdate = true;
				}
			}
			starttrail = true;
			if (projectile.hide == false && toCursor != Vector2.Zero)
			{
				Vector2 toProjectile = projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
				int direction = 1;
				if (toCursor.X < 0)
					direction = -1;
				projectile.alpha = 0;
				player.ChangeDir(direction);
				player.heldProj = projectile.whoAmI;
				player.itemRotation = MathHelper.WrapAngle(toProjectile.ToRotation() + (direction == -1 ? MathHelper.ToRadians(180) : 0));
				player.itemTime = 2;
				player.itemAnimation = 2;
			}
			projectile.hide = false;
		}
		Vector2 dustAway = Vector2.Zero;
		Vector2 cursorArea = Vector2.Zero;
		Vector2 toCursor = Vector2.Zero;
		bool runOnce = true;
		float distance = 0;
		float counterOffset;
		float timeLeftCounter = 0;
        public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			float randMod = projectile.ai[1];
			if (runOnce)
			{
				Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 71, 0.9f, 1f * randMod);
				if (Main.myPlayer == projectile.owner)
				{
					cursorArea = Main.MouseWorld;
					projectile.netUpdate = true;
					distance = Vector2.Distance(player.Center, cursorArea) * randMod;
					if (distance < 120)
						distance = 120;
					if (distance > 320)
						distance = 320;
					toCursor = cursorArea - player.Center;
					spinSpeed = (1.0f + (4.5f / (float)Math.Pow(distance / 100f, 2f))) * randMod * 5f * (1 + SOTSPlayer.ModPlayer(player).attackSpeedMod) / player.meleeSpeed;
				}
				counterOffset = 205 + 45f / randMod;
				float slashOffset = counterOffset * projectile.ai[0];
				counter = slashOffset;
				runOnce = false;
			}
			return base.PreAI();
        }
        public override void AI()
		{
			Player player = Main.player[projectile.owner];
			float randMod = projectile.ai[1];
			float mult = 1f * projectile.ai[1];
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.2f / 255f, (255 - projectile.alpha) * 0.7f / 255f, (255 - projectile.alpha) * 1.0f / 255f);
			if(toCursor != Vector2.Zero)
			{
				float distMult = 10f / (float)Math.Pow(distance, 0.5);
				double deg = counter; 
				double rad = deg * (Math.PI / 180);
				Vector2 ovalArea = new Vector2(distance * 0.25f * mult, 0).RotatedBy(toCursor.ToRotation());
				Vector2 ovalArea2 = new Vector2(distance * 0.75f * mult, 0).RotatedBy((float)rad);
				ovalArea2.Y *= 1f / randMod * distMult;
				ovalArea2 = ovalArea2.RotatedBy(toCursor.ToRotation());
				ovalArea.X += ovalArea2.X;
				ovalArea.Y += ovalArea2.Y;
				projectile.position = player.Center + ovalArea - new Vector2(projectile.width/2, projectile.height/2); 
				dustAway = ovalArea;
				projectile.rotation = dustAway.ToRotation();
			}
			float iterator2 = (float)Math.Abs(spinSpeed * projectile.ai[0] / randMod);
			timeLeftCounter += iterator2;
			counter += spinSpeed * projectile.ai[0] / randMod;
			if(timeLeftCounter > (235.0f + (4000f / distance)) / randMod)
            {
				projectile.hide = true;
				projectile.Kill();
            }
			else
            {
				if (dustAway != Vector2.Zero && starttrail)
				{
					float amt = Main.rand.NextFloat(2, 3) * distance / 300f;
					for (int i = 0; i < amt; i++)
					{
						float dustScale = 1f;
						float rand = Main.rand.NextFloat(0.9f, 1.35f);
						int type = mod.DustType("CodeDust2");
						if (Main.rand.NextBool(3))
                        {
							type = DustID.Electric;
							dustScale *= 0.3f;
						}
						int num = Dust.NewDust(new Vector2(projectile.Center.X - 12, projectile.Center.Y - 12) + dustAway.SafeNormalize(Vector2.Zero) * 32, 16, 16, type);
						Dust dust = Main.dust[num];
						dust.velocity *= 1.2f / rand;
						dust.velocity += dustAway.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1.5f, 2.4f) * rand;
						dust.noGravity = true;
						dust.scale *= 0.5f / rand;
						dust.scale += 2.2f / rand * dustScale;
					}
					Vector2 toProjectile = projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
					for (int i = 0; i < amt / 2; i++)
					{
						float rand = Main.rand.NextFloat(0.9f, 1.35f);
						int type = mod.DustType("CodeDust2");
						if (Main.rand.NextBool(2))
						{
							type = DustID.Electric;
							rand *= 0.3f;
						}
						int num = Dust.NewDust(new Vector2(projectile.Center.X - 12, projectile.Center.Y - 12) + -toProjectile * Main.rand.NextFloat(0.95f), 16, 16, type);
						Dust dust = Main.dust[num];
						dust.velocity *= 0.15f / rand;
						dust.velocity += dustAway.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(90 * projectile.ai[0])) * Main.rand.NextFloat(0.5f, 1f) * rand;
						dust.noGravity = true;
						dust.scale *= 0.15f / rand;
						dust.scale += 1.00f * rand;
					}
				}
			}
		}
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(counter);
			writer.Write(spinSpeed);
			writer.Write(toCursor.X);
			writer.Write(toCursor.Y);
			writer.Write(cursorArea.X);
			writer.Write(cursorArea.Y);
			writer.Write(distance);
			writer.Write(projectile.localAI[0]);
			writer.Write(projectile.localAI[1]);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			counter = reader.ReadSingle();
			spinSpeed = reader.ReadSingle();
			toCursor.X = reader.ReadSingle();
			toCursor.Y = reader.ReadSingle();
			cursorArea.X = reader.ReadSingle();
			cursorArea.Y = reader.ReadSingle();
			distance = reader.ReadSingle();
			projectile.localAI[0] = reader.ReadSingle();
			projectile.localAI[1] = reader.ReadSingle();
		}
    }
}
		
			