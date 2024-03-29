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
			 this.SetResearchCost(1);
        }
		public override void SetDefaults()
		{
            Item.damage = 27;  
            Item.DamageType = DamageClass.Ranged;    
            Item.width = 62;  
            Item.height = 26;   
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
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return !Main.rand.NextBool(5);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10f, -3f);
        }
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DissolvingNature>(), 1).AddIngredient(ModContent.ItemType<CorrosiveGel>(), 20).AddIngredient(ModContent.ItemType<Wormwood>(), 20).AddIngredient(ModContent.ItemType<Peanut>(), 40).AddTile(TileID.Anvils).Register();
		}
	}
}
