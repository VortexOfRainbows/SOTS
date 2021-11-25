using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Nature;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Nature
{
	public class BiomassBarrage : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Biomass Bloom");
			Tooltip.SetDefault("Launches a cluster of acorns");
		}
		public override void SetDefaults()
		{
            item.damage = 24; 
            item.magic = true; 
            item.width = 54;   
            item.height = 60;   
            item.useTime = 30;   
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.noMelee = true;  
            item.knockBack = 3.25f;
            item.value = Item.sellPrice(0, 4, 50, 0);
            item.rare = ItemRarityID.LightPurple;
            item.UseSound = SoundID.Item8;
            item.shoot = ModContent.ProjectileType<AcornsOfJustice>(); 
            item.shootSpeed = 17.5f;
			item.autoReuse = true;
			item.mana = 11;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1f, 0);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BiomassBlast>(), 1);
			recipe.AddIngredient(ModContent.ItemType <CursedMatter>(), 3);
			recipe.AddIngredient(ItemID.OrichalcumBar, 12);
			recipe.AddIngredient(ItemID.SoulofLight, 12);
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddIngredient(ItemID.Emerald, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BiomassBlast>(), 1);
			recipe.AddIngredient(ModContent.ItemType<CursedMatter>(), 3);
			recipe.AddIngredient(ItemID.MythrilBar, 12);
			recipe.AddIngredient(ItemID.SoulofLight, 12);
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddIngredient(ItemID.Emerald, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
