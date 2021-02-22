<template>
  <Nav
    grid-column="1"
    :activeListLength="getActiveListLength"
    @change-tab="activeTab = $event"
  />

  <section id="content">
    <Items
      v-show="activeTab === 'Items'"
      :allItems="allItems"
      :activeListId="activeList.id"
      @add-to-list="addToList($event)"
      @select-item="selectItem($event)"
    />

    <History
      v-show="activeTab === 'History'"
      :AllLists="allLists"
      @refresh-lists="refreshLists"
      @refresh-chart="getStatistics"
      @show-notification="showNotification($event[0], $event[1])"
    />

    <Statistics
      v-if="activeTab === 'Statistics'"
      :topItems="topItems"
      :topCategories="topCategories"
      :monthlyData="monthlyData"
      :monthlyCategories="monthlyCategories"
    />
  </section>

  <section id="side">
    <AddItem
      v-if="isAddingItem"
      :allCategories="allCategories"
      @close="isAddingItem = false"
      @closeReload="refreshItems"
      @reload-categories="getCategories"
      @show-notification="showNotification($event[0], $event[1])"
    />

    <ItemDetails
      v-if="selectedItem"
      :item="selectedItem"
      @close="selectedItem = undefined"
      @reload="refreshItems(); refreshLists();"
      @add-to-list="addToList($event)"
      @show-notification="showNotification($event[0], $event[1])"
    />

    <ShoppingList
      v-show="!selectedItem && !isAddingItem"
      :ShoppingList="activeList"
      :displayStatsInfo="isStatsFocused"
      @addItem="isAddingItem = true"
      @refresh-list="refreshLists"
      @refresh-chart="getStatistics"
      @show-notification="showNotification($event[0], $event[1])"
    />
  </section>

  <transition name="slide">
    <Notification        
      v-show="notificationVisible"      
      :content="notificationContent"
      :type="notificationType"
    />
  </transition>

  <Preloader v-if="!loaded" />
</template>

<script>
import API from "@/API";

import Nav from "./components/Nav";
import Notification from "./components/Notification";
import Preloader from "./components/Preloader";

import Items from "./views/Items";
import History from "./views/History";
import Statistics from "./views/Statistics";
import AddItem from "@/views/AddItem";
import ShoppingList from "@/views/ShoppingList";
import ItemDetails from "@/views/ItemDetails";

export default {
  name: "App",
  components: {
    Nav,
    Items,
    History,
    AddItem,
    Preloader,
    Statistics,
    ItemDetails,
    ShoppingList,
    Notification,
  },
  data() {
    return {
      allItems: [],
      allLists: [],
      topItems: [],
      loaded: false,
      activeList: {},
      monthlyData: [],
      topCategories: [],
      allCategories: [],
      monthlySummary: [],
      activeTab: "Items",
      isAddingItem: false,
      monthlyCategories: [],
      hasSelectedItem: false,
      selectedItem: undefined,
      notificationVisible: false,
      notificationType: "",
      notificationContent: "",
    };
  },
  methods: {
    async getItems() {
      this.allItems = await API.getItems();
    },
    async getCategories() {
      this.allCategories = await API.getCategories();
    },
    async getAllLists() {
      this.allLists = await API.getAllLists(true);
    },
    async getShoppingList() {
      this.activeList = await API.getActiveList();
    },
    async getTopItems() {
      this.topItems = await API.getTopItems();
    },
    async getTopCategories() {
      this.topCategories = await API.getTopCategories();
    },
    async getMonthlySummary() {
      this.monthlySummary = await API.getMonthlySummary();
    },
    async getStatistics() {
      await this.getMonthlySummary();
      await this.getTopItems();
      await this.getTopCategories();
      await this.getChartData();
    },
    async getInitialData() {
      await this.getItems();
      await this.getAllLists();
      await this.getShoppingList();
      await this.getCategories();
      await this.getStatistics();
    },
    async addToList(item) {
      const result = await API.addItemToList(this.activeList.id, item.id);

      if (result instanceof Error) {
        this.showNotification(result.message, "error");
        return;
      } else {
        await this.getShoppingList();
        this.showNotification("Successfully added to list", "success");
        this.getStatistics();
        this.resetSideContainer();
      }
    },
    async refreshItems() {
      await this.getItems();
      this.resetSideContainer();
    },
    async refreshLists() {
      this.resetSideContainer();
      await this.getShoppingList();
      await this.getAllLists();
      this.resetSideContainer();
    },
    getChartData() {
      let chartData = [];
      let categories = [];

      this.monthlySummary.forEach((stat) => {
        categories.push(stat.label);
        chartData.push(stat.value);
      });

      this.monthlyCategories = categories;
      this.monthlyData = chartData;
    },
    selectItem(item) {
      this.isAddingItem = false;
      this.selectedItem = item;
    },
    resetSideContainer() {
      this.isAddingItem = false;
      this.selectedItem = undefined;
    },
    showNotification(notificationContent, notificationType) {      
      this.notificationType = notificationType;
      this.notificationContent = notificationContent;
      this.notificationVisible = true;
      setTimeout(() => (this.notificationVisible = false), 1.5 * 1000);
    },
  },
  computed: {
    getActiveListLength() {
      let length = 0;

      this.activeList.groups?.forEach((group) => {
        group.items?.forEach((item) => {
          length += item.quantity;
        });
      });

      return length;
    },
    isStatsFocused() {
      return this.activeTab === "Statistics";
    },
  },
  async beforeMount() {
    await this.getInitialData();
    this.getChartData();
    this.loaded = true;
  },
};
</script>

<style>
#app {
  display: grid;
  grid-template-columns: 100px 2fr auto;
}

#content {
  grid-column: 2;

  background: #fafafe;
}

#items-tab,
#history-tab,
#statistics-tab {
  padding-top: 4rem;
  padding-left: 8rem;
  max-height: 100vh;
  height: 100vh;
  overflow-y: auto;
}

#side {
  grid-column: 3;
}

.shopping-list-container,
.add-item-container,
.item-details {
  max-width: 40rem;
  width: 40rem;
  min-height: 100vh;
  max-height: 100vh;
}

.slide-enter-active,
.slide-leave-active {
  transition: all 1s ease-in-out;
}
.slide-enter,
.slide-leave-to {
  opacity: 0;
  transform: translateY(300px);
  transition: all 1s ease-in-out;
}

.slide-enter-to,
.slider-levae {
  opacity: 1;
  transform: translateY(0px);
  transition: all 2s ease-in-out;
}
</style>
