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
            Projectile.width = 54;
            Projectile.height = 54; 
            Projectile.timeLeft = 120;
            Projectile.penetrate = 1; 
            Projectile.friendly = true; 
            Projectile.hostile = false; 
            Projectile.tileCollide = false;    
            Projectile.ignoreWater = true; 
		}
        public Color projColor(bool reverse = false)
        {
            bool fat = Projectile.ai[0] == 0;
            if (reverse)
                fat = !fat;
            return fat ? new Color(85, 167, 237) : new Color(220, 139, 226);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Color color = projColor();
            color.A = 0;
            float scale = Projectile.scale;
            for(int i = 8; i > 0; i--)
            {
                Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(i * 45));
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + circular, null, color * 0.5f, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            }
            color = new Color(100, 100, 100, 0);
            for (int i = 4; i > 0; i--)
            {
                Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(i * 90));
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + circular, null, color, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 8;
            height = 8;
            return true;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int width = (int)(Projectile.width * Projectile.scale * 1.1f);
            hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
        }
        bool runOnce = true;
        public override bool PreAI()
        {
            if (runOnce)
            {
                SoundEngine.PlaySound(SoundLoader.customSoundType, (int)Projectile.Center.X, (int)Projectile.Center.Y, Mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/StarLaser"), 0.6f, 0.2f + Main.rand.NextFloat(-0.1f, 0.1f));
                Projectile.scale = 0.6f;
                if (Main.netMode != NetmodeID.Server)
                    SOTS.primitives.CreateTrail(new StarTrail(Projectile, projColor(), projColor(true), 12));
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
                    int width = (int)(Projectile.width * Projectile.scale * 1.0f);
                    Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(width, width) / 2, width, width, ModContent.DustType<CopyDust4>());
                    dust.velocity = dust.velocity * 0.8f + Projectile.velocity.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1f) * (float)Math.Sqrt(Projectile.velocity.Length());
                    dust.noGravity = true;
                    dust.fadeIn = 0.2f;
                    dust.color = projColor();
                    dust.scale *= 1.4f;
                }
            }
        }
        public override void AI()
        {
            int target = SOTSNPCs.FindTarget_Basic(Projectile.Center, 270f, Projectile);
            if (target != -1)
            {
                var normal = (Main.npc[target].Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, normal * Projectile.velocity.Length(), 0.075f);
            }
            if(Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
                dust.velocity *= 0.5f;
                dust.noGravity = true;
                dust.fadeIn = 0.2f;
                dust.color = projColor();
                dust.scale *= 1.2f;
                Projectile.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * 0.36f;
            }
        }
	}	
}
			