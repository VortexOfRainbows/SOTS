using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Nature
{    
    public class Peanut : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Peanut");
		}
        public override void SetDefaults()
        {
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.timeLeft = 3000;
			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = false;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 12;
			height = 12;
            return true;
        }
        public override bool PreAI()
		{
			if (Projectile.velocity.X > 0)
			{
				Projectile.rotation -= MathHelper.Pi;
				Projectile.spriteDirection = -1;
			}
			else
			{
				Projectile.spriteDirection = 1;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + (float)MathHelper.Pi / 4 * Projectile.spriteDirection;
			if (Projectile.velocity.X < 0)
			{
				Projectile.rotation -= MathHelper.Pi;
			}
			return base.PreAI();
        }
        public override void AI()
		{
			Projectile.velocity.Y += 0.09f;
			Projectile.alpha = 0;
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 5; i++)
			{
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), 26, 26, DustID.Dirt);
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), 26, 26, DustID.WoodFurniture);
			}
			if(Main.myPlayer == Projectile.owner)
			{
				Vector2 location = new Vector2(Projectile.Center.X, Projectile.Center.Y - 480);
				Vector2 fromLocation = location + new Vector2(1920 * (Main.rand.Next(2) * 2 - 1), Main.rand.NextFloat(-128, 128));
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), fromLocation, Vector2.Zero, ModContent.ProjectileType<PinkyBomber>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, location.X, location.Y);
			}
		}
	}
}
		