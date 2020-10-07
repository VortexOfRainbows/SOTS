using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.IceStuff
{
	public class FrigidJavelin : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Javelin");
			Tooltip.SetDefault("Throw a powerful, fast traveling javelin");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 42;  
            item.magic = true;  
            item.width = 46;    
            item.height = 46;
			item.useAnimation = 44;
			item.useTime = 44;
			item.useStyle = 5;    
            item.knockBack = 5.25f;
            item.value = Item.sellPrice(0, 0, 80, 0);
            item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("FrigidJavelin"); 
            item.shootSpeed = 12f;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.crit = 16;
		}
		public override bool BeforeDrainMana(Player player)
		{
			return true;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 7;
		}
		public override float UseTimeMultiplier(Player player)
		{
			return 1f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "FrigidBar", 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
