using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;
using System.Collections.Generic;
using SOTS.Buffs;
using SOTS.Projectiles.Permafrost;
using Terraria.Localization;

namespace SOTS.Items.Void
{
	public abstract class VoidConsumable : ModItem
	{
        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(Mod, "VoidConsumable", Language.GetTextValue("Mods.SOTS.Common.ACZV"));
			tooltips.Add(line);
			base.ModifyTooltips(tooltips);
		}
		public sealed override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 0, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 9999;
			Item.useStyle = 2;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults()
        {

        }
        public sealed override bool CanUseItem(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			return !voidPlayer.frozenVoid && !player.HasBuff(ModContent.BuffType<Satiated>());
        }
        public sealed override bool? UseItem(Player player)
		{
			return true;
		}
		public void RefillEffect(Player player, int amt)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidMeter += (amt * voidPlayer.VoidFoodGainMultiplier);
			VoidPlayer.VoidEffect(player, (int)(amt * voidPlayer.VoidFoodGainMultiplier));
		}
		public sealed override bool ConsumeItem(Player player)
		{
			return true;
		}
		public sealed override void OnConsumeItem(Player player)
		{
			Item.stack++;
			Activate(player);
		}
		public void Activate(Player player)
		{
			OnActivation(player);
			if (ConsumeStack())
			{
				Item.stack--;
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
		public virtual int GetSatiateDuration()
		{
			return 5;
		}
		public virtual void OnActivation(Player player)
		{
			RefillEffect(player, GetVoidAmt());
		}
		public void SealedUpdateInventory(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			int consumeAt = 0;
			int currentMax = voidPlayer.voidMeterMax2 - voidPlayer.lootingSouls - voidPlayer.VoidMinionConsumption;
			int leniency = currentMax - GetVoidAmt();
			while (voidPlayer.voidMeter <= consumeAt && leniency >= 0 && Item.stack > 0 && CanUseItem(player))
			{
				Activate(player);
			}
		}
	}
	public class AlmondMilk : VoidConsumable
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
		}
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item3;
		}
        public override int GetSatiateDuration()
        {
            return 7;
        }
        public override int GetVoidAmt()
        {
            return 20;
        }
    }
	public class CoconutMilk : VoidConsumable
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
		}
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = 2;
			Item.UseSound = SoundID.Item3;
		}
        public override int GetVoidAmt()
        {
            return 30;
		}
		public override int GetSatiateDuration()
		{
			return 10;
		}
	}
	public class CookedMushroom : VoidConsumable
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
		}
		public override void SafeSetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 0, 12, 50);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item2;
		}
        public override int GetVoidAmt()
        {
            return 13;
		}
		public override int GetSatiateDuration()
		{
			return 4;
		}
		public override void OnActivation(Player player)
		{
			RefillEffect(player, GetVoidAmt());
			player.AddBuff(BuffID.Poisoned, 120, true);
        }
	}
	public class DigitalCornSyrup : VoidConsumable
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
		}
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 40;
			Item.value = Item.sellPrice(0, 0, 5, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item3;
		}
        public override int GetVoidAmt()
        {
			return 15;
		}
		public override int GetSatiateDuration()
		{
			return 5;
		}
	}
	public class Chocolate : VoidConsumable
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
		}
		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item2;
		}
		public override int GetVoidAmt()
		{
			return 15;
		}
		public override int GetSatiateDuration()
		{
			return 4;
		}
	}
	public class CursedCaviar : VoidConsumable
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
		}
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 12, 50);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item2;
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
		public override int GetSatiateDuration()
		{
			return 3;
		}
		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient(null, "Curgeon", 1).AddTile(TileID.CookingPots).Register();
		}
	}
	public class FoulConcoction : VoidConsumable
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
		}
		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 0, 50);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item2;
		}
		public override int GetVoidAmt()
		{
			return 4;
		}
		public override int GetSatiateDuration()
		{
			return 3;
		}
	}
	public class StrawberryIcecream : VoidConsumable
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
		}
		public override void SafeSetDefaults()
		{
			Item.width = 18;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 7, 50);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item2;
		}
        public override int GetVoidAmt()
        {
            return 10;
		}
		public override int GetSatiateDuration()
		{
			return 3;
		}
		public override void OnActivation(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int rand = Main.rand.Next(2);
			if (rand == 0)
			{
				RefillEffect(player, GetVoidAmt());
				Vector2 circularSpeed = new Vector2(0, -12);
				int calc = 10 + modPlayer.bonusShardDamage;
				if (calc <= 0) calc = 1;
				Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center.X, player.Center.Y, circularSpeed.X, circularSpeed.Y, ModContent.ProjectileType<ShatterShard>(), calc, 3f, player.whoAmI);

			}
			else
			{
				RefillEffect(player, 6);
				for (int i = 0; i < 2; i++)
				{
					Vector2 circularSpeed = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(i * 180));
					int calc = 10 + modPlayer.bonusShardDamage;
					if (calc <= 0) calc = 1;
					Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center.X, player.Center.Y, circularSpeed.X, circularSpeed.Y, ModContent.ProjectileType<ShatterShard>(), calc, 3f, player.whoAmI);
				}
			}
		}
	}
	public class AvocadoSoup : VoidConsumable
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
		}
		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 0, 25, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item3;
		}
		public override void OnActivation(Player player)
		{
			RefillEffect(player, 24);
			player.AddBuff(BuffID.Regeneration, 3600, true);
		}
		public override int GetSatiateDuration()
		{
			return 5;
		}
	}
}