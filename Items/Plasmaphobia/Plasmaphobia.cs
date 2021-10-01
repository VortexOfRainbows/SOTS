using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SOTS.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Plasmaphobia
{
	public class Plasmaphobia : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasmaphobia");
			Tooltip.SetDefault("Fires supercharged plasma arrows");
		}
		public override void SetDefaults()
		{
            item.damage = 55; 
            item.ranged = true;  
            item.width = 36;   
            item.height = 54; 
            item.useTime = 25; 
            item.useAnimation = 25;
            item.useStyle = 5;    
            item.noMelee = true;
            item.knockBack = 4f;
            item.value = Item.sellPrice(0, 2, 25, 0);
            item.rare = 6;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("SharangaBolt"); 
            item.shootSpeed = 10.5f;
			item.useAmmo = ItemID.WoodenArrow;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<PlasmaphobiaBolt>(), damage, knockBack, player.whoAmI);
			return false;
		}
	}
    public class PlasmaphobiaBolt : ModProjectile, IPixellated
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plasma Bolt");
            //ProjectileID.Sets.TrailCacheLength[projectile.type] = 30;
            //ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            //Main.projFrames[projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            projectile.damage = 15;
            projectile.width = 24;
            projectile.height = 24;
            projectile.aiStyle = 0;
            projectile.scale = 0f;
            projectile.tileCollide = true;
            projectile.penetrate = 8;
            projectile.timeLeft = 270;
            projectile.friendly = true;
            projectile.ranged = true;
        }
        public override void AI()
        {
            projectile.ai[0] += 0.01f;
            if (projectile.scale < 1)
                projectile.scale += 0.05f;
            projectile.rotation = projectile.velocity.ToRotation();
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<PlasmaStar>(), projectile.damage, projectile.knockBack, projectile.owner);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;
        public void Draw(SpriteBatch spriteBatch, Color drawColor)
        {
            #region shader
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Color color = Color.White;
            color.A = 0;
            Vector4 colorMod = color.ToVector4();
            SOTS.FireballShader.Parameters["colorMod"].SetValue(colorMod);
            SOTS.FireballShader.Parameters["noise"].SetValue(mod.GetTexture("TrailTextures/vnoise"));
            SOTS.FireballShader.Parameters["pallette"].SetValue(mod.GetTexture("TrailTextures/Pallette2"));
            SOTS.FireballShader.Parameters["opacity2"].SetValue(0.25f);
            SOTS.FireballShader.Parameters["counter"].SetValue(projectile.ai[0]);
            SOTS.FireballShader.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), (projectile.Center - Main.screenPosition) / 2, null, Color.Pink * projectile.Opacity, projectile.rotation, new Vector2(50, 50), (projectile.scale * new Vector2(2f, 0.3f) / 2) / 2, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            #endregion
        }
    }
    public class PlasmaStar : ModProjectile, IPixellated
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plasma Star");
            //ProjectileID.Sets.TrailCacheLength[projectile.type] = 30;
            //ProjectileID.Sets.TrailingMode[projectile.type] = 0;
            //Main.projFrames[projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            projectile.damage = 15;
            projectile.width = 75;
            projectile.height = 75;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 200;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.scale = 0;
        }

        public override void AI()
        {
            if (projectile.scale < 1)
                projectile.scale += 0.05f;
            projectile.rotation += 0.01f;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;
        public void Draw(SpriteBatch spriteBatch, Color drawColor)
        {
            Color color = Color.Purple;
            color.A = 0;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            SOTS.GodrayShader.Parameters["distance"].SetValue(3);
            SOTS.GodrayShader.Parameters["colorMod"].SetValue(color.ToVector4());
            SOTS.GodrayShader.Parameters["noise"].SetValue(mod.GetTexture("TrailTextures/noise"));
            SOTS.GodrayShader.Parameters["rotation"].SetValue(projectile.rotation);
            SOTS.GodrayShader.Parameters["opacity2"].SetValue(0.4f);
            SOTS.GodrayShader.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), (projectile.Center - Main.screenPosition) / 2, null, Color.White, 0f, new Vector2(50, 50), projectile.scale / 2, SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < 2; i++)
                spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), (projectile.Center - Main.screenPosition) / 2, null, color * 3, projectile.rotation, new Vector2(50, 50), projectile.scale / 4, SpriteEffects.None, 0f);
        }
    }
}
