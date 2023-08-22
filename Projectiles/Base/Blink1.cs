using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Base
{    
    public class Blink1 : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Blinding Assault");
		}
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24; 
            Projectile.timeLeft = 6;
            Projectile.penetrate = 1; 
            Projectile.friendly = true; 
            Projectile.hostile = false; 
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
		}
        bool runOnce = true;
        bool hit = false;
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            if (runOnce)
            {
                runOnce = false;
                float counter = 0;
                for(int i = 0; i < 20; i ++)
                {
                    Vector2 loc = new Vector2(player.Center.X - 14 * player.direction, player.Center.Y + player.gfxOffY) - new Vector2(5);
                    int num1 = Dust.NewDust(new Vector2(loc.X, loc.Y), 2, 2, DustID.LifeDrain);
                    Main.dust[num1].noGravity = true;
                    Main.dust[num1].velocity *= 1.75f;
                    Main.dust[num1].scale = 2.25f;
                    Main.dust[num1].shader = GameShaders.Armor.GetSecondaryShader(player.cBack, player);
                }
                for (int i = 0; i < 320; i += 2)
                {
                    player.direction = Projectile.velocity.X > 0 ? 1 : -1;
                    counter += 2.55f;
                    if (Collision.SolidCollision(player.position + Projectile.velocity * 2 + new Vector2(8, 8), player.width - 8, player.height - 8))
                        break;

                    for (int n = 0; n < Main.npc.Length; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.Hitbox.Intersects(new Rectangle((int)player.position.X - 12, (int)player.position.Y - 12, player.width + 12, player.height + 12)) && !npc.friendly && !npc.dontTakeDamage && npc.immune[player.whoAmI] <= 0)
                        {
                            if (Projectile.owner == Main.myPlayer && Projectile.friendly)
                                Projectile.NewProjectile(Projectile.GetSource_FromThis(), npc.Center.X, npc.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<BlinkDamage1>(), SOTSPlayer.ModPlayer(player).BlinkDamage, Projectile.knockBack, Main.myPlayer);
                            npc.immune[player.whoAmI] = 1;
                            hit = true;
                            break;
                        }
                    }
                    player.position += Projectile.velocity * 2;
                    Vector2 loc = new Vector2(player.Center.X - 12 * player.direction, player.Center.Y + player.gfxOffY) - new Vector2(5);
                    int num1 = Dust.NewDust(new Vector2(loc.X, loc.Y), 2, 2, DustID.LifeDrain);
                    Main.dust[num1].noGravity = true;
                    Main.dust[num1].velocity *= 0.75f / (1 + SOTSPlayer.ModPlayer(player).BlinkedAmount);
                    Main.dust[num1].scale = 1.75f;
                    Main.dust[num1].shader = GameShaders.Armor.GetSecondaryShader(player.cBack, player);
                }
                for (int i = 0; i < 20; i++)
                {
                    Vector2 loc = new Vector2(player.Center.X - 12 * player.direction, player.Center.Y + player.gfxOffY) - new Vector2(5);
                    int num1 = Dust.NewDust(new Vector2(loc.X, loc.Y), 2, 2, DustID.LifeDrain);
                    Main.dust[num1].noGravity = true;
                    Main.dust[num1].velocity *= 1.75f;
                    Main.dust[num1].scale = 2.25f;
                    Main.dust[num1].shader = GameShaders.Armor.GetSecondaryShader(player.cBack, player);
                }
                if(player.velocity.X > Projectile.velocity.X && Projectile.velocity.X < 0)
                {
                    player.velocity.X = Projectile.velocity.X;
                }
                if (player.velocity.X < Projectile.velocity.X && Projectile.velocity.X > 0)
                {
                    player.velocity.X = Projectile.velocity.X;
                }
                if (player.velocity.Y > Projectile.velocity.Y && Projectile.velocity.Y < 0)
                {
                    player.velocity.Y = Projectile.velocity.Y;
                }
                if (player.velocity.Y < Projectile.velocity.Y && Projectile.velocity.Y > 0)
                {
                    player.velocity.Y = Projectile.velocity.Y;
                }
                float addX = Projectile.velocity.X * 4 * 2.0f;
                float addY = Projectile.velocity.Y * 4 * 1.4f;
                if (Math.Abs(player.velocity.X + addX) < 27)
                    player.velocity.X += addX;
                if (Math.Abs(player.velocity.Y + addY) < 27)
                    player.velocity.Y += addY - 1;

                if(!hit || modPlayer.BlinkedAmount >= 2)
                {
                    float temp = modPlayer.BlinkedAmount;
                    if (temp > 2)
                        temp = 2;
                     
                    float bonus = 0.85f + temp * 0.5f;
                    player.AddBuff(BuffID.ChaosState, (int)(bonus * (60 + counter) * modPlayer.blinkPackMult));
                }
                player.immuneTime = 10 + (int)(counter / 12f);
                player.immune = true;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, Main.player[Projectile.owner].Center);
                modPlayer.BlinkedAmount += 1.25f;
            }
        }
	}
}
		
			