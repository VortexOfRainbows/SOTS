using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Dusts;
using SOTS.NPCs;
using SOTS.Common.GlobalNPCs;
using SOTS.Void;
using SOTS.WorldgenHelpers;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{    
    public class RelocatorBeam : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Relocator Beam");
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
        }
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20; 
            Projectile.timeLeft = 60;
            Projectile.penetrate = -1; 
            Projectile.friendly = true; 
            Projectile.hostile = false; 
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.localNPCHitCooldown = 200;
            Projectile.usesLocalNPCImmunity = true;
        }
        List<Vector2> drawPositionList = new List<Vector2>();
        Vector2 firstDestination = Vector2.Zero;
        List<int> ignoreNPC = new List<int>();
        bool runOnce = true;
        public const float Speed = 3f;
        public const float SeekOutOthersRange = 96f;
        public int GrowthRange = 20;
        public int DegradeRange = 10;
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            knockback = 0;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.myPlayer == Projectile.owner)
                DebuffNPC.SetTimeFreeze(Main.player[Projectile.owner], target, 90);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return ignoreNPC.Contains(target.whoAmI);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (runOnce)
                return false;
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Color color = new Color(140, 140, 140, 0);
            int alpha = 0;
            float endPercent = Projectile.timeLeft / 60f;
            float rotation = Projectile.velocity.ToRotation();
            int max = drawPositionList.Count;
            int startPos = (int)((1 - endPercent) * max);
            int progress = 0;
            for (int i = startPos; i < max; i++)
            {
                float scale = 1f;
                if(progress < GrowthRange)
                {
                    scale = progress / (float)GrowthRange;
                }
                else if(i > max - DegradeRange)
                {
                    int index = i - (max - DegradeRange);
                    scale = 1 - index / (float)DegradeRange;
                }
                Vector2 drawPos = drawPositionList[i];
                Color otherC = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 3), false);
                otherC.A = 0;
                Vector2 sinusoid = new Vector2(0, 12 * scale * (float)Math.Sin(MathHelper.ToRadians(Main.GameUpdateCount * 6 + i * 6))).RotatedBy(rotation);
                Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, color * ((255 - Projectile.alpha) / 255f), rotation, origin, new Vector2(1, scale * 0.75f) * Projectile.scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture, drawPos + sinusoid - Main.screenPosition, null, otherC * ((255 - Projectile.alpha) / 255f), rotation, origin, new Vector2(1, scale * 0.375f) * Projectile.scale, SpriteEffects.None, 0f);
                if (i != drawPositionList.Count - 1)
                    rotation = (drawPositionList[i + 1] - drawPos).ToRotation();
                progress++;
            }
            return false;
        }
        public void SetupLaser()
        {
            float radians = (float)Projectile.velocity.ToRotation();
            Vector2 finalDestination = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            int initialEnemyID = (int)Projectile.knockBack;
            int totalPossibleEnemies = 10;
            if(initialEnemyID >= 0)
            {
                NPC first = Main.npc[initialEnemyID];
                if(first.CanBeChasedBy())
                {
                    firstDestination = first.Center;
                }
            }
            Vector2 position = Projectile.Center;
            Vector2 velocity = Projectile.velocity.SafeNormalize(new Vector2 (0, 1)) * Speed;
            bool end = false;
            int counter = 0;
            while(!end)
            {
                position += velocity;
                drawPositionList.Add(position);
                if (Main.rand.NextBool(3))
                {
                    Dust dust2 = Dust.NewDustPerfect(position, ModContent.DustType<CopyDust4>(), Main.rand.NextVector2Circular(3, 3), 120);
                    dust2.velocity += Projectile.velocity * 0.1f;
                    dust2.noGravity = true;
                    dust2.color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(0, 6.28f));
                    dust2.noGravity = true;
                    dust2.fadeIn = 0.2f;
                    dust2.scale *= 2.2f;
                }
                bool continueToFinal = true;
                if (firstDestination != Vector2.Zero)
                {
                    NPC target = Main.npc[initialEnemyID];
                    Rectangle hitbox = new Rectangle((int)position.X - Projectile.width / 2, (int)position.Y - Projectile.height / 2, Projectile.width, Projectile.height);
                    if (hitbox.Contains(target.Center.ToPoint()))
                    {
                        ignoreNPC.Add(initialEnemyID);
                        firstDestination = Vector2.Zero;
                    }
                    else
                    {
                        redirectGrowth = 0;
                        radians = Redirect(radians, position, firstDestination);
                        continueToFinal = false;
                    }
                }
                else if (totalPossibleEnemies > 0 && counter > 10)
                {
                    initialEnemyID = SOTSNPCs.FindTarget_Ignore(position, ignoreNPC, SeekOutOthersRange);
                    if (initialEnemyID >= 0)
                    {
                        NPC target = Main.npc[initialEnemyID];
                        Rectangle hitbox = new Rectangle((int)position.X - Projectile.width / 2, (int)position.Y - Projectile.height / 2, Projectile.width, Projectile.height);
                        if (hitbox.Contains(target.Center.ToPoint()))
                        {
                            redirectGrowth = 0;
                            ignoreNPC.Add(initialEnemyID);
                            totalPossibleEnemies--;
                        }
                        else
                        {
                            radians = Redirect(radians, position, target.Center);
                            continueToFinal = false;
                        }
                    }
                }
                if (continueToFinal)
                {
                    Rectangle hitbox = new Rectangle((int)position.X - Projectile.width / 2, (int)position.Y - Projectile.height / 2, Projectile.width, Projectile.height);
                    if (hitbox.Contains(finalDestination.ToPoint()))
                    {
                        end = true;
                    }
                    radians = Redirect(radians, position, finalDestination);
                }
                Point16 tileP = position.ToTileCoordinates16();
                if (SOTSWorldgenHelper.TrueTileSolid(tileP.X, tileP.Y))
                {
                    end = true;
                    position -= velocity.SafeNormalize(Vector2.Zero) * 16f;
                }
                velocity = new Vector2(1, 0).RotatedBy(radians) * Speed;
                counter++;
            }
            if (drawPositionList.Count / 3 < DegradeRange)
                DegradeRange = drawPositionList.Count / 3;
            if (drawPositionList.Count / 6 < GrowthRange)
                GrowthRange = drawPositionList.Count / 6;
            //Projectile.velocity = velocity;
            Projectile.Center = position;
        }
        float redirectGrowth = 0.0f;
        public float Redirect(float radians, Vector2 pos, Vector2 npc)
        {
            Vector2 toNPC = npc - pos;
            float speed = 1.0f + redirectGrowth;
            Vector2 rnVelo = new Vector2(Speed, 0).RotatedBy(radians);
            rnVelo += toNPC.SafeNormalize(Vector2.Zero) * speed;
            float npcRad = rnVelo.ToRotation();
            redirectGrowth += 0.1f;
            return npcRad;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (runOnce)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
                    dust2.velocity += Projectile.velocity * 0.1f;
                    dust2.noGravity = true;
                    dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 18));
                    dust2.noGravity = true;
                    dust2.fadeIn = 0.2f;
                    dust2.scale *= 2.2f;
                }
                SetupLaser();
                runOnce = false;
                player.immuneTime = 40;
                player.immune = true;
                player.AddBuff(BuffID.ChaosState, 960);
                player.AddBuff(ModContent.BuffType<SurpriseAttack>(), 420);
                player.Center = Projectile.Center;
                player.velocity *= 0.1f;
                player.velocity += Projectile.velocity * 4.6f;
                player.velocity.Y -= 2;
                SOTSUtils.PlaySound(SoundID.Item72, (int)player.Center.X, (int)player.Center.Y, 1.2f, 0.1f);
                for (int i = 0; i < 20; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>(), 0, 0, 120);
                    dust2.velocity += Projectile.velocity * 0.1f;
                    dust2.noGravity = true;
                    dust2.color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 18));
                    dust2.noGravity = true;
                    dust2.fadeIn = 0.2f;
                    dust2.scale *= 2.2f;
                }
            }
            float endPercent = Projectile.timeLeft / 60f;
            Projectile.alpha = (int)(255 - 255 * endPercent * endPercent);
        }
	}
}
		
			