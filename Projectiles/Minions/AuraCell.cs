using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
 
namespace SOTS.Projectiles.Minions
{
    public class AuraCell : ModProjectile
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aura Cell");
			
		}
        public override void SetDefaults()
        {
            projectile.width = 38;
            projectile.height = 38; 
            projectile.hostile = false; 
            projectile.friendly = false; 
            projectile.ignoreWater = true;  
            Main.projFrames[projectile.type] = 1;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.penetrate = -1;
            projectile.tileCollide = false;
			projectile.netImportant = true;
			projectile.sentry = true;
        }
		float sphereRadius = 215f;
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) 
		{
			//SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			spriteBatch.Draw(mod.GetTexture("Gores/CircleAura"), projectile.Center - Main.screenPosition, null, Color.Red * (30f / 255f), 0f, new Vector2(300f, 300f), sphereRadius / 300f, SpriteEffects.None, 0f);
			spriteBatch.Draw(mod.GetTexture("Gores/CircleBorder"), projectile.Center - Main.screenPosition, null, Color.DarkRed * 0.35f, 0f, new Vector2(300f, 300f), sphereRadius / 300f, SpriteEffects.None, 0f);
			return true;
		}
        public override void AI()
		{
			Main.player[projectile.owner].UpdateMaxTurrets();
			if(projectile.owner == Main.myPlayer)
				projectile.netUpdate = true;

			Player player = Main.player[projectile.owner];
			sphereRadius = 255f + (150f * (player.minionDamage - 1f)) + (150f * (player.allDamage - 1f));
			Lighting.AddLight(projectile.Center, sphereRadius / 255f, sphereRadius / 255f, sphereRadius / 255f);
			if(projectile.ai[0] >= 10)
			{
				projectile.ai[0] = -85;
			}
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC target = Main.npc[i];
				if((projectile.Center - target.Center).Length() <= sphereRadius + 4f && !target.friendly && target.active)
				{
					if(projectile.ai[0] == 9 && Main.myPlayer == projectile.owner)
					{
						int proj = Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, mod.ProjectileType("RedExplosion"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
					}
				}
            }
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player target = Main.player[i];
				if((projectile.Center - target.Center).Length() <= sphereRadius + 4f && target.active)
				{
					target.AddBuff(mod.BuffType("AuraBoost"), 630, false);
				}
            }
            projectile.ai[0] += 1;
        }
    }
}