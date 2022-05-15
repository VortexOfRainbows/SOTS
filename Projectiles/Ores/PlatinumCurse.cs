using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Ores
{    
    public class PlatinumCurse : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platinum Curse");
		}
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18; 
            Projectile.timeLeft = 360;
            Projectile.penetrate = -1; 
            Projectile.friendly = false; 
            Projectile.hostile = false; 
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true; 
            Projectile.DamageType = DamageClass.Melee; 
            Projectile.aiStyle = 0; 
			Projectile.alpha = 0;
			Projectile.timeLeft = 100;
		}
		public override void AI()
		{
			Projectile.ai[1]++;
			int count = 0;
			Player player  = Main.player[Projectile.owner];
			if((int)Projectile.ai[0] != -1)
			{
				NPC npc = Main.npc[(int)Projectile.ai[0]];
				if(npc.active && !npc.friendly && npc.CanBeChasedBy())
				{
					for(int i = 0; i < Main.maxProjectiles; i++)
					{
						Projectile proj = Main.projectile[i];
						if(Projectile.type == proj.type && proj.active && Projectile.active && proj.ai[0] == Projectile.ai[0])
						{
							proj.ai[1] = Projectile.ai[1]; //syncing up attack 
							count++;
							if(proj.whoAmI == Projectile.whoAmI) break;
						}
					}
					if(count >= 10)
					{
						Projectile.Kill();
					}
					else
					{
						Vector2 circularPos = new Vector2(0, npc.height - 12f).RotatedBy(MathHelper.ToRadians((count -1)* 40));
						Projectile.position.X = circularPos.X + npc.Center.X - Projectile.width/2;
						Projectile.position.Y = circularPos.Y + npc.Center.Y - Projectile.height/2;
						Projectile.timeLeft = 6;
						if(Projectile.ai[1] % 90 == 10 * (count - 1))
						{
							LaunchLaser(npc.Center);
						}
					}
				}
				else
				{
					Projectile.ai[0] = -1;
				}
			}
			else
			{
				Projectile.Kill();
			}
		}
		public void LaunchLaser(Vector2 area)
		{
			Projectile proj = Projectile.NewProjectileDirect(Projectile.Center, Vector2.Zero, Mod.Find<ModProjectile>("BrightRedLaser").Type, (int)(Projectile.damage * 1f), 0, Projectile.owner, area.X, area.Y);
			proj.DamageType = DamageClass.Melee;
			proj.minion = false;
		}
	}
}
		
			