using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;
using System;



namespace SOTS.Items.Blood
{
	public class SoulTelegrapher : ModItem
	{	int timer = 0;
		int amount = 0;
		int fatigueTimer = 0;
		int fatigueAmount = 0;
		bool fatigue = true;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Telegrapher");
			Tooltip.SetDefault("Displays information about the amount of soul you have and the stats it affects");

		}
		public override void SetDefaults()
		{

			item.width = 34;
			item.height = 28;
			item.useTime = 48;
			item.useAnimation = 48;
			item.useStyle = 3;
			item.value = 250000;
			item.rare = 9;
			item.UseSound = SoundID.Item1;

		}
		public override void UpdateInventory(Player player)
		{
			
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) 
		{
			
		}
		public override bool UseItem(Player player)
		{
		SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			//soul stats
			int bonusLife = modPlayer.soulAmount;
			float bonusDamage = (modPlayer.soulAmount) * .005f;
			int debuff = 0;
			float endurance = -modPlayer.soulAmount * .005f;
			int defense = (int)(-modPlayer.soulAmount * 0.1f);
			
			//negative soul effects
				if(modPlayer.soulAmount <= -5)
				{
					//player.AddBuff(BuffID.Darkness, 300,false);
					Main.NewText("Darkness Enabled", 200, 200, 200);
					debuff--;
				}
				if(modPlayer.soulAmount <= -15)
				{
					//player.AddBuff(BuffID.WaterCandle, 300,false);
					debuff--;
					Main.NewText("Water Candle Enabled", 200, 200, 200);
				}
				if(modPlayer.soulAmount <= -35)
				{
					//player.AddBuff(BuffID.Blackout, 300,false);
					debuff--;
					Main.NewText("Blackout Enabled", 200, 200, 200);
				}
				if(modPlayer.soulAmount <= -50)
				{
					//player.AddBuff(BuffID.Silenced, 300,false);
					debuff--;
					Main.NewText("Silence Enabled", 200, 200, 200);
				}
				if(modPlayer.soulAmount <= -80)
				{
					bonusDamage -= .4f;
				}
				if(modPlayer.soulAmount <= -100)
				{
					debuff--;	
					bonusDamage -= 1;
					Main.NewText("Shadowdodge Enabled", 200, 200, 200);
				}
				
				
				//Positive Soul effects
				if(modPlayer.soulAmount >= 25)
				{
					player.AddBuff(BuffID.ChaosState, 300,false);
					Main.NewText("Chaos State Enabled", 200, 200, 200);
				}
				if(modPlayer.soulAmount >= 50)
				{
					player.AddBuff(148, 300,false);
					Main.NewText("Feral Bite Enabled", 200, 200, 200);
				}
				if(modPlayer.soulAmount >= 150)
				{
					player.AddBuff(BuffID.Confused, 300,false);
					Main.NewText("Confusion Enabled", 200, 200, 200);
				}
				if(modPlayer.soulAmount >= 400)
				{
					player.AddBuff(BuffID.Obstructed, 300,false);
					Main.NewText("Obstruction Enabled", 200, 200, 200);
				}
				
				endurance = Math.Abs(endurance);
				bonusDamage = Math.Abs(bonusDamage);
				string soulAmountText = ((int)(bonusLife + 100)).ToString();
				Main.NewText("Soul amount: " + soulAmountText + "%", 200, 200, 200);
				
				if(bonusDamage < 0)
				{
				string damageText = ((int)(bonusDamage * 100)).ToString();
				Main.NewText("Damage modifier: -" + damageText + "%", 200, 200, 200);
				}
				if(bonusDamage > 0)
				{
				string damageText = ((int)(bonusDamage * 100)).ToString();
				Main.NewText("Damage modifier: " + damageText + "%", 200, 200, 200);
				}
				
				if(endurance < 0)
				{
				string enduranceText = ((int)(endurance * 100)).ToString();
				Main.NewText("Endurance modifier: -" + modPlayer.soulAmount + "%", 200, 200, 200);
				}
				
				if(endurance > 0)
				{
				string enduranceText = ((int)(endurance * 100)).ToString();
				Main.NewText("Endurance modifier: " + modPlayer.soulAmount + "%", 200, 200, 200);
				}
				
				string defenseText = defense.ToString();
				Main.NewText("Defense modifier: " + defenseText, 200, 200, 200);
			
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SoulFragment", 5);
			recipe.AddIngredient(null, "BloodEssence", 5);
			recipe.AddIngredient(null, "BluePowerChamber", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}