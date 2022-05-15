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
			Projectile.CloneDefaults(48);
            AIType = 48; 
			// Projectile.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			// Projectile.magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			// Projectile.melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 900;
			Projectile.alpha = 0;
		}
		public override void AI()
		{
			if(Projectile.timeLeft >= 800)
			{
				Projectile.scale = 0.01f * Main.rand.Next(96, 131);
				Projectile.timeLeft = Main.rand.Next(22, 44);
			}
		}
		public override void Kill(int timeLeft)
        {
			Vector2 position = Projectile.Center;
			SoundEngine.PlaySound(SoundID.NPCHit1, (int)position.X, (int)position.Y);  
			for(int i = 0; i < 20; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 39);
				Main.dust[num1].noGravity = true;
			}
			if(Projectile.owner == Main.myPlayer)
			{
				for(int i = 0; i < Main.rand.Next(2) + 2; i++)
				{ 
					Projectile proj = Projectile.NewProjectileDirect(Projectile.Center, Main.rand.NextVector2Circular(2, 2), ProjectileID.SporeCloud, (int)(Projectile.damage * 0.50f) + 1, 0, Projectile.owner);
					proj.timeLeft = Main.rand.Next(16, 35);
				}
			}
		}
	}
}
		
			