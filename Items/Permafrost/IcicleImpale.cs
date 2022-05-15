using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Void;


namespace SOTS.Items.Permafrost
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
            Item.damage = 75;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 78;
            Item.height = 30;
            Item.useTime = 17; 
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
			Item.knockBack = 1f;  
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.shoot = Mod.Find<ModProjectile>("HypericeRocket").Type; 
            Item.shootSpeed = 19f;
		}
		public override int GetVoid(Player player)
		{
			return  7;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, -1);
		}
		int shot = 0;
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			shot++;
			if(shot % 3 == 0)
			{
				for(int i = 0; i < 2; i ++)
				{
				Vector2 angle = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(2.5f - (5 * i)));
				Projectile.NewProjectile(position.X, position.Y, angle.X, angle.Y, Mod.Find<ModProjectile>("HypericeRocket").Type, damage, knockBack, player.whoAmI);
				}
			}
			Projectile.NewProjectile(position.X, position.Y, speedX * 1.6f, speedY * 1.6f, Mod.Find<ModProjectile>("IceImpale").Type, damage, knockBack, player.whoAmI);
			return false; 
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "HypericeClusterCannon", 1).AddIngredient(ModContent.ItemType<HelicopterParts>(), 1).AddIngredient(null, "DissolvingAurora", 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
