using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Void;
using Steamworks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Buffs.Debuffs
{
    public class EmberOiled : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                bool triggeredOnce = false;
                for (int i = 0; i < npc.buffType.Length; i++)
                {
                    int buffID = npc.buffType[i];
                    int damage = CalculateDamageBasedOnDebuff(npc, buffID);
                    if (damage > 1)
                    {
                        Projectile.NewProjectile(npc.GetSource_Buff(buffIndex), npc.Center, Vector2.Zero, ModContent.ProjectileType<EmberDamage>(), 1, 0, Main.myPlayer, npc.whoAmI, damage - 1, buffID);
                        npc.DelBuff(npc.FindBuffIndex(buffID));
                        i--;
                        triggeredOnce = true;
                    }
                }
                if (triggeredOnce)
                    npc.DelBuff(npc.FindBuffIndex(ModContent.BuffType<EmberOiled>()));
            }
            for (int i = 0; i < Main.rand.Next(1, 3); i++)
            {
                Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, ModContent.DustType<SootDust>());
                if(i == 0)
                    dust.noGravity = true;
                dust.velocity *= 0.6f;
                dust.velocity.Y -= 0.5f;
                dust.scale = dust.scale * 0.3f + 0.6f;
            }
        }
        public int CalculateDamageBasedOnDebuff(NPC npc, int buffID)
        {
            if(npc.HasBuff(buffID))
            {
                int index = npc.FindBuffIndex(buffID);
                int duration = npc.buffTime[index];
                if(duration > 0)
                {
                    int dot = 0;
                    if (buffID == BuffID.OnFire)
                        dot = 4;
                    if (buffID == BuffID.OnFire3)
                        dot = 15;
                    if (buffID == BuffID.CursedInferno)
                        dot = 24;
                    if (buffID == BuffID.ShadowFlame)
                        dot = 15;
                    if (buffID == BuffID.Frostburn)
                        dot = 8;
                    if (buffID == BuffID.Frostburn2)
                        dot = 25;
                    if(npc.HasBuff(BuffID.Oiled))
                    {
                        dot += 25;
                    }
                    return (int)(dot * duration / 60f);
                }
            }
            return 0;
        }
    }
    public class EmberDamage : ModProjectile
    {
        public int GetDustType()
        {
            int type = (int)Projectile.ai[2];
            if (type == BuffID.OnFire || type == BuffID.OnFire3)
            {
                return DustID.Torch;
            }
            if (type == BuffID.Frostburn || type == BuffID.Frostburn2)
            {
                return DustID.IceTorch;
            }
            if (type == BuffID.CursedInferno)
            {
                return DustID.CursedTorch;
            }
            if (type == BuffID.ShadowFlame)
            {
                return DustID.Shadowflame;
            }
            return -1;
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.alpha = 255;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
        }
        public override string Texture => "SOTS/Buffs/Debuffs/EmberOiled";
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return target.whoAmI == (int)Projectile.ai[0];
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.DisableCrit();
            modifiers.FinalDamage += Projectile.ai[1];
        }
        public override void OnKill(int timeLeft)
        {
            DustCircle();
        }
        public void DustCircle()
        {
            int type = GetDustType();
            if(type > 0)
            {
                for (int i = 0; i < 50; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, type);
                    if(Main.rand.NextBool(3))
                        dust2.noGravity = false;
                    else
                    {
                        Vector2 circular = new Vector2(2, 0).RotatedBy(i / 50f * MathHelper.TwoPi);
                        dust2.velocity += circular;
                        if (!Main.rand.NextBool(3))
                            dust2.noGravity = true;
                        else
                            dust2.velocity *= 0.5f;
                    }
                    dust2.velocity *= 2f;
                    dust2.scale = dust2.scale * 0.25f + 1.5f;
                }
            }
            SOTSUtils.PlaySound(SoundID.Item73, Projectile.Center, 1.1f, -0.3f);
        }
    }
}