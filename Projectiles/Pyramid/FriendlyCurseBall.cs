using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{    
    public class FriendlyCurseBall : ModProjectile 
    {	          
		int bounce = 999;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curse");
		}
        public override void SetDefaults()
        {
			projectile.height = 18;
			projectile.width = 18;
			projectile.friendly = true;
			projectile.timeLeft = 2400;
			projectile.hostile = false;
			projectile.alpha = 155;
			projectile.penetrate = 5;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(projectile.alpha);
			writer.Write(projectile.timeLeft);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			projectile.alpha = reader.ReadInt32();
			projectile.timeLeft = reader.ReadInt32();
		}
		public override void AI()
		{
			if(Main.myPlayer == projectile.owner)
				projectile.netUpdate = true;

			if (projectile.velocity.Length() > 1)
				projectile.tileCollide = true;
			else
				projectile.tileCollide = false;
			projectile.alpha -= 1;
			projectile.rotation += Main.rand.Next(-3,4);
			if(projectile.timeLeft <= 200)
			{
				projectile.alpha += 2;
				if(projectile.alpha >= 200)
				{
					projectile.Kill();
				}
			}
			int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 18, 18, mod.DustType("CurseDust"));
			Main.dust[num1].noGravity = true;
			Main.dust[num1].alpha = projectile.alpha;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			projectile.timeLeft -= 120;
			bounce--;
			if (bounce <= 0)
			{
				projectile.Kill();
			}
			else
			{
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
		}
	}
}
		