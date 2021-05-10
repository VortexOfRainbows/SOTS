using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;

namespace SOTS.Projectiles.Celestial
{    
    public class PlasmaCutter : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Time to die");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;    
		}        
		public override void SetDefaults()
        {
            projectile.width = 52;
            projectile.height = 50; 
            projectile.timeLeft = 360000;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.melee = true; 
			projectile.alpha = 0;
			projectile.extraUpdates = 2;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 40;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int iFrame = (int)(28f - spinSpeed);
			if (iFrame < 4)
				iFrame = 4;
			projectile.localNPCHitCooldown = 1 + iFrame * 3;
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player  = Main.player[projectile.owner];
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Celestial/PlasmaCutterGlow");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) 
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(new Color(100, 155, 100, 0)) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				float disX = player.Center.X - projectile.oldPos[k].X;
				float disY = player.Center.Y - projectile.oldPos[k].Y;
				float rotation2 = (float)Math.Atan2(disY,disX) + MathHelper.ToRadians(225f);
				spriteBatch.Draw(texture, drawPos, null, color, rotation2, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			texture = ModContent.GetTexture("SOTS/Projectiles/Celestial/PlasmaCutterChain");  
            Vector2 position = projectile.Center;
            Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
            Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            float num1 = (float)texture.Height;
            Vector2 vector2_4 = mountedCenter - position;
            float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;
            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                flag = false;
            while (flag)
            {
                if ((double)vector2_4.Length() < (double)num1 + 1.0)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    vector2_4 = mountedCenter - position;
                    Color color2 = projectile.GetAlpha(new Color(100, 100, 100, 0));
					for(int i = 0; i < 5; i ++)
					{
						float x = Main.rand.NextFloat(-1f, 1f);
						float y = Main.rand.NextFloat(-1f, 1f);
						Main.spriteBatch.Draw(texture, position - Main.screenPosition + new Vector2(x, y), sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
					}
                }
            }
			return true;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Celestial/PlasmaCutterGlow");
			Vector2 origin = new Vector2(texture.Width / 2, projectile.height / 2);
			Color color = new Color(100, 155, 100, 0);
			for (int i = 0; i < 360; i += 60)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
				Main.spriteBatch.Draw(texture, projectile.Center + circular - Main.screenPosition, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), color * ((255f - projectile.alpha) / 255f), projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			}
        }
		float counter = 225;
		float spinSpeed = 0;
		float counter2 = 0;
        bool ReturnTo = false;
		public override void AI()
		{
			if(counter2 < 45)
				counter2++;
			float mult = counter2 / 45f;
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.2f / 255f, (255 - projectile.alpha) * 1.1f / 255f, (255 - projectile.alpha) * 0.4f / 255f);
			Player player  = Main.player[projectile.owner];
			if(player.whoAmI == Main.myPlayer)
			{
				projectile.netUpdate = true;
				Vector2 cursorArea = Main.MouseWorld;
					
				float cursorX = cursorArea.X - player.Center.X;
				float cursorY = cursorArea.Y - player.Center.Y;
				float disX = player.Center.X - projectile.Center.X;
				float disY = player.Center.Y - projectile.Center.Y;
				float distance = Vector2.Distance(player.Center, cursorArea);
				if (distance < 48)
					distance = 48;
				if (distance > 896)
					distance = 896;
				projectile.rotation = (float)Math.Atan2(disY,disX) + MathHelper.ToRadians(225f);
				
				double deg = counter; 
				double rad = deg * (Math.PI / 180);

				Vector2 ovalArea = new Vector2(distance * 0.25f * mult, 0).RotatedBy((float)Math.Atan2(cursorY,cursorX));
				Vector2 ovalArea2 = new Vector2(distance * 0.75f * mult, 0).RotatedBy((float)rad);
				ovalArea2.Y *= 0.175f;
				ovalArea2 = ovalArea2.RotatedBy((float)Math.Atan2(cursorY,cursorX));
				ovalArea.X += ovalArea2.X;
				ovalArea.Y += ovalArea2.Y;
				if(player.channel && !ReturnTo)
				{
					projectile.position.X = player.Center.X + ovalArea.X - projectile.width/2;
					projectile.position.Y = player.Center.Y + ovalArea.Y - projectile.height/2;
				}
				else
				{
					ReturnTo = true;
					Vector2 newVelocity = new Vector2(5, 0).RotatedBy(Math.Atan2(disY, disX));
					projectile.velocity = newVelocity;
					if(Math.Abs(disX) < 22f && Math.Abs(disY) < 22f)
					{
						projectile.Kill();
					}
				}
				disX = player.Center.X - projectile.Center.X;
				disY = player.Center.Y - projectile.Center.Y;
				projectile.rotation = (float)Math.Atan2(disY, disX) + MathHelper.ToRadians(225f);
				spinSpeed = 1 + (2574f / distance);
			}
			counter += spinSpeed;
		}
		int storeData = -1;
		int storeData2 = -1;
		public override void PostAI()
		{
			if (storeData == -1 && projectile.owner == Main.myPlayer)
			{
				storeData = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("PlasmaCutterTrail"), (int)(projectile.damage * 1f) + 1, projectile.knockBack * 0.5f, projectile.owner, 16, projectile.whoAmI);
				projectile.ai[1] = storeData;
				projectile.netUpdate = true;
			}
			if (storeData2 == -1 && projectile.owner == Main.myPlayer)
			{
				storeData2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("PlasmaCutterTrail"), (int)(projectile.damage * 1f) + 1, projectile.knockBack * 0.5f, projectile.owner, -16, projectile.whoAmI);
				projectile.ai[0] = storeData2;
				projectile.netUpdate = true;
			}
		}
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(counter);
			writer.Write(spinSpeed);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			counter = reader.ReadSingle();
			spinSpeed = reader.ReadSingle();
		}
    }
}
		
			