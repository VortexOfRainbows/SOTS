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
            Item.DamageType = DamageClass.Magic; 
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
			CreateRecipe(1).AddIngredient(ModContent.ItemType<BiomassBlast>(), 1).AddIngredient(ModContent.ItemType <CursedMatter>(), 3).AddIngredient(ItemID.OrichalcumBar, 12).AddIngredient(ItemID.SoulofLight, 12).AddIngredient(ItemID.SoulofNight, 12).AddIngredient(ItemID.Emerald, 1).AddTile(TileID.MythrilAnvil).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<BiomassBlast>(), 1).AddIngredient(ModContent.ItemType<CursedMatter>(), 3).AddIngredient(ItemID.MythrilBar, 12).AddIngredient(ItemID.SoulofLight, 12).AddIngredient(ItemID.SoulofNight, 12).AddIngredient(ItemID.Emerald, 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
