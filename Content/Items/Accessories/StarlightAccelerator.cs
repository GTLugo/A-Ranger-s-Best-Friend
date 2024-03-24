using BestFriend.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BestFriend.Content.Items.Accessories;

public class StarlightAccelerator : ModItem {
  public override string Texture => BestFriend.ASSET_PATH + "Textures/Items/Accessories/StarlightAccelerator";
  private const int ARMOR_PENETRATION = 12;
  private const int CRIT_CHANCE = 8;

  public override void SetStaticDefaults() {
    CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
  }

  public override void SetDefaults() {
    Item.width = 32;
    Item.height = 32;
    Item.accessory = true;
    Item.rare = ItemRarityID.Pink;
    Item.value = Item.sellPrice(0, 0, 20, 10);
  }

  public override void UpdateAccessory(Player player, bool hide_visual) {
    var abilities = player.GetModPlayer<BestFriendAbilitiesPlayer>();
    abilities.hit_scan = true;

    player.GetArmorPenetration(DamageClass.Ranged) += ARMOR_PENETRATION;
    player.GetCritChance(DamageClass.Ranged) += CRIT_CHANCE;
  }

  public override void AddRecipes() {
    CreateRecipe()
      .AddIngredient(ItemID.MeteoriteBar, 16)
      .AddIngredient(ItemID.FallenStar, 8)
      .AddTile(TileID.TinkerersWorkbench)
      .Register();
  }
}