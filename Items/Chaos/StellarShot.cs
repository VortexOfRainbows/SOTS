using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Items.ChestItems;
using SOTS.Projectiles.Chaos;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Chaos
{
	public class StellarShot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Shot");
			Tooltip.SetDefault("Rapidly fires piercing lasers");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 53;   
            Item.DamageType = DamageClass.Magic;   
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
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 speed = velocity;
			position += speed.SafeNormalize(Vector2.Zero) * 48;
			speed = speed.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-5, 5)));
			velocity = speed;
		}
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PhaseBar>(), 24).AddIngredient(ModContent.ItemType<PerfectStar>(), 1).AddIngredient(ItemID.LaserMachinegun, 1).AddIngredient(ItemID.Razorpine, 1).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}
