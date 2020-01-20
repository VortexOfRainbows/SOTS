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

namespace SOTS.Projectiles.Nature
{    
    public class GrowTree : ModProjectile 
    {	
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tree Grow");
			
		}
		
        public override void SetDefaults()
        {
			projectile.width = 72;
			projectile.height = 64;
            Main.projFrames[projectile.type] = 5;
			projectile.penetrate = -1;
			projectile.timeLeft = 60;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.ranged = false;
			projectile.magic = true;
            projectile.netImportant = true;
			projectile.alpha = 35;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(projectile.rotation);
			writer.Write(projectile.spriteDirection);
			writer.Write(projectile.frame);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			projectile.rotation = reader.ReadSingle();
			projectile.spriteDirection = reader.ReadInt32();
			projectile.frame = reader.ReadInt32();
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) 
		{
			return true;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) 
		{
			Vector2 direction = new Vector2(24,0).RotatedBy(projectile.rotation - MathHelper.ToRadians(90));
			
			if(direction.X > 0)
			hitDirection = 1;
		
			if(direction.X < 0)
			hitDirection = -1;
		}
		private bool loaded = false;
		public override bool PreAI()
		{
			if(!loaded)
			{
				projectile.netUpdate = true;
				loaded = true;
			}
			return true;
		}
		public override void AI()
        {
			Player player = Main.player[projectile.owner];
			projectile.alpha += 2;
			float distance = 24 - projectile.ai[1]/3;
			if(projectile.ai[0] == 1)
			{
				distance = 18 - projectile.ai[1]/3;
			}
			if(projectile.ai[0] == 9)
			{
				distance = 38 - projectile.ai[1]/3;
			}
			Vector2 direction = new Vector2(distance,0).RotatedBy(projectile.rotation - MathHelper.ToRadians(90 - projectile.ai[1]));
			if(projectile.timeLeft < 58)
			{
				if(projectile.ai[0] != 10 && !projectile.friendly)
				{
					if(projectile.owner == Main.myPlayer)
					{
						
						int Probe = Projectile.NewProjectile(projectile.Center.X + direction.X, projectile.Center.Y + direction.Y, 0, 0, mod.ProjectileType("GrowTree"), projectile.damage, projectile.knockBack, player.whoAmI, projectile.ai[0] + 1, projectile.ai[1]);
						Main.projectile[Probe].rotation = projectile.rotation - projectile.ai[1];
						
						if(Main.projectile[Probe].ai[0] != 10)
						Main.projectile[Probe].frame = 4;
								
								
						if(Main.projectile[Probe].ai[0] == 1)
						{
							Main.projectile[Probe].frame = 3;
						}
						
						
						if(Main.projectile[Probe].ai[0] >= 5 && Main.projectile[Probe].ai[0] <= 9 && Main.projectile[Probe].frame == 4)
						{
							int type = Main.rand.Next(3);
						
						
							if(type == 0 && projectile.frame != 1)
							Main.projectile[Probe].frame = 1;
						
							if(type == 1 && projectile.frame != 2)
							Main.projectile[Probe].frame = 2;
						}
					}
				}
				projectile.friendly = true;
			}
				
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough) 
		{
			width = 4;
			height = 4;
			fallThrough = true;
			return true;
		}
		public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 15; i++)
			{
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 2);
			Main.dust[num1].noGravity = true;
			}
		}
	}
}
		