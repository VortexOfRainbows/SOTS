using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using SOTS.Buffs.MinionBuffs;
using SOTS.Projectiles.Earth.Glowmoth;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Earth.Glowmoth
{
    public class NightIlluminator : ModItem
    {
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}
		public override void SetDefaults()
		{
			Item.damage = 8;
			Item.knockBack = 3f;
			Item.mana = 10;
			Item.width = 58;
			Item.height = 58;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.shootSpeed = 12;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = ModContent.BuffType<NightIlluminatorBuff>();
			Item.shoot = ModContent.ProjectileType<Projectiles.Earth.Glowmoth.NightIlluminator>();
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Earth/Glowmoth/NightIlluminatorGlow").Value;
			}
		}
        public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			if (player.altFunctionUse == 2)
				mult = 0;
			else
				mult = 1;
		}
        public override bool? UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.noUseGraphic = true;
			}
			else
			{
				Item.noUseGraphic = false;
			}
			return base.UseItem(player);
		}
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				return true;
			}
			player.AddBuff(Item.buffType, 2);
			for(int i = 0; i< 3; i++)
				player.SpawnMinionOnCursor(source, player.whoAmI, ModContent.ProjectileType<MothMinion>(), Item.damage, knockback);
			return false;
		}
	}
}