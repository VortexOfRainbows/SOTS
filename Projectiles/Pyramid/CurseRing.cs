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
			Projectile.height = 36;
			Projectile.width = 36;
			Projectile.friendly = false;
			Projectile.timeLeft = 120;
			Projectile.hostile = true;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.tileCollide = true;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = (int)(36f * Projectile.scale);
			hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity = -oldVelocity;
			return false;
		}
		public override void AI()
		{
			int parentID = (int)Projectile.ai[0];
			if (parentID >= 0)
			{
				NPC npc = Main.npc[parentID];
				if (npc.active && npc.type == ModContent.NPCType<PharaohsCurse>())
				{
					if (Main.netMode != NetmodeID.Server)
					{
						Vector2 OwnerPos = npc.Center;
						Vector2 distanceToOwner = Projectile.Center - OwnerPos;
						PharaohsCurse curse = npc.ModNPC as PharaohsCurse;
						PharaohsCurse.SpawnPassiveDust(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, Projectile.Center, 1.05f * Projectile.scale, curse.foamParticleList1, 0.2f, 3, 30, distanceToOwner.ToRotation() + MathHelper.ToRadians(90));
					}
				}
				else
				{
					Projectile.Kill();
				}
			}
			Projectile.scale *= 0.985f;
			Projectile.velocity *= 1.0125f;
		}
	}
}
		