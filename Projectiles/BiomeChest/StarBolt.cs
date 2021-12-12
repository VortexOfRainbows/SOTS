using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.NPCs;
using SOTS.Prim.Trails;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.BiomeChest
{    
    public class StarBolt : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starlight Bolt");
		}
        public override void SetDefaults()
        {
            projectile.width = 54;
            projectile.height = 54; 
            projectile.timeLeft = 120;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;    
            projectile.ignoreWater = true; 
		}
        public Color projColor(bool reverse = false)
        {
            bool fat = projectile.ai[0] == 0;
            if (reverse)
                fat = !fat;
            return fat ? new Color(85, 167, 237) : new Color(220, 139, 226);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Color color = projColor();
            color.A = 0;
            float scale = projectile.scale;
            for(int i = 8; i > 0; i--)
            {
                Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(i * 45));
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + circular, null, color * 0.5f, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            }
            color = new Color(100, 100, 100, 0);
            for (int i = 4; i > 0; i--)
            {
                Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(i * 90));
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + circular, null, color, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 8;
            height = 8;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int width = (int)(projectile.width * projectile.scale * 1.1f);
            hitbox = new Rectangle((int)projectile.Center.X - width / 2, (int)projectile.Center.Y - width / 2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
        }
        bool runOnce = true;
        public override bool PreAI()
        {
            if (runOnce)
            {
                Main.PlaySound(SoundLoader.customSoundType, (int)projectile.Center.X, (int)projectile.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/StarLaser"), 0.6f, 0.2f + Main.rand.NextFloat(-0.1f, 0.1f));
                projectile.scale = 0.6f;
                SOTS.primitives.CreateTrail(new StarTrail(projectile, projColor(), projColor(true), 12));
                runOnce = false;
            }
            return true;
        }
        public override void Kill(int timeLeft)
        {
            if(Main.netMode != NetmodeID.Server)
            {
                for(int i = 0; i < 15; i++)
                {
                    int width = (int)(projectile.width * projectile.scale * 1.0f);
                    Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(width, width) / 2, width, width, ModContent.DustType<CopyDust4>());
                    dust.velocity = dust.velocity * 0.8f + projectile.velocity.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1f) * (float)Math.Sqrt(projectile.velocity.Length());
                    dust.noGravity = true;
                    dust.fadeIn = 0.2f;
                    dust.color = projColor();
                    dust.scale *= 1.4f;
                }
            }
        }
        public override void AI()
        {
            int target = SOTSNPCs.FindTarget_Basic(projectile.Center, 270f, projectile);
            if (target != -1)
            {
                var normal = (Main.npc[target].Center - projectile.Center).SafeNormalize(Vector2.Zero);
                projectile.velocity = Vector2.Lerp(projectile.velocity, normal * projectile.velocity.Length(), 0.075f);
            }
            if(Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
                dust.velocity *= 0.5f;
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
                dust.color = projColor();
                dust.scale *= 1.2f;
                projectile.velocity += projectile.velocity.SafeNormalize(Vector2.Zero) * 0.36f;
            }
        }
	}	
}
			