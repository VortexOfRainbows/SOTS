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
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(48);
            aiType = 48; 
			projectile.thrown = false;
			projectile.magic = false;
			projectile.melee = false;
			projectile.ranged = true;
			projectile.width = 14;
			projectile.height = 14;
			projectile.penetrate = 1;
			projectile.timeLeft = 900;
			projectile.alpha = 0;
		}
		public override void AI()
		{
			if(projectile.timeLeft >= 800)
			{
				projectile.scale = 0.01f * Main.rand.Next(96, 131);
				projectile.timeLeft = Main.rand.Next(22, 44);
			}
		}
		public override void Kill(int timeLeft)
        {
			Vector2 position = projectile.Center;
			Main.PlaySound(SoundID.NPCHit1, (int)position.X, (int)position.Y);  
			for(int i = 0; i < 20; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 39);
				Main.dust[num1].noGravity = true;
			}
			if(projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < Main.rand.Next(2) + 2; i++)
				{ 
					Projectile proj = Projectile.NewProjectileDirect(projectile.Center, Main.rand.NextVector2Circular(2, 2), ProjectileID.SporeCloud, (int)(projectile.damage * 0.50f) + 1, 0, projectile.owner);
					proj.timeLeft = Main.rand.Next(16, 35);
				}
			}
		}
	}
}
		
			