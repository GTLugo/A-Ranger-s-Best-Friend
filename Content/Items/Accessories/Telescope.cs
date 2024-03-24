using BestFriend.Common;
using BestFriend.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BestFriend.Content.Items.Accessories;

public class Telescope : ModItem {
  public override string Texture => BestFriend.ASSET_PATH + "Textures/Items/Accessories/StarlightScope";
  public override void SetStaticDefaults() {
    CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
  }

  public override void SetDefaults() {
    Item.width = 30;
    Item.height = 30;
    Item.accessory = true;
    Item.rare = ItemRarityID.Yellow;
    Item.value = Item.sellPrice(0, 2, 10);
  }

  public override void UpdateAccessory(Player player, bool hide_visual) {
    var config = ModContent.GetInstance<BestFriendConfig>();

    var abilities = player.GetModPlayer<BestFriendAbilitiesPlayer>();
    abilities.hit_scan = true;

    player.scope = !config.disable_scope;
    player.aggro -= 400;

    player.GetArmorPenetration(DamageClass.Ranged) += 10;
    player.GetDamage(DamageClass.Ranged) += 0.10f;
    player.GetCritChance(DamageClass.Ranged) += 10;
  }

  public override void AddRecipes() {
    CreateRecipe()
      .AddIngredient(ItemID.ReconScope)
      .AddIngredient<StarlightAccelerator>()
      .AddTile(TileID.TinkerersWorkbench)
      .Register();
  }
}