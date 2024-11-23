using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.Items.Pyramid;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Tide
{    
    public class FishBomb : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
            Main.projFrames[Projectile.type] = 5;
        }
        public override void SetDefaults()
        {
			Projectile.height = 66;
			Projectile.width = 66;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.alpha = 0;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 32;
            height = 32;
            return true;
        }
        public override bool PreAI()
		{
			return true;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity.X *= 0.95f;
            return false;
        }
        public override void AI()
        {
            if (Projectile.ai[0] <= 0)
            {
                Projectile.velocity.Y -= 1.2f;
            }
            Projectile.velocity.X *= 0.992f;
            Projectile.rotation += Projectile.velocity.X * 0.01f;
            Projectile.spriteDirection = -SOTSUtils.SignNoZero(Projectile.velocity.X);
            int i = (int)Projectile.Center.X / 16;
            int j = (int)Projectile.Center.Y / 16;
            if (WorldGen.InWorld(i, j) && Main.tile[i, j].LiquidAmount > 100)
            {
                Projectile.velocity.Y -= 0.06f;
                if (Projectile.velocity.Y > 0)
                    Projectile.velocity.Y *= 0.95f;
                Projectile.ai[0] += 2;
            }
            else
            {
                Projectile.velocity.Y += 0.1f;
                if (Projectile.velocity.Y < 0)
                    Projectile.velocity.Y *= 0.95f;
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 170 && Projectile.ai[0] < 173)
            {
                SOTSUtils.PlaySound(SoundID.Item131, Projectile.Center, 1.4f, 0.35f);
                Projectile.ai[0] = 173;
            }
            if (Projectile.ai[0] > 180)
            {
                Projectile.frameCounter++;
                if (Projectile.frameCounter >= 8)
                {
                    if (Projectile.frame > 3)
                    {
                        Projectile.Kill();
                    }
                    Projectile.friendly = false;
                    Projectile.frameCounter = 0;
                    Projectile.frame++;
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            Projectile.position.Y += 16;
            SOTSUtils.PlaySound(SoundID.DD2_BetsyFireballImpact, Projectile.Center, 1.0f, 0.8f);
            SOTSUtils.PlaySound(SoundID.DD2_KoboldExplosion, Projectile.Center, 1.0f, 0.8f);
            for(int i = 0; i < 8; i++)
            {
                Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center - new Vector2(15, 15) + Main.rand.NextVector2Circular(33, 33), new Vector2(Main.rand.NextFloat(2, 3), 0).RotatedBy((i + Main.rand.NextFloat()) * MathHelper.PiOver4), Main.rand.Next(61, 64), 0.9f);
            }
            for(int i = 0; i < 50; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Scale: 1.7f);
                d.velocity *= 1.7f;
            }
            if(Projectile.owner != Main.myPlayer)
            {
                return;
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2CircularEdge(3, 3) + new Vector2(0, -2f);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + velocity.SNormalize() * Main.rand.NextFloat(16, 24), velocity * Main.rand.NextFloat(.5f, 1f), ModContent.ProjectileType<FishBlood>(), Projectile.damage, Projectile.damage, Main.myPlayer);
                }
            }
            Player player = Main.player[Projectile.owner];
            bool beforeWet = player.wet;
            bool beforeLine = player.accFishingLine;
            bool beforeLava = player.accLavaFishing;
            player.wet = false;
            player.accFishingLine = true;
            player.accLavaFishing = true;
            Vector2 save = Main.LocalPlayer.Center;
            SOTSDetours.UsingFishBomb = true;

            int fishToCatch = 5;
            if (player.SOTSPlayer().PurpleBalloon)
                fishToCatch++;
            if(player.SOTSPlayer().DoubleVisionActive)
                fishToCatch += player.SOTSPlayer().BonusFishingLines;
            for(int i = 0; i < fishToCatch; i++)
            {
                int attempts = 1;
                while (Projectile.localAI[1] == 0)
                {
                    Projectile.FishingCheck();
                    if (attempts > 3 && Main.rand.NextFloat(1) > 3f / attempts)
                        break;
                    attempts++;
                }
                if (Projectile.localAI[1] != 0)
                {
                    if (Projectile.localAI[1] < 0)
                    {
                        bool spawnNPCs = true;
                        int type = (int)-Projectile.localAI[1];
                        if(type == NPCID.TownSlimeRed)
                        {
                            if (NPC.CountNPCS(type) > 0)
                            {
                                spawnNPCs = false;
                            }
                        }
                        if(spawnNPCs)
                            Projectile.NewProjectile(player.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SpawnEnemyProj>(), 0, 0, Main.myPlayer, type);
                    }
                    else
                    {
                        int type = (int)Projectile.localAI[1];
                        Vector2 tryPosition = Projectile.Center;
                        for(int j = 0; j < 5; j++)
                        {
                            tryPosition = Projectile.Center + Main.rand.NextVector2Circular(120, 120);
                            int x = (int)tryPosition.X / 16;
                            int y = (int)tryPosition.Y / 16;
                            if (WorldGen.InWorld(x, y) && Main.tile[x, y].LiquidAmount > 100)
                            {
                                break;
                            }
                            tryPosition = Projectile.Center;
                        }
                        Main.LocalPlayer.Center = tryPosition;
                        Item item = new Item(type);
                        item.stack = FishingItemStack(item);
                        item.alpha = 255;
                        if(item.TryGetGlobalItem(out PrefixItem preItem))
                        {
                            preItem.FloatsInWater = true;
                        }
                        Main.LocalPlayer.QuickSpawnItemDirect(Projectile.GetSource_CatchEntity(item), item, item.stack);
                    }
                    Projectile.localAI[1] = 0;
                }
            }

            SOTSDetours.UsingFishBomb = false;

            player.wet = beforeWet;
            player.accFishingLine = beforeLine;
            player.accLavaFishing = beforeLava;
            Main.LocalPlayer.Center = save;
        }
        private int FishingItemStack(Item item)
        {
            Player player = Main.player[Projectile.owner];
            if (item.type == ItemID.BombFish)
            {
                int finalFishingLevel = player.GetFishingConditions().FinalFishingLevel;
                int minValue = (finalFishingLevel / 20 + 3) / 2;
                int num = (finalFishingLevel / 10 + 6) / 2;
                if (Main.rand.Next(50) < finalFishingLevel)
                    num++;

                if (Main.rand.Next(100) < finalFishingLevel)
                    num++;

                if (Main.rand.Next(150) < finalFishingLevel)
                    num++;

                if (Main.rand.Next(200) < finalFishingLevel)
                    num++;

                int stack = Main.rand.Next(minValue, num + 1);
                item.stack = stack;
            }

            if (item.type == ItemID.FrostDaggerfish)
            {
                int finalFishingLevel2 = player.GetFishingConditions().FinalFishingLevel;
                int minValue2 = (finalFishingLevel2 / 4 + 15) / 2;
                int num2 = (finalFishingLevel2 / 2 + 40) / 2;
                if (Main.rand.Next(50) < finalFishingLevel2)
                    num2 += 6;

                if (Main.rand.Next(100) < finalFishingLevel2)
                    num2 += 6;

                if (Main.rand.Next(150) < finalFishingLevel2)
                    num2 += 6;

                if (Main.rand.Next(200) < finalFishingLevel2)
                    num2 += 6;

                int stack2 = Main.rand.Next(minValue2, num2 + 1);
                item.stack = stack2;
            }

            PlayerLoader.ModifyCaughtFish(player, item);
            ItemLoader.CaughtFishStack(item);
            return item.stack;
        }
    }
    public class FishBlood : ModProjectile
    {
        public override string Texture => "SOTS/Projectiles/Tide/FishBomb";
        public override bool PreDraw(ref Color lightColor)
        {
            float scaler = 1f;
            Texture2D texture = SOTSUtils.WhitePixel;
            Vector2 drawOrigin = new Vector2(0, 1);
            Vector2 previous = Projectile.Center;
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                float perc = 1 - i / (float)Projectile.oldPos.Length;
                Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2;
                Vector2 toPrev = previous - center;
                float dist = toPrev.Length();
                if (dist > 1600)
                    break;
                float rot = toPrev.ToRotation();
                Vector2 stretch = new Vector2(dist / texture.Width, perc * 2f * scaler);
                Color color = Projectile.GetAlpha(new Color(200, 0, 0)).MultiplyRGBA(lightColor);
                Main.EntitySpriteDraw(texture, center - Main.screenPosition, null, color * perc, rot, drawOrigin, stretch, SpriteEffects.FlipVertically, 0f);
                previous = center;
            }
            return false;
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 30;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = Projectile.height = 16;
            Projectile.timeLeft = 128;
            Projectile.penetrate = 5;
            Projectile.alpha = 0;
            Projectile.localNPCHitCooldown = 40;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = false;
        }
        public override void AI()
        {
            Projectile.velocity.Y += 0.032f;
            Projectile.velocity *= 0.996f;
            Projectile.alpha += 2;
        }
        public override void OnKill(int timeLeft)
        {

        }
    }
}
		