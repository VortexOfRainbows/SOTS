using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Buffs.WhipBuffs
{
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
        public override void HitEffect(NPC npc, int hitDirection, double damage)
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
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			// Only player attacks should benefit from this buff, hence the NPC and trap checks.
			if (!projectile.npcProj && !projectile.trap && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]))
			{
				if(npc.HasBuff<KelpWhipBuff>() && !npc.HasBuff<KelpWhipCooldown>())
				{
					crit = true;
					npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<KelpWhipBuff>()));
					npc.AddBuff(ModContent.BuffType<KelpWhipCooldown>(), 120, false);
				}
			}
		}
	}
}