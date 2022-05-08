using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
			DisplayName.SetDefault("Blinding Assault");
		}
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24; 
            projectile.timeLeft = 6;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.alpha = 255;
		}
        bool runOnce = true;
        bool hit = false;
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            if (runOnce)
            {
                runOnce = false;
                float counter = 0;
                for(int i = 0; i < 20; i ++)
                {
                    Vector2 loc = new Vector2(player.Center.X - 14 * player.direction, player.Center.Y + player.gfxOffY) - new Vector2(5);
                    int num1 = Dust.NewDust(new Vector2(loc.X, loc.Y), 2, 2, 235);
                    Main.dust[num1].noGravity = true;
                    Main.dust[num1].velocity *= 1.75f;
                    Main.dust[num1].scale = 2.25f;
                    Main.dust[num1].shader = GameShaders.Armor.GetSecondaryShader(player.cBack, player);
                }
                for (int i = 0; i < 320; i += 2)
                {
                    player.direction = projectile.velocity.X > 0 ? 1 : -1;
                    counter += 2.55f;
                    if (Collision.SolidCollision(player.position + projectile.velocity * 2 + new Vector2(8, 8), player.width - 8, player.height - 8))
                        break;

                    for (int n = 0; n < Main.npc.Length; n++)
                    {
                        NPC npc = Main.npc[n];
                        if (npc.active && npc.Hitbox.Intersects(new Rectangle((int)player.position.X - 12, (int)player.position.Y - 12, player.width + 12, player.height + 12)) && !npc.friendly && !npc.dontTakeDamage && npc.immune[player.whoAmI] <= 0)
                        {
                            if (projectile.owner == Main.myPlayer && projectile.friendly)
                                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("BlinkDamage1"), SOTSPlayer.ModPlayer(player).BlinkDamage, projectile.knockBack, Main.myPlayer);
                            npc.immune[player.whoAmI] = 1;
                            hit = true;
                            break;
                        }
                    }
                    player.position += projectile.velocity * 2;
                    Vector2 loc = new Vector2(player.Center.X - 12 * player.direction, player.Center.Y + player.gfxOffY) - new Vector2(5);
                    int num1 = Dust.NewDust(new Vector2(loc.X, loc.Y), 2, 2, 235);
                    Main.dust[num1].noGravity = true;
                    Main.dust[num1].velocity *= 0.75f / (1 + SOTSPlayer.ModPlayer(player).BlinkedAmount);
                    Main.dust[num1].scale = 1.75f;
                    Main.dust[num1].shader = GameShaders.Armor.GetSecondaryShader(player.cBack, player);
                }
                for (int i = 0; i < 20; i++)
                {
                    Vector2 loc = new Vector2(player.Center.X - 12 * player.direction, player.Center.Y + player.gfxOffY) - new Vector2(5);
                    int num1 = Dust.NewDust(new Vector2(loc.X, loc.Y), 2, 2, 235);
                    Main.dust[num1].noGravity = true;
                    Main.dust[num1].velocity *= 1.75f;
                    Main.dust[num1].scale = 2.25f;
                    Main.dust[num1].shader = GameShaders.Armor.GetSecondaryShader(player.cBack, player);
                }
                if(player.velocity.X > projectile.velocity.X && projectile.velocity.X < 0)
                {
                    player.velocity.X = projectile.velocity.X;
                }
                if (player.velocity.X < projectile.velocity.X && projectile.velocity.X > 0)
                {
                    player.velocity.X = projectile.velocity.X;
                }
                if (player.velocity.Y > projectile.velocity.Y && projectile.velocity.Y < 0)
                {
                    player.velocity.Y = projectile.velocity.Y;
                }
                if (player.velocity.Y < projectile.velocity.Y && projectile.velocity.Y > 0)
                {
                    player.velocity.Y = projectile.velocity.Y;
                }
                float addX = projectile.velocity.X * 4 * 2.0f;
                float addY = projectile.velocity.Y * 4 * 1.4f;
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
                SoundEngine.PlaySound(SoundID.Item8, Main.player[projectile.owner].Center);
                modPlayer.BlinkedAmount += 1.25f;
            }
        }
	}
}
		
			