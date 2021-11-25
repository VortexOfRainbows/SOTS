using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles;
using SOTS.Items.Fragments;

namespace SOTS.Items.Evil
{
	public class TomeOfTheReaper : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tome of the Reaper");
			Tooltip.SetDefault("Cast three scythes that move towards your cursor");
		}
		public override void SetDefaults()
		{
            item.damage = 48; 
            item.magic = true; 
            item.width = 42;   
            item.height = 44;   
            item.useTime = 18;   
            item.useAnimation = 18;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.noMelee = true;  
            item.knockBack = 4.5f;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.LightPurple;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<ReaperScythe>(); 
            item.shootSpeed = 9.5f;
			item.mana = 14;
			item.reuseDelay = 16;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DemonScythe, 1);
			recipe.AddIngredient(ItemID.SoulofFright, 10);
			recipe.AddIngredient(ModContent.ItemType<DissolvingUmbra>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for(int i = -1; i<= 1; i++)
			{
				Vector2 perturbedSpeed = new Vector2(-speedX, -speedY).RotatedBy(MathHelper.ToRadians(15) * i) * (i != 0 ? 0.9f : 1);
				Projectile.NewProjectile(position, perturbedSpeed, type, damage, knockBack, player.whoAmI);
			}
			return false; 
		}
	}
}
