using Microsoft.Xna.Framework;
using SOTS.Items.Fragments;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Nature;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Nature
{
	public class BiomassBlast : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Biomass Blast");
			Tooltip.SetDefault("Launches an acorn that rapidly accelerates its growth upon hitting an enemy or tile");
		}
		public override void SetDefaults()
		{
            Item.damage = 13; 
            Item.magic = true; 
            Item.width = 42;   
            Item.height = 46;   
            Item.useTime = 39;   
            Item.useAnimation = 39;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;  
            Item.knockBack = 3.25f;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item8;
            Item.shoot = ModContent.ProjectileType<AcornOfJustice>(); 
            Item.shootSpeed = 15.5f;
			Item.mana = 15;
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-0.5f, 0);
        }
        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Scatterseed>(), 1);
			recipe.AddIngredient(ModContent.ItemType<Snakeskin>(), 12);
			recipe.AddIngredient(ModContent.ItemType<DissolvingNature>(), 1);
			recipe.AddIngredient(ItemID.Vilethorn, 1);
			recipe.AddIngredient(ItemID.Acorn, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Scatterseed>(), 1);
			recipe.AddIngredient(ModContent.ItemType<Snakeskin>(), 12);
			recipe.AddIngredient(ModContent.ItemType<DissolvingNature>(), 1);
			recipe.AddIngredient(ItemID.CrimsonRod, 1);
			recipe.AddIngredient(ItemID.Acorn, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
