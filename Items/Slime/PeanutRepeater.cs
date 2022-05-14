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
            Item.damage = 27;  
            Item.DamageType = DamageClass.Ranged;    
            Item.width = 66;  
            Item.height = 22;   
            Item.useTime = 26;  
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true; 
            Item.knockBack = 5f;
            Item.value = Item.sellPrice(0, 2, 50, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Nature.Peanut>(); 
            Item.shootSpeed = 19f;
			Item.useAmmo = ModContent.ItemType<Peanut>();
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
			Recipe recipe = new Recipe(mod);
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
