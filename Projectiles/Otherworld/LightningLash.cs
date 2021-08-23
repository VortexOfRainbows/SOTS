using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{    
    public class LightningLash : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lightning Lash");
		}
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(someRandomization);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			someRandomization = reader.ReadInt32();
        }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10; 
            projectile.timeLeft = 60;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.magic = true; 
			projectile.alpha = 0;
            projectile.extraUpdates = 2;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 40;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
		Vector2[] trailPos = new Vector2[30];
		int[] randStorage = new int[30];
		public void SetTrails()
		{
			for (int i = 0; i < randStorage.Length; i++)
			{
				randStorage[i] = Main.rand.Next(-45, 46);
			}
			Player player = Main.player[projectile.owner];
			Vector2 center = player.Center + new Vector2(16 * player.direction, -16);
			Vector2 toVelo = projectile.Center - center;
			toVelo = toVelo.SafeNormalize(Vector2.Zero);

			Vector2 addPos = center;
			int dist = trailPos.Length;
			for (int i = 0; i < dist; i++)
			{
				center += toVelo * 5.5f;
				for (int reps = 0; reps < 3; reps++)
				{
					Vector2 attemptToPosition = (projectile.Center + toVelo * 3f) - addPos;
					addPos += new Vector2(10, 0).RotatedBy(attemptToPosition.ToRotation() + MathHelper.ToRadians(randStorage[i]));
					trailPos[i] = addPos;
					if(projectile.Hitbox.Contains(addPos.ToPoint()) || (projectile.Center - center).Length() < (addPos - center).Length())
					{
						trailPos[i] = projectile.Center;
						for(int j = i + 1; j < trailPos.Length; j++)
                        {
							trailPos[j] = Vector2.Zero;
                        }
						return;
					}
					if(Main.rand.NextBool(100))
					{
						int num1 = Dust.NewDust(addPos - new Vector2(5), 4, 4, mod.DustType("CopyDust4"));
						Dust dust = Main.dust[num1];
						dust.velocity *= 0.4f;
						dust.noGravity = true;
						dust.scale += 0.1f;
						dust.color = new Color(255, 240, 50, 100);
						dust.fadeIn = 0.1f;
						dust.scale *= 1.6f;
						dust.alpha = 150;
					}
				}
			}
			trailPos[29] = projectile.Center;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			Vector2 center = player.Center + new Vector2(16 * player.direction, -16);
			if (runOnce)
				return false;
			Texture2D texture = mod.GetTexture("Projectiles/Otherworld/LightningLash");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = projectile.scale * 0.5f + 0.5f * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				Color color = new Color(120, 130, 100, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = projectile.GetAlpha(color) * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
				float max = betweenPositions.Length() / (10 * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 7; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.2f * scale;
						float y = Main.rand.Next(-10, 11) * 0.2f * scale;
						if (j < 2)
						{
							x = 0;
							y = 0;
						}
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
			return false;
		}
		int direction = 1;
		bool runOnce = true;
		Vector2 toCursor;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Player player = Main.player[projectile.owner];
            Vector2 center = player.Center + new Vector2(16 * player.direction, -16);
            float point = 0f;
            Vector2 previousPosition = projectile.Center;
            float scale = projectile.scale * 1f;
            Vector2 currentPos = center;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), previousPosition, currentPos, 10f * scale, ref point))
            {
                return true;
            }
            return false;
        }
        public override bool? CanHitNPC(NPC target)
		{
			Player player = Main.player[projectile.owner];
			Vector2 center = player.Center + new Vector2(16 * player.direction, -16);
			bool hitThroughWall = Collision.CanHitLine(center - new Vector2(5, 5), projectile.width, projectile.height, target.Hitbox.TopLeft(), target.Hitbox.Width, target.Hitbox.Height) && !target.friendly;
			return hitThroughWall || target.behindTiles;
        }
        int initialDirection = 1;
		int someRandomization = 0;
        public override void AI()
		{
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.7f / 255f, (255 - projectile.alpha) * 0.8f / 255f, (255 - projectile.alpha) * 0.4f / 255f);
			Player player = Main.player[projectile.owner];
            Vector2 center = player.Center + new Vector2(16 * player.direction, -16);
			if(runOnce)
			{
				initialDirection = player.direction;
				for (int i = 0; i < randStorage.Length; i++)
				{
					randStorage[i] = Main.rand.Next(-45, 46);
					trailPos[i] = Vector2.Zero;
				}
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 93, 0.7f);
                if (player.whoAmI == Main.myPlayer)
                {
                    Vector2 cursorArea = Main.MouseWorld;
                    toCursor = Main.MouseWorld - center;
					someRandomization = Main.rand.Next(-20, 21);
					projectile.netUpdate = true;
				}
                direction = player.direction;
                projectile.ai[0] = -180 * direction;
                runOnce = false;
			}
			if(player.whoAmI == Main.myPlayer)
			{
				Vector2 initialCenter = player.Center + new Vector2(16 * initialDirection, -16);
				projectile.netUpdate = true;

				int length = 180 + someRandomization;
				double rad = MathHelper.ToRadians(projectile.ai[0]);
				Vector2 ovalArea = new Vector2(length, 0).RotatedBy(toCursor.ToRotation());
				Vector2 ovalArea2 = new Vector2(length, 0).RotatedBy((float)rad);
				ovalArea2.Y *= 0.85f;
				ovalArea2 = ovalArea2.RotatedBy(toCursor.ToRotation());
				ovalArea.X += ovalArea2.X;
				ovalArea.Y += ovalArea2.Y;

				projectile.position.X = initialCenter.X + ovalArea.X - projectile.width / 2;
				projectile.position.Y = initialCenter.Y + ovalArea.Y - projectile.height / 2;
			}
			SetTrails();
			projectile.ai[0] += 6f * direction;
			
		}
        public override bool ShouldUpdatePosition()
        {
            return false;
		}
		int storeData = -1;
		public override void PostAI()
		{
			if (storeData == -1 && projectile.owner == Main.myPlayer)
			{
				storeData = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("LightningLashTrail"), (int)(projectile.damage * 1f) + 1, projectile.knockBack * 0.75f, projectile.owner, 0, projectile.whoAmI);
				projectile.ai[1] = storeData;
				projectile.netUpdate = true;
			}
		}
	}
}
		
			