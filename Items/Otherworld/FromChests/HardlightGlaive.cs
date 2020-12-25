using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	public class HardlightGlaive : VoidItem
	{
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/HardlightGlaiveGlow");
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hardlight Glaive");
			Tooltip.SetDefault("Unleash a burst of lightning that deals 160% damage");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 40;
			item.melee = true;
			item.width = 48;
			item.height = 54;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 5;
			item.knockBack = 4.5f;
            item.value = Item.sellPrice(0, 5, 75, 0);
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.DD2_GhastlyGlaivePierce;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("HardlightGlaive"); 
            item.shootSpeed = 6.2f;
			item.noUseGraphic = true;
			item.noMelee = true;
		}
		public override bool BeforeUseItem(Player player)
		{
			return player.ownedProjectileCounts[item.shoot] < 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CursedImpale", 1);
			recipe.AddIngredient(null, "ImperialPike", 1);
			recipe.AddIngredient(null, "GoldGlaive", 1);
			recipe.AddIngredient(null, "HardlightAlloy", 12);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
              int numberProjectiles = 1;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0));
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
		}
        public override void GetVoid(Player player)
        {
			voidMana = 7;
        }
    }
}
	
