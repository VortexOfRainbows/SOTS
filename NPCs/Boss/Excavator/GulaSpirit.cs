using System;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Evil;
using SOTS.Projectiles.Minions;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Boss.Excavator
{
	public class GulaSpirit : ModNPC
	{
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.TrailCacheLength[NPC.type] = 5;  
			NPCID.Sets.TrailingMode[NPC.type] = 0;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
        }
		public override void SetDefaults()
		{
			NPC.aiStyle = -1;
            NPC.lifeMax = 450; 
            NPC.damage = 50; 
            NPC.defense = 0;   
            NPC.knockBackResist = 0f;
            NPC.width = 80;
            NPC.height = 80;
            NPC.value = 32500;
            NPC.npcSlots = 4f;
            NPC.boss = false;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.Item62 with { Pitch = -0.1f };
            NPC.netAlways = true;
			NPC.rarity = 4;
			NPC.dontTakeDamage = true;
		}
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
		{
			NPC.damage = (int)(NPC.damage * 7 / 10);
			NPC.lifeMax = (int)(NPC.lifeMax * 5 / 6);
		}
		public override void SendExtraAI(BinaryWriter writer)
		{

		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{

		}
        public bool runOnce = true;
		public override bool PreAI()
        {
            if(runOnce)
            {
                NPC.ai[0] = -180;
                runOnce = false;
            }
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];
			NPC.timeLeft = 300000;
			NPC.ai[0]++;
			float percent = MathHelper.Clamp(NPC.ai[0] / 360f, 0, 1);
			float iPer = 1 - percent;
			Vector2 toPlayer = player.Center - NPC.Center;

            if (NPC.ai[0] > 0)
            {
                NPC.velocity += toPlayer.SNormalize() * (toPlayer.Length() * 0.0004f + 0.25f);
                NPC.velocity *= 0.1f + 0.9f * iPer;
            }
            else
            {
                NPC.velocity += toPlayer.SNormalize() * (toPlayer.Length() * 0.0001f + 0.1f);
                NPC.velocity *= 0.985f;
            }

            Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, ModContent.DustType<CopyDust4>());
            dust.color = Color.Lerp(ColorHelper.EarthColor, ColorHelper.RedEvilColor, percent);
            dust.noGravity = true;
            dust.fadeIn = 0.1f;
            dust.scale *= 1.8f;
            dust.alpha = NPC.alpha;
            NPC.scale = 0.8f + 0.2f * percent;
            if (NPC.ai[0] > 90)
			{
				NPC.ai[1] += 0.1f + percent * 2f;
                if (NPC.ai[1] >= 20)
				{
					NPC.ai[1] -= 20;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 spawnPos = Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(80f, 100), Main.rand.NextFloat(80f, 100));
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + spawnPos, (toPlayer - spawnPos).SNormalize(),
                            ModContent.ProjectileType<EvilEye>(), NPC.GetBaseDamage() / 2, 0, Main.myPlayer);
                    }
                }
                NPC.ai[3] += 0.1f + percent * 3f;
                if (NPC.ai[3] > 80)
                {
                    NPC.ai[3] -= 80;
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 6; ++i)
                        {
                            Vector2 circular = new Vector2(0, 1).RotatedBy(MathHelper.ToRadians(i * 60 + NPC.ai[0]));
                            Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center + circular * 64, 3.5f * circular,
                                ModContent.ProjectileType<EvilBolt>(), NPC.GetBaseDamage() / 2, 0, Main.myPlayer);
                        }
                    }
                }
                if (NPC.ai[0] >= 200)
                {
                    percent = NPC.ai[2] / 220f * percent;
                    NPC.scale += (MathF.Sin(percent * MathF.PI) - percent * 0.25f) * 0.25f;
                    NPC.ai[2]++;
                }
            }
            if (NPC.ai[0] >= 420 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.StrikeInstantKill();
			}
            return true;
		}
		public override void AI()
		{	
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            Vector2 drawOrigin = texture.Size() / 2;
            for (int k = 0; k < NPC.oldPos.Length; k++) {
				Vector2 drawPos = NPC.oldPos[k] - screenPos + NPC.Size / 2;
				Color color = drawColor * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f * ((255 - NPC.alpha) / 255f), NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			}
			return false;
		}	
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life <= 0)
			{
				if (Main.netMode != NetmodeID.Server)
                {
                    for (int i = 0; i < 240; i++)
                    {
                        float r = MathHelper.ToRadians(i * 1.5f);
                        float sin = MathF.Sin(r * 8) * 0.1f + 0.9f;
                        Vector2 circular = new Vector2(sin, 0).RotatedBy(r);
                        Dust d = Dust.NewDustDirect(NPC.Center - new Vector2(4, 4), 0, 0, DustID.RainbowMk2);
                        d.velocity *= 0.1f;
                        d.velocity += circular * 12 * Main.rand.NextFloat(0.9f, 1.1f);
                        d.color = ColorHelper.RedEvilColor;
                        d.noGravity = true;
                        d.fadeIn = 0.1f;
                        d.scale *= 2.5f;
                        if(Main.rand.NextBool(3))
                        {
                            d = Dust.NewDustDirect(NPC.Center - new Vector2(4, 4), 0, 0, DustID.RainbowMk2);
                            d.velocity *= 0.1f;
                            d.velocity += circular * Main.rand.NextFloat(12);
                            d.color = ColorHelper.RedEvilColor;
                            d.noGravity = true;
                            d.fadeIn = 0.1f;
                            d.scale *= 2.5f;
                        }
                    }
                    if (SOTS.Config.screenShake)
                    {
                        PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2(), 16f, 6f, 20, 1000f);
                        Main.instance.CameraModifiers.Add(modifier);
                    }
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    Main.BestiaryTracker.Kills.RegisterKill(NPC);
            }
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            float percent = MathF.Min(1, NPC.ai[0] / 360f);
            float percent2 = NPC.ai[2] / 220f * percent * 0.5f;
            float alphaScale = 2 - NPC.scale;
			float iAlphaScale = 1 - alphaScale;
            Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Texture2D overlay = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/Excavator/GulaSpiritCover").Value;
			Color color = Color.Lerp(new(100, 100, 100, 0), new Color(1, 0, 0, 0.2f + iAlphaScale), percent) * alphaScale * alphaScale * (1 - percent2);
			Vector2 drawOrigin = texture.Size() / 2;
            float shakeNum = 1 + 4 * percent * MathF.Sin(percent * MathF.PI);
            Vector2 drawPos = NPC.Center - screenPos + Main.rand.NextVector2Circular(shakeNum, shakeNum);
            for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.NextFloat(-0.3f, 0.3f);
				float y = Main.rand.NextFloat(-0.3f, 0.3f);
				spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, NPC.GetAlpha(color), 0f, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
            }
            color = new Color(255, 100, 100, 120) * (0.5f + 0.5f * percent) * alphaScale * alphaScale * (1 - percent2);
            for (int k = 0; k < 6; k++)
            {
				Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(k * 60 + SOTSWorld.GlobalCounter));
                spriteBatch.Draw(overlay, drawPos + circular, null, NPC.GetAlpha(color), 0f, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
            }
            spriteBatch.Draw(overlay, drawPos, null, NPC.GetAlpha(ColorHelper.RedEvilColor), 0f, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
        }
		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DissolvingEarth>(), 1, 3, 3));
        }
	}
}