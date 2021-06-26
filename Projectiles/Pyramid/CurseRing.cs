using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.NPCs.Boss.Curse;

namespace SOTS.Projectiles.Pyramid
{    
    public class CurseRing : ModProjectile 
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
			projectile.timeLeft = 120;
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
					PharaohsCurse.SpawnPassiveDust(Main.projectileTexture[projectile.type], projectile.Center, 1.05f * projectile.scale, curse.foamParticleList1, 0.2f, 3, 24, distanceToOwner.ToRotation() + MathHelper.ToRadians(90));
				}
				else
				{
					projectile.Kill();
				}
			}
			projectile.scale *= 0.985f;
			projectile.velocity *= 1.0125f;
		}
	}
}
		