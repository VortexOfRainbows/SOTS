using System.IO;
using Microsoft.Xna.Framework;
using SOTS.Dusts;
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
			Projectile.height = 18;
			Projectile.width = 18;
			Projectile.friendly = true;
			Projectile.timeLeft = 2400;
			Projectile.hostile = false;
			Projectile.alpha = 155;
			Projectile.penetrate = 5;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(Projectile.alpha);
			writer.Write(Projectile.timeLeft);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			Projectile.alpha = reader.ReadInt32();
			Projectile.timeLeft = reader.ReadInt32();
		}
		public override void AI()
		{
			if(Main.myPlayer == Projectile.owner)
				Projectile.netUpdate = true;
			if (Projectile.velocity.Length() > 1)
				Projectile.tileCollide = true;
			else
				Projectile.tileCollide = false;
			Projectile.alpha -= 1;
			Projectile.rotation += Main.rand.Next(-3,4);
			if(Projectile.timeLeft <= 200)
			{
				Projectile.alpha += 2;
				if(Projectile.alpha >= 200)
				{
					Projectile.Kill();
				}
			}
			int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 18, 18, ModContent.DustType<CurseDust>());
			Main.dust[num1].noGravity = true;
			Main.dust[num1].alpha = Projectile.alpha;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			Projectile.timeLeft -= 120;
			bounce--;
			if (bounce <= 0)
			{
				Projectile.Kill();
			}
			else
			{
				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
		}
	}
}
		