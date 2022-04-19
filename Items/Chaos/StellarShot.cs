using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Items.ChestItems;
using SOTS.Projectiles.Chaos;

namespace SOTS.Items.Chaos
{
	public class StellarShot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Shot");
			Tooltip.SetDefault("Rapidly fires piercing lasers");
		}
		public override void SetDefaults()
		{
            item.damage = 53;   
            item.magic = true;   
            item.width = 102;    
            item.height = 34;  
            item.useTime = 6;  
            item.useAnimation = 6;
            item.useStyle = ItemUseStyleID.HoldingOut;    
            item.noMelee = true; 
            item.knockBack = 3.5f;
            item.value = Item.sellPrice(0, 20, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item91;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<StarLaser>(); 
            item.shootSpeed = 9f;
			item.mana = 3;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Chaos/StellarShotGlow");
				item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -32;
				item.GetGlobalItem<ItemUseGlow>().glowOffsetY = 2;
			}
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-32, 2);
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			Vector2 speed = new Vector2(speedX, speedY);
			position += speed.SafeNormalize(Vector2.Zero) * 48;
			speed = speed.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-5, 5)));
			speedX = speed.X;
			speedY = speed.Y;
            return true; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PhaseBar>(), 24);
			recipe.AddIngredient(ModContent.ItemType<PerfectStar>(), 1);
			recipe.AddIngredient(ItemID.LaserMachinegun, 1);
			recipe.AddIngredient(ItemID.Razorpine, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
