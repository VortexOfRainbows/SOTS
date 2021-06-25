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
            projectile.width = 52;
            projectile.height = 50; 
            projectile.timeLeft = 20;
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
			int iFrame = 10;
			projectile.localNPCHitCooldown = iFrame;
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return true;
		}
		float counter = 225;
		float spinSpeed = 0;
        public override bool PreAI()
		{
			Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 8, 8, mod.DustType("CopyDust4"));
			dust.velocity *= 0.1f;
			dust.noGravity = true;
			dust.scale += 0.1f;
			dust.color = Color.White;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.6f;
			dust.alpha = projectile.alpha;
			return true;
        }
        public override void AI()
		{
			float mult = 1f;
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
				disX = player.Center.X - projectile.Center.X;
				disY = player.Center.Y - projectile.Center.Y;
				projectile.rotation = (float)Math.Atan2(disY, disX) + MathHelper.ToRadians(225f);
				projectile.Center = player.Center + ovalArea;
				spinSpeed = 4f;
			}
			counter += spinSpeed;
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
		
			