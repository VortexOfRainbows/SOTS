using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;
using System;
using SOTS.Projectiles.Inferno;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Camera;
using System.Collections.Generic;
using Terraria.Localization;

namespace SOTS.Items.Secrets
{
	public class DreamLamp : VoidItem
	{
		public override void ModifyTooltip(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if (line.Mod == "Terraria" && line.Name == "Tooltip0")
				{
					string NewName = Language.GetTextValue("Mods.SOTS.ItemTooltip.DreamingLamp");
					if (IsItemForgotten)
						NewName = Language.GetTextValue("Mods.SOTS.ItemTooltip.FalseLamp");
					line.Text = NewName;
				}
			}
        }
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public void SetStats()
		{
			Item.rare = ItemRarityID.Gray;
			Item.DamageType = DamageClass.Magic;
			Item.damage = 24;
			Item.width = 42;
			Item.height = 50;
			Item.value = Item.sellPrice(gold: 5);
			Item.shoot = ModContent.ProjectileType<Projectiles.Camera.DreamLamp>();
			Item.shootSpeed = 5f;
			Item.knockBack = 5;
			Item.noMelee = true;
			Item.scale = 0.8f;
			Item.UseSound = null;
			Item.maxStack = 1;
			Item.autoReuse = false;
			Item.useTurn = false;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -2;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = -7;
			}
		}
		public void SetCommonStats()
		{
			Item.shoot = ModContent.ProjectileType<Projectiles.Camera.DreamLamp>();
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.createTile = -1;
			Item.consumable = false;
			Item.useTime = 48;
			Item.useAnimation = 48;
			Item.noUseGraphic = true;
			Item.channel = true;
		}
		public void SetDreamStats()
		{
			Item.rare = ItemRarityID.LightRed;
			SetCommonStats();
		}
		public void SetFalseStats()
		{
			SetCommonStats();
		}
		public void SetPlacementStats()
		{
			Item.shoot = 0;
			Item.rare = ItemRarityID.Gray;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.createTile = ModContent.TileType<ForgottenLampTile>();
			Item.consumable = true;
			Item.channel = false;
			Item.noUseGraphic = false;
		}
        public override float UseSpeedMultiplier(Player player)
        {
            return player.altFunctionUse == 2 ? 2 : 1;
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			ModifyStats(player);
			if (IsItemForgotten)
				damage /= 3;
		}
        public override bool BeforeUseItem(Player player)
        {
			ModifyStats(player);
            return base.BeforeUseItem(player);
        }
		public void ModifyStats(Player player)
		{
			if (IsItemForgotten)
			{
				if (player.altFunctionUse == 2)
				{
					SetFalseStats();
				}
				else
				{
					SetPlacementStats();
				}
			}
			else
			{
				SetDreamStats();
			}
		}
        public override void UpdateInventory(Player player)
		{
			SetOverridenName();
		}
        public override void PostUpdate()
        {
			SetOverridenName();
		}
		public void SetOverridenName()
		{
			ItemID.Sets.ItemNoGravity[Type] = !IsItemForgotten;
			Item.SetNameOverride(appropriateNameRightNow);
		}
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemNoGravity[Type] = false;
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
			this.SetResearchCost(1);
		}
		public static bool IsItemForgotten => !Main.dayTime;
        public static Texture2D texture => ModContent.Request<Texture2D>("SOTS/Items/Secrets/DreamingLamp").Value;
		public Texture2D inventoryBoxTexture => Terraria.GameContent.TextureAssets.InventoryBack.Value;
		public string appropriateNameRightNow => IsItemForgotten ? Language.GetTextValue("Mods.SOTS.ItemName.ForgottenLamp") : Language.GetTextValue("Mods.SOTS.ItemName.DreamingLamp");
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			scale *= Item.scale;
			if (IsItemForgotten)
            {
				return true;
            }
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 sinusoid = new Vector2(0, 6 * scale * (float)Math.Cos(1.7f * MathHelper.ToRadians(VoidPlayer.soulColorCounter))) + new Vector2(0, -3 * scale);
			rotation = 15 * (float)Math.Sin(1f * MathHelper.ToRadians(VoidPlayer.soulColorCounter));
			spriteBatch.Draw(texture, Item.position + origin + sinusoid - Main.screenPosition, null, lightColor, rotation * MathHelper.Pi / 180f, origin, scale, SpriteEffects.None, 0f);
			return false;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D realTexture = texture;
			origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 inventoryBoxTextureCenter = new Vector2(inventoryBoxTexture.Width / 2 - 5, inventoryBoxTexture.Height / 2 - 1) * scale; //this puts the center in the box's usual position
			Vector2 sinusoid = new Vector2(0, 3 * scale * (float)Math.Cos(0.85f * MathHelper.ToRadians(VoidPlayer.soulColorCounter))) + new Vector2(0, -scale);
			float rotation = 14 * (float)Math.Sin(0.5f * MathHelper.ToRadians(VoidPlayer.soulColorCounter));
			if (IsItemForgotten)
			{
				realTexture = Terraria.GameContent.TextureAssets.Item[Type].Value;
				scale *= 1.2f;
				sinusoid = Vector2.Zero;
				rotation = 0;
				position -= new Vector2(0, 6 * scale);
			}
			spriteBatch.Draw(realTexture, position + inventoryBoxTextureCenter + sinusoid, frame, drawColor, rotation * MathHelper.Pi / 180f, origin, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void SafeSetDefaults()
		{
			SetStats();
			SetFalseStats();
		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, -7);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				Vector2 normal = velocity.SafeNormalize(Vector2.Zero);
				Projectile.NewProjectile(source, position, normal * 12, type, damage, knockback, player.whoAmI, IsItemForgotten ? 2 : 1, (int)(Item.useTime / SOTSPlayer.ModPlayer(player).attackSpeedMod * 0.5f));
			}
			else
            {
				if(Item.createTile >= 0)
				{
					return false;
				}
				Vector2 normal = velocity.SafeNormalize(Vector2.Zero);
				Projectile.NewProjectile(source, position, normal * 12, type, damage, knockback, player.whoAmI);
				Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<DreamingFrame>(), damage, knockback, player.whoAmI);
            }
			return false;
		}
		public override int GetVoid(Player player)
		{
			if (Item.createTile >= 0)
				return 0;
			return 5;
		}
	}
}