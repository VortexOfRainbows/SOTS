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

namespace SOTS.Projectiles.Celestial
{    
    public class StarCluster : ModProjectile 
    {	
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Cluster");
			
		}
		
        public override void SetDefaults()
        {
			projectile.height = 54;
			projectile.width = 58;
            Main.projFrames[projectile.type] = 8;
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.timeLeft = 960;
			projectile.tileCollide = true;
			projectile.hostile = true;
			projectile.alpha = 45;
			projectile.netImportant = true;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(projectile.timeLeft);
			writer.Write(projectile.active);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			projectile.timeLeft = reader.ReadInt32();
			projectile.active = reader.ReadBoolean();
		}
		public override void AI()
        {
			if(Main.netMode != 1)
			{
				projectile.netUpdate = true;
			}
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 2.5f / 255f, (255 - projectile.alpha) * 1.6f / 255f, (255 - projectile.alpha) * 2.4f / 255f);
			
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 54, 58, 226);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].alpha = projectile.alpha;
			
			num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 54, 58, 205);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].alpha = projectile.alpha;
			
			if(projectile.timeLeft % 40 == 20)
			{
				Projectile.NewProjectile(projectile.Center.X + Main.rand.Next(-50,51), projectile.Center.Y + Main.rand.Next(-50,51), Main.rand.Next(-2,3), Main.rand.Next(-2,3), mod.ProjectileType("plusBeam"), projectile.damage, 0, 0);
			}
			if(projectile.timeLeft % 40 == 0)
			{
				Projectile.NewProjectile(projectile.Center.X + Main.rand.Next(-50,51), projectile.Center.Y + Main.rand.Next(-50,51), Main.rand.Next(-2,3), Main.rand.Next(-2,3), mod.ProjectileType("XBeam"), projectile.damage, 0, 0);
			}
            projectile.frameCounter++;
            if (projectile.frameCounter >= 4)
            {
				projectile.friendly = false;
                projectile.frameCounter = 0;
                projectile.frame = (projectile.frame + 1) % 8;
            }
        }
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}
	}
}
		