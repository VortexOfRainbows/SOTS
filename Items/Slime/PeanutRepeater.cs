using System;
using Microsoft.Xna.Framework;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	public class PeanutRepeater : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Peanut Repeater");
			Tooltip.SetDefault("Launches tasty peanuts that attract Pinky Air Raids\n20% chance to not consume ammo");
		}
		public override void SetDefaults()
		{
            item.damage = 27;  
            item.ranged = true;    
            item.width = 66;  
            item.height = 22;   
            item.useTime = 26;  
            item.useAnimation = 26;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.noMelee = true; 
            item.knockBack = 5f;
            item.value = Item.sellPrice(0, 2, 50, 0);
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<Projectiles.Nature.Peanut>(); 
            item.shootSpeed = 19f;
			item.useAmmo = ModContent.ItemType<Peanut>();
		}
        public override bool ConsumeAmmo(Player player)
        {
            return !Main.rand.NextBool(5);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10f, -3f);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DissolvingNature>(), 1);
			recipe.AddIngredient(ModContent.ItemType<CorrosiveGel>(), 20);
			recipe.AddIngredient(ModContent.ItemType<Wormwood>(), 20);
			recipe.AddIngredient(ModContent.ItemType<Peanut>(), 40);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
