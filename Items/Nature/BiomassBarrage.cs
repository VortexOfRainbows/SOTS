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
            Item.damage = 24; 
            Item.magic = true; 
            Item.width = 54;   
            Item.height = 60;   
            Item.useTime = 30;   
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;  
            Item.knockBack = 3.25f;
            Item.value = Item.sellPrice(0, 4, 50, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item8;
            Item.shoot = ModContent.ProjectileType<AcornsOfJustice>(); 
            Item.shootSpeed = 17.5f;
			Item.autoReuse = true;
			Item.mana = 11;
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
