using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.NPCs.Boss.Curse;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Pyramid;

namespace SOTS.Projectiles.Pyramid
{    
    public class GasBlast : ModProjectile 
    {	          
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gas Blast");
		}
        public override void SetDefaults()
        {
			projectile.width = 12;
			projectile.height = 34;
			projectile.friendly = true;
			projectile.timeLeft = 80;
			projectile.hostile = false;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.netImportant = true;
			projectile.tileCollide = false;
			projectile.hide = true;
			projectile.magic = true;
			projectile.extraUpdates = 2;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 80 * (projectile.extraUpdates + 1); //nerf immunity ignoring to make it less overpowered on single target
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			target.immune[player.whoAmI] = 0;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = (int)(20f * projectile.scale);
			hitbox = new Rectangle((int)projectile.Center.X - width / 2, (int)projectile.Center.Y - width / 2, width, width);
		}
		public List<CurseFoam> foamParticleList1 = new List<CurseFoam>();
		public void catalogueParticles()
		{
			for (int i = 0; i < foamParticleList1.Count; i++)
			{
				CurseFoam particle = foamParticleList1[i];
				particle.Update();
				if (!particle.active)
				{
					particle = null;
					foamParticleList1.RemoveAt(i);
					i--;
				}
				else
				{
					particle.Update();
					if (!particle.active)
					{
						particle = null;
						foamParticleList1.RemoveAt(i);
						i--;
					}
					else if (!particle.noMovement)
						particle.position += projectile.velocity * 0.925f;
				}
			}
		}
		bool runOnce = true;
		float Direction1 = 0;
		float Direction2 = 0;
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }
        public override bool PreAI()
		{
			if (runOnce)
			{
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 62, 0.7f, 0.4f);
				Direction1 = projectile.ai[1];
				Direction2 = Direction1;
				runOnce = false;
			}
			float veloMult = 1.01f;
			float rotateMod = 0.9f;
			Vector2 temp = projectile.velocity;
			projectile.velocity = Collision.TileCollision(projectile.Center - new Vector2(4, 4), projectile.velocity, 8, 8, true, true);
			projectile.Center += projectile.velocity;
			if (projectile.velocity != temp)
			{
				if (projectile.velocity.X != temp.X)
					projectile.velocity.X = -temp.X;
				if (projectile.velocity.Y != temp.Y)
					projectile.velocity.Y = -temp.Y;
				Direction2 = -Direction2;
			}
			projectile.velocity *= veloMult;
			projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(rotateMod * Direction2));
			return base.PreAI();
        }
        public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			if(player.active)
			{
				SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
				for (int j = 0; j < 24; j++)
				{
					Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(0.75f, 2f)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f))) + projectile.velocity * 0.5f;
					modPlayer.foamParticleList1.Add(new CurseFoam(projectile.Center, rotational, 1.15f, true));
				}
				for(int j = 0; j < foamParticleList1.Count; j++)
				{
					foamParticleList1[j].noMovement = true;
					modPlayer.foamParticleList1.Add(foamParticleList1[j]);
				}
			}
			foamParticleList1 = null;
			//Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 62, 0.9f, 0.45f);
		}
        public override void AI()
		{
			if(foamParticleList1 != null)
			{
				PharaohsCurse.SpawnPassiveDust(Main.projectileTexture[projectile.type], projectile.Center, 1.0f * projectile.scale, foamParticleList1, 0.12f, 4, 27, projectile.velocity.ToRotation() + MathHelper.ToRadians(90));
				for (float i = 0; i < 1; i += 0.2f)
				{
					foamParticleList1.Add(new CurseFoam(projectile.Center - projectile.velocity * i, new Vector2(Main.rand.NextFloat(-0.15f, 0.15f), Main.rand.NextFloat(-0.15f, 0.15f)), 0.45f, true));
				}
				catalogueParticles();
			}
			projectile.scale *= 0.995f; 
		}
	}
}
		