using Microsoft.Xna.Framework;
using SOTS.Items.Fragments;
using SOTS.Items.Otherworld.Furniture;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Otherworld.FromChests
{
	[AutoloadEquip(EquipType.Back)]
	public class BlinkPack : ModItem	
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("temp"); //this is needed for the tooltip to be modified later.
			this.SetResearchCost(1);
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			foreach (string key in SOTS.BlinkHotKey.GetAssignedKeys()) //gets the key configured to this hotkey
			{
				foreach (TooltipLine line in tooltips) //goes through each tooltip line
				{
					if (line.Mod == "Terraria" && line.Name == "Tooltip0") //checks the name of the tootip line
					{
						line.Text = Language.GetTextValue("Mods.SOTS.BlinkPackText", key);
						return;
					}
				}
			}
			foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if (line.Mod == "Terraria" && line.Name == "Tooltip0")
				{
					string Textkey = Language.GetTextValue("Mods.SOTS.Common.Unbound");
					line.Text = Language.GetTextValue("Mods.SOTS.BlinkPackText2", Textkey);
				}
			}
			base.ModifyTooltips(tooltips);
        }
        public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Melee;
			Item.damage = 45;
			Item.maxStack = 1;
            Item.width = 32;     
            Item.height = 32;
            Item.value = Item.sellPrice(0, 4, 20, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.BlinkType = 1;
			modPlayer.BlinkDamage += SOTSPlayer.ApplyDamageClassModWithGeneric(player, Item.DamageType, Item.damage);
			player.noFallDmg = true;
		}
        public override void EquipFrameEffects(Player player, EquipType type)
        {
			if(player.velocity.Length() > 1 && !player.mount.Active)
			{
				Vector2 loc = new Vector2(player.Center.X - 14 * player.direction, player.Center.Y + player.gfxOffY) - new Vector2(5);
				int num1 = Dust.NewDust(new Vector2(loc.X, loc.Y), 2, 2, 235);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				Main.dust[num1].shader = GameShaders.Armor.GetSecondaryShader(player.cBack, player);
			}
		}
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<FragmentOfChaos>(5).AddIngredient<OtherworldlyAlloy>(12).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}
}