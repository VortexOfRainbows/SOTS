using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using ReLogic.Content;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Items.Tide;
using SOTS.Projectiles.Base;
using SOTS.Projectiles.Celestial;
using SOTS.Void;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Channels;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.Player;

namespace SOTS.FakePlayer
{
    public class FakePlayer
    {
        public static bool SupressNetMessage13and41 = false;
        public bool ShouldUseWingsArmPosition = false;
        public int WingFrame = 0;
        public int TrailingType = 0;
        public const int Width = 20;
        public const int Height = 42;
        public int weaponDrawOrder = 0;
        public Vector2 Position = Vector2.Zero;
        public Vector2 Velocity = Vector2.Zero;
        public Vector2 SecondPosition = Vector2.Zero;
        public Vector2 OldPosition = Vector2.Zero;
        public Rectangle bodyFrame = Rectangle.Empty;
        public static bool IsValidUseStyle(Item item)
        {
            return item.useStyle == ItemUseStyleID.Swing ||
                item.useStyle == ItemUseStyleID.Shoot || 
                item.useStyle == ItemUseStyleID.MowTheLawn || 
                item.useStyle == ItemUseStyleID.RaiseLamp || 
                item.useStyle == ItemUseStyleID.HoldUp || 
                item.useStyle == ItemUseStyleID.Guitar || 
                item.useStyle == ItemUseStyleID.HiddenAnimation ||
                item.useStyle == ItemUseStyleID.Rapier ||
                item.useStyle == ItemUseStyleID.Thrust;
        }
        public static bool IsPlaceable(Item item)
        {
            return item.createTile != -1;
        }
        public bool CheckItemValidityFull(Player player, Item item)
        {
            SubspacePlayer subspacePlayer = SubspacePlayer.ModPlayer(player);
            bool canUseItem = true;
            #region check if item is useable
            if (!FakePlayerHelper.FakePlayerItemWhitelist.Contains(item.type) || lastUsedItem == null)
            {
                Projectile proj = new Projectile();
                proj.SetDefaults(item.shoot);
                if (FakePlayerHelper.FakePlayerItemBlacklist.Contains(item.type))
                {
                    canUseItem = false;
                }
                else if (item.ammo > 0 || item.fishingPole > 0 || (item.CountsAsClass(DamageClass.Summon) && !item.CountsAsClass(DamageClass.SummonMeleeSpeed)) || lastUsedItem == null || item.mountType != -1)
                {
                    canUseItem = false;
                }
                else if(item.consumable && item.damage <= 0 && !IsPlaceable(item))
                {
                    canUseItem = false;
                }
                proj.active = false;
                proj.Kill();
            }
            bool validItem = canUseItem && lastUsedItem.type == item.type && item.active && !item.IsAir && IsValidUseStyle(item) && !subspacePlayer.servantIsVanity;
            return validItem;
            #endregion
        }
        public void ItemCheckHack(Player player)
        {
            SupressNetMessage13and41 = true;
            SubspacePlayer subspacePlayer = SubspacePlayer.ModPlayer(player);
            Item item = player.inventory[UseItemSlot];
            if (Main.netMode != NetmodeID.Server)
            {
                if (!TextureAssets.Item[item.type].IsLoaded)
                    Main.instance.LoadItem(item.type);
            }
            heldItem = item;
            TrailingType = 0;
            bool saveCursorIconEnabled = player.cursorItemIconEnabled;
            int saveCursorIconID = player.cursorItemIconID;
            bool saveAutoSwing = item.autoReuse;
            bool saveUseTurn = item.useTurn;
            item.autoReuse = true;
            item.useTurn = true;
            bool valid = CheckItemValidityFull(player, item);
            if (item.IsAir)
            {
                RunItemCheck(player, true);
            }
            else if (valid)
            {
                subspacePlayer.foundItem = true;
                RunItemCheck(player, true);
                //Main.NewText(player.ItemUsesThisAnimation);
            }
            else
            {
                if (lastUsedItem != null && lastUsedItem.type != item.type && !subspacePlayer.servantIsVanity)
                {
                    lastUsedItem = heldItem.Clone();
                    RunItemCheck(player, true);
                }
                else
                    RunItemCheck(player, false);
                ResetVariables();
            }
            if (itemAnimation > 0)
            {
                ShouldUseWingsArmPosition = false;
                if (item.CountsAsClass(DamageClass.Melee) || item.CountsAsClass(DamageClass.SummonMeleeSpeed) || item.type == ItemID.Toxikarp || item.type == ItemID.SpiritFlame || item.type == ItemID.LawnMower || item.type == ModContent.ItemType<LashesOfLightning>() || item.type == ModContent.ItemType<SharkPog>()
                    || item.consumable)
                {
                    if (item.noMelee && !IsPlaceable(item))
                        TrailingType = 4;
                    else
                        TrailingType = 3;
                }
                else if (item.CountsAsClass(DamageClass.Ranged))
                {
                    TrailingType = 2;
                }
                else if (item.CountsAsClass(DamageClass.Magic))
                {
                    TrailingType = 1;
                }
                else
                    TrailingType = 2;
            }
            else
            {
                if (itemAnimation == 0)
                    lastUsedItem = heldItem.Clone();
                if (item.holdStyle == 0 || !valid)
                    ShouldUseWingsArmPosition = Velocity.Length() > 2f;
                else
                    ShouldUseWingsArmPosition = false;
            }
            player.cursorItemIconEnabled = saveCursorIconEnabled;
            player.cursorItemIconID = saveCursorIconID;
            item.autoReuse = saveAutoSwing;
            item.useTurn = saveUseTurn;
            SupressNetMessage13and41 = false;
        }
        public SavedPlayerValues PlayerSavedProperties;
        public CompositeArmData compositeFrontArm;
        public CompositeArmData compositeBackArm;
        public float compositeFrontArmRotation;
        public float compositeBackArmRotation;
        public int projectileDrawPosition;
        public bool heldProjOverHand;
        public Vector2 bodyVect;
        public Rectangle compFrontArmFrame = Rectangle.Empty;
        public Rectangle compBackArmFrame = Rectangle.Empty;
        public Item heldItem;                        
        Item lastUsedItem;
        private int FakePlayerType = 0; //For now, FakePlayerType of 0 will mean SubspaceServant. Other FakePlayers may have other types in the future for organization.
        private int FakePlayerID = 0;
        public int UseItemSlot => 49; //it should always use the last slot. Maybe add a config to this later, or an individual slot.
        public bool compositeFrontArmEnabled = false;
        public bool compositeBackArmEnabled = false;

