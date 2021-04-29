using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            music = MusicID.Boss2;
            for (int i = 0; i < Main.maxBuffTypes; i++)
            {
                npc.buffImmune[i] = true;
            }
            //npc.aiStyle = 6;
            bossBag = mod.ItemType("SubspaceBag");
            Main.npcFrameCount[npc.type] = 8;
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
            if (Main.netMode != 1)
            {
                if (npc.ai[0] == 0)
                {
                    npc.realLife = npc.whoAmI;
                    int latestNPC = npc.whoAmI;
                    int randomWormLength = 50;
                    for (int i = 0; i < randomWormLength; ++i)
                    {
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SubspaceSerpentBody"), npc.whoAmI, 0, latestNPC);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        Main.npc[(int)latestNPC].ai[3] = npc.whoAmI;
                    }
                    latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SubspaceSerpentTail"), npc.whoAmI, 0, latestNPC);
                    Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                    Main.npc[(int)latestNPC].ai[3] = npc.whoAmI;

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
            Texture2D texture = Main.npcTexture[npc.type];
            Vector2 origin = new Vector2(texture.Width * 0.5f, npc.height * 0.5f);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, npc.frame, drawColor, npc.rotation, origin, npc.scale, SpriteEffects.None, 0);
            return false;
        }
        int counter = 0;
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = mod.GetTexture("NPCs/Boss/SubspaceSerpentHeadGlow");
            Vector2 origin = new Vector2(texture.Width * 0.5f, npc.height * 0.5f);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, npc.frame, Color.White, npc.rotation, origin, npc.scale, SpriteEffects.None, 0);
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
        bool runOnce = true;
        float rotate = 0;
        public void TransitionPhase(int Tphase)
        {
            if (Tphase == 0)
            {
                ai1 = 480;
                ai2 = 0;
                ai3 = 0;
                ai4 = 0;
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
                ai4 = 0;
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
            if(currentFrame != targetFrame)
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
            npc.TargetClosest(false);
            rotate++;
            if (runOnce)
            {
                left = player.Center.X < Main.spawnTileX * 16;
                ai1 = 360;
                runOnce = false;
                rotate = Main.rand.Next(120);
            }
            if (phase == 0)
            {
                ai1--;
                if (ai1 <= 0)
                {
                    TransitionPhase(1);
                    return;
                }
            }
            if (phase == 1)
            {
                ai1--;
                int numCrosses = 2;
                if (Main.expertMode && Main.rand.NextBool(3))
                    numCrosses++;
                CircularAttack(30, numCrosses);
                if (ai1 <= 0)
                {
                    TransitionPhase(2);
                    return;
                }
            }
            if (phase == 2)
            {
                ai1--;
                Vector2 toPlayer = player.Center - npc.Center;
                float dist = toPlayer.Length();
                if (ai1 <= 0)
                {
                    if (ai4 > 6)
                    {
                        TransitionPhase(3);
                        return;
                    }
                    else if (dist > 1500)
                    {
                        ai1 = 250;
                        prevLocation = DoIndicator(Main.rand.Next(-120, 121), Main.rand.Next(-14, 15), Main.rand.Next(-120, 121));
                    }
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

                    if (ai4 <= 6)
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
                    {
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
            }
            if (phase == 3)
            {
                ai1--;
                int worldSide = left ? -1 : 1;
                if (ai1 > 1900)
                {
                    ai1 = 1900;
                    prevLocation = player.Center;
                    prevLocation = DoIndicator(worldSide, 540 + ai2, 0, true);
                }
                else if(ai1 <= 1850 && ai1 > 1750 && ai1 % 10 == 0)
                { 
                    ai2 += 100;
                    DoIndicator(worldSide, ai2, 0, true);
                }
                if (ai1 == 1760)
                {
                    DoDash(1, true);
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
                    ai4 += 100;
                    DoIndicator(-worldSide, ai4, ModContent.ProjectileType<EnergySerpentHead>(), true);
                }
                ai1--;
                if (ai1 < 1900 && ai1 % 10 == 0 && !player.dead && player.ZoneUnderworldHeight)
                {
                    if(player.Center.X > npc.Center.X && !left)
                    {
                        Projectile.NewProjectile(player.Center + new Vector2(0, 800), new Vector2(0, -36), ModContent.ProjectileType<EnergySerpentHead>(), 1000, 0, Main.myPlayer, 12, -2);
                    }
                    if (player.Center.X < npc.Center.X && left)
                    {
                        Projectile.NewProjectile(player.Center + new Vector2(0, 800), new Vector2(0, -36), ModContent.ProjectileType<EnergySerpentHead>(), 1000, 0, Main.myPlayer, 12, -2);
                    }
                }
                ai2++;
                ai3 = npc.whoAmI;
                SlitherWall(worldSide, ai2);
                if((Main.expertMode && ai1 % 40 == 0) || (!Main.expertMode && ai1 % 50 == 0))
                {
                    SnakeFromWall(worldSide);
                }
                if(ai1 % 300 == 0)
                {
                    Vector2 circular = new Vector2(1200, 0).RotatedBy(MathHelper.ToRadians(30 + (rotate * 2 + Main.rand.Next(-30, 31)) % 120));
                    SerpentRing(circular + player.Center);
                }
                if (ai1 < 900)
                {
                    TransitionPhase(0);
                    left = !left;
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
                Vector2 circular = new Vector2(Main.rand.NextFloat(12f, 18f) * direction, 0);
                Projectile.NewProjectile(area, circular, ModContent.ProjectileType<EnergySerpentHead>(), (int)(damage2 * 0.8f), 0, Main.myPlayer, 6, -1);
            }
        }
        public void SlitherWall(int direction, float rotate)
        {
            Player player = Main.player[npc.target];
            Vector2 circular = new Vector2(0, -620).RotatedBy(MathHelper.ToRadians(rotate * 2 * direction));
            Vector2 toLocation = new Vector2(npc.Center.X + 20 * direction, player.Center.Y + circular.Y);
            Vector2 goTo = toLocation - npc.Center;
            float speed = 15f;
            if (speed > goTo.Length())
                speed = goTo.Length();
            directVelo = goTo.SafeNormalize(Vector2.Zero) * speed;
        }
        public Vector2 DoIndicator(float rand1, float rand2, float rand3, bool phase3 = false)
        {
            Player player = Main.player[npc.target];
            if (!phase3)
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
                        Projectile.NewProjectile(selectArea + prevLocation + new Vector2(0, 1500), new Vector2(0, -56), ModContent.ProjectileType<EnergySerpentHead>(), damage2 * 2, 0, Main.myPlayer, 32, npc.whoAmI);
                    }
                }
                else if (Main.netMode != 1)
                {
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
        public void CircularAttack(float speed = 30, int amt = 2)
        {
            Player player = Main.player[npc.target];
            Vector2 toLocation = new Vector2(640, 0).RotatedBy(MathHelper.ToRadians(rotate * 2.15f));
            toLocation.Y *= 0.75f;
            toLocation += player.Center;
            Vector2 goTo = toLocation - npc.Center;
            if (goTo.Length() > 48 && ai2 != 1)
                directVelo = goTo.SafeNormalize(Vector2.Zero) * speed;
            else
            {
                directVelo = goTo.SafeNormalize(Vector2.Zero) * 1;
                ai2 = 1;
                npc.Center = toLocation;
            }
            if (ai1 % 120 == 0 && Main.netMode != 1)
            {
                if (amt > 8)
                    amt = 8;
                List<int> unavailable = new List<int>();
                for (int i = 0; i < amt; i++)
                {
                    int rand = Main.rand.Next(8);
                    while (unavailable.Contains(rand))
                    {
                        rand = Main.rand.Next(8);
                    }
                    unavailable.Add(rand);
                    Vector2 spawnLocation = LaserArea(rand);
                    int type = Main.rand.Next(10);
                    if (type < 5)
                        type = 0;
                    else
                        type = 1;
                    float odds = Main.rand.NextFloat(15);
                    if (odds < 2)
                        type = 2;
                    int damage2 = npc.damage / 2;
                    if (Main.expertMode)
                    {
                        damage2 = (int)(damage2 / Main.expertDamage);
                    }
                    Projectile.NewProjectile(spawnLocation + player.Center, Vector2.Zero, ModContent.ProjectileType<CrossLaser>(), (int)(damage2 * 0.8f), 0, Main.myPlayer, type);
                }
            }
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