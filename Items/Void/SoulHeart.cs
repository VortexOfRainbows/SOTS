using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Items.Fragments;
using Microsoft.Xna.Framework;
using SOTS.Items.Conduit;

namespace SOTS.Items.Void
{
	public class SoulHeart : ModItem
	{
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 32;
			Item.useAnimation = 12;
			Item.useTime = 12;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = 0;
			Item.rare = ItemRarityID.LightRed;
			Item.maxStack = 999;
			Item.autoReuse = false;
			Item.consumable = true;
			ItemID.Sets.ItemNoGravity[Item.type] = false;
		}
		public override bool? UseItem(Player player)
		{
			Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath39, player.Center);
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			if (voidPlayer.voidSoul < 1)
			{
				voidPlayer.voidMeterMax += 50;
				VoidPlayer.VoidEffect(player, 50);
				voidPlayer.voidSoul += 1;
				return true;
			}
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.LifeCrystal, 1).AddIngredient(ModContent.ItemType<SkipSoul>(), 50).AddTile(TileID.DemonAltar).Register();
		}
	}
}