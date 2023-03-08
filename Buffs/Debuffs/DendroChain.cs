using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Camera;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Buffs.Debuffs
{
    public class DendroChain : ModBuff //All the actual updates for this file will be ran in NPC
    {	
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.buffTime[buffIndex]++; //so the timer only goes down where it is supposed to!
        }
    }
    public static class DendroChainNPCOperators
    {
        public static int BuffType => ModContent.BuffType<DendroChain>();
        public const int DendroChainStandardDuration = 1200;
        public const float StandardPullRate = 0.2f; //Speed of pull rate in pixels per frame, which is lowered by distance until 0 at greater than 640 pixels (40 blocks)
        public const float maxPullDistance = 640f;
        public static void DrawFloralBloomImage(NPC npc)
        {
            if (npc.active && npc.HasBuff(BuffType))
            {
                int buffIndex = npc.FindBuffIndex(BuffType);
                int currentBuffTime = npc.buffTime[buffIndex];
                if (currentBuffTime < DendroChainStandardDuration)
                {
                    float progress = 1 - ((currentBuffTime - DendroChainStandardDuration + 30f) / 30f);
                    if (currentBuffTime < 30)
                    {
                        progress = currentBuffTime / 30f;
                    }
                    progress = Math.Clamp(progress, 0, 1);
                    Color color = DreamingFrame.Green1;
                    float scaleMult = 1.0f;
                    float starWindUp = progress;
                    if (starWindUp > 0)
                    {
                        scaleMult *= (1.4f - 0.4f * (float)Math.Cos(MathHelper.ToRadians(420 * starWindUp)));
                        SOTSProjectile.DrawStar(npc.Center + new Vector2(0, -2), color * 0.5f, starWindUp, 0f, 0f, 4, 4.5f * scaleMult, 2.0f * scaleMult, 1f, 180, 2.0f * scaleMult, 0);
                        SOTSProjectile.DrawStar(npc.Center + new Vector2(0, -2), color * 0.375f, starWindUp, MathHelper.PiOver4, 0f, 4, 4.25f * scaleMult, 2.75f * scaleMult, 1f, 180, 2.0f * scaleMult, 1);
                        Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Camera/CameraCenterCross");
                        for (int i = 0; i < 4; i++)
                        {
                            Vector2 circular = new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(90 * i));
                            Main.spriteBatch.Draw(texture, npc.Center + new Vector2(0, -2) + circular - Main.screenPosition, null, color * 1f * starWindUp * starWindUp, MathHelper.PiOver4, texture.Size() / 2, scaleMult * 0.5f, SpriteEffects.None, 0);
                        }
                    }
                }
            }
        }
        public static void DrawChainsBetweenNPC(NPC npc, SpriteBatch spriteBatch)
        {
            if (npc.active && npc.HasBuff(BuffType))
            {
                Texture2D textureGradient = (Texture2D)ModContent.Request<Texture2D>("SOTS/Assets/DuelGradient"); //This could be swapped for a more suitable chain texture in the future, but it looks decent and fits the current asthetic
                Color color = DreamingFrame.Green1;
                int maxConnections = 6;
                int currentBuffTime = npc.buffTime[npc.FindBuffIndex(BuffType)];
                if (currentBuffTime < DendroChainStandardDuration)
                {
                    for (int i = npc.whoAmI + 1; i < 200; i++) //prevent chains from drawing on the side of both NPCs
                    {
                        NPC otherNPC = Main.npc[i];
                        if (otherNPC.active && otherNPC.HasBuff(BuffType))
                        {
                            int otherBuffTime = otherNPC.buffTime[otherNPC.FindBuffIndex(BuffType)];
                            if(otherBuffTime < DendroChainStandardDuration)
                            {
                                float progress = 1 - ((currentBuffTime - DendroChainStandardDuration + 30f) / 30f);
                                float otherProgress = 1 - ((otherBuffTime - DendroChainStandardDuration + 30f) / 30f);
                                if (currentBuffTime < 30)
                                {
                                    progress = currentBuffTime / 30f;
                                }
                                if (otherBuffTime < 30)
                                {
                                    otherProgress = otherBuffTime / 30f;
                                }
                                progress *= otherProgress * (maxConnections / 6f);
                                progress = Math.Clamp(progress, 0, 1);
                                if (progress > 0)
                                {
                                    Vector2 toOther = otherNPC.Center - npc.Center;
                                    float dist = toOther.Length();
                                    float multiplier = Math.Clamp((1 - (dist / maxPullDistance)), 0, 1) * progress * (1.0f - 0.5f * (float)Math.Cos(MathHelper.ToRadians(2 * currentBuffTime)));
                                    spriteBatch.Draw(textureGradient, npc.Center - Main.screenPosition, null, color * (0.3f * multiplier), toOther.ToRotation(), new Vector2(1, 1), new Vector2(1f / textureGradient.Width * dist, 1f + 1f * (1 - multiplier)), SpriteEffects.None, 0);
                                    maxConnections--;
                                }
                            }
                        }
                        if (maxConnections <= 0)
                            break;
                    }
                }
            }
        }
        public static void PullOtherNPCs(NPC npc)
        {
            if(npc.HasBuff(BuffType))
            {
                for(int i = 0; i < 200; i++)
                {
                    NPC otherNPC = Main.npc[i];
                    if(otherNPC.active && otherNPC.HasBuff(BuffType) && npc.whoAmI != i)
                    {
                        float pullRate = StandardPullRate;
                        if (otherNPC.boss || Common.GlobalNPCs.DebuffNPC.miniBosses.Contains(otherNPC.type))
                        {
                            pullRate *= 0.2f;
                        }
                        Vector2 toThis = npc.Center - otherNPC.Center;
                        float dist = toThis.Length();
                        toThis = toThis.SafeNormalize(Vector2.Zero);
                        if (Math.Abs(otherNPC.velocity.Y) <= 0.33f)
                            toThis.Y *= 0.0f;
                        toThis *= MathHelper.Lerp(pullRate, 0, Math.Clamp(dist / maxPullDistance, 0, 1));
                        if (dist < 64)
                            toThis *= MathHelper.Lerp(0, 1, dist / 64f);
                        bool pullThroughTiles = false;
                        if (npc.noTileCollide)
                            pullThroughTiles = true;
                        if(!pullThroughTiles)
                            toThis = Collision.TileCollision(otherNPC.position, toThis, otherNPC.width, otherNPC.height, true);
                        otherNPC.position += toThis;
                    }
                }
            }
        }
        public static void HurtOtherNPCs(NPC npc, int DamageToDeal)
        {
            if (DamageToDeal > 0)
            {
                for (int i = 0; i < 200; i++)
                {
                    NPC otherNPC = Main.npc[i];
                    if (otherNPC.active && otherNPC.HasBuff(BuffType) && npc.whoAmI != i)
                    {
                        Vector2 toOther = otherNPC.Center - npc.Center;
                        float dist = toOther.Length();
                        float damageMultiplier = 1f - dist / maxPullDistance;
                        if(damageMultiplier > 0.0f)
                        {
                            int damage = (int)(DamageToDeal * damageMultiplier) + 1;
                            if(Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, toOther.SafeNormalize(Vector2.Zero), ModContent.ProjectileType<DreamLaser>(), damage, 0, Main.myPlayer, i, dist);
                            }
                        }
                    }
                }
            }
        }
        public static void InitiateNPCDamageStats(NPC npc, ref int outDamage)
        {
            if(npc.HasBuff(BuffType))
            {
                int buffIndex = npc.FindBuffIndex(BuffType);
                int currentBuffTime = npc.buffTime[buffIndex];
                if(currentBuffTime > DendroChainStandardDuration)
                {
                    int damageToHave = currentBuffTime - DendroChainStandardDuration;
                    outDamage = damageToHave;
                    npc.buffTime[buffIndex] = DendroChainStandardDuration;
                }
                else
                {
                    npc.buffTime[buffIndex]--;
                }
            }
        }
    }
}