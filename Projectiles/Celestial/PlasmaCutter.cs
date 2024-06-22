using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;
using SOTS.Items.Planetarium.Furniture;

namespace SOTS.Projectiles.Celestial
{    
    public class PlasmaCutter : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Time to die");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;    
		}        
		public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 50; 
            Projectile.timeLeft = 360000;
            Projectile.penetrate = -1; 
            Projectile.friendly = true; 
            Projectile.hostile = false; 
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true; 
            Projectile.DamageType = DamageClass.Melee; 
			Projectile.alpha = 0;
			Projectile.extraUpdates = 2;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 40;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			int iFrame = (int)(28f - spinSpeed);
			if (iFrame < 4)
				iFrame = 4;
			Projectile.localNPCHitCooldown = 1 + iFrame * 3;
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
		}
        public override bool PreDraw(ref Color lightColor)
		{
			Player player  = Main.player[Projectile.owner];
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/PlasmaCutterGlow");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) 
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(new Color(100, 155, 100, 0)) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				float disX = player.Center.X - Projectile.oldPos[k].X;
				float disY = player.Center.Y - Projectile.oldPos[k].Y;
				float rotation2 = (float)Math.Atan2(disY,disX) + MathHelper.ToRadians(225f);
				Main.spriteBatch.Draw(texture, drawPos, null, color, rotation2, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/PlasmaCutterChain");  
            Vector2 position = Projectile.Center;
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
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
                    Color color2 = Projectile.GetAlpha(new Color(100, 100, 100, 0));
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
        public override void PostDraw(Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/PlasmaCutterGlow");
			Vector2 origin = new Vector2(texture.Width / 2, Projectile.height / 2);
			Color color = new Color(100, 155, 100, 0);
			for (int i = 0; i < 360; i += 60)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
				Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color * ((255f - Projectile.alpha) / 255f), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
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
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.2f / 255f, (255 - Projectile.alpha) * 1.1f / 255f, (255 - Projectile.alpha) * 0.4f / 255f);
			Player player  = Main.player[Projectile.owner];
			if(player.whoAmI == Main.myPlayer)
			{
				Projectile.netUpdate = true;
				Vector2 cursorArea = Main.MouseWorld;
					
				float cursorX = cursorArea.X - player.Center.X;
				float cursorY = cursorArea.Y - player.Center.Y;
				Projectile.direction = Math.Sign(cursorX);
				player.direction = Projectile.direction;
				float disX = player.Center.X - Projectile.Center.X;
				float disY = player.Center.Y - Projectile.Center.Y;
				float distance = Vector2.Distance(player.Center, cursorArea);
				if (distance < 48)
					distance = 48;
				if (distance > 896)
					distance = 896;
				Projectile.rotation = (float)Math.Atan2(disY,disX) + MathHelper.ToRadians(225f);
				
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
					Projectile.Center = player.Center + ovalArea;
				}
				else
				{
					ReturnTo = true;
					Vector2 newVelocity = new Vector2(5, 0).RotatedBy(Math.Atan2(disY, disX));
					Projectile.velocity = newVelocity;
					if(Math.Abs(disX) < 22f && Math.Abs(disY) < 22f)
					{
						Projectile.Kill();
					}
				}
				disX = player.Center.X - Projectile.Center.X;
				disY = player.Center.Y - Projectile.Center.Y;
				Projectile.rotation = (float)Math.Atan2(disY, disX) + MathHelper.ToRadians(225f);
				spinSpeed = 1 + (2574f / distance);
			}
			if(player.channel)
			{
				if(player.itemAnimation <= 2)
				{
					player.itemAnimation = 2;
                }
                if (player.itemTime <= 2)
                {
                    player.itemTime = 2;
                }
                Vector2 toProjectile = Projectile.Center - player.Center;
                player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, 0f);
                player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.WrapAngle(player.gravDir * toProjectile.ToRotation() + MathHelper.ToRadians(-90)));
            }
			counter += spinSpeed;
		}
		int storeData = -1;
		int storeData2 = -1;
		public override void PostAI()
		{
			if (storeData == -1 && Projectile.owner == Main.myPlayer)
			{
				storeData = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, Mod.Find<ModProjectile>("PlasmaCutterTrail").Type, (int)(Projectile.damage * 1f) + 1, Projectile.knockBack * 0.5f, Projectile.owner, 16, Projectile.identity);
				Projectile.ai[1] = storeData;
				Projectile.netUpdate = true;
			}
			if (storeData2 == -1 && Projectile.owner == Main.myPlayer)
			{
				storeData2 = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, Mod.Find<ModProjectile>("PlasmaCutterTrail").Type, (int)(Projectile.damage * 1f) + 1, Projectile.knockBack * 0.5f, Projectile.owner, -16, Projectile.identity);
				Projectile.ai[0] = storeData2;
				Projectile.netUpdate = true;
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
		
			