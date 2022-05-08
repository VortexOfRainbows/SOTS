using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	public class SupernovaHammer : ModItem
	{
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/SupernovaHammerGlow");
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[Item.type].Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Supernova Hammer");
			Tooltip.SetDefault("Critical strikes release 3 seekers that do 50% critical damage\nKilling enemies releases 3 seekers that do 70% damage\nEnemies killed by seekers release 2 more seekers, each doing 75% damage\nSeekers home onto enemies, but do not attack the enemies they are released from");
		}
		public override void SetDefaults()
		{
			Item.damage = 30;
			Item.melee = true;
			Item.width = 50;
			Item.height = 50;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 5;
			Item.knockBack = 5f;
			Item.value = Item.sellPrice(0, 6, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.DD2_MonkStaffSwing;
			Item.autoReuse = true;
			Item.shoot = mod.ProjectileType("SupernovaHammer");
			Item.shootSpeed = 24f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Diamond, 5);
			recipe.AddIngredient(null, "StarlightAlloy", 15);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}