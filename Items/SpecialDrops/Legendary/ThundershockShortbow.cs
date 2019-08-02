using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops.Legendary
{
	public class ThundershockShortbow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thundershock Shortbow");
			Tooltip.SetDefault("Legendary drop\nLevels up as you progress");
		}
		public override void SetDefaults()
		{
			item.damage = 10;
			item.useTime = 30;
			item.useAnimation = 30;
			item.ranged = true;
			item.width = 40;
			item.height = 106;
			item.useStyle = 5;
			item.knockBack = 6;
            item.value = Item.sellPrice(1, 25, 0, 0);
			item.rare = 9;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("FriendlyLightning"); 
            item.shootSpeed = 4.5f;

		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(6, 0);
		}
		public override void UpdateInventory(Player player)
		{
			
			item.damage = 10;
			item.useTime = 30;
			item.useAnimation = 30;
            item.shootSpeed = 4.5f;
			if(SOTSWorld.legendLevel == 1)
			{
				item.damage = 15;
				item.useTime = 30;
				item.useAnimation = 30;
				item.shootSpeed = 4.75f;
			}
			if(SOTSWorld.legendLevel == 2)
			{
				item.damage = 18;
				item.useTime = 29;
				item.useAnimation = 29;
				item.shootSpeed = 5f;
			}
			if(SOTSWorld.legendLevel == 3)
			{
				item.damage = 20;
				item.useTime = 27;
				item.useAnimation = 27;
				item.shootSpeed = 6.25f;
			}
			if(SOTSWorld.legendLevel == 4)
			{
				item.damage = 20;
				item.useTime = 25;
				item.useAnimation = 25;
				item.shootSpeed = 7.75f;
			}
			if(SOTSWorld.legendLevel == 5)
			{
				item.damage = 22;
				item.useTime = 25;
				item.useAnimation = 25;
				item.shootSpeed = 8f;
			}
			if(SOTSWorld.legendLevel == 6)
			{
				item.damage = 24;
				item.useTime = 25;
				item.useAnimation = 25;
				item.shootSpeed = 8.5f;
			}
			if(SOTSWorld.legendLevel == 7)
			{
				item.damage = 24;
				item.useTime = 24;
				item.useAnimation = 24;
				item.shootSpeed = 9f;
			}
			if(SOTSWorld.legendLevel == 8)
			{
				item.damage = 25;
				item.useTime = 24;
				item.useAnimation = 24;
				item.shootSpeed = 9.25f;
			}
			if(SOTSWorld.legendLevel == 9)
			{
				item.damage = 28;
				item.useTime = 22;
				item.useAnimation = 22;
				item.shootSpeed = 9.5f;
			}
			if(SOTSWorld.legendLevel == 10)
			{
				item.damage = 30;
				item.useTime = 22;
				item.useAnimation = 22;
				item.shootSpeed = 9.75f;
			}
			if(SOTSWorld.legendLevel == 11)
			{
				item.damage = 32;
				item.useTime = 22;
				item.useAnimation = 22;
				item.shootSpeed = 10f;
			}
			if(SOTSWorld.legendLevel == 12)
			{
				item.damage = 33;
				item.useTime = 21;
				item.useAnimation = 21;
				item.shootSpeed = 10f;
			}
			if(SOTSWorld.legendLevel == 13)
			{
				item.damage = 35;
				item.useTime = 21;
				item.useAnimation = 21;
				item.shootSpeed = 10f;
			}
			if(SOTSWorld.legendLevel == 14)
			{
				item.damage = 37;
				item.useTime = 20;
				item.useAnimation = 20;
				item.shootSpeed = 10.2f;
			}
			if(SOTSWorld.legendLevel == 14)
			{
				item.damage = 40;
				item.useTime = 20;
				item.useAnimation = 20;
				item.shootSpeed = 10.5f;
			}
			if(SOTSWorld.legendLevel == 15)
			{
				item.damage = 45;
				item.useTime = 20;
				item.useAnimation = 20;
				item.shootSpeed = 10.75f;
			}
			if(SOTSWorld.legendLevel == 16)
			{
				item.damage = 45;
				item.useTime = 19;
				item.useAnimation = 19;
				item.shootSpeed = 11f;
			}
			if(SOTSWorld.legendLevel == 17)
			{
				item.damage = 50;
				item.useTime = 17;
				item.useAnimation = 17;
				item.shootSpeed = 11.1f;
			}
			if(SOTSWorld.legendLevel == 18)
			{
				item.damage = 52;
				item.useTime = 17;
				item.useAnimation = 17;
				item.shootSpeed = 11.2f;
			}
			if(SOTSWorld.legendLevel == 19)
			{
				item.damage = 55;
				item.useTime = 15;
				item.useAnimation = 15;
				item.shootSpeed = 11.3f;
			}
			if(SOTSWorld.legendLevel == 20)
			{
				item.damage = 57;
				item.useTime = 15;
				item.useAnimation = 15;
				item.shootSpeed = 11.4f;
			}
			if(SOTSWorld.legendLevel == 21)
			{
				item.damage = 60;
				item.useTime = 12;
				item.useAnimation = 12;
				item.shootSpeed = 11.5f;
			}
			if(SOTSWorld.legendLevel == 22)
			{
				item.damage = 61;
				item.useTime = 10;
				item.useAnimation = 10;
				item.shootSpeed = 11.7f;
			}
			if(SOTSWorld.legendLevel == 23)
			{
				item.damage = 62;
				item.useTime = 9;
				item.useAnimation = 9;
				item.shootSpeed = 11.8f;
			}
			if(SOTSWorld.legendLevel == 24)
			{
				item.damage = 63;
				item.useTime = 7;
				item.useAnimation = 7;
				item.shootSpeed = 11.8f;
			}
			if(SOTSWorld.legendLevel == 25)
			{
				item.damage = 65;
				item.useTime = 5;
				item.useAnimation = 5;
				item.shootSpeed = 12f;
			}
			
		}
	}
}