using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Items.Permafrost;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tools
{
	public class FrigidPickaxe : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Pickaxe");
			Tooltip.SetDefault("Able to mine Hellstone");
		}
		public override void SetDefaults()
		{
            Item.damage = 11;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 34;   
            Item.height = 34;   
            Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
            Item.useTime = 15;
            Item.useAnimation = 21;
			Item.pick = 65;
			Item.knockBack = 3.25f;
			Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.tileBoost = 1;
			Item.autoReuse = true;
			Item.consumable = false;
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
			CreateRecipe(1).AddIngredient(ModContent.ItemType<FrigidBar>(), 12).AddTile(TileID.Anvils).Register();
		}
	}
}
