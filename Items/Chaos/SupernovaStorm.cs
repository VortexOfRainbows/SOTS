using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;
using SOTS.Items.Permafrost;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Chaos;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace SOTS.Items.Chaos
{
	public class SupernovaStorm : ModItem
	{
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Chaos/SupernovaStormGlow").Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Color color = Color.White;
			for (int k = 0; k < 6; k++)
			{
				Vector2 circular = new Vector2(3 * scale, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount * 2));
				color = ColorHelpers.pastelAttempt(MathHelper.ToRadians(k * 60)) * 0.33f;
				color.A = 0;
				Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)) + circular, null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Chaos/SupernovaStormGlow").Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 60;
			Item.DamageType = DamageClass.Magic;
            Item.width = 40;    
            Item.height = 52; 
            Item.useTime = 14; 
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item92;
            Item.noMelee = true; 
            Item.autoReuse = true;
            Item.shootSpeed = 3f; 
			Item.shoot = ModContent.ProjectileType<SupernovaLaser>();
			Item.mana = 7;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Chaos/SupernovaStormGlow").Value;
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PhaseBar>(), 18).AddIngredient(ModContent.ItemType<ShardstormSpell>(), 1).AddTile(TileID.MythrilAnvil).Register();
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			Vector2 cursorPos = Main.MouseWorld;
			Vector2 skyPosition = new Vector2(MathHelper.Lerp(position.X, cursorPos.X, 0.44f), position.Y - 960);
            int numberProjectiles = 1; 
            for (int i = 0; i < numberProjectiles; i++)
            {
				Vector2 rotateArea = new Vector2(160, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
				skyPosition += rotateArea;
				Vector2 speed = (cursorPos - skyPosition).SafeNormalize(Vector2.Zero) * velocity.Length();
                Projectile.NewProjectile(source, skyPosition, speed, type, damage, knockback, player.whoAmI);
            }
            return false;
		}
	}
}
