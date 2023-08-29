using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace SOTS.Buffs.WhipBuffs
{
    public class GlowWhipDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
        }
    }
    public class KelpWhipBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
		}
	}
	public class KelpWhipCooldown : ModBuff
	{
		public override void SetStaticDefaults()
		{
			BuffID.Sets.IsAnNPCWhipDebuff[Type] = true;
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			if(npc.HasBuff<KelpWhipBuff>())
				npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<KelpWhipBuff>()));
		}
	}
	public class WhipDebuffNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public bool RunOnceKelpWhip = false;
		public override void ResetEffects(NPC npc)
		{
			if (!npc.HasBuff<KelpWhipCooldown>())
				RunOnceKelpWhip = false;
		}
        public override void HitEffect(NPC npc, NPC.HitInfo hit)
		{
			if (npc.HasBuff<KelpWhipCooldown>())
			{
				if (!RunOnceKelpWhip)
				{
					SOTSUtils.PlaySound(SoundID.Grass, npc.Center, 1.2f, -0.1f, 0.05f);
					int maxDust = (int)(12 + npc.Size.Length() / 3);
					for (int i = 0; i < maxDust; i++)
					{
						Vector2 circular = new Vector2(4.7f, 0).RotatedBy(i * MathHelper.TwoPi / maxDust);
						Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Grass);
						dust.velocity *= 0.4f;
						dust.noGravity = true;
						dust.velocity += circular * Main.rand.NextFloat(0.8f, 1f);
						dust.scale *= 0.3f;
						dust.scale += 1.4f;
						dust.color = new Color(83, 113, 14) * 1.5f;
					}
				}
				RunOnceKelpWhip = true;
			}
		}
        // Inconsistent with vanilla, increasing damage AFTER it is randomised, not before. Change to a different hook in the future.
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			// Only player attacks should benefit from this buff, hence the NPC and trap checks.
			if (!projectile.npcProj && !projectile.trap && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]))
			{
				if(npc.HasBuff<KelpWhipBuff>() && !npc.HasBuff<KelpWhipCooldown>())
				{
					modifiers.SetCrit();
					npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<KelpWhipBuff>()));
					npc.AddBuff(ModContent.BuffType<KelpWhipCooldown>(), 120, false);
                }
            }
		}
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            // Only player attacks should benefit from this buff, hence the NPC and trap checks.
            if (!projectile.npcProj && !projectile.trap && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]))
            {
                if (npc.HasBuff<GlowWhipDebuff>())
                {
					int count = 4;
					if (projectile.type == ProjectileID.Smolstar)
                        count = 2;
                    for (int i = 0; i < count; i++)
                    {
						Projectile.NewProjectile(new EntitySource_OnHit(projectile, npc), npc.Center, new Vector2(1, 0) * hit.HitDirection, ModContent.ProjectileType<Projectiles.Earth.Glowmoth.IlluminationSparkle>(), 1, 1f, Main.myPlayer, npc.whoAmI, 3 + 4 * i);
                    }
                }
            }
        }
    }
}