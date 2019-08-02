using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Secrets
{
	[AutoloadEquip(EquipType.Wings)]
	public class SaturnWings : ModItem
	{ int Probe = -1;
	int Probe2 = -1;
	int Probe3 = -1;
		public override void SetStaticDefaults()
		{	
		DisplayName.SetDefault("Saturn's Wings");
			Tooltip.SetDefault("Secret Item\nThrowing Lock");
		}

		public override void SetDefaults()
		{
			item.width = 78;
			item.height = 56;
			item.value = 0;
			item.rare = -12;
			item.accessory = true;
		}
		//these wings use the same values as the solar wings
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.thrownDamage += 0.25f;
			player.rangedDamage -= player.rangedDamage;
			player.meleeDamage -= player.meleeDamage;
			player.magicDamage -= player.magicDamage;
			player.minionDamage -= player.minionDamage;
			player.wingTimeMax += 20000;
		
		}
		

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.75f;
			ascentWhenRising = 0.75f;
			maxCanAscendMultiplier = 0.155f;
			maxAscentMultiplier = 7f;
			constantAscend = 0.155f;
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			speed = 20f;
			acceleration *= 2f;
		}
	}
}