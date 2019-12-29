using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;
using System;

namespace SOTS.Items.Celestial
{
	public class Apocalypse : VoidItem
	{	float coolDown = 25f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Apocalypse");
			Tooltip.SetDefault("'Power straight from the underworld'\nFirerate increases after each use\nProvides a small light source\nWill not hurt players");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 66;
			item.magic = true;
			item.width = 30;
			item.height = 26;
            item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = 8;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = 5;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("GreenLightning"); 
			item.shootSpeed = 1;
			item.knockBack = 5;
			item.channel = true;
			item.UseSound = SoundID.Item92;
			item.noUseGraphic = true;
		}
		public override void UpdateInventory(Player player)
		{
			Lighting.AddLight(player.Center, 1.25f, 1.25f, 1.25f);
			coolDown += 0.01f;
			
			if(coolDown > 25f)
				coolDown = 25f;
			
			if(coolDown < 10f)
				coolDown = 10f;
			
			item.useTime = (int)coolDown;
			item.useAnimation = (int)coolDown;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SanguiteBar", 15);
			recipe.AddIngredient(null, "GreenJellyfishStaff", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Vector2 cursorArea = Main.MouseWorld;
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, cursorArea.X, cursorArea.Y);
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, cursorArea.X, cursorArea.Y);
			coolDown -= 1.95f;
			return false; 
		}
		public override void GetVoid(Player player)
		{
			voidMana = 5;
		}
	}
}