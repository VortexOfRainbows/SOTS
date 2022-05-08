using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{    
    public class AdvisorPet : ModProjectile
    {
        Vector2[] hookPos = { new Vector2(-1, -1), new Vector2(-1, -1), new Vector2(-1, -1), new Vector2(-1, -1) };
		Vector2[] hookPos2 = { new Vector2(-1, -1), new Vector2(-1, -1), new Vector2(-1, -1), new Vector2(-1, -1) };
		public float glow = 14f;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Advisor Pet");
			Main.projFrames[projectile.type] = 1;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.LightPet[projectile.type] = true;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(fireToX);
			writer.Write(fireToY);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			fireToX = reader.ReadSingle();
			fireToY = reader.ReadSingle();
			base.ReceiveExtraAI(reader);
		}
		public override void SetDefaults()
        {
			projectile.netImportant = true;
            projectile.width = 42;
            projectile.height = 48; 
            projectile.timeLeft = 20000;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.light = 0.5f;
		}
		public float fireToX = 0;
		public float fireToY = 0;
		public float eyeReset = 2.5f;
		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.hornet = false;  
			if (Main.myPlayer != projectile.owner)
				projectile.timeLeft = 20;
			if (eyeReset < 2.5f)
			{
				eyeReset += 0.25f;
				if (eyeReset > 2.5f)
					eyeReset = 2.5f;
			}
			for(int i = 0; i < Main.projectile.Length; i++)
            {
				Projectile proj = Main.projectile[i];
				if(proj.type == projectile.type && proj.owner == projectile.owner && proj.active && proj.whoAmI != projectile.whoAmI)
                {
					projectile.Kill();
                }
            }
			return true;
        }
        public override void AI()
		{
			if (projectile.velocity.X == 0f)
				projectile.ai[1] += Main.player[projectile.owner].direction;
			projectile.ai[1] += projectile.velocity.X * 1.5f;
			for (int i = 0; i < 4; i++)
			{
				float extraX = 0;
				if (i == 0)
					extraX = -38;
				if (i == 1)
					extraX = -30;
				if (i == 2)
					extraX = 30;
				if (i == 3)
					extraX = 38;
				float extraY = 0;
				if (i == 0 || i == 3)
					extraY = 20;
				hookPos2[i] = new Vector2(extraX, 56 - extraY);
				hookPos[i] = hookPos2[i] + new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(projectile.ai[1] + 90 * i));
			}
			Player player = Main.player[projectile.owner];
            SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();

			Vector2 toLocation = projectile.Center;
			projectile.velocity *= 0.1f;
			toLocation.X = player.Center.X - (float)(40 * Main.player[projectile.owner].direction);
			if (modPlayer.PurpleBalloon)
			{
				toLocation.X = player.Center.X - (float)(64 * Main.player[projectile.owner].direction);
			}
			toLocation.Y = player.Center.Y - 46 + Main.player[projectile.owner].gfxOffY;
            Vector2 goTo = toLocation - projectile.Center;
            Vector2 newGoTo = goTo.SafeNormalize(Vector2.Zero);
            float dist = 4f + goTo.Length() * 0.015f;
			if (dist > 10)
			{
				if(glow < 14)
					glow++;
			}
			else if(glow > 0)
				glow--;
            if (dist > goTo.Length())
                dist = goTo.Length();
            projectile.velocity = newGoTo * dist;
			projectile.ai[0]++;
			if(projectile.owner == Main.myPlayer)
            {
				if(eyeReset >= 2.5f)
				{
					fireToX = Main.MouseWorld.X;
					fireToY = Main.MouseWorld.Y;
				}
				projectile.netUpdate = true;
			}
		}
		public void DrawGlow(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/AdvisorPetSpirit");
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 7; k++)
			{
				spriteBatch.Draw(texture, drawPos + Main.rand.NextVector2Circular(1, 1), null, color, 0f, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int j = 0; j < 4;)
			{
				float scale = projectile.scale * 0.85f;
				int ai2 = 0;
				if (j > 1)
					ai2 += 180;
				Vector2 toPos = projectile.Center + hookPos[j];
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/AdvisorPetVine");
				Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/AdvisorPetVineGlow");
				Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
				Vector2 projectilePos = new Vector2(projectile.Center.X + (-24 + 16 * j) * 1f * 0.25f, projectile.position.Y + projectile.height);
				Vector2 distanceToOwner = projectilePos - toPos;
				Vector2 centerOfCircle = toPos + distanceToOwner / 2;
				float startingRadians = distanceToOwner.ToRotation();
				float radius = distanceToOwner.Length() / 3;
				float distance = distanceToOwner.Length();
				float minDist = 66f;
				float midPointDist = minDist - distance;
				if (midPointDist < 0) midPointDist = 0;
				midPointDist /= 1.75f;
				Vector2 point1 = centerOfCircle + new Vector2(0, midPointDist).RotatedBy(startingRadians + MathHelper.ToRadians(ai2));
				Vector2 point2 = centerOfCircle + new Vector2(0, midPointDist).RotatedBy(startingRadians + MathHelper.ToRadians(180 + ai2));

				Vector2 pointprojectileTo1 = projectilePos - point1;
				pointprojectileTo1 = pointprojectileTo1.SafeNormalize(new Vector2(1, 0));
				pointprojectileTo1 *= radius;
				point1 = projectilePos - pointprojectileTo1;
				Vector2 pointEndTo2 = point2 - toPos;
				pointEndTo2 = pointEndTo2.SafeNormalize(new Vector2(1, 0));
				pointEndTo2 *= radius;
				point2 = toPos + pointEndTo2;
				Vector2 point1To2 = point1 - point2;
				float dynamLength = 0.2f;
				Color color = new Color(100, 100, 100, 0);
				int totalSeg = 12;
				int currentSeg = 0;
				int currentSegMult = 0;
				int max = 4;
				for (int i = 0; i < max; i++)
				{
					Vector2 dist = -pointprojectileTo1 / (float)(max);
					Vector2 pos = projectilePos + dist * i;
					Vector2 dynamicAddition = new Vector2(dynamLength, 0).RotatedBy(MathHelper.ToRadians(currentSeg * 180f / totalSeg + projectile.ai[0]));
					Vector2 drawPos = pos - Main.screenPosition;
					spriteBatch.Draw(texture, drawPos + dynamicAddition, null, projectile.GetAlpha(lightColor), pointprojectileTo1.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
					for (int k = 0; k < glow / 2; k++)
					{
						float x = Main.rand.Next(-10, 11) * 0.1f;
						float y = Main.rand.Next(-10, 11) * 0.1f;
						spriteBatch.Draw(texture2, drawPos + dynamicAddition + new Vector2(x, y), null, color, pointprojectileTo1.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
					}
					currentSeg++;
					currentSegMult++;
				}
				max = 4;
				for (int i = 0; i < max; i++)
				{
					Vector2 bobbing = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(180f / max * i)); //creates length of circle
					bobbing.Y *= (midPointDist / 40f);
					Vector2 circularLocation = new Vector2(0, bobbing.Y).RotatedBy(point1To2.ToRotation() + MathHelper.ToRadians(ai2)); //applies rotation
					Vector2 dist = -point1To2 / (float)(max * 2);
					Vector2 pos = point1 + dist * i;
					Vector2 dynamicAddition = new Vector2(dynamLength, 0).RotatedBy(MathHelper.ToRadians(currentSeg * 180f / totalSeg + projectile.ai[0]));
					Vector2 drawPos = pos + circularLocation - Main.screenPosition;
					spriteBatch.Draw(texture, drawPos + dynamicAddition, null, projectile.GetAlpha(lightColor), point1To2.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
					for (int k = 0; k < glow / 2; k++)
					{
						float x = Main.rand.Next(-10, 11) * 0.1f;
						float y = Main.rand.Next(-10, 11) * 0.1f;
						spriteBatch.Draw(texture2, drawPos + dynamicAddition + new Vector2(x, y), null, color, point1To2.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
					}
					currentSeg++;
					currentSegMult++;
				}
				for (int i = 0; i < max; i++)
				{
					Vector2 bobbing = new Vector2(-12, 0).RotatedBy(MathHelper.ToRadians(180f / max * i));
					bobbing.Y *= (midPointDist / 40f);
					Vector2 circularLocation = new Vector2(0, bobbing.Y).RotatedBy(point1To2.ToRotation() + MathHelper.ToRadians(ai2)); //applies rotation
					Vector2 dist = -point1To2 / (float)(max * 2);
					Vector2 pos = centerOfCircle + dist * i;
					Vector2 dynamicAddition = new Vector2(dynamLength, 0).RotatedBy(MathHelper.ToRadians(currentSeg * 180f / totalSeg + projectile.ai[0]));
					Vector2 drawPos = pos + circularLocation - Main.screenPosition;
					spriteBatch.Draw(texture, drawPos + dynamicAddition, null, projectile.GetAlpha(lightColor), point1To2.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
					for (int k = 0; k < glow / 2; k++)
					{
						float x = Main.rand.Next(-10, 11) * 0.1f;
						float y = Main.rand.Next(-10, 11) * 0.1f;
						spriteBatch.Draw(texture2, drawPos + dynamicAddition + new Vector2(x, y), null, color, point1To2.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
					}
					currentSeg++;
					currentSegMult--;
				}
				max = 4;
				for (int i = 0; i < max; i++)
				{
					Vector2 dist = -pointEndTo2 / (float)(max);
					Vector2 pos = point2 + dist * i;
					Vector2 dynamicAddition = new Vector2(dynamLength, 0).RotatedBy(MathHelper.ToRadians(currentSeg * 180f / totalSeg + projectile.ai[0]));
					Vector2 drawPos = pos - Main.screenPosition;
					spriteBatch.Draw(texture, drawPos + dynamicAddition, null, projectile.GetAlpha(lightColor), pointEndTo2.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
					for (int k = 0; k < glow / 2; k++)
					{
						float x = Main.rand.Next(-10, 11) * 0.1f;
						float y = Main.rand.Next(-10, 11) * 0.1f;
						spriteBatch.Draw(texture2, drawPos + dynamicAddition + new Vector2(x, y), null, color, pointEndTo2.ToRotation() + MathHelper.ToRadians(45), drawOrigin, scale, SpriteEffects.None, 0f);
					}
					currentSeg++;
					currentSegMult--;
				}
				if (j == 0)
					j = 3;
				else if (j == 3)
					j = 1;
				else if (j == 1)
					j = 2;
				else if (j == 2)
					j = 4;
			}
			DrawGlow(spriteBatch, lightColor);
			return true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/AdvisorPetEye");
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/AdvisorPetHighlight");
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 drawPos = projectile.Center - Main.screenPosition;

			float shootToX = fireToX - projectile.Center.X;
			float shootToY = fireToY - projectile.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));

			float reset = eyeReset;
			distance = reset * 1f * projectile.scale / distance;

			shootToX *= distance;
			shootToY *= distance;

			drawPos.X += shootToX;
			drawPos.Y += shootToY;
			Color color = new Color(100, 100, 100, 0);
			spriteBatch.Draw(texture, drawPos, null, projectile.GetAlpha(drawColor), projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			for(int k = 0; k < glow / 2; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(texture2, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color, 0f, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
		}
	}
}
