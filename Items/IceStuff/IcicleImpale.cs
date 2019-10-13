using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;

namespace SOTS.Items.IceStuff
{
	public class IcicleImpale : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Icicle Impale");
			Tooltip.SetDefault("Launches large icicles\nRegenerates void upon hit");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 45;
            item.ranged = true;
            item.width = 62;
            item.height = 30;
            item.useTime = 14; 
            item.useAnimation = 14;
            item.useStyle = 5;    
            item.noMelee = true;
			item.knockBack = 1f;  
            item.value = Item.sellPrice(0, 8, 0, 0);
            item.rare = 7;
            item.UseSound = SoundID.Item61;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("HypericeRocket"); 
            item.shootSpeed = 8;
			item.useAmmo = ItemID.Snowball;
		}
		public override void GetVoid(Player player)
		{
				voidMana = 4;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(7, 0);
		}
		int shot = 0;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			shot++;
			if(shot % 4 == 0)
			{
				for(int i = 0; i < 2; i ++)
				{
				Vector2 angle = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(7.5f - (5 * i)));
				Projectile.NewProjectile(position.X, position.Y, angle.X, angle.Y, mod.ProjectileType("HypericeRocket"), damage, knockBack, player.whoAmI);
				}
			}
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("IceImpale"), damage, knockBack, player.whoAmI);
			return false; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "HypericeClusterCannon", 1);
			recipe.AddIngredient(null, "SanguiteBar", 12);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}
	}
}
