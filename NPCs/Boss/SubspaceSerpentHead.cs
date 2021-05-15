using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Dusts;
using SOTS.Projectiles.Celestial;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.NPCs.Boss
{[AutoloadBossHead]
    public class SubspaceSerpentHead : ModNPC
    {
        float ai1 = 240;
        private float ai2
        {
            get => npc.ai[1];
            set => npc.ai[1] = value;
        }

        private float ai3
        {
            get => npc.ai[2];
            set => npc.ai[2] = value;
        }

        private float ai4
        {
            get => npc.ai[3];
            set => npc.ai[3] = value;
        }
        int phase = 0;
        int despawn = 0;
        Vector2 directVelo;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Subspace Serpent");
        }
        public override void SetDefaults()
        {
            npc.aiStyle = 0;
            npc.lifeMax = 130000;
            npc.damage = 100;
            npc.defense = 50;
            npc.knockBackResist = 0f;
            npc.width = 48;
            npc.height = 38;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath32;
            npc.value = 100000;
            npc.npcSlots = 25;
            npc.netAlways = true;
            npc.target = -1;
            music = MusicID.Boss2;
            for (int i = 0; i < Main.maxBuffTypes; i++)
            {
                npc.buffImmune[i] = true;
            }
            //npc.aiStyle = 6;
            bossBag = mod.ItemType("SubspaceBag");
            Main.npcFrameCount[npc.type] = 8;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !npc.dontTakeDamage;
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override void BossLoot(ref string name, ref int potionType)
        {
            Player player = Main.player[npc.target];
            SOTSWorld.downedSubspace = true;
            potionType = ItemID.GreaterHealingPotion;
            npc.position = player.position;
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SanguiteBar"), Main.rand.Next(16, 25));
            }
        }
        public override bool PreAI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (npc.ai[0] == 0)
                {
                    npc.realLife = npc.whoAmI;
                    int latestNPC = npc.whoAmI;
                    int randomWormLength = 50;
                    for (int i = 0; i < randomWormLength; ++i)
                    {
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SubspaceSerpentBody"), npc.whoAmI, 0, latestNPC);
                        Main.npc[latestNPC].realLife = npc.whoAmI;
                        Main.npc[latestNPC].ai[3] = npc.whoAmI;
                    }
                    latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SubspaceSerpentTail"), npc.whoAmI, 0, latestNPC);
                    Main.npc[latestNPC].realLife = npc.whoAmI;
                    Main.npc[latestNPC].ai[3] = npc.whoAmI;

                    // We're setting npc.ai[0] to 1, so that this 'if' is not triggered again.
                    npc.ai[0] = 1;
                    npc.netUpdate = true;
                }
            }
            if (phase == 0)
                DoWormStuff();
            return true;
        }
        public void DoWormStuff()
        {
            float speed = 17.5f;
            float acceleration = 0.2f;
            Vector2 npcCenter = npc.Center;
            Vector2 targetPos = Main.player[npc.target].Center;
            float targetRoundedPosX = targetPos.X;
            float targetRoundedPosY = targetPos.Y;
            float dirX = targetRoundedPosX - npcCenter.X;
            float dirY = targetRoundedPosY - npcCenter.Y;
            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            float absDirX = Math.Abs(dirX);
            float absDirY = Math.Abs(dirY);
            float newSpeed = speed / length;
            dirX = dirX * newSpeed;
            dirY = dirY * newSpeed;
            if (directVelo.X > 0.0 && dirX > 0.0 || directVelo.X < 0.0 && dirX < 0.0 || (directVelo.Y > 0.0 && dirY > 0.0 || directVelo.Y < 0.0 && dirY < 0.0))
            {
                if (directVelo.X < dirX)
                    directVelo.X = directVelo.X + acceleration;
                else if (directVelo.X > dirX)
                    directVelo.X = directVelo.X - acceleration;
                if (directVelo.Y < dirY)
                    directVelo.Y = directVelo.Y + acceleration;
                else if (directVelo.Y > dirY)
                    directVelo.Y = directVelo.Y - acceleration;
                if (Math.Abs(dirY) < speed * 0.2 && (directVelo.X > 0.0 && dirX < 0.0 || directVelo.X < 0.0 && dirX > 0.0))
                {
                    if (directVelo.Y > 0.0)
                        directVelo.Y = directVelo.Y + acceleration * 2f;
                    else
                        directVelo.Y = directVelo.Y - acceleration * 2f;
                }
                if (Math.Abs(dirX) < speed * 0.2 && (directVelo.Y > 0.0 && dirY < 0.0 || directVelo.Y < 0.0 && dirY > 0.0))
                {
                    if (directVelo.X > 0.0)
                        directVelo.X = directVelo.X + acceleration * 2f;
                    else
                        directVelo.X = directVelo.X - acceleration * 2f;
                }
            }
            else if (absDirX > absDirY)
            {
                if (directVelo.X < dirX)
                    directVelo.X = directVelo.X + acceleration * 1.1f;
                else if (directVelo.X > dirX)
                    directVelo.X = directVelo.X - acceleration * 1.1f;
                if (Math.Abs(directVelo.X) + Math.Abs(directVelo.Y) < speed * 0.5)
                {
                    if (directVelo.Y > 0.0)
                        directVelo.Y = directVelo.Y + acceleration;
                    else
                        directVelo.Y = directVelo.Y - acceleration;
                }
            }
            else
            {
                if (directVelo.Y < dirY)
                    directVelo.Y = directVelo.Y + acceleration * 1.1f;
                else if (directVelo.Y > dirY)
                    directVelo.Y = directVelo.Y - acceleration * 1.1f;
                if (Math.Abs(directVelo.X) + Math.Abs(directVelo.Y) < speed * 0.5)
                {
                    if (directVelo.X > 0.0)
                        directVelo.X = directVelo.X + acceleration;
                    else
                        directVelo.X = directVelo.X - acceleration;
                }
            }
            if (npc.localAI[0] != 1)
                npc.netUpdate = true;
            npc.localAI[0] = 1f;
            if ((directVelo.X > 0.0 && npc.oldVelocity.X < 0.0 || directVelo.X < 0.0 && npc.oldVelocity.X > 0.0 || (directVelo.Y > 0.0 && npc.oldVelocity.Y < 0.0 || directVelo.Y < 0.0 && npc.oldVelocity.Y > 0.0)) && !npc.justHit)
                npc.netUpdate = true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/Boss/SubspaceSerpentHeadFill");
            Vector2 origin = new Vector2(texture.Width * 0.5f, npc.height * 0.5f);
            if (hasEnteredSecondPhase)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 toTheSide = new Vector2(2, 0).RotatedBy(npc.rotation + MathHelper.ToRadians(i * -90));
                    Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + toTheSide, npc.frame, new Color(0, 255, 0) * ((255f - npc.alpha) / 255f) * ((255f - npc.alpha) / 255f), npc.rotation, origin, 1f, SpriteEffects.None, 0);
                }
            }
            texture = Main.npcTexture[npc.type];
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, npc.frame, drawColor * ((255f - npc.alpha) / 255f), npc.rotation, origin, npc.scale, SpriteEffects.None, 0);
            return false;
        }
        int counter = 0;
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/Boss/SubspaceSerpentHeadGlow");
            Vector2 origin = new Vector2(texture.Width * 0.5f, npc.height * 0.5f);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, npc.frame, Color.White * ((255f - npc.alpha) / 255f), npc.rotation, origin, npc.scale, SpriteEffects.None, 0);
            counter++;
            if (counter > 12)
                counter = 0;
            for (int j = 0; j < 2; j++)
            {
                float bonusAlphaMult = 1 - 1 * (counter / 12f);
                float dir = j * 2 - 1;
                Vector2 offset = new Vector2(counter * 0.8f * dir, 0).RotatedBy(npc.rotation);
                Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + offset, npc.frame, new Color(100, 100, 100, 0) * bonusAlphaMult * ((255f - npc.alpha) / 255f), npc.rotation, origin, 1.00f, SpriteEffects.None, 0.0f);
            }
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1f;
            return null;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * bossLifeScale * 0.75f);  //boss life scale in expertmode
            npc.damage = 160;
        }
        public bool hasEnteredSecondPhase = false;
        bool runOnce = true;
        float rotate = 0;
        public void TransitionPhase(int Tphase)
        {
            if (Tphase == 0)
            {
                ai1 = 540;
                ai2 = 0;
                ai3 = 300;
                ai4 = -1;
            }
            if (Tphase == 1)
            {
                ai1 = 720;
                ai2 = 0;
                ai3 = 0;
                ai4 = 0;
            }
            if (Tphase == 2)
            {
                directVelo = directVelo.SafeNormalize(new Vector2(1, 0)) * 30;
                ai1 = 0;
                ai2 = 0;
                ai3 = 1;
                ai4 = 0;
            }
            if (Tphase == 3 || Tphase == 4)
            {
                ai1 = 2000;
                ai2 = 0;
                ai3 = 0;
                ai4 = 1;
            }
            if (Tphase == 5)
            {
                ai1 = 510;
                ai2 = 0;
                ai3 = 0;
                ai4 = 0;
            }
            else
            {
                npc.dontTakeDamage = false;
            }
            phase = Tphase;
        }
        bool left;
        public override void FindFrame(int frameHeight)
        {
            int targetFrame = 0;
            if (directVelo.X < 0)
                targetFrame = 4;
            int currentFrame = npc.frame.Y / frameHeight;
            if(currentFrame != targetFrame || (phase == 4 || (phase == 0 && hasEnteredSecondPhase && ai3 < 250 && ai3 > 160)))
            {
                npc.frameCounter++;
                if(npc.frameCounter >= 6)
                {
                    currentFrame += 1;
                    npc.frameCounter = 0;
                }
            }
            else
            {
                npc.frameCounter = 0;
            }
            if (currentFrame > 7)
                currentFrame = 0;
            npc.frame.Y = currentFrame * frameHeight;
        }
        public override void AI()
        {
            Player player = Main.player[npc.target];
            if (npc.target < 0 || npc.target == (int)byte.MaxValue || (player.dead || !player.active))
            {
                npc.TargetClosest(true);
                player = Main.player[npc.target];
                npc.netUpdate = true;
            }
            npc.spriteDirection = 1;
            rotate++;
            if (runOnce)
            {
                hasEnteredSecondPhase = false;
                left = player.Center.X < (Main.maxTilesX / 2) * 16;
                runOnce = false;
                rotate = Main.rand.Next(120);
                TransitionPhase(0);
            }
            if (phase == 0)
            {
                ai1--;
                if (ai1 <= 0)
                {
                    float mult = 0.6f;
                    if (Main.expertMode)
                        mult = 0.65f;
                    if (npc.life < npc.lifeMax * mult && !hasEnteredSecondPhase)
                    {
                        TransitionPhase(5);
                    }
                    else
                        TransitionPhase(1);
                    return;
                }
                if(hasEnteredSecondPhase)
                {
                    rotate++;
                    ai3--;
                    if(ai3 > 190 && ai3 < 250)
                    {
                        directVelo *= 0.94f;
                    }
                    if(ai3 == 200)
                    {
                        prevLocation = player.Center;
                    }
                    if(ai3 == 190)
                    {
                        Vector2 goTo = prevLocation - npc.Center;
                        directVelo = goTo.SafeNormalize(Vector2.Zero) * 24f;
                        Main.PlaySound(SoundID.Roar, (int)npc.Center.X, (int)npc.Center.Y, 0, 0.8f);
                        if (Main.netMode != 1)
                        {
                            for(int i = 0; i < 16; i++)
                            {
                                Vector2 bulletSpread = new Vector2(15, 0).RotatedBy(MathHelper.ToRadians(i * 22.5f));
                                bulletSpread.Y -= 3.5f;
                                int damage2 = npc.damage / 2;
                                if (Main.expertMode)
                                {
                                    damage2 = (int)(damage2 / Main.expertDamage);
                                }
                                Projectile.NewProjectile(npc.Center, bulletSpread, ModContent.ProjectileType<SubspaceLingeringFlame>(), (int)(damage2 * 0.75f), 0, Main.myPlayer);
                            }
                        }
                    }
                    if(ai3 <= 190 && ai3 > 160 && ai3 % 3 == 0)
                    {
                        if (Main.netMode != 1)
                        {
                            int damage2 = npc.damage / 2;
                            if (Main.expertMode)
                            {
                                damage2 = (int)(damage2 / Main.expertDamage);
                            }
                            Vector2 goTo = player.Center - npc.Center;
                            Vector2 velocity = goTo.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-45.5f, 45.5f)));
                            Projectile.NewProjectile(npc.Center + velocity * 32, velocity * 11.5f + new Vector2(0, -4.5f), ModContent.ProjectileType<SubspaceLingeringFlame>(), (int)(damage2 * 0.75f), 0, Main.myPlayer);
                        }
                    }
                    if (ai3 < 160)
                        ai3 = 250;
                }
            }
            if (phase == 1)
            {
                Vector2 playerCenter = player.Center;
                float num = 1;
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player target = Main.player[i];
                    if (player.whoAmI != target.whoAmI && !target.dead && target.ZoneUnderworldHeight && Vector2.Distance(target.Center, npc.Center) < 2000)
                    {
                        playerCenter += target.Center;
                        num++;
                    }
                }
                playerCenter = playerCenter /= num;
                ai1--;
                int numCrosses = 2;
                if (Main.expertMode && Main.rand.NextBool(4) && !hasEnteredSecondPhase)
                    numCrosses++;
                if (Main.expertMode && Main.rand.NextBool(10) && hasEnteredSecondPhase)
                    numCrosses++;
                if (hasEnteredSecondPhase)
                    numCrosses++;
                CircularAttack(playerCenter, 30, numCrosses);
                if (ai1 <= 0 ||(hasEnteredSecondPhase && ai1 < 120))
                {
                    TransitionPhase(2);
                    return;
                }
            }
            if (phase == 2)
            {
                ai1--;
                int max = 6;
                if (hasEnteredSecondPhase)
                    max = 4;
                if (ai1 <= 0)
                {
                    if (ai4 > max)
                    {
                        TransitionPhase(3);
                        return;
                    }
                    else // if (dist > 1500)
                    {
                        ai1 = 250;
                        prevLocation = DoIndicator(Main.rand.Next(-120, 121), Main.rand.Next(-14, 15), Main.rand.Next(-120, 121));
                    }
                }
                else if(ai1 == 240 && hasEnteredSecondPhase)
                {
                    DoIndicator(ai3, Main.rand.Next(-14, 15), 0, false, true);
                }
                else if (ai1 == 140)
                {
                    ai2 = -70;
                    DoDash((int)ai3, true);
                    ai3 *= -1;
                    ai4++;
                    savedir = prevdir;
                    /*
                    #region make sure it is going towards the player lol
                    Vector2 velo = savedir.RotatedBy(MathHelper.ToRadians(90)) * 0.8f;
                    Vector2 slope = savedir.SafeNormalize(Vector2.Zero);
                    Vector2 toPlayerFromPoint = prevLocation - player.Center;
                    float alphaAngle = MathHelper.WrapAngle(slope.ToRotation());
                    float betaAngle = MathHelper.WrapAngle(toPlayerFromPoint.ToRotation());
                    slope = slope.RotatedBy(-alphaAngle);
                    Main.NewText("( " + slope.X + ", " + slope.Y + ")");

                    if(MathHelper.ToDegrees(alphaAngle) > 90)
                    {
                        alphaAngle -= MathHelper.ToRadians(90);
                        alphaAngle *= -1;
                    }
                    if (MathHelper.ToDegrees(alphaAngle) < -90)
                    {
                        alphaAngle += MathHelper.ToRadians(90);
                        alphaAngle *= -1;
                    }
                    Main.NewText("beta" + MathHelper.ToDegrees(betaAngle));
                    float total = Math.Abs(alphaAngle) + Math.Abs(betaAngle);
                    total = MathHelper.ToDegrees(total);
                    if (velo.X < 0)
                        savedir *= -1;
                    if (total > 180f)
                        savedir *= -1;
                    #endregion
                    */
                    if (ai4 <= max)
                        ai1 = 0;
                }
                ai2++;
                if (ai2 > -54 && ai2 < 0 && ai2 % 2 == 0)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        int dust2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModContent.DustType<CopyDust4>());
                        Dust dust = Main.dust[dust2];
                        dust.color = new Color(100, 255, 100, 0);
                        dust.noGravity = true;
                        dust.fadeIn = 0.1f;
                        dust.scale *= 3.5f;
                        dust.velocity *= 2.5f;
                    }
                    //for (int i = 0; i < 2; i++)
                    Vector2 velo = savedir.RotatedBy(MathHelper.ToRadians(90)) * 0.8f;// * (i * 2 - 1);
                    if (Main.netMode != 1)
                    {
                        int damage2 = npc.damage / 2;
                        if (Main.expertMode)
                        {
                            damage2 = (int)(damage2 / Main.expertDamage);
                        }
                        Projectile.NewProjectile(npc.Center, velo, ModContent.ProjectileType<CellBlast>(), (int)(damage2 * 0.75f), 0, Main.myPlayer);
                    }
                }
            }
            if (phase == 3)
            {
                ai1--;
                int worldSide = left ? -1 : 1;
                if (ai1 > 1900)
                {
                    ai1 = 1900;
                    prevLocation = player.Center;
                    prevLocation = DoIndicator(worldSide, 540 + ai2, 0, true, false);
                }
                else if(ai1 <= 1850 && ai1 > 1750 && ai1 % 10 == 0)
                { 
                    ai2 += 100;
                    DoIndicator(worldSide, ai2, 0, true, hasEnteredSecondPhase);
                }
                if (ai1 == 1760)
                {
                    DoDash(1, true);
                    if(!hasEnteredSecondPhase)
                        SerpentRing();
                }
                if (ai1 <= 1710)
                {
                    directVelo *= 0.5f;
                }
                if (ai1 <= 1708)
                {
                    TransitionPhase(4);
                }
            }
            if (phase == 4)
            {
                int worldSide = left ? 1 : -1;
                if (ai1 <= 2000 && ai1 > 1900 && ai1 % 10 == 0)
                {
                    if ((int)ai1 == 1980)
                    {
                        if (hasEnteredSecondPhase)
                            Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Celestial.SubspaceEye>(), 0, 0, Main.myPlayer, npc.whoAmI, -1);
                    }
                    ai3 += 100;
                    DoIndicator(-worldSide, ai3, ModContent.ProjectileType<EnergySerpentHead>(), true);
                }
                if ((int)ai1 == 1900)
                {
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Celestial.SubspaceEyeWall>(), 0, (hasEnteredSecondPhase ? 1 : 0), Main.myPlayer, npc.whoAmI, worldSide);
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Celestial.SubspaceEyeWall>(), 0, (hasEnteredSecondPhase ? 1 : 0), Main.myPlayer, npc.whoAmI, 1000 * worldSide);
                }
                if(ai1 < 1800 && ai2 % 20 <= 1 && Main.netMode != 1)
                {
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player target = Main.player[i];
                        if (!target.dead && target.ZoneUnderworldHeight)
                        {
                            if (target.Center.X - 64 > npc.Center.X && !left)
                            {
                                target.AddBuff(ModContent.BuffType<SulfurBurn>(), 20);
                                Projectile.NewProjectile(target.Center + new Vector2(0, 800), new Vector2(0, -56), ModContent.ProjectileType<EnergySerpentHead>(), 1000, 0, Main.myPlayer, 12, -2);
                            }
                            if (target.Center.X + 64 < npc.Center.X && left)
                            {
                                target.AddBuff(ModContent.BuffType<SulfurBurn>(), 20);
                                Projectile.NewProjectile(target.Center + new Vector2(0, 800), new Vector2(0, -56), ModContent.ProjectileType<EnergySerpentHead>(), 1000, 0, Main.myPlayer, 12, -2);
                            }
                            if (target.Center.X - 2324 > npc.Center.X && left)
                            {
                                target.AddBuff(ModContent.BuffType<SulfurBurn>(), 20);
                                if (target.Center.X - 2400 > npc.Center.X)
                                   Projectile.NewProjectile(target.Center + new Vector2(0, 800), new Vector2(0, -56), ModContent.ProjectileType<EnergySerpentHead>(), 1000, 0, Main.myPlayer, 12, -2);
                            }
                            if (target.Center.X + 2324 < npc.Center.X && !left)
                            {
                                target.AddBuff(ModContent.BuffType<SulfurBurn>(), 20);
                                if (target.Center.X + 2400 < npc.Center.X)
                                    Projectile.NewProjectile(target.Center + new Vector2(0, 800), new Vector2(0, -56), ModContent.ProjectileType<EnergySerpentHead>(), 1000, 0, Main.myPlayer, 12, -2);
                            }
                        }
                    }
                }
                if(hasEnteredSecondPhase)
                {
                    if (ai1 > 1880)
                        ai1--;
                    else if (ai1 > 0)
                    {
                        if (ai3 <= 0 || ai3 > 900)
                            ai3 = 900;
                        ai1 = -1; 
                        ai4 = 0;
                    }
                }
                else
                {
                    ai1--;
                }
                Vector2 toPlayer = player.Center - npc.Center;
                float distToPlayer = toPlayer.Length();
                ai2++;
                float dist = 620;
                float speed = 20;
                if (hasEnteredSecondPhase)
                {
                    ai2 += 0.5f;
                    dist = 240;
                    speed = 3.5f + distToPlayer * 0.0013f;
                }
                SlitherWall(worldSide, ai2, dist, speed);
                if(!hasEnteredSecondPhase)
                {
                    if(ai1 > 1000)
                    {
                        if ((Main.expertMode && ai1 % 45 == 0) || (!Main.expertMode && ai1 % 55 == 0))
                        {
                            SnakeFromWall(worldSide);
                        }
                        if (ai1 % 330 == 0)
                        {
                            Vector2 circular = new Vector2(1200, 0).RotatedBy(MathHelper.ToRadians(30 + (rotate * 2 + Main.rand.Next(-30, 31)) % 120));
                            SerpentRing(circular + player.Center);
                        }
                    }
                    if(ai1 < 935)
                        ai4 = -1;
                    if (ai1 < 920)
                    {
                        TransitionPhase(0);
                        left = !left;
                        return;
                    }
                }
                else if(ai1 <= 0)
                {
                    ai3--;
                    if(ai3 >= 700 && ai3  % 50 == 0)
                    {
                        if (ai4 <= 10)
                        {
                            ai4 = player.Center.Y;
                        }
                        int num = 3;
                        if (Main.expertMode)
                            num = 4;
                        int interval = Main.rand.Next(num);
                        int counter = 0;
                        for (int i = -12; i < 13; i++)
                        {
                            counter++;
                            if(counter % num != interval)
                            {
                                Vector2 spawnPos = new Vector2((250 - ai3 + 650) * 1.5f * worldSide, 75 * i);
                                spawnPos.X += npc.Center.X;
                                spawnPos.Y += ai4;
                                if (Main.netMode != 1)
                                {
                                    int damage2 = npc.damage / 2;
                                    if (Main.expertMode)
                                    {
                                        damage2 = (int)(damage2 / Main.expertDamage);
                                    }
                                    Projectile.NewProjectile(spawnPos, new Vector2(worldSide * 4, 0), ModContent.ProjectileType<GreaterCellBlast>(), (int)(damage2 * 1.1f), 0, Main.myPlayer, 0, npc.whoAmI);
                                }
                            }
                        }
                        if (Main.netMode != 1)
                            Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Celestial.SubspaceEyeWall>(), 0, 0, Main.myPlayer, npc.whoAmI, (250 - ai3 + 700) * 1.5f * worldSide);
                    }
                    if (ai3 <= 380)
                    {
                        ai4 = -1;
                    }
                    if (ai3 < 350)
                    {
                        ai3 = 900;
                        ai1--;
                    }
                    if(ai1 < -2)
                    {
                        TransitionPhase(0);
                        left = !left;
                        return;
                    }
                }
            }
            if (phase == 5)
            {
                if((int)ai3 == 0)
                {
                    Vector2 playerCenter = player.Center;
                    float num = 1;
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player target = Main.player[i];
                        if (player.whoAmI != target.whoAmI && !target.dead && target.ZoneUnderworldHeight && Vector2.Distance(target.Center, npc.Center) < 2000)
                        {
                            playerCenter += target.Center;
                            num++;
                        }
                    }
                    playerCenter = playerCenter /= num;
                    CircularAttack(playerCenter, 28f, 0, (int)ai1 + 60, 1f);
                    ai1--;
                    if (npc.alpha < 0)
                        npc.alpha = 0;
                    if ((int)ai1 % 2 == 0)
                    {
                        if(npc.alpha < 255)
                            npc.alpha++;
                    }
                    if ((int)ai1 == 255)
                    {
                        npc.dontTakeDamage = true;
                        if (Main.netMode != 1)
                        {
                            ai4 = NPC.NewNPC((int)playerCenter.X, (int)playerCenter.Y + (int)player.height, ModContent.NPCType<SubspaceEye>());
                            NPC eye = Main.npc[(int)ai4];
                            eye.realLife = npc.whoAmI;
                            prevLocation = playerCenter;
                        }
                    }
                    if(ai1 < 0)
                    {
                        ai1 = 960;
                        ai3++;
                    }
                }
                else
                {
                    ai1--;
                    NPC eye = Main.npc[(int)ai4];
                    if ((!eye.active || eye.type != ModContent.NPCType<SubspaceEye>()) && ai3 != 3)
                    {
                        ai1 = 510;
                        ai3 = 3;
                    }
                    else
                    {
                        for(int i = 0; i < Main.maxPlayers; i++)
                        {
                            Player target = Main.player[i];
                            if(target.active && !target.dead && target.ZoneUnderworldHeight && Vector2.Distance(target.Center, eye.Center) > 660)
                                target.AddBuff(ModContent.BuffType<SulfurBurn>(), 20);
                        }
                        if ((int)ai3 == 1)
                        {
                            if (ai1 % 5 == 0 && Main.netMode != 1)
                            {
                                for (int i = 0; i < 6; i++)
                                {
                                    float randEdit = Main.rand.NextFloat(-6, 6);
                                    Vector2 away = new Vector2(900 + randEdit, 0).RotatedBy(MathHelper.ToRadians(i * 60 + ai1 * 0.72f));
                                    Vector2 to = away.SafeNormalize(Vector2.Zero) * -4.5f;
                                    away += eye.Center;
                                    int damage2 = npc.damage / 2;
                                    if (Main.expertMode)
                                    {
                                        damage2 = (int)(damage2 / Main.expertDamage);
                                    }
                                    int type = 0;   
                                    if (Main.expertMode && Main.rand.NextBool(16))
                                        type = 1;
                                    Projectile.NewProjectile(away, to, ModContent.ProjectileType<WaveBlast>(), (int)(damage2 * 0.8f), 3, Main.myPlayer, type);
                                }
                            }
                            if (ai1 < 0)
                            {
                                ai1 = 510;
                                ai3++;
                            }
                        }
                        if((int)ai3 == 2)
                        {
                            ai3++;
                            /*
                            SubspaceEye subEye = eye.modNPC as SubspaceEye;
                            Vector2 fromEye = player.Center - eye.Center;
                            if(ai1 % 120 == 0 && Main.netMode != 1 && ai1 < 720)
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    subEye.eyeRecoil = -0.8f;
                                    Vector2 away = new Vector2(24, 0).RotatedBy(fromEye.ToRotation() + MathHelper.ToRadians(Main.rand.NextFloat(-22.5f, 22.5f)));
                                    Vector2 to = away.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(4, 12);
                                    away += eye.Center;
                                    int damage2 = npc.damage / 2;
                                    if (Main.expertMode)
                                    {
                                        damage2 = (int)(damage2 / Main.expertDamage);
                                    }
                                    int type = Main.rand.Next(10);
                                    if (type < 5)
                                        type = 0;
                                    else
                                        type = 1;
                                    Projectile.NewProjectile(away, to, ModContent.ProjectileType<CrossLaser>(), (int)(damage2 * 0.80f), 3, Main.myPlayer, type);
                                }
                            }
                            if (ai1 < 0)
                            {
                                ai1 = 510;
                                ai3++;
                            }
                            */
                        }
                        if ((int)ai3 == 3)
                        {
                            hasEnteredSecondPhase = true;
                            if (npc.alpha > 255)
                                npc.alpha = 255;
                            if ((int)ai1 % 2 == 0)
                            {
                                if(npc.alpha > 0)
                                    npc.alpha--;
                            }
                            if ((int)ai1 == 255)
                            {
                                npc.dontTakeDamage = false;
                                eye.ai[3] = -1;
                                eye.netUpdate = true;
                            }
                            if(ai1 < 0)
                            {
                                TransitionPhase(2);
                                return;
                            }
                        }
                    }
                    CircularAttack(prevLocation, 28f, 0, 660, 1f);
                }
            }
            #region active check
            if (player.dead || !player.ZoneUnderworldHeight)
            {
                despawn++;
            }
            if (despawn >= 600)
            {
                npc.active = false;
            }
            npc.timeLeft = 10000;
            if (Main.netMode != 1)
            {
                npc.netUpdate = true;
            }
            #endregion
        }
        Vector2 prevLocation;
        Vector2 prevdir;
        Vector2 savedir;
        int slither = 1;
        public void SnakeFromWall(int direction)
        {
            if (Main.netMode != 1)
            {
                Vector2 area = npc.Center - new Vector2(2000 * direction, Main.rand.NextFloat(-360, 360));
                int damage2 = npc.damage / 2;
                if (Main.expertMode)
                {
                    damage2 = (int)(damage2 / Main.expertDamage);
                }
                Vector2 circular = new Vector2(Main.rand.NextFloat(12f, 16f) * direction, 0);
                Projectile.NewProjectile(area, circular, ModContent.ProjectileType<EnergySerpentHead>(), (int)(damage2 * 0.95f), 0, Main.myPlayer, Main.rand.Next(5, 7), -1);
            }
        }
        public void SlitherWall(int direction, float rotate, float dist = 620, float offset = 20)
        {
            Player player = Main.player[npc.target];
            Vector2 circular = new Vector2(0, -dist).RotatedBy(MathHelper.ToRadians(rotate * 2 * direction));
            Vector2 toLocation = new Vector2(npc.Center.X + offset * direction, player.Center.Y + circular.Y);
            Vector2 goTo = toLocation - npc.Center;
            float speed = 15f;
            if (speed > goTo.Length())
                speed = goTo.Length();
            directVelo = goTo.SafeNormalize(Vector2.Zero) * speed;
        }
        public Vector2 DoIndicator(float rand1, float rand2, float rand3, bool phase3 = false, bool redIndicator = false)
        {
            Player player = Main.player[npc.target];
            if (!phase3)
            {
                if(!redIndicator)
                {
                    float dist = rand1 + 360;
                    Vector2 selectArea = new Vector2(dist, 0).RotatedBy(MathHelper.ToRadians(rotate * 1.75f + rand3));
                    Vector2 velo = selectArea.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(rand2 + 90));
                    prevdir = velo.SafeNormalize(Vector2.Zero);
                    if (Main.netMode != 1)
                    {
                        Projectile.NewProjectile(selectArea + player.Center, velo, ModContent.ProjectileType<DashIndicator>(), 0, 0, Main.myPlayer);
                    }
                    return selectArea + player.Center;
                }
                else
                {
                    Vector2 offset = prevdir * 360f * rand1;
                    int damage2 = npc.damage / 2;
                    if (Main.expertMode)
                    {
                        damage2 = (int)(damage2 / Main.expertDamage);
                    }
                    if (Main.netMode != 1)
                    {
                        Projectile.NewProjectile(prevLocation + offset, prevdir.RotatedBy(MathHelper.ToRadians(rand2 + 90)), ModContent.ProjectileType<DashIndicator2>(), (int)(damage2 * 0.75f), 0, Main.myPlayer, rand1);
                    }
                }
            }
            else
            {
                Vector2 selectArea = new Vector2(rand2 * rand1, 0);
                prevdir = new Vector2(0, -1);
                if((int)rand3 == ModContent.ProjectileType<EnergySerpentHead>())
                {
                    Main.PlaySound(SoundID.Item119, (int)(selectArea + prevLocation).X, (int)(selectArea + prevLocation).Y);
                    if (Main.netMode != 1)
                    {
                        int damage2 = npc.damage / 2;
                        if (Main.expertMode)
                        {
                            damage2 = (int)(damage2 / Main.expertDamage);
                        }
                        if(hasEnteredSecondPhase)
                            Projectile.NewProjectile(selectArea + prevLocation + new Vector2(0, 1500), new Vector2(0, -56), ModContent.ProjectileType<EnergySerpentHead2>(), damage2 * 2, 0, Main.myPlayer, 32, npc.whoAmI);
                        else
                            Projectile.NewProjectile(selectArea + prevLocation + new Vector2(0, 1500), new Vector2(0, -56), ModContent.ProjectileType<EnergySerpentHead>(), damage2 * 2, 0, Main.myPlayer, 32, npc.whoAmI);
                    }
                }
                else if (Main.netMode != 1)
                {
                    if (redIndicator)
                        Projectile.NewProjectile(selectArea + prevLocation, new Vector2(0, -1), ModContent.ProjectileType<DashIndicator>(), 0, 0, Main.myPlayer, 0, -1);
                    else
                        Projectile.NewProjectile(selectArea + prevLocation, new Vector2(0, -1), ModContent.ProjectileType<DashIndicator>(), 0, 0, Main.myPlayer);
                }
                return selectArea + prevLocation;
            }
            return player.Center;
        }
        public void DoDash(int direction = 1, bool push = false)
        {
            Main.PlaySound(SoundID.Item119, (int)prevLocation.X, (int)prevLocation.Y);
            Vector2 velo = prevdir;
            if (push)
               npc.Center = prevLocation - velo * 2700 * direction;
            directVelo = velo * 56 * direction;
        }
        public void SerpentRing(Vector2 area)
        {
            if (Main.netMode != 1)
            {
                int damage2 = npc.damage / 2;
                if (Main.expertMode)
                {
                    damage2 = (int)(damage2 / Main.expertDamage);
                }
                for (int i = 0; i < 180; i += 30)
                {
                    Vector2 circular = new Vector2(-Main.rand.NextFloat(6f, 12f), 0).RotatedBy(MathHelper.ToRadians(i + Main.rand.Next(-10, 11)));
                    Projectile.NewProjectile(area, circular, ModContent.ProjectileType<EnergySerpentHead>(), (int)(damage2 * 0.6f), 0, Main.myPlayer, 6, -1);
                }
            }
        }
        public void SerpentRing()
        {
            if(Main.netMode != 1)
            {
                Vector2 velo = prevdir;
                Vector2 area = prevLocation - velo * 1500;
                SerpentRing(area);
            }
        }
        public void CircularAttack(Vector2 newCenter, float speed = 30, int amt = 2, int distance = 640, float verticalMult = 0.8f)
        {
            Player player = Main.player[npc.target];
            Vector2 toLocation = new Vector2(distance, 0).RotatedBy(MathHelper.ToRadians(rotate * 2.15f));
            toLocation.Y *= verticalMult;
            toLocation += newCenter;
            Vector2 goTo = toLocation - npc.Center;
            if (goTo.Length() > 48 && ai2 != 1)
                directVelo = goTo.SafeNormalize(Vector2.Zero) * speed;
            else
            {
                directVelo = goTo.SafeNormalize(Vector2.Zero) * 1;
                ai2 = 1;
                npc.Center = toLocation;
            }
            if(phase == 1 && amt > 0)
            {
                if (ai1 % 120 == 0 && Main.netMode != 1)
                {
                    if (amt > 8)
                        amt = 8;
                    List<int> unavailable = new List<int>();
                    List<int> storeValues = new List<int>();
                    List<Vector2> spawnPositions = new List<Vector2>();
                    List<Vector2> veloPositions = new List<Vector2>();
                    for (int i = 0; i < amt; i++)
                    {
                        int rand = Main.rand.Next(8);
                        while (unavailable.Contains(rand))
                        {
                            rand = Main.rand.Next(8);
                        }
                        unavailable.Add(rand);
                        storeValues.Add(rand);
                        Vector2 spawnLocation = LaserArea(rand);
                        spawnPositions.Add(spawnLocation);
                    }
                    for (int i = 0; i < amt; i++)
                    {
                        int Moving = 0;
                        if (hasEnteredSecondPhase && Main.rand.NextBool(3))
                        {
                            Moving = 3;
                        }
                        Vector2 spawnLocation = spawnPositions[i];
                        List<int> moveAvailable = CapableAreas(storeValues[i]);
                        int rand = Main.rand.Next(moveAvailable.Count);
                        int rand2 = moveAvailable[rand];
                        while (unavailable.Contains(rand2))
                        {
                            moveAvailable.Remove(rand2);
                            if (moveAvailable.Count <= 0)
                            {
                                rand2 = -1;
                                break;
                            }
                            rand = Main.rand.Next(moveAvailable.Count);
                            rand2 = moveAvailable[rand];
                        }
                        Vector2 velo = Vector2.Zero;
                        if (rand2 >= 0 && Moving == 3)
                        {
                            Vector2 moveTo = LaserArea(rand2);
                            unavailable.Add(rand2);
                            velo = (moveTo - spawnLocation) / 320f;
                        }
                        veloPositions.Add(velo);
                    }
                    for (int i = 0; i < spawnPositions.Count; i++)
                    {
                        Vector2 spawnLocation = spawnPositions[i];
                        Vector2 velo = veloPositions[i];
                        int Moving = 0;
                        if (!velo.Equals(Vector2.Zero))
                        {
                            Moving = 3;
                        }
                        int damage2 = npc.damage / 2;
                        if (Main.expertMode)
                        {
                            damage2 = (int)(damage2 / Main.expertDamage);
                        }
                        int type = Main.rand.Next(10);
                        if (type < 5)
                            type = 0;
                        else
                            type = 1;
                        float odds = Main.rand.NextFloat(15);
                        if(hasEnteredSecondPhase)
                            odds = Main.rand.NextFloat(20);
                        if (odds < 2)
                            type = 2;
                        Projectile.NewProjectile(spawnLocation + newCenter, velo, ModContent.ProjectileType<CrossLaser>(), (int)(damage2 * 0.8f), 0, Main.myPlayer, type + Moving);
                    }
                }
            }
        }
        public List<int> CapableAreas(int rnArea)
        {
            List<int> available = new List<int>();
            if(rnArea == 0)
            {
                available.Add(1);
                available.Add(7);
            }
            if (rnArea == 1)
            {
                available.Add(0);
                available.Add(2);
                available.Add(3);
                available.Add(7);
            }
            if (rnArea == 2)
            {
                available.Add(1);
                available.Add(3);
            }
            if (rnArea == 3)
            {
                available.Add(2);
                available.Add(4);
                available.Add(1);
                available.Add(5);
            }
            if (rnArea == 4)
            {
                available.Add(5);
                available.Add(3);
            }
            if (rnArea == 5)
            {
                available.Add(6);
                available.Add(7);
                available.Add(3);
                available.Add(4);
            }
            if (rnArea == 6)
            {
                available.Add(5);
                available.Add(7);
            }
            if (rnArea == 7)
            {
                available.Add(6);
                available.Add(0);
                available.Add(1);
                available.Add(5);
            }
            return available;
        }
        public Vector2 LaserArea(int area)
        {
            Vector2 location = new Vector2(-320, -320);
            if (area == 0)
            {
                location = new Vector2(-320, -320);
            }
            if (area == 1)
            {
                location = new Vector2(0, -320);
            }
            if (area == 2)
            {
                location = new Vector2(320, -320);
            }
            if (area == 3)
            {
                location = new Vector2(320, 0);
            }
            if (area == 4)
            {
                location = new Vector2(320, 320);
            }
            if (area == 5)
            {
                location = new Vector2(0, 320);
            }
            if (area == 6)
            {
                location = new Vector2(-320, 320);
            }
            if (area == 7)
            {
                location = new Vector2(-320, 0);
            }
            return location;
        }
		public override void PostAI()
		{
            npc.velocity = directVelo;
            if(phase == 0)
               ApplySlither();
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 2.5f / 255f, (255 - npc.alpha) * 1.6f / 255f, (255 - npc.alpha) * 2.4f / 255f);
            npc.rotation = npc.velocity.ToRotation() + MathHelper.ToRadians(90);
        }
        public void ApplySlither()
        {
            if (slither > 0)
            {
                slither += 2;
                npc.velocity = directVelo.RotatedBy(MathHelper.ToRadians(slither - 30));
            }
            if (slither > 60)
            {
                slither = -1;
            }
            if (slither < 0)
            {
                slither -= 2;
                npc.velocity = directVelo.RotatedBy(MathHelper.ToRadians(slither + 30));
            }
            if (slither < -60)
            {
                slither = 1;
            }
        }
		public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(phase);
            writer.Write(hasEnteredSecondPhase);
            writer.Write(left);
            writer.Write(rotate);
            writer.Write(ai1);
			writer.Write(directVelo.X);
			writer.Write(directVelo.Y);
            writer.Write(prevLocation.X);
            writer.Write(prevLocation.Y);
            writer.Write(prevdir.X);
            writer.Write(prevdir.Y);
        }
		public override void ReceiveExtraAI(BinaryReader reader)
        {
            phase = reader.ReadInt32();
            hasEnteredSecondPhase = reader.ReadBoolean();
            left = reader.ReadBoolean();
            rotate = reader.ReadSingle();
            ai1 = reader.ReadSingle();
            directVelo.X = reader.ReadSingle();
            directVelo.Y = reader.ReadSingle();
            prevLocation.X = reader.ReadSingle();
            prevLocation.Y = reader.ReadSingle();
            prevdir.X = reader.ReadSingle();
            prevdir.Y = reader.ReadSingle();
        }
	}
}