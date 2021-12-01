using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Items.Permafrost;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tools
{
	public class FrigidHamaxe : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Hamaxe");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.damage = 18;
            item.melee = true;  
            item.width = 34;   
            item.height = 34;   
            item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
            item.useTime = 21;
            item.useAnimation = 21;
			item.hammer = 50;
			item.axe = 14;
			item.knockBack = 4f;
			item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.tileBoost = 1;
			item.autoReuse = true;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(6))
			{
				int num1 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<ModIceDust>(), player.direction * 2, 0f, 0, default(Color), 1.3f);
				Main.dust[num1].velocity *= 0.2f;
				Main.dust[num1].noGravity = true;
			}
			base.MeleeEffects(player, hitbox);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<FrigidBar>(), 16);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
