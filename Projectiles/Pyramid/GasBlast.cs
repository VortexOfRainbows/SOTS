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
			Projectile.width = 12;
			Projectile.height = 34;
			Projectile.friendly = true;
			Projectile.timeLeft = 80;
			Projectile.hostile = false;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.tileCollide = false;
			Projectile.hide = true;
			Projectile.magic = true;
			Projectile.extraUpdates = 2;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 80 * (Projectile.extraUpdates + 1); //nerf immunity ignoring to make it less overpowered on single target
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[Projectile.owner];
			target.immune[player.whoAmI] = 0;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = (int)(20f * Projectile.scale);
			hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
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
						particle.position += Projectile.velocity * 0.925f;
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
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override bool PreAI()
		{
			if (runOnce)
			{
				SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 62, 0.7f, 0.4f);
				Direction1 = Projectile.ai[1];
				Direction2 = Direction1;
				runOnce = false;
			}
			float veloMult = 1.01f;
			float rotateMod = 0.9f;
			Vector2 temp = Projectile.velocity;
			Projectile.velocity = Collision.TileCollision(Projectile.Center - new Vector2(4, 4), Projectile.velocity, 8, 8, true, true);
			Projectile.Center += Projectile.velocity;
			if (Projectile.velocity != temp)
			{
				if (Projectile.velocity.X != temp.X)
					Projectile.velocity.X = -temp.X;
				if (Projectile.velocity.Y != temp.Y)
					Projectile.velocity.Y = -temp.Y;
				Direction2 = -Direction2;
			}
			Projectile.velocity *= veloMult;
			Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(rotateMod * Direction2));
			return base.PreAI();
        }
        public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			if(player.active)
			{
				SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
				for (int j = 0; j < 24; j++)
				{
					Vector2 rotational = new Vector2(0, -Main.rand.NextFloat(0.75f, 2f)).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f))) + Projectile.velocity * 0.5f;
					modPlayer.foamParticleList1.Add(new CurseFoam(Projectile.Center, rotational, 1.15f, true));
				}
				for(int j = 0; j < foamParticleList1.Count; j++)
				{
					foamParticleList1[j].noMovement = true;
					modPlayer.foamParticleList1.Add(foamParticleList1[j]);
				}
			}
			foamParticleList1 = null;
			//SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 62, 0.9f, 0.45f);
		}
        public override void AI()
		{
			if(foamParticleList1 != null)
			{
				PharaohsCurse.SpawnPassiveDust(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, Projectile.Center, 1.0f * Projectile.scale, foamParticleList1, 0.12f, 4, 27, Projectile.velocity.ToRotation() + MathHelper.ToRadians(90));
				for (float i = 0; i < 1; i += 0.2f)
				{
					foamParticleList1.Add(new CurseFoam(Projectile.Center - Projectile.velocity * i, new Vector2(Main.rand.NextFloat(-0.15f, 0.15f), Main.rand.NextFloat(-0.15f, 0.15f)), 0.45f, true));
				}
				catalogueParticles();
			}
			Projectile.scale *= 0.995f; 
		}
	}
}
		