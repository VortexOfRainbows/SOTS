using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Ores
{    
    public class PlatinumDart : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platinum Dart");
			
		}
		
        public override void SetDefaults()
        {
			projectile.aiStyle = 1;
			projectile.width = 26;
			projectile.height = 26;
            projectile.magic = true;
			projectile.penetrate = 2;
			projectile.ranged = false;
			projectile.alpha = 0; 
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 9000;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(projectile.rotation);
			writer.Write(projectile.spriteDirection);
			//writer.Write(damageCounter);
			writer.Write(latch);
			writer.Write(enemyIndex);
			writer.Write(diffPosX);
			writer.Write(diffPosY);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			projectile.rotation = reader.ReadSingle();
			projectile.spriteDirection = reader.ReadInt32();
			//damageCounter = reader.ReadInt32();
			latch = reader.ReadBoolean();
			enemyIndex = reader.ReadInt32();
			diffPosX = reader.ReadSingle();
			diffPosY = reader.ReadSingle();
		}
		bool latch = false;
		int enemyIndex = -1;
		float diffPosX = 0;
		float diffPosY = 0;
		public override void AI()
        {
			Player player = Main.player[projectile.owner];
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(45);
			projectile.spriteDirection = 1;
			
			if(latch && enemyIndex != -1)
			{
				projectile.netUpdate = true;
				NPC target = Main.npc[enemyIndex];
				projectile.alpha += projectile.timeLeft % 2 == 0 ? 1 : 0;
				if(projectile.alpha >= 210)
				{
					projectile.Kill();
				}
				if(target.active && !target.friendly)
				{
					projectile.aiStyle = 0;
					projectile.position.X = target.Center.X - projectile.width/2 - diffPosX;
					projectile.position.Y = target.Center.Y - projectile.height/2 - diffPosY;
					target.velocity *= target.boss ? 0.998f : 0.985f;
				}
				else
				{
					projectile.Kill();
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[projectile.owner];
			projectile.friendly = false;
            target.immune[projectile.owner] = 0;
			projectile.tileCollide = false;
			latch = true;
			
			if(diffPosX == 0)
			diffPosX = target.Center.X - projectile.Center.X;
				
			if(diffPosY == 0)
			diffPosY = target.Center.Y - projectile.Center.Y;
				
			enemyIndex = target.whoAmI;
			
			if(target.life <= 0)
			{
				projectile.Kill();
			}
        }
		public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 15; i++)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 16);
			Main.dust[num1].noGravity = true;
			}
		}
	}
}