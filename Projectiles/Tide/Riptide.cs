using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;

namespace SOTS.Projectiles.Tide
{    
    public class Riptide : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Riptide");
		}
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(64);
            AIType = 64;
            Projectile.DamageType = DamageClass.Melee;
			Projectile.alpha = 0;
            Projectile.scale = 1.0f;
            Projectile.ignoreWater = true;
		}
        bool runOnce = true;
        float wasInWaterX = 0;
        float wasInWaterY = 0;
        int counter = 11;
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int width = !runOnce ? Projectile.width * 2 : Projectile.width;
            hitbox.X = (int)Projectile.Center.X - width / 2;
            hitbox.Y = (int)Projectile.Center.Y - width / 2;
            hitbox.Width = width;
            hitbox.Height = width;
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if(runOnce && Projectile.ai[0] == 1)
            {
                runOnce = false;
                if (player.wet || player.HasBuff(BuffID.Wet))
                {
                    wasInWaterX = 2f;
                    wasInWaterY = 1.4f;
                }
                else if(Main.raining && player.Center.Y / 16f < Main.rockLayer)
                {
                    wasInWaterX = 1.2f; 
                    wasInWaterY = 0.75f;
                }
                if(wasInWaterX > 0)
                    SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 71, 1.1f, -0.33f);
            }
            counter--;
            if (wasInWaterX > 0)
            {
                if (counter > -10 * player.meleeSpeed)
                {
                    player.immune = true;
                    player.immuneTime = 2;
                }
                if(counter > 0)
                {
                    player.velocity.Y -= 0.05f;
                    player.velocity.X += Projectile.velocity.X * 2f * wasInWaterX;
                    player.velocity.Y += Projectile.velocity.Y * 2.5f * wasInWaterY;
                }
                else
                {
                    player.velocity.X *= 0.95f;
                    if(player.velocity.Y < 0)
                        player.velocity.Y *= 0.945f;
                    else if(!player.mount.Active)
                        player.velocity.Y += 1f;
                }
            }
            float veloLength = (float)Math.Sqrt(player.velocity.X * player.velocity.X + player.velocity.Y * 0.9f * player.velocity.Y * 0.9f);
            if (counter > -30 && !runOnce)
            {
                for (float j = -1; j < 1f; j += 0.15f)
                {
                    if ((wasInWaterX > 0 && (veloLength > 8f || counter > -24)) || j < 0.0f)
                        for (int i = 0; i < 2; i++)
                        {
                            Entity ent = j < 0.0f ? (Entity)Projectile : player;
                            Vector2 circular = new Vector2(1f, 0).RotatedBy(MathHelper.ToRadians(counter * 11 + i * 180 + player.velocity.ToRotation()));
                            float playerW = ent.width * circular.X;
                            float playerH = ent.height * circular.Y;
                            Vector2 newCircular = new Vector2(playerW, playerH) * 0.7f;
                            Dust dust = Dust.NewDustDirect(ent.Center - ent.velocity * j + newCircular - new Vector2(5), 0, 0, 33, 0, 0, 0, new Color(Main.rand.Next(90, 120), Main.rand.Next(90 + (int)(j * 100), 255), (int)(Main.rand.Next(140, 255) * (1 - j * 0.15f)), 100 + Main.rand.Next(100)));
                            dust.noGravity = true;
                            dust.scale *= 1.2f;
                            dust.velocity *= 0.5f;
                            dust.velocity += circular * 8.0f * (1 + j) - ent.velocity * j;
                        }
                    if (j < 0.0f)
                        j += 0.4f;
                }
            }
            return true;
        }
    }
}