        public int AltFunctionUse;
        public int toolTime;
        public int itemAnimation;
        public int itemAnimationMax;
        public int itemTime;
        public int itemTimeMax;
        public float itemRotation;
        public Vector2 itemLocation = Vector2.Zero;

        public int itemWidth;
        public int itemHeight;
        public int direction;
        public int reuseDelay;
        public bool releaseUseItem;
        public bool justDroppedAnItem;

        public int AttackCD;
        public int ItemUsesThisAnimation;
        public int BoneGloveTimer;
        public bool Channel;
        public int HeldProj = -1;
        public void ResetVariables()
        {
            lastUsedItem = null;
            toolTime = 0;
            itemAnimation = 0;
            itemAnimationMax = 0;
            itemTime = 0;
            itemTimeMax = 0;
            itemRotation = 0;
            itemLocation = Vector2.Zero;
            itemWidth = 0;
            itemHeight = 0;
            //direction = 0;
            reuseDelay = 0;
            releaseUseItem = false;
            justDroppedAnItem = false;
            AttackCD = 0;
            ItemUsesThisAnimation = 0;
            BoneGloveTimer = 0;
        }
        public FakePlayer(int type = 0, int identity = -1)
        {
            FakePlayerType = type;
            PlayerSavedProperties = new SavedPlayerValues();
            FakePlayerID = identity;
        }
        public void Update(Player player)
        {
            if (player.boneGloveTimer > 0)
            {
                player.boneGloveTimer--;
            }
            player.heldProj = -1;
            HeldProj = -1;
        }
        public void RunItemCheck(Player player, bool canUseItem = false)
        {
            FakePlayerProjectile.OwnerOfThisUpdateCycle = FakePlayerID; //Temporarily assign the owner of the update cycle, which will make any projectile spawned during the update cycle a child of the fake player
            int whoAmI = player.whoAmI;
            SaveRealPlayerValues(player);
            CopyFakeToReal(player);
            Update(player);
            if (canUseItem || player.channel)
            {
                player.ItemCheck_ManageRightClickFeatures(); //Manages the right click functionality of the weapons
                if(!player.HeldItem.IsAir)
                    player.StartChanneling(player.HeldItem); //This is a double check in case channeling fails for certain modded items
                player.ItemCheck(); //Run the actual item use code
            }
            player.oldPosition = Position;
            UpdateMyProjectiles(player); //Projectile updates usually happen after player updates anyway, so this shouldm ake sense in the order of operations (after item check)
            SetupBodyFrame(player); //run code to get frame after
            CopyRealToFake(player);
            LoadRealPlayerValues(player);
            FakePlayerProjectile.OwnerOfThisUpdateCycle = -1;
        }
        public void SaveRealPlayerValues(Player player)
        {
            //Save Player original values
            PlayerSavedProperties.SavePosition = player.position;
            PlayerSavedProperties.SaveOldPos = player.oldPosition;
            PlayerSavedProperties.SaveVelocity = player.velocity;
            PlayerSavedProperties.SavedSelectedItem = player.selectedItem;
            PlayerSavedProperties.PlayerOwnedLastVisualizedSelectedItem = player.lastVisualizedSelectedItem.Clone();
            PlayerSavedProperties.saveChannel = player.channel;
            PlayerSavedProperties.saveFrozen = player.frozen;
            PlayerSavedProperties.saveWebbed = player.webbed;
            PlayerSavedProperties.saveStoned = player.stoned;
            PlayerSavedProperties.saveWet = player.wet;
            PlayerSavedProperties.saveMount = player.mount.Active;
            PlayerSavedProperties.saveAltFunctionUse = player.altFunctionUse;
            PlayerSavedProperties.savePulley = player.pulley;
            PlayerSavedProperties.savePettingAnimal = player.isPettingAnimal;
            PlayerSavedProperties.saveHeldProj = player.heldProj;
            PlayerSavedProperties.saveStealth = player.stealth;
            PlayerSavedProperties.saveGravDir = player.gravDir;
            PlayerSavedProperties.saveInvis = player.invis;
            PlayerSavedProperties.saveGFXOffY = player.gfxOffY;

            //Save Player original values that have corresponding fakeplayer values
            PlayerSavedProperties.savecompositeFrontArmEnabled = player.compositeFrontArm.enabled;
            PlayerSavedProperties.savecompositeBackArmEnabled = player.compositeBackArm.enabled;
            PlayerSavedProperties.saveToolTime = player.toolTime;
            PlayerSavedProperties.saveItemAnimation = player.itemAnimation;
            PlayerSavedProperties.saveItemAnimationMax = player.itemAnimationMax;
            PlayerSavedProperties.saveItemTime = player.itemTime;
            PlayerSavedProperties.saveItemTimeMax = player.itemTimeMax;
            PlayerSavedProperties.saveItemRotation = player.itemRotation;
            PlayerSavedProperties.saveItemLocation = player.itemLocation;
            PlayerSavedProperties.saveItemWidth = player.itemWidth;
            PlayerSavedProperties.saveItemHeight = player.itemHeight;
            PlayerSavedProperties.saveDirection = player.direction;
            PlayerSavedProperties.saveReuseDelay = player.reuseDelay;
            PlayerSavedProperties.saveReleaseUseItem = player.releaseUseItem;
            PlayerSavedProperties.saveJustDroppedAnItem = player.JustDroppedAnItem;
            PlayerSavedProperties.saveAttackCD = player.attackCD;
            PlayerSavedProperties.saveItemUsesThisAnimation = player.ItemUsesThisAnimation;
            PlayerSavedProperties.saveBoneGloveTimer = player.boneGloveTimer;
            PlayerSavedProperties.saveFrontArm = player.compositeFrontArm;
            PlayerSavedProperties.saveBackArm = player.compositeBackArm;
            PlayerSavedProperties.saveBodyFrame = player.bodyFrame;
        }
        public void CopyFakeToReal(Player player)
        {
            //Set default values (ones that aren't used/modified by FakePlayer)
            player.selectedItem = UseItemSlot;
            player.lastVisualizedSelectedItem = lastUsedItem;
            player.channel = Channel;
            player.frozen = player.stoned = player.webbed = player.wet = false;
            player.mount._active = false;
            player.pulley = false;
            player.isPettingAnimal = false;
            player.heldProj = HeldProj;
            player.stealth = 1f;
            player.gravDir = 1f;
            player.invis = false;
            player.bodyFrame = bodyFrame;
            player.gfxOffY = 0;

            //Set values that player uses to the FakePlayer's values
            player.position = Position;
            player.oldPosition = OldPosition;
            player.velocity = Velocity;
            player.compositeFrontArm.enabled = compositeFrontArmEnabled;
            player.compositeBackArm.enabled = compositeBackArmEnabled;
            player.toolTime = toolTime;
            player.itemAnimation = itemAnimation;
            player.itemAnimationMax = itemAnimationMax;
            player.itemTime = itemTime;
            player.itemTimeMax = itemTimeMax;
            player.itemWidth = itemWidth;
            player.itemHeight = itemHeight;
            player.direction = direction;
            player.reuseDelay = reuseDelay;
            player.releaseUseItem = releaseUseItem;
            player.JustDroppedAnItem = justDroppedAnItem;
            player.itemRotation = itemRotation;
            player.itemLocation = itemLocation;
            player.attackCD = AttackCD;
            player.altFunctionUse = AltFunctionUse;
            player.boneGloveTimer = BoneGloveTimer;
            player.compositeFrontArm = compositeFrontArm;
            player.compositeBackArm = compositeBackArm;
            SetPlayerItemUsesThisAnimationViaReflection(player, ItemUsesThisAnimation);
        }
        public void CopyRealToFake(Player player)
        {
            //Run using FakePlayer values, then set FakePlayer values to the newly updated ones
            HeldProj = player.heldProj;
            Channel = player.channel;
            Position = player.position;
            OldPosition = player.oldPosition;
            Velocity = player.velocity;
            compositeFrontArmEnabled = player.compositeFrontArm.enabled;
            compositeBackArmEnabled = player.compositeBackArm.enabled;
            toolTime = player.toolTime;
            itemAnimation = player.itemAnimation;
            itemAnimationMax = player.itemAnimationMax;
            itemTime = player.itemTime;
            itemTimeMax = player.itemTimeMax;
            itemWidth = player.itemWidth;
            itemHeight = player.itemHeight;
            itemRotation = player.itemRotation;
            itemLocation = player.itemLocation;
            direction = player.direction;
            reuseDelay = player.reuseDelay;
            releaseUseItem = player.releaseUseItem;
            justDroppedAnItem = player.JustDroppedAnItem;
            AttackCD = player.attackCD;
            AltFunctionUse = player.altFunctionUse;
            ItemUsesThisAnimation = player.ItemUsesThisAnimation;
            BoneGloveTimer = player.boneGloveTimer;
            compositeFrontArm = player.compositeFrontArm;
            compositeBackArm = player.compositeBackArm;
        }
        public void LoadRealPlayerValues(Player player)
        {
            //Reset player values back to normal
            player.position = PlayerSavedProperties.SavePosition;
            player.oldPosition = PlayerSavedProperties.SaveOldPos;
            player.velocity = PlayerSavedProperties.SaveVelocity;
            player.selectedItem = PlayerSavedProperties.SavedSelectedItem;
            player.lastVisualizedSelectedItem = PlayerSavedProperties.PlayerOwnedLastVisualizedSelectedItem;
            player.channel = PlayerSavedProperties.saveChannel;
            player.frozen = PlayerSavedProperties.saveFrozen;
            player.webbed = PlayerSavedProperties.saveWebbed;
            player.stoned = PlayerSavedProperties.saveStoned;
            player.wet = PlayerSavedProperties.saveWet;
            player.mount._active = PlayerSavedProperties.saveMount;
            player.altFunctionUse = PlayerSavedProperties.saveAltFunctionUse;
            player.pulley = PlayerSavedProperties.savePulley;
            player.isPettingAnimal = PlayerSavedProperties.savePettingAnimal;
            player.heldProj = PlayerSavedProperties.saveHeldProj;
            player.stealth = PlayerSavedProperties.saveStealth;
            player.gravDir = PlayerSavedProperties.saveGravDir;
            player.invis = PlayerSavedProperties.saveInvis;
            player.gfxOffY = PlayerSavedProperties.saveGFXOffY;

            //Reset other player values back to normal
            player.compositeFrontArm.enabled = PlayerSavedProperties.savecompositeFrontArmEnabled;
            player.compositeBackArm.enabled = PlayerSavedProperties.savecompositeBackArmEnabled;
            player.toolTime = PlayerSavedProperties.saveToolTime;
            player.itemAnimation = PlayerSavedProperties.saveItemAnimation;
            player.itemAnimationMax = PlayerSavedProperties.saveItemAnimationMax;
            player.itemTime = PlayerSavedProperties.saveItemTime;
            player.itemTimeMax = PlayerSavedProperties.saveItemTimeMax;
            player.itemRotation = PlayerSavedProperties.saveItemRotation;
            player.itemLocation = PlayerSavedProperties.saveItemLocation;
            player.itemWidth = PlayerSavedProperties.saveItemWidth;
            player.itemHeight = PlayerSavedProperties.saveItemHeight;
            player.direction = PlayerSavedProperties.saveDirection;
            player.reuseDelay = PlayerSavedProperties.saveReuseDelay;
            player.releaseUseItem = PlayerSavedProperties.saveReleaseUseItem;
            player.JustDroppedAnItem = PlayerSavedProperties.saveJustDroppedAnItem;
            player.attackCD = PlayerSavedProperties.saveAttackCD;
            player.boneGloveTimer = PlayerSavedProperties.saveBoneGloveTimer;
            player.compositeFrontArm = PlayerSavedProperties.saveFrontArm;
            player.compositeBackArm = PlayerSavedProperties.saveBackArm;
            player.bodyFrame = PlayerSavedProperties.saveBodyFrame;
            SetPlayerItemUsesThisAnimationViaReflection(player, PlayerSavedProperties.saveItemUsesThisAnimation);
        }
        public void SetPlayerItemUsesThisAnimationViaReflection(Player player, int setUses)
        {
            Type type = player.GetType();
            PropertyInfo prop = type.GetProperty("ItemUsesThisAnimation");
            prop.SetValue(player, setUses, null);
        }
        public SpriteEffects playerEffect;
        public SpriteEffects itemEffect;
        public void HijackItemDrawing(ref PlayerDrawSet drawInfo, bool green)
        {
            if(green)
            {
                ConvertItemTextureToGreen(drawInfo.heldItem);
                Draw27_HeldItem(ref drawInfo, new Vector2(1, 0));
                Draw27_HeldItem(ref drawInfo, new Vector2(-1, 0));
                Draw27_HeldItem(ref drawInfo, new Vector2(0, 1));
                Draw27_HeldItem(ref drawInfo, new Vector2(0, -1));
                ConvertItemTextureBackToNormal(drawInfo.heldItem);
            }
            else
            {
                Draw27_HeldItem(ref drawInfo, Vector2.Zero);
                Common.PlayerDrawing.UseItemGlowmask.DrawStatic(ref drawInfo);
            }
        }
        public void Draw27_HeldItem(ref PlayerDrawSet drawInfo, Vector2 offset)
        {
            Player player = drawInfo.drawPlayer;
            Vector2 savePlayerPos = player.itemLocation;
            Vector2 saveDrawinfoPos = drawInfo.ItemLocation;

            player.itemLocation += offset;
            drawInfo.ItemLocation += offset;

            FakeItem.overrideLightColor = true;
            FakeItem.runOnce = true;
            PlayerDrawLayers.DrawPlayer_27_HeldItem(ref drawInfo);
            FakeItem.overrideLightColor = false;

            player.itemLocation = savePlayerPos;    
            drawInfo.ItemLocation = saveDrawinfoPos;
        }
        public bool DrawFakePlayer(ref PlayerDrawSet drawInfo, Player player)
        {
            FakePlayerProjectile.OwnerOfThisDrawCycle = FakePlayerID;
            if (bodyFrame.IsEmpty)
                return false;
            SaveRealPlayerValues(player);
            CopyFakeToReal(player);
            FakePlayerDrawing.SetupCompositeDrawing(ref drawInfo, this, player);
            drawInfo.shadow = 0f; //shadow should be 1f for this draw.
            drawInfo.heldItem = heldItem;
            drawInfo.ItemLocation = itemLocation;
            drawInfo.playerEffect = playerEffect;
            drawInfo.itemEffect = itemEffect;
            drawInfo.Position = Position;

            SetupSpriteDirection(ref drawInfo, player);
            //Copy this arm data
            //drawInfo.drawPlayer.compositeFrontArm = this.compositeFrontArm;
            //drawInfo.drawPlayer.compositeBackArm = this.compositeBackArm;
            drawInfo.compFrontArmFrame = this.compFrontArmFrame;
            drawInfo.compBackArmFrame = this.compBackArmFrame;
            drawInfo.compositeFrontArmRotation = this.compositeFrontArmRotation;
            drawInfo.compositeBackArmRotation = this.compositeBackArmRotation;
            drawInfo.projectileDrawPosition = this.projectileDrawPosition;
            drawInfo.heldProjOverHand = this.heldProjOverHand;
            drawInfo.bodyVect = this.bodyVect;

            DrawTail(ref drawInfo, true);
            HijackItemDrawing(ref drawInfo, true);
            FakePlayerDrawing.DrawBackArm(ref drawInfo, true);
            FakePlayerDrawing.DrawBody(ref drawInfo, true);
            FakePlayerDrawing.DrawFrontArm(ref drawInfo, true);
            FakePlayerDrawing.DrawWings(ref drawInfo, WingFrame);

            if (weaponDrawOrder == 0)
                HijackItemDrawing(ref drawInfo, false);
            FakePlayerDrawing.DrawBackArm(ref drawInfo, false);
            DrawTail(ref drawInfo, false);
            FakePlayerDrawing.DrawBody(ref drawInfo, false);
            if(weaponDrawOrder == 1)
                HijackItemDrawing(ref drawInfo, false);
            if (player.heldProj == -1 || heldProjOverHand)
            {
                FakePlayerDrawing.DrawFrontArm(ref drawInfo, false);
            }
            if (weaponDrawOrder == 2)
                HijackItemDrawing(ref drawInfo, false);

            //Save this arm data
            //compositeFrontArm = drawInfo.drawPlayer.compositeFrontArm;
            //compositeBackArm = drawInfo.drawPlayer.compositeBackArm;
            compFrontArmFrame = drawInfo.compFrontArmFrame;
            compBackArmFrame = drawInfo.compBackArmFrame;
            compositeFrontArmRotation = drawInfo.compositeFrontArmRotation;
            compositeBackArmRotation = drawInfo.compositeBackArmRotation;
            projectileDrawPosition = drawInfo.projectileDrawPosition;
            heldProjOverHand = drawInfo.heldProjOverHand;
            bodyVect = drawInfo.bodyVect;
            itemLocation = drawInfo.ItemLocation;
            heldItem = drawInfo.heldItem;
            playerEffect = drawInfo.playerEffect;
            itemEffect = drawInfo.itemEffect;
            Position = drawInfo.Position;

            DrawMyProjectiles(player); //Doesn't matter where in the order this is called... as drawInfoDrawing will happen later anyway
            CopyRealToFake(player);
            LoadRealPlayerValues(player);
            FakePlayerProjectile.OwnerOfThisDrawCycle = -1;
            return true;
        }
        public void SecondaryFakePlayerDrawing(SpriteBatch spriteBatch, Player player)
        {
            if (HeldProj != -1)
            {
                SaveRealPlayerValues(player);
                CopyFakeToReal(player);
                DrawMyHeldProjectile(player);
                if(!heldProjOverHand)
                {
                    FakePlayerDrawing.DrawFrontArm(this, spriteBatch);
                }
                CopyRealToFake(player);
                LoadRealPlayerValues(player);
            }
        }
        public Texture2D saveGreenTexture;
        public Texture2D saveNormalTexture;
        public int lastItemID = -1;
        public void ConvertItemTextureToGreen(Item item)
        {
            saveNormalTexture = TextureAssets.Item[item.type].Value;
            if (item.type != lastItemID)
            {
                Texture2D itemTexture = TextureAssets.Item[item.type].Value;
                lastItemID = item.type;
                Color[] colors = SOTSItem.ConvertToSingleColor(itemTexture, new Color(0, 255, 0));
                saveGreenTexture = new Texture2D(Main.graphics.GraphicsDevice, itemTexture.Width, itemTexture.Height);
                saveGreenTexture.SetData(0, null, colors, 0, itemTexture.Width * itemTexture.Height);
            }
            SetTextureValueViaReflection(TextureAssets.Item[item.type], saveGreenTexture);
        }
        public void ConvertItemTextureBackToNormal(Item item)
        {
            SetTextureValueViaReflection(TextureAssets.Item[item.type], saveNormalTexture);
        }
        public void SetTextureValueViaReflection(Asset<Texture2D> texture, Texture2D newValue)
        {
            Type type = texture.GetType();
            FieldInfo fieldInfo = type.GetField("ownValue", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo == null)
                return;
            fieldInfo.SetValue(texture, newValue);
        }
        public void SetupSpriteDirection(ref PlayerDrawSet drawInfo, Player player)
        {
            drawInfo.playerEffect = (SpriteEffects)0;
            drawInfo.itemEffect = (SpriteEffects)1;
            if (player.gravDir == 1f)
            {
                if (player.direction == 1)
                {
                    drawInfo.playerEffect = 0;
                    drawInfo.itemEffect = 0;
                }
                else
                {
                    drawInfo.playerEffect = (SpriteEffects)1;
                    drawInfo.itemEffect = (SpriteEffects)1;
                }
                /*if (!player.dead)
                {
                    player.legPosition.Y = 0f;
                    player.headPosition.Y = 0f;
                    player.bodyPosition.Y = 0f;
                }*/
            }
            else
            {
                if (player.direction == 1)
                {
                    drawInfo.playerEffect = (SpriteEffects)2;
                    drawInfo.itemEffect = (SpriteEffects)2;
                }
                else
                {
                    drawInfo.playerEffect = (SpriteEffects)3;
                    drawInfo.itemEffect = (SpriteEffects)3;
                }
                /*if (!player.dead)
                {
                    player.legPosition.Y = 6f;
                    player.headPosition.Y = 6f;
                    player.bodyPosition.Y = 6f;
                }*/
            }
            switch (drawInfo.heldItem.type)
            {
                case 3182:
                case 3184:
                case 3185:
                case 3782:
                    drawInfo.itemEffect = (SpriteEffects)((int)drawInfo.itemEffect ^ 3);
                    break;
                case 5118:
                    if (player.gravDir < 0f)
                    {
                        drawInfo.itemEffect = (SpriteEffects)((int)drawInfo.itemEffect ^ 3);
                    }
                    break;
            }
        }
        public void SetupBodyFrame(Player player)
        {
            Player PlayerToFrame = new Player();
            PlayerToFrame.itemAnimation = itemAnimation;
            PlayerToFrame.itemAnimationMax = itemAnimationMax;
            PlayerToFrame.selectedItem = UseItemSlot;
            PlayerToFrame.itemRotation = itemRotation;
            PlayerToFrame.direction = direction;
            PlayerToFrame.inventory[UseItemSlot] = player.inventory[UseItemSlot];
            if(CheckItemValidityFull(player, PlayerToFrame.inventory[UseItemSlot]) || PlayerToFrame.inventory[UseItemSlot].IsAir)
                PlayerToFrame.PlayerFrame(); //don't care about what happens in here except for the body frame outcome
            bodyFrame = PlayerToFrame.bodyFrame;
            if (ShouldUseWingsArmPosition)
            {
                bodyFrame.Y = bodyFrame.Height * 6;
            }
        }
        public void DrawTail(ref PlayerDrawSet drawInfo, bool outLine = false)
        {
            Texture2D texture = ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantTail").Value;
            Texture2D textureOutline = ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantTailOutline").Value;
            Texture2D texture2 = ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceServantTailScales").Value;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 center = drawInfo.Position + new Vector2(Width / 2, Height / 2 + 2);
            Vector2 velo = new Vector2(0, 4f);
            float scale = 1f;
            List<Vector2> positions = new List<Vector2>();
            List<float> rotations = new List<float>();
            for (int i = 0; i < 9; i++)
            {
                Vector2 toOldPosition = SecondPosition - drawInfo.Position;
                toOldPosition.SafeNormalize(Vector2.Zero);
                velo += toOldPosition * 0.333f;
                velo = velo.SafeNormalize(Vector2.Zero) * scale * 4;
                center += velo;
                Vector2 drawPos = center - Main.screenPosition + new Vector2(0, -16 + Height / 2);
                positions.Add(drawPos);
                rotations.Add(velo.ToRotation() - MathHelper.ToRadians(90));
                scale -= 0.0725f;
            }
            if (outLine)
            {
                for (int i = 8; i >= 0; i--)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        Vector2 circular = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(90 * k));
                        drawInfo.DrawDataCache.Add(new DrawData(textureOutline, positions[i] + circular, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotations[i], origin, (1 - i * 0.08f), direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0));
                    }
                }
            }
            else
            {
                for (int i = 8; i >= 0; i--)
                {
                    drawInfo.DrawDataCache.Add(new DrawData(texture, positions[i], new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotations[i], origin, 1 - i * 0.08f, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0));
                }
                for (int i = 8; i >= 0; i--)
                {
                    drawInfo.DrawDataCache.Add(new DrawData(texture2, positions[i], new Rectangle(0, 0, texture.Width, texture.Height), Color.White, rotations[i], origin, 1 - i * 0.08f, direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0));
                }
            }
        }
        public void UpdateMyProjectiles(Player player)
        {
            bool hasFoundYOYO = false;
            for(int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if(projectile.active && projectile.owner == player.whoAmI)
                {
                    FakePlayerProjectile fPPInstance;
                    bool canGetGlobal = projectile.TryGetGlobalProjectile(out fPPInstance);
                    if (canGetGlobal)
                    {
                        if (fPPInstance.FakeOwnerIdentity == FakePlayerID)
                        {
                            projectile.Update(i);
                            if(projectile.aiStyle == ProjAIStyleID.Yoyo && !hasFoundYOYO)
                            {
                                hasFoundYOYO = true;
                                if(player.direction == 1)
                                    player.itemRotation = MathHelper.WrapAngle((projectile.Center - player.Center).ToRotation());
                                else
                                    player.itemRotation = MathHelper.WrapAngle((player.Center - projectile.Center).ToRotation());
                            }
                        }
                    }
                }
            }
        }
        public void DrawMyProjectiles(Player player)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.owner == player.whoAmI && projectile.type > 0 && !projectile.hide)
                {
                    FakePlayerProjectile fPPInstance;
                    bool canGetGlobal = projectile.TryGetGlobalProjectile(out fPPInstance);
                    if (canGetGlobal)
                    {
                        if (fPPInstance.FakeOwnerIdentity == FakePlayerID)
                        {
                            Main.instance.DrawProj(i);
                        }
                    }
                }
            }
        }
        public void DrawMyHeldProjectile(Player player)
        {
            FakePlayerProjectile.OwnerOfThisDrawCycle = FakePlayerID;
            DrawHeldProj(player, Main.projectile[player.heldProj]);
            FakePlayerProjectile.OwnerOfThisDrawCycle = -1;
        }
        public static void DrawHeldProj(Player player, Projectile proj)
        {
            if (!proj.active)
                return;
            float saveGFX = player.gfxOffY;
            player.gfxOffY = 0;
            proj.gfxOffY = 0;
            try
            {
                Main.instance.DrawProjDirect(proj);
            }
            catch
            {
                proj.active = false;
            }
            player.gfxOffY = saveGFX;
        }
        public void LoadValuesForCachedProjectileDraw(Player player, bool load)
        {
            if(load)
            {
                SaveRealPlayerValues(player);
                CopyFakeToReal(player);
            }
            else
            {
                CopyRealToFake(player);
                LoadRealPlayerValues(player);
            }
        }
        //public int[] OwnedProjectileCounts = new int[ProjectileID.Count];
        /*private void UpdateProjectileCaches(int i) //Was originally going to allow fake players to modify owned projectile counts, so you could double wield certain modded items. This was deemed to be a bad idea because of various runtime issues loading massive arrays would cause :)... Also, balance.
        {
            for (int j = 0; j < 1000; j++)
            {
                if (!Main.projectile[j].active || Main.projectile[j].owner != i)
                {
                    continue;
                }
                OwnedProjectileCounts[Main.projectile[j].type]++;
            }
        }
        private void ResetProjectileCaches()
        {
            for (int i = 0; i < OwnedProjectileCounts.Length; i++)
            {
                OwnedProjectileCounts[i] = 0;
            }
        }*/
    }
    public class SavedPlayerValues
    {
        public Vector2 SavePosition;
        public Vector2 SaveOldPos;
        public Vector2 SaveVelocity;
        public int SavedSelectedItem;
        public Item PlayerOwnedLastVisualizedSelectedItem;
        public bool saveChannel;
        public bool saveFrozen;
        public bool saveWebbed;
        public bool saveStoned;
        public bool saveWet;
        public bool saveMount;
        public int saveAltFunctionUse;
        public bool savePulley;
        public bool savePettingAnimal;
        public int saveHeldProj;
        public float saveStealth;
        public float saveGravDir;
        public bool saveInvis;

        public bool savecompositeFrontArmEnabled;
        public bool savecompositeBackArmEnabled;
        public int saveToolTime;
        public int saveItemAnimation;
        public int saveItemAnimationMax;
        public int saveItemTime;
        public int saveItemTimeMax;
        public float saveItemRotation;
        public Vector2 saveItemLocation;
        public int saveItemWidth;
        public int saveItemHeight;
        public int saveDirection;
        public int saveReuseDelay;
        public bool saveReleaseUseItem;
        public bool saveJustDroppedAnItem;
        public int saveAttackCD;
        public int saveItemUsesThisAnimation;
        public int saveBoneGloveTimer;
        public CompositeArmData saveFrontArm;
        public CompositeArmData saveBackArm;
        public Rectangle saveBodyFrame;
        public float saveGFXOffY;
    }
    public class FakeItem : GlobalItem
    {
        public static bool overrideLightColor = false;
        public static bool runOnce = false;
        public static bool superOverrideLightColor = false;
        public override Color? GetAlpha(Item item, Color lightColor)
        {
            if(superOverrideLightColor)
            {
                return Color.White;
            }
            if(overrideLightColor)
            {
                lightColor = Color.White;
                if(runOnce)
                {
                    runOnce = false;
                    Color color = item.GetAlpha(lightColor);
                    return color;
                }
            }
            return base.GetAlpha(item, lightColor);
        }
    }
}