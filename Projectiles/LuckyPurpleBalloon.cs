using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class LuckyPurpleBalloon : ModProjectile 
    {	int wait = 0;
		int plusY = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lucky Purple Balloon");
			Main.projFrames[Projectile.type] = 1;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.LightPet[Projectile.type] = true;
		}
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(198);
            AIType = 198;
			Projectile.netImportant = true;
            Projectile.width = 18;
            Projectile.height = 34; 
            Projectile.timeLeft = 255;
            Projectile.penetrate = -1; 
            Projectile.friendly = true; 
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true; 
            Projectile.light = 0.5f;
		}
        public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			player.hornet = false; // Relic from AIType
            return true;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
            if (player.dead || (player.ownedProjectileCounts[Mod.Find<ModProjectile>("LuckyPurpleBalloon").Type] > 1 && Projectile.alpha < 30))
            {
                modPlayer.PurpleBalloon = false;
            }
            if (modPlayer.PurpleBalloon)
            {
                Projectile.timeLeft = 2;
            }
			wait++;
			if(wait % 100 == 0)
			{
				plusY = 2;
			}
			if(wait % 100 == 20)
			{
				plusY = 0;
			}
			if(wait % 100 == 40)
			{
				plusY = -2;
			}
			if(wait % 100 == 60)
			{
				plusY = 0;
			}
			if(wait % 100 == 80)
			{
				plusY = 2;
			}
			Projectile.velocity *= 0.1f;
			Projectile.position.X = player.Center.X - Projectile.width/2;
			Projectile.position.Y = player.Center.Y - Projectile.height/2;
			Projectile.position.X -= (float)(27 * Main.player[Projectile.owner].direction);
            float gravDir = Main.player[Projectile.owner].gravDir;
            Projectile.position.Y -= (40f + plusY) * gravDir;
			Projectile.position.Y += Main.player[Projectile.owner].gfxOffY;
			Projectile.rotation = player.velocity.X * 0.03f;
			if(gravDir == -1)
			{
				Projectile.rotation = MathHelper.ToRadians(180) + (player.velocity.X * -0.03f);
			}
		}
	}
	
}
