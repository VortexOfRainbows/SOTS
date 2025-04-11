using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.ModPlayers;
using SOTS.Dusts;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Fragments;
using SOTS.Projectiles.AbandonedVillage;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.AbandonedVillage
{
	public class CoalCart : ModNPC
	{
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Position = new Vector2(0, 24),
            };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }
        public override void SetDefaults()
		{
			NPC.aiStyle = NPCAIStyleID.Unicorn;
			NPC.width = 62;
			NPC.height = 78;
			NPC.lifeMax = 60;
			NPC.damage = 16;
            NPC.value = Item.buyPrice(0, 0, 3, 0);
            NPC.scale = 1.0f;
			NPC.defense = 18;
			NPC.knockBackResist = 0.35f;
			NPC.HitSound = SoundID.NPCHit4 with { Pitch = -0.1f };
			NPC.DeathSound = new SoundStyle("SOTS/Sounds/Tiles/WoodBreaking") with { Pitch = 0.3f };
			NPC.noTileCollide = false;
            NPC.noGravity = false;
            NPC.localAI[3] = 145; //This is good starting position for the legs in the bestiary
            //Banner = NPC.type;
            //BannerItem = ItemType<TeratomaBanner>();
        }
        public void DrawArmIK(SpriteBatch spriteBatch, Vector2 screenPos, int dir)
        {
            Texture2D leg = Request<Texture2D>(this.Texture + "Leg").Value;
            Texture2D foot = Request<Texture2D>(this.Texture + "Foot").Value;
            Texture2D toes = Request<Texture2D>(this.Texture + "Toes").Value;
            int j = SOTSUtils.SignNoZero(dir);
            int xOffset = MathF.Abs(dir) == 2 ? 36 : 0;
            float sideOffset = (-17 + xOffset) * j;
            Vector2 legOrigin = new Vector2(0, 0);
            Vector2 revLegOrigin = new Vector2(leg.Width - legOrigin.X, legOrigin.Y);
            Vector2 footOrigin = new Vector2(13, foot.Height);
            Vector2 revFootOrigin = new Vector2(foot.Width - footOrigin.X, footOrigin.Y);
            Vector2 armPosition = new Vector2(sideOffset, -6);
            armPosition = armPosition.RotatedBy(NPC.rotation) + NPC.Center;
            Color drawColor = Color.White;

            float r = NPC.localAI[3] * 1.0f * j + dir * 180;
            //float outwardSize = (isBigArm ? 72 : 38) - 16 * MathF.Sin(MathHelper.ToRadians(r + 90 * j));
            Vector2 targetLegPos = new Vector2(sideOffset, NPC.height / 2 - 6).RotatedBy(NPC.rotation);
            targetLegPos = targetLegPos + NPC.Center;
            Vector2 circular = new Vector2(40, 0).RotatedBy(MathHelper.ToRadians(r));
            circular = circular.RotatedBy(NPC.rotation);
            if (circular.Y > 0)
            {
                circular.Y *= 0.2f;
            }
            else
                circular.Y *= 0.5f;
            targetLegPos += circular;

            float maxDifference = 0;
            float A = foot.Height; //size of hand
            float B = leg.Height; //size of arm
            float maxSize = A + B;
            Vector2 end = targetLegPos;
            Vector2 start = armPosition;
            if (end.Distance(start) < maxDifference)
            {
                end = start + (end - start).SNormalize() * maxDifference;
            }
            Vector2 startToEnd = end - start;
            float C = startToEnd.Length();
            float angleA = C - A - B > 0 ? 0 : MathF.Acos((B * B + C * C - A * A) / (2 * B * C));
            float angleB = C - A - B > 0 ? 0 : MathF.Acos((A * A + C * C - B * B) / (2 * A * C));
            Vector2 endToMid = -startToEnd.RotatedBy(-angleB * j);
            Vector2 startToMid = startToEnd.RotatedBy(angleA * j);
            float endHandRot = endToMid.ToRotation();
            float endArmRot = startToMid.ToRotation();
            end -= new Vector2(2, -9 * j).RotatedBy(endArmRot);
            if (end.Distance(start) > maxSize)
            {

            }
            Vector2 mid = startToMid.SNormalize() * B + start;
            mid -= new Vector2(2, -9 * j).RotatedBy(endArmRot);
            float stretch = MathF.Max(1, end.Distance(mid) / A);
            //Visual representations of the IK happening
            //spriteBatch.Draw(SOTSUtils.WhitePixel, end - screenPos, null, drawColor, endHandRot, new Vector2(0, 1), new Vector2(A * 0.5f, 2), SpriteEffects.None, 0);
            //spriteBatch.Draw(SOTSUtils.WhitePixel, start - screenPos, null, drawColor, 0, Vector2.One, NPC.scale * 4, SpriteEffects.None, 0);
            //spriteBatch.Draw(SOTSUtils.WhitePixel, start - screenPos, null, drawColor, endArmRot, new Vector2(0, 1), new Vector2(B * 0.5f, 2), SpriteEffects.None, 0);

            spriteBatch.Draw(foot, end - screenPos, null, drawColor, endHandRot + MathHelper.PiOver2, j == -1 ? footOrigin : revFootOrigin, new Vector2(1, stretch), j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(leg, start - screenPos, null, drawColor, endArmRot + MathHelper.Pi * 1.5f, j == -1 ? legOrigin : revLegOrigin, NPC.scale, j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            footOrigin = new Vector2(10, 16);
            revFootOrigin = new Vector2(foot.Width - footOrigin.X, footOrigin.Y);
            end += new Vector2(6, 4 * j).RotatedBy(endHandRot);
            spriteBatch.Draw(toes, end - screenPos, null, drawColor, (endHandRot + MathHelper.PiOver2) * 0.4f, j == -1 ? footOrigin : revFootOrigin, new Vector2(1, stretch), j == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            //spriteBatch.Draw(SOTSUtils.WhitePixel, end - screenPos, null, drawColor, 0, Vector2.One, NPC.scale * 2, SpriteEffects.None, 0);
            if (!Main.gameInactive && !Main.gamePaused)
            {
                PixelDust.Spawn(end + new Vector2(Main.rand.NextFloat(-8, 8), 4), 0, 0, Main.rand.NextVector2Circular(0.5f, 0.5f) + new Vector2(NPC.velocity.X, NPC.velocity.Y * 0.1f), new Color(132, 42, 0), 12).scale = Main.rand.NextFloat(1.25f, 1.6f);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            Texture2D head = Request<Texture2D>(this.Texture + "Head").Value;
            Texture2D glow = Request<Texture2D>(this.Texture + "Glow").Value;
            Texture2D hot = Request<Texture2D>(this.Texture + "CoalHot").Value;
            int height = texture.Height / Main.npcFrameCount[NPC.type];
            Vector2 drawOrigin = new Vector2(texture.Width / 2, height / 2);
            Vector2 drawPos = NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY);
            Rectangle frame = new Rectangle(0, NPC.frame.Y, texture.Width, height);
            float sin = MathF.Sin(MathHelper.ToRadians(NPC.localAI[3] * 2.0f));
            float sin2 = -MathF.Sin(MathHelper.ToRadians(NPC.localAI[3] * 1.0f));
            Vector2 bobbing = new Vector2(0, 2 * sin);
            drawPos += bobbing;
            DrawArmIK(spriteBatch, screenPos - bobbing, 2 * NPC.spriteDirection);
            float r = NPC.rotation + MathHelper.ToRadians(5 * sin2);
            spriteBatch.Draw(head, drawPos, null, drawColor, r, drawOrigin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            spriteBatch.Draw(glow, drawPos, null, Color.White, r, drawOrigin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            float percent = 1 - (float)NPC.life / NPC.lifeMax;
            Color c = new Color(100, 100, 100, 0) * percent * percent;
            for (int i = 0; i < 4; ++i)
            {
                Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.PiOver2 * i + MathHelper.ToRadians(SOTSWorld.GlobalCounter * 2));
                spriteBatch.Draw(hot, drawPos + circular, null, c, r, drawOrigin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
            spriteBatch.Draw(hot, drawPos, null, c, r, drawOrigin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            DrawArmIK(spriteBatch, screenPos - bobbing, NPC.spriteDirection);
            //DrawArmIK(spriteBatch, screenPos, -1);
            return false;
        }
        public override bool PreAI()
        {
            Vector2 trueVelo = NPC.position - NPC.oldPosition;
            float speed = trueVelo.Length();
            if (speed < 1000)
            {
                NPC.localAI[3] += 4.5f * MathF.Sqrt(MathF.Max(0.5f, MathF.Abs(NPC.velocity.X) - MathF.Abs(NPC.velocity.Y)));
            }
            if(NPC.velocity.Y < 0)
            {
                NPC.velocity.Y *= 0.98f;
            }
            return true;
        }
        public override void AI()
		{
			NPC.TargetClosest(true);
            Vector2 toPlayer = Main.player[NPC.target].Center - NPC.Center;
            NPC.spriteDirection = MathF.Abs(NPC.velocity.X) > 2 ? SOTSUtils.SignNoZero(NPC.velocity.X) : SOTSUtils.SignNoZero(toPlayer.X);
            if (MathF.Sign(toPlayer.X) != SOTSUtils.SignNoZero(NPC.velocity.X))
            {
                NPC.velocity.X += MathF.Sign(toPlayer.X) * 0.1f;
            }
		}
        public override void PostAI()
        {

        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (NPC.life <= 0)
                {
                    int count = 3;
                    if (Main.expertMode)
                        count += Main.rand.Next(2);
                    if (Main.masterMode)
                        count += Main.rand.Next(3);
                    for (int i = 1; i < count + 1; i++)
                    {
                        Vector2 RandomVelocity = new Vector2(Main.rand.NextFloat(-i, i) * 0.5f, Main.rand.NextFloat(-9f, -4f)) + NPC.velocity * Main.rand.NextFloat(0.5f, 1.0f);
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y - 35), RandomVelocity, ProjectileType<CoalCartCoal>(), NPC.GetBaseDamage() / 2, 0f, Main.myPlayer, i - 1, Main.rand.NextFloat(0.8f, 1f));
                    }
                }
                else if(NPC.life < NPC.lifeMax * 3 / 4)
                {
                    int count = 3;
                    if (Main.expertMode)
                        count += Main.rand.Next(2);
                    if (Main.masterMode)
                        count += Main.rand.Next(2);
                    for (int i = 1; i < count; ++i)
                    {
                        float secondaryChance = 0.04f * hit.Damage;
                        int chance = Main.masterMode ? 4 : Main.expertMode ? 5 : 6;
                        if (Main.rand.NextBool(chance) && Main.rand.NextFloat() < secondaryChance)
                        {
                            float percent = 1 - (float)NPC.life / NPC.lifeMax;
                            Vector2 RandomVelocity = new Vector2(Main.rand.NextFloat(-i, i) * 0.5f, Main.rand.NextFloat(-6f, -3f)) + NPC.velocity * Main.rand.NextFloat(0.5f, 1.0f);
                            Projectile.NewProjectile(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.Center.Y - 35), RandomVelocity, ProjectileType<CoalCartCoal>(), NPC.GetBaseDamage() / 2, 0f, Main.myPlayer, 0, Main.rand.NextFloat(0.0f, percent));
                        }
                    }
                }
            }
            if (Main.netMode == NetmodeID.Server)
                return;
            int num = NPC.life > 0 ? (int)(hit.Damage / (float)NPC.lifeMax * 30f) : 60;
            if(NPC.life < 0)
            {
                string dir = "Gores/CoalCart/CoalCartGore";
                Vector2 velo = new Vector2(NPC.velocity.X * 0.5f + hit.HitDirection, NPC.velocity.Y * 0.2f - 1);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, velo, ModGores.GoreType($"{dir}1"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(40, 0), velo, ModGores.GoreType($"{dir}2"), 1f);

                for (int i = 0; i < Main.rand.Next(1, 4); ++i)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 22), velo, ModGores.GoreType($"{dir}3"), 1f);
                for (int i = 0; i < Main.rand.Next(1, 4); ++i)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(26, 22), velo, ModGores.GoreType($"{dir}4"), 1f);
                for (int i = 0; i < Main.rand.Next(1, 4); ++i)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(4, 30), velo, ModGores.GoreType($"{dir}5"), 1f);
                for (int i = 0; i < Main.rand.Next(1, 4); ++i)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(40, 16), velo, ModGores.GoreType($"{dir}6"), 1f);
                for (int i = 0; i < Main.rand.Next(1, 4); ++i)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(40, 30), velo, ModGores.GoreType($"{dir}7"), 1f);

                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(6, 34), velo, ModGores.GoreType($"{dir}8"), 1f);
                for(int i = 0; i < 2; ++i)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(24, 34), velo, ModGores.GoreType($"{dir}8"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(42, 34), velo, ModGores.GoreType($"{dir}8"), 1f);
            }
            for (; num > 0; --num)
            {
                if(Main.rand.NextBool(2))
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<CharredWoodDust>(), hit.HitDirection, -1, NPC.alpha, Scale: 1.35f);
                }
                else
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Silver, hit.HitDirection, -1, NPC.alpha, Scale: 1.15f);
                    if (Main.rand.NextBool(2))
                        Dust.NewDust(NPC.position, NPC.width, 4, DustID.Stone, hit.HitDirection, -Main.rand.NextFloat(1, 4), NPC.alpha, Scale: 1.2f, newColor: new Color(155, 155, 155));
                }
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemType<AncientSteelBar>(), 2, 1, 2));
            npcLoot.Add(ItemDropRule.Common(ItemType<CharredWood>(), 2, 1, 20));
            npcLoot.Add(ItemDropRule.Common(ItemType<SootBlock>(), 2, 1, 20));
            npcLoot.Add(ItemDropRule.Common(ItemType<FragmentOfEarth>(), 2, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ItemType<FragmentOfEvil>(), 2, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ItemType<OldKey>(), 20));
            npcLoot.Add(ItemDropRule.Common(ItemID.Minecart, 100));
            npcLoot.Add(ItemDropRule.Common(ItemID.Coal, 100));
        }
    }
}