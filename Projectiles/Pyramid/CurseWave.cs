using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.NPCs.Boss.Curse;
using System.Collections.Generic;

namespace SOTS.Projectiles.Pyramid
{    
    public class CurseWave : ModProjectile 
    {	          
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Curse");
		}
        public override void SetDefaults()
        {
			projectile.height = 36;
			projectile.width = 36;
			projectile.friendly = false;
			projectile.timeLeft = 180;
			projectile.hostile = true;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.netImportant = true;
			projectile.tileCollide = true;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = (int)(36f * projectile.scale);
			hitbox = new Rectangle((int)projectile.Center.X - width / 2, (int)projectile.Center.Y - width / 2, width, width);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.velocity = -oldVelocity;
			return false;
		}
		public List<CurseFoam> foamParticleList1 = new List<CurseFoam>();
		public void cataloguePos()
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
		public override void AI()
		{
			int parentID = (int)projectile.ai[0];
			if (parentID >= 0 && Main.netMode != NetmodeID.Server)
			{
				NPC npc = Main.npc[parentID];
				if (npc.active && npc.type == ModContent.NPCType<PharaohsCurse>())
				{
					Vector2 OwnerPos = npc.Center;
					Vector2 distanceToOwner = projectile.Center - OwnerPos;
					PharaohsCurse curse = npc.modNPC as PharaohsCurse;
					PharaohsCurse.SpawnPassiveDust(Main.projectileTexture[projectile.type], projectile.Center, 1.1f * projectile.scale, foamParticleList1, 0.2f, 4, 28, projectile.velocity.ToRotation() + MathHelper.ToRadians(90));
				}
				else
				{
					projectile.Kill();
				}
			}
			cataloguePos();
			projectile.scale *= 0.998f;
			projectile.velocity *= 1.0075f;
			int direction = (int)projectile.ai[1];
			projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(1.5f * direction));
		}
	}
}
		