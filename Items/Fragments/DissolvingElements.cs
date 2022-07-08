using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using System.Collections.Generic;

namespace SOTS.Items.Fragments
{
	public abstract class DissolvingElement : ModItem
	{
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if (line.Mod == "Terraria" && line.Name == "Tooltip0")
				{
					if(!PolarizeElement)
						line.Text = NormalToolTip;
					else
						line.Text = PolarizeToolTip;
				}
			}
		}
		public virtual string NormalToolTip => "";
		public virtual string PolarizeToolTip => "";
		public int FrameCounter;
		public int Frame;
		public virtual int FrameX => Item.width;
		public virtual int FrameY => Item.height;
		public virtual int FrameSpeed => 5;
		public virtual int TotalFrames => 6;
		public virtual Color glowColor => new Color(100, 100, 100, 0);
		public sealed override void SetStaticDefaults()
		{
			SafeSetStaticDefaults();
			//Tooltip.SetDefault("WILL BE FILLED IN GAME");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(FrameSpeed, TotalFrames));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
			Tooltip.SetDefault("Temporary Tooltip");
			this.SetResearchCost(3);
		}
		public virtual void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Dissolving Nature");
		}
        public sealed override void SetDefaults()
		{
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 999;
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults()
		{

		}
        public sealed override void UpdateInventory(Player player)
		{
			UpdateElement(DissolvingElementsPlayer.ModPlayer(player));
			UpdateFrames();
		}
		public void UpdateFrames()
		{
			FrameCounter++;
			if (FrameCounter >= FrameSpeed)
			{
				FrameCounter = 0;
				Frame++;
			}
			if (Frame >= TotalFrames)
			{
				Frame = 0;
			}
		}
		public virtual void UpdateElement(DissolvingElementsPlayer DEP)
        {
			DEP.DissolvingNature += Item.stack;
		}
		public virtual bool PolarizeElement => DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeNature;
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (PolarizeElement)
			{
				spriteEffects = SpriteEffects.FlipVertically;
			}
			Texture2D texture = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
			Color color = glowColor;
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture, position + new Vector2(x, y), new Rectangle(0, FrameY * this.Frame, FrameX, FrameY), color * (1f - (Item.alpha / 255f)), 0f, origin, scale, spriteEffects, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			UpdateFrames();
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (PolarizeElement)
			{
				spriteEffects = SpriteEffects.FlipVertically;
			}
			Texture2D texture = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
			Color color = glowColor;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, FrameY * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition + new Vector2(x, y), new Rectangle(0, FrameY * this.Frame, FrameX, FrameY), color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, spriteEffects, 0f);
			}
			return false;
		}
	}
	public class DissolvingNature : DissolvingElement
	{
		public override void SafeSetStaticDefaults() => DisplayName.SetDefault("Dissolving Nature");
		public override string NormalToolTip => "Reduces damage dealt by 10% while in the inventory";
		public override string PolarizeToolTip => "Increases life regeneration by 1 while in the inventory, up to 4 total";
        public override int FrameSpeed => 5;
        public override int TotalFrames => 6;
		public override bool PolarizeElement => DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeNature;
		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 42;
		}
		public override void UpdateElement(DissolvingElementsPlayer DEP)
		{
			DEP.DissolvingNature += Item.stack;
		}
	}
	public class DissolvingEarth : DissolvingElement
	{
		public override void SafeSetStaticDefaults() => DisplayName.SetDefault("Dissolving Earth");
		public override string NormalToolTip => "Reduces endurance by 10% while in the inventory";
		public override string PolarizeToolTip => "Increases defense by 2 while in the inventory, up to 8 total";
		public override int FrameSpeed => 6;
		public override int TotalFrames => 8;
		public override bool PolarizeElement => DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeEarth;
		public override void SafeSetDefaults()
		{
			Item.width = 28;
			Item.height = 42;
		}
		public override void UpdateElement(DissolvingElementsPlayer DEP)
		{
			DEP.DissolvingEarth += Item.stack;
		}
	}
	public class DissolvingAurora : DissolvingElement
	{
		public override void SafeSetStaticDefaults() => DisplayName.SetDefault("Dissolving Aurora");
		public override string NormalToolTip => "Reduces movement speed by 20% while in the inventory";
		public override string PolarizeToolTip => "Increases movement speed by 5% while in the inventory, up to 20% total";
		public override int FrameSpeed => 8;
		public override int TotalFrames => 5;
		public override bool PolarizeElement => DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeAurora;
		public override void SafeSetDefaults()
		{
			Item.width = 34;
			Item.height = 38;
		}
		public override void UpdateElement(DissolvingElementsPlayer DEP)
		{
			DEP.DissolvingAurora += Item.stack;
		}
	}
	public class DissolvingDeluge : DissolvingElement
	{
		public override void SafeSetStaticDefaults() => DisplayName.SetDefault("Dissolving Deluge");
		public override string NormalToolTip => "Decreases max life and mana by 10 while in the inventory";
		public override string PolarizeToolTip => "Increases ranged damage by 3% while in the inventory, up to 12% total";
		public override int FrameSpeed => 6;
		public override int TotalFrames => 12;
		public override bool PolarizeElement => DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeDeluge;
		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 38;
			Item.rare = ItemRarityID.LightRed;
		}
		public override void UpdateElement(DissolvingElementsPlayer DEP)
		{
			DEP.DissolvingDeluge += Item.stack;
		}
	}
	public class DissolvingAether : DissolvingElement
	{
		public override void SafeSetStaticDefaults() => DisplayName.SetDefault("Dissolving Aether");
		public override string NormalToolTip => "Reduces gravity while in the inventory";
		public override string PolarizeToolTip => "Increases magic damage by 3% while in the inventory, up to 12% total";
		public override int FrameSpeed => 6;
        public override int TotalFrames => 8;
		public override bool PolarizeElement => DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeAether;
        public override Color glowColor => VoidPlayer.OtherworldColor;
        public override void SafeSetDefaults()
		{
			Item.width = 34;
			Item.height = 48;
		}
		public override void UpdateElement(DissolvingElementsPlayer DEP)
		{
			DEP.DissolvingAether += Item.stack;
		}
	}
	public class DissolvingUmbra : DissolvingElement
	{
		public override void SafeSetStaticDefaults() => DisplayName.SetDefault("Dissolving Umbra");
		public override string NormalToolTip => "Reduces max void by 20 while in the inventory";
		public override string PolarizeToolTip => "Increases void damage by 3% while in the inventory, up to 12% total";
		public override Color glowColor => VoidPlayer.EvilColor * 1.2f;
		public override int FrameSpeed => 5;
		public override int TotalFrames => 10;
		public override bool PolarizeElement => DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeUmbra;
		public override void SafeSetDefaults()
		{
			Item.width = 38;
			Item.height = 48;
			Item.rare = ItemRarityID.LightRed;
		}
		public override void UpdateElement(DissolvingElementsPlayer DEP)
		{
			DEP.DissolvingUmbra += Item.stack;
		}
	}
	public class DissolvingNether : DissolvingElement
	{
		public override void SafeSetStaticDefaults() => DisplayName.SetDefault("Dissolving Nether");
		public override string NormalToolTip => "Decreases life regeneration by 2 while in the inventory";
		public override string PolarizeToolTip => "Increases melee damage by 3% while in the inventory, up to 12% total";
		public override int FrameSpeed => 5;
		public override int TotalFrames => 8;
		public override bool PolarizeElement => DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeNether;
		public override void SafeSetDefaults()
		{
			Item.width = 42;
			Item.height = 40;
			Item.rare = ItemRarityID.LightRed;
		}
		public override void UpdateElement(DissolvingElementsPlayer DEP)
		{
			DEP.DissolvingNether += Item.stack;
		}
	}
	public class DissolvingBrilliance : DissolvingElement
	{
		public override void SafeSetStaticDefaults() => DisplayName.SetDefault("Dissolving Brilliance");
		public override string NormalToolTip => "Increases void drain by 0.5 while in the inventory";
		public override string PolarizeToolTip => "Increases summon damage by 3% while in the inventory, up to 12% total";
		public override int FrameSpeed => 5;
		public override int TotalFrames => 8;
		public override bool PolarizeElement => DissolvingElementsPlayer.ModPlayer(Main.LocalPlayer).PolarizeBrilliance;
		public override void SafeSetDefaults()
		{
			Item.width = 66;
			Item.height = 66;
			Item.rare = ItemRarityID.LightRed;
		}
		public override void UpdateElement(DissolvingElementsPlayer DEP)
		{
			DEP.DissolvingBrilliance += Item.stack;
		}
	}
	public class DissolvingElementsPlayer : ModPlayer
	{
		public bool PolarizeNature = false;
		public bool PolarizeEarth = false;
		public bool PolarizeAurora = false;
		public bool PolarizeAether = false;
		public bool PolarizeDeluge = false;
		public bool PolarizeUmbra = false;
		public bool PolarizeNether = false;
		public bool PolarizeBrilliance = false;
		public static DissolvingElementsPlayer ModPlayer(Player player)
		{
			return player.GetModPlayer<DissolvingElementsPlayer>();
		}
		public int DissolvingNature = 0;
		public int DissolvingEarth = 0;
		public int DissolvingAurora = 0;
		public int DissolvingAether = 0;
		public int DissolvingDeluge = 0;
		public int DissolvingUmbra = 0;
		public int DissolvingNether = 0;
		public int DissolvingBrilliance = 0;
        public override void UpdateBadLifeRegen()
        {
			NetherEffects();
			DissolvingNether = 0;
		}
        public override void ResetEffects()
		{
			if (DissolvingAether != 0)
				AetherEffects();
			NatureEffects();
			EarthEffects();
			AuroraEffects();
			if (DissolvingDeluge != 0)
				DelugeEffects();
			if (DissolvingUmbra != 0)
				UmbraEffects();
			BrillianceEffects();
			PolarizeNature = false;
			PolarizeEarth = false;
			PolarizeAurora = false;
			PolarizeAether = false;
			PolarizeDeluge = false;
			PolarizeUmbra = false;
			PolarizeNether = false;
			PolarizeBrilliance = false;
			DissolvingNature = 0;
			DissolvingEarth = 0;
			DissolvingAurora = 0;
			DissolvingDeluge = 0;
			DissolvingAether = 0;
			DissolvingBrilliance = 0;
			DissolvingUmbra = 0;
		}
		public void NatureEffects()
		{
			if(PolarizeNature)
            {
				if (DissolvingNature > 4)
					DissolvingNature = 4;
				Player.lifeRegen += DissolvingNature;
				return;
            }
			Player.GetDamage(DamageClass.Generic) -= 0.1f * MathHelper.Clamp(DissolvingNature, 0, 10);
		}
		public void EarthEffects()
		{
			if (PolarizeEarth)
			{
				if (DissolvingEarth > 4)
					DissolvingEarth = 4;
				Player.statDefense += DissolvingEarth * 2;
				return;
			}
			Player.endurance -= 0.1f * MathHelper.Clamp(DissolvingEarth, 0, 20);
		}
		public void AuroraEffects()
		{
			if (PolarizeAurora)
			{
				if (DissolvingAurora > 4)
					DissolvingAurora = 4;
				Player.moveSpeed += DissolvingAurora * 0.05f;
				return;
			}
			Player.moveSpeed -= 0.2f * MathHelper.Clamp(DissolvingAurora, 0, 5);
		}
		public void DelugeEffects()
		{
			if (PolarizeDeluge)
			{
				if (DissolvingDeluge > 4)
					DissolvingDeluge = 4;
				Player.GetDamage(DamageClass.Ranged) += DissolvingDeluge * 0.03f;
				return;
			}
			Player.statLifeMax2 = (int)MathHelper.Clamp(Player.statLifeMax2 - DissolvingDeluge * 10, 100, Player.statLifeMax2);
			Player.statManaMax2 = (int)MathHelper.Clamp(Player.statManaMax2 - DissolvingDeluge * 10, 20, Player.statManaMax2);
		}
        public void AetherEffects()
		{
			if (PolarizeAether)
			{
				if (DissolvingAether > 4)
					DissolvingAether = 4;
				Player.GetDamage(DamageClass.Magic) += DissolvingAether * 0.03f;
				return;
			}
			float projectedGravity = Player.gravity;
			float projectedFallSpeed = Player.maxFallSpeed;
			float projectedJumpSpeedBoost = Player.jumpSpeedBoost;
			float mult = 1f - 1f / (0.3f * DissolvingAether + 1); //around 0.3f at 1
			float mult2 = 1f - 1f / (0.3f * DissolvingAether + 1); //around 0.3f at 1
			projectedGravity -= 1f * mult;
			projectedFallSpeed -= 10f * mult2;
			projectedJumpSpeedBoost += 5f * mult;
			if (projectedJumpSpeedBoost > 5)
			{
				projectedJumpSpeedBoost = 5;
			}
			if (projectedGravity < 0.125f)
			{
				projectedGravity = 0.125f;
			}
			if (projectedFallSpeed < 1.75f)
			{
				projectedFallSpeed = 1.75f;
			}
			if (Player.gravity > projectedGravity)
				Player.gravity = projectedGravity;

			if (Player.maxFallSpeed > projectedFallSpeed)
				Player.maxFallSpeed = projectedFallSpeed;

			if (Player.jumpSpeedBoost < projectedJumpSpeedBoost)
				Player.jumpSpeedBoost = projectedJumpSpeedBoost;
			if (DissolvingAether >= 4)
			{
				Player.noFallDmg = true;
			}
		}
		public void UmbraEffects()
		{
			if (PolarizeUmbra)
			{
				if (DissolvingUmbra > 4)
					DissolvingUmbra = 4;
				Player.GetDamage(ModContent.GetInstance<VoidGeneric>()) += DissolvingUmbra * 0.03f;
				return;
			}
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(Player);
			vPlayer.voidMeterMax2 -= 20 * DissolvingUmbra;
			if (vPlayer.voidMeterMax2 < 20)
			{
				vPlayer.voidMeterMax2 = 20;
			}
		}
		public void NetherEffects()
		{
			if (PolarizeNether)
			{
				if (DissolvingNether > 4)
					DissolvingNether = 4;
				Player.GetDamage(DamageClass.Melee) += DissolvingNether * 0.03f;
				return;
			}
			if (DissolvingNether > 10)
				DissolvingNether = 10;
			Player.lifeRegen -= DissolvingNether * 2;
		}
		public void BrillianceEffects()
		{
			if (PolarizeBrilliance)
			{
				if (DissolvingBrilliance > 4)
					DissolvingBrilliance = 4;
				Player.GetDamage(DamageClass.Summon) += DissolvingBrilliance * 0.03f;
				return;
			}
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(Player);
			vPlayer.flatVoidRegen -= 0.5f * DissolvingBrilliance;
		}
	}
}