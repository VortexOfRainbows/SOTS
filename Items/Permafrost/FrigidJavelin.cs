using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Permafrost
{
	public class FrigidJavelin : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Javelin");
			Tooltip.SetDefault("Throw a powerful, fast traveling javelin that ricochets of surfaces");
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 42;  
            Item.DamageType = DamageClass.Magic;  
            Item.width = 38;    
            Item.height = 38;
			Item.useAnimation = 44;
			Item.useTime = 44;
			Item.useStyle = ItemUseStyleID.Shoot;    
            Item.knockBack = 5.25f;
            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<Projectiles.Permafrost.FrigidJavelin>(); 
            Item.shootSpeed = 12f;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.crit = 16;
		}
		public override bool BeforeDrainMana(Player player)
		{
			return true;
		}
		public override int GetVoid(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (modPlayer.frigidJavelinNoCost)
			{
				return 0;
			}
			return 5;
		}
		public override float UseTimeMultiplier(Player player)
		{
			return 1f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<FrigidBar>(12).AddTile(TileID.Anvils).Register();
		}
	}
}
