using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Nature
{    
    public class SporeBomb : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spore Bomb");
			Main.projFrames[Projectile.type] = 3;
		}
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(48);
            AIType = 48; 
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 20;
			Projectile.height = 22;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 900;
			Projectile.alpha = 0;
			Projectile.hide = true;	
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 14;
			height = 14;
			return true;
		}
		public override void AI()
		{
			if(Main.rand.NextBool(6))
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.JungleSpore);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.5f;
				Main.dust[num1].scale *= 1.4f;
			}
			Lighting.AddLight(Projectile.Center, new Vector3(0.05f, 0.3f, 0.1f));
			if(Projectile.timeLeft >= 800)
			{
				Projectile.spriteDirection = Projectile.direction;
				Projectile.hide = false;
				Projectile.frame = Main.rand.Next(3);
				Projectile.scale = 0.01f * Main.rand.Next(96, 131);
				Projectile.timeLeft = Main.rand.Next(22, 44);
			}
		}
		public override void Kill(int timeLeft)
        {
			Vector2 position = Projectile.Center;
			SOTSUtils.PlaySound(SoundID.NPCHit1, (int)position.X, (int)position.Y);  
			if(Main.netMode != NetmodeID.Server)
			{
				for (int i = 0; i < 20; i++)
				{
					int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.JungleGrass);
					Main.dust[num1].noGravity = true;
					if (Main.rand.NextBool(6))
					{
						num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.JungleSpore);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].velocity *= 0.75f;
						Main.dust[num1].scale *= 1.4f;
					}
				}
			}
			if(Projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < Main.rand.Next(2) + 2; i++)
				{ 
					Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Circular(2, 2), ProjectileID.SporeCloud, (int)(Projectile.damage * 0.50f) + 1, 0, Projectile.owner);
					proj.timeLeft = Main.rand.Next(16, 35);
				}
			}
		}
	}
}
		
			