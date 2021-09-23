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
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Supernova Hammer");
			Tooltip.SetDefault("Critical strikes release 3 seekers that do 50% critical damage\nKilling enemies releases 3 seekers that do 70% damage\nEnemies killed by seekers release 2 more seekers, each doing 75% damage\nSeekers home onto enemies, but do not attack the enemies they are released from");
		}
		public override void SetDefaults()
		{
			item.damage = 27;
			item.melee = true;
			item.width = 50;
			item.height = 50;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.knockBack = 5f;
			item.value = Item.sellPrice(0, 6, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.DD2_MonkStaffSwing;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("SupernovaHammer");
			item.shootSpeed = 24f;
			item.noMelee = true;
			item.noUseGraphic = true;
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