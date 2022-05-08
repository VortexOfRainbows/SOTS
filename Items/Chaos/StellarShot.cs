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
            Item.damage = 53;   
            Item.magic = true;   
            Item.width = 102;    
            Item.height = 34;  
            Item.useTime = 6;  
            Item.useAnimation = 6;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true; 
            Item.knockBack = 3.5f;
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.UseSound = SoundID.Item91;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<StarLaser>(); 
            Item.shootSpeed = 9f;
			Item.mana = 3;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Chaos/StellarShotGlow").Value;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -32;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = 2;
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
