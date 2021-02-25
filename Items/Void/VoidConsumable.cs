using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;
using System.Collections.Generic;

namespace SOTS.Items.Void
{
	public abstract class VoidConsumable : ModItem
	{
        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(mod, "VoidConsumable", "Automatically consumed when void is low");
			tooltips.Add(line);
			base.ModifyTooltips(tooltips);
        }
        public sealed override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.value = Item.sellPrice(0, 0, 0, 0);
			item.rare = 1;
			item.maxStack = 999;
			item.useStyle = 2;
			item.useTime = 15;
			item.useAnimation = 15;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults()
        {

        }
		public sealed override bool UseItem(Player player)
		{
			return true;
		}
		public void RefillEffect(Player player, int amt)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidMeter += amt;
			VoidPlayer.VoidEffect(player, amt);
		}
		public sealed override bool ConsumeItem(Player player)
		{
			return true;
		}
		public sealed override void OnConsumeItem(Player player)
		{
			OnActivation(player);
			if(!ConsumeStack())
            {
				item.stack++;
            }
		}
		public virtual bool ConsumeStack()
        {
			return true;
        }
		public virtual int GetVoidAmt()
        {
			return 20;
        }
		public virtual void OnActivation(Player player)
		{
			RefillEffect(player, GetVoidAmt());
		}
		public sealed override void UpdateInventory(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			while (voidPlayer.voidMeter < (voidPlayer.voidMeterMax2 - voidPlayer.lootingSouls) / 10 && voidPlayer.voidMeterMax2 - voidPlayer.lootingSouls > GetVoidAmt() && item.stack > 0)
			{
				OnActivation(player);
				if(ConsumeStack())
					item.stack--;
			}
		}
	}
	public class AlmondMilk : VoidConsumable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Almond Milk");
			Tooltip.SetDefault("Refills 20 void");
		}
		public override void SafeSetDefaults()
		{
			item.width = 22;
			item.height = 38;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = 1;
			item.UseSound = SoundID.Item3;
		}
	}
	public class CoconutMilk : VoidConsumable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coconut Milk");
			Tooltip.SetDefault("Refills 30 void");
		}
		public override void SafeSetDefaults()
		{
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = 2;
			item.UseSound = SoundID.Item3;
		}
        public override int GetVoidAmt()
        {
            return 30;
        }
    }
	public class CookedMushroom : VoidConsumable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cooked Mushroom");
			Tooltip.SetDefault("Refills 13 void\nCauses temporary poison");
		}
		public override void SafeSetDefaults()
		{
			item.width = 22;
			item.height = 22;
			item.value = Item.sellPrice(0, 0, 12, 50);
			item.rare = 1;
			item.UseSound = SoundID.Item2;
		}
        public override int GetVoidAmt()
        {
            return 13;
        }
        public override void OnActivation(Player player)
		{
			RefillEffect(player, GetVoidAmt());
			player.AddBuff(BuffID.Poisoned, 120, true);
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.VileMushroom, 2);
			recipe.AddTile(TileID.CookingPots);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ViciousMushroom, 2);
			recipe.AddTile(TileID.CookingPots);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class DigitalCornSyrup : VoidConsumable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Digital Corn Syrup");
			Tooltip.SetDefault("Refills 15 void\n'Yes, really'");
		}
		public override void SafeSetDefaults()
		{
			item.width = 28;
			item.height = 40;
			item.value = Item.sellPrice(0, 0, 5, 0);
			item.rare = 1;
			item.UseSound = SoundID.Item3;
		}
        public override int GetVoidAmt()
        {
			return 15;
        }
    }
	public class Chocolate : VoidConsumable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chocolate");
			Tooltip.SetDefault("Refills 15 void\n'The number one thing to bring on pirating adventures'");
		}
		public override void SafeSetDefaults()
		{
			item.width = 26;
			item.height = 32;
			item.value = Item.sellPrice(0, 0, 2, 0);
			item.rare = 1;
			item.UseSound = SoundID.Item2;
		}
		public override int GetVoidAmt()
		{
			return 15;
		}
	}
	public class CursedCaviar : VoidConsumable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Caviar");
			Tooltip.SetDefault("40% chance to refill 20 void and recieve Well Fed for 90 seconds\n35% chance to refill 15 void and recieve Mana Regeneration for 90 seconds\n25% chance to refill 10 void and recieve Battle for 90 seconds");
		}
		public override void SafeSetDefaults()
		{
			item.width = 28;
			item.height = 22;
			item.value = Item.sellPrice(0, 0, 12, 50);
			item.rare = 1;
			item.UseSound = SoundID.Item2;
		}
        public override void OnActivation(Player player)
        {
			int rand = Main.rand.Next(20);
			if (rand <= 7) //40%, 0, 1, 2, 3, 4, 5, 6, 7
			{
				RefillEffect(player, 20);
				player.AddBuff(BuffID.WellFed, 5400, true);
			}
			else if (rand <= 14) //35%, 8, 9, 10, 11, 12, 13, 14
			{
				RefillEffect(player, 15);
				player.AddBuff(BuffID.ManaRegeneration, 5400, true);
			}
			else //25%, 15, 16, 17, 18, 19
			{
				RefillEffect(player, 10);
				player.AddBuff(BuffID.Battle, 5400, true);
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Curgeon", 1);
			recipe.AddTile(TileID.CookingPots);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
	public class FoulConcoction : VoidConsumable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Foul Concoction");
			Tooltip.SetDefault("Refills 4 void");
		}
		public override void SafeSetDefaults()
		{
			item.width = 30;
			item.height = 18;
			item.value = Item.sellPrice(0, 0, 0, 50);
			item.rare = 1;
			item.UseSound = SoundID.Item2;
		}
		public override int GetVoidAmt()
		{
			return 4;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Peanut", 1);
			recipe.AddIngredient(ItemID.PinkGel, 1);
			recipe.AddIngredient(ItemID.Acorn, 1);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class StrawberryIcecream : VoidConsumable
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Strawberry Icecream");
			Tooltip.SetDefault("50% chance to refill 10 void and surround you with an ice shard\n50% chance to refill 6 void and surround you with 2 ice shards");
		}
		public override void SafeSetDefaults()
		{
			item.width = 22;
			item.height = 30;
			item.value = Item.sellPrice(0, 0, 7, 50);
			item.rare = 1;
			item.UseSound = SoundID.Item2;
		}
        public override int GetVoidAmt()
        {
            return 10;
        }
        public override void OnActivation(Player player)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			int rand = Main.rand.Next(2);
			if (rand == 0)
			{
				RefillEffect(player, GetVoidAmt());
				Vector2 circularSpeed = new Vector2(0, -12);
				int calc = 10 + modPlayer.bonusShardDamage;
				if (calc <= 0) calc = 1;
				Projectile.NewProjectile(player.Center.X, player.Center.Y, circularSpeed.X, circularSpeed.Y, mod.ProjectileType("ShatterShard"), calc, 3f, player.whoAmI);

			}
			else
			{
				RefillEffect(player, 6);
				for (int i = 0; i < 2; i++)
				{
					Vector2 circularSpeed = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(i * 180));
					int calc = 10 + modPlayer.bonusShardDamage;
					if (calc <= 0) calc = 1;
					Projectile.NewProjectile(player.Center.X, player.Center.Y, circularSpeed.X, circularSpeed.Y, mod.ProjectileType("ShatterShard"), calc, 3f, player.whoAmI);
				}
			}
		}
	}
}