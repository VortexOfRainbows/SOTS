using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	[AutoloadEquip(EquipType.Back)]
	public class BlinkPack : ModItem	
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blink Pack");
			Tooltip.SetDefault("temp");
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			foreach (string key in SOTS.BlinkHotKey.GetAssignedKeys()) //gets the key configured to this hotkey
			{
				foreach (TooltipLine line in tooltips) //goes through each tooltip line
				{
					if (line.Mod == "Terraria" && line.Name == "Tooltip0") //checks the name of the tootip line
					{
						line.Text = "Press the " + "'" + key + "' key to blink towards your cursor\nBlinking through enemies will deal damage and trigger a lower cooldown, but can only be done up to 3 times in quick succession\nProvides some immunity after dashing\nNegates fall damage";
						return;
					}
				}
			}
			foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if (line.Mod == "Terraria" && line.Name == "Tooltip0")
				{
					string key = "Unbound";
					line.Text = "Press the " + "'" + key + "' key to blink towards your cursor\nBlinking through enemies will deal damage and trigger a lower cooldown, but can only be done up to 3 times in quick succession\nProvides some immunity after dashing\nNegates fall damage";
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
			modPlayer.BlinkDamage += (int)(Item.damage * (1f + (player.GetDamage(DamageClass.Melee) - 1f) + (player.allDamage - 1f)));
			player.noFallDmg = true;
		}
        public override void UpdateVanity(Player player, EquipType type)
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
			CreateRecipe(1).AddIngredient(null, "FragmentOfChaos", 5).AddIngredient(null, "OtherworldlyAlloy", 12).AddTile(Mod.Find<ModTile>("HardlightFabricatorTile").Type).Register();
		}
	}
